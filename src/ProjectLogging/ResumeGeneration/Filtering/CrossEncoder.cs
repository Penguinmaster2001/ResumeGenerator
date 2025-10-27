
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.Tokenizers;



namespace ProjectLogging.ResumeGeneration.Filtering;



public class CrossEncoder : IDisposable
{
    public readonly Tokenizer Tokenizer;
    private readonly InferenceSession _session;
    private readonly int _maxLength;



    public CrossEncoder(string modelPath, string vocabPath, int maxLength)
    {
        Tokenizer = BertTokenizer.Create(vocabPath);
        _session = new InferenceSession(modelPath);
        _maxLength = maxLength;
    }



    public float Score(string query, string text)
    {
        var queryIds = Tokenizer.EncodeToIds(query).Select(id => (long)id).ToArray();

        return Score(queryIds, text);
    }



    public float Score(long[] query, string text)
    {
        long clsId = 101L;
        long sepId = 102L;
        // Tokenize both texts together
        var inputIds = query.Prepend(clsId).Append(sepId).Concat(Tokenizer.EncodeToIds(text.ToLower(), 512, out _, out _).Select(id => (long)id)).Append(sepId).ToArray();

        var tokenTypeIds = new long[inputIds.Length];
        bool segmentB = false;
        for (int i = 0; i < inputIds.Length; i++)
        {
            tokenTypeIds[i] = segmentB ? 1 : 0;
            if (!segmentB && inputIds[i] == sepId) segmentB = true;
        }

        // Truncate/pad to fit model input
        if (inputIds.Length > _maxLength)
        {
            Console.WriteLine($"Too long!! ({inputIds.Length} > {_maxLength})");
            inputIds = inputIds[.._maxLength];
        }
        // else if (inputIds.Length < _maxLength)
        // {
        //     inputIds = [.. inputIds, .. Enumerable.Repeat(0, _maxLength - inputIds.Length)];
        // }

        var attentionMask = Enumerable.Repeat(1L, inputIds.Length).ToArray();// inputIds.Select(id => id == 0 ? 0L : 1L).ToArray();
                                                                             // var tokenTypeIds = new long[_maxLength]; // all zeros (simplified single sequence)

        // Console.WriteLine(text);
        // Console.WriteLine(string.Join(", ", inputIds));
        // Console.WriteLine(string.Join(", ", tokenTypeIds));
        // Console.WriteLine(string.Join(", ", attentionMask));

        // Create ONNX inputs
        var shape = new[] { 1, inputIds.Length };
        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input_ids", new DenseTensor<long>(inputIds, shape)),
            NamedOnnxValue.CreateFromTensor("attention_mask", new DenseTensor<long>(attentionMask, shape)),
            NamedOnnxValue.CreateFromTensor("token_type_ids", new DenseTensor<long>(tokenTypeIds, shape)),
        };

        // Run model
        using var results = _session.Run(inputs);
        var logits = results[0].AsTensor<float>();

        // The model outputs a single score [batch_size, 1]
        return logits[0];
    }



    public void Dispose()
    {
        _session.Dispose();

        GC.SuppressFinalize(this);
    }
}
