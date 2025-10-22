
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
        // Tokenize both texts together
        var inputIds = query.Concat(Tokenizer.EncodeToIds($" [SEP] {text} [SEP]").Select(id => (long)id)).ToArray();

        // Truncate/pad to fit model input
        if (inputIds.Length > _maxLength)
        {
            // DESIGN ISSUE: Console.WriteLine in library code creates unwanted side effects and couples
            // the code to console I/O. Libraries should not write directly to console; instead, consider
            // using a logging framework (e.g., ILogger), throwing an exception, or providing a callback
            // mechanism for warnings. This also makes the code difficult to test and use in non-console
            // applications (e.g., web services, GUI apps).
            Console.WriteLine($"Too long!! ({inputIds.Length} > {_maxLength})");
            inputIds = inputIds[.._maxLength];
        }
        else if (inputIds.Length < _maxLength)
        {
            inputIds = [.. inputIds, .. Enumerable.Repeat(0, _maxLength - inputIds.Length)];
        }

        var attentionMask = inputIds.Select(id => id == 0 ? 0L : 1L).ToArray();
        var tokenTypeIds = new long[_maxLength]; // all zeros (simplified single sequence)

        // Create ONNX inputs
        var shape = new[] { 1, _maxLength };
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
