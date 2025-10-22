
namespace ProjectLogging;



public static class MathHelpers
{
    // DESIGN ISSUE: This method does not validate that arrays 'a' and 'b' have the same length.
    // If they differ, the loop will either skip elements or throw an IndexOutOfRangeException.
    // Add a length check: if (a.Length != b.Length) throw new ArgumentException(...).
    // Also consider handling the edge case where arrays are empty (would cause division by zero).
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
