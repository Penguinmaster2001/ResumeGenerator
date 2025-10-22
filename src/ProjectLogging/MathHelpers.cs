
namespace ProjectLogging;



public static class MathHelpers
{
    public static float CosineSimilarity(float[] a, float[] b)
    {
        int minLength = a.Length < b.Length ? a.Length : b.Length;

        float dot = 0.0f;
        float magA = 0.0f;
        float magB = 0.0f;

        for (int i = 0; i < minLength; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }

        if (magA is <= float.Epsilon and >= -float.Epsilon)
        {
            if (magB is <= float.Epsilon and >= -float.Epsilon)
            {
                return 1.0f;
            }

            return 0.0f;
        }

        return dot / (MathF.Sqrt(magA) * MathF.Sqrt(magB));
    }
}
