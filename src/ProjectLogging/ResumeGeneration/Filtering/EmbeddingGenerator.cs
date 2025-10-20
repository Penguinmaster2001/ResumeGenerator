
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.Tokenizers;



namespace ProjectLogging.ResumeGeneration.Filtering;




public class EmbeddingGenerator : IDisposable
{
    private readonly Tokenizer _tokenizer;
    private readonly InferenceSession _session;



    public EmbeddingGenerator(string modelPath, string vocabPath)
    {
        _tokenizer = BertTokenizer.Create(vocabPath);
        _session = new InferenceSession(modelPath);
    }



    public float[] GetEmbedding(string text)
    {
        var ids = _tokenizer.EncodeToIds(text, 256, out var normalizedText, out var charsConsumed);
        var inputIds = ids.Select(id => (long)id).ToArray();

        // Build attention mask (1 for all tokens)
        var attentionMask = Enumerable.Repeat(1L, inputIds.Length).ToArray();
        var tokenTypeIds = new long[inputIds.Length];

        // Tensors
        var shape = new[] { 1, inputIds.Length };
        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input_ids", new DenseTensor<long>(inputIds, shape)),
            NamedOnnxValue.CreateFromTensor("attention_mask", new DenseTensor<long>(attentionMask, shape)),
            NamedOnnxValue.CreateFromTensor("token_type_ids", new DenseTensor<long>(tokenTypeIds, shape)),
        };

        // Run model
        using var results = _session.Run(inputs);
        var lastHiddenState = results[0].AsTensor<float>();

        // Mean-pool token embeddings
        int hiddenSize = lastHiddenState.Dimensions[2];
        int seqLen = lastHiddenState.Dimensions[1];
        var pooled = new float[hiddenSize];

        for (int i = 0; i < seqLen; i++)
        {
            for (int j = 0; j < hiddenSize; j++)
            {
                pooled[j] += lastHiddenState[0, i, j];
            }
        }

        for (int j = 0; j < hiddenSize; j++)
        {
            pooled[j] /= seqLen;
        }

        return pooled;
    }



    public void Dispose()
    {
        _session.Dispose();

        GC.SuppressFinalize(this);
    }
}
