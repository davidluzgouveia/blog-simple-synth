namespace SimpleSynth.Synthesizer
{
    using System;
    using Microsoft.Xna.Framework;

    public static class SoundHelper
    {
        public static float MixSamples(float a, float b)
        {
            if(a < 0 && b < 0)
                return (a + b) - ((a * b) / -1.0f);
            if(a > 0 && b > 0)
                return (a + b) - ((a * b) / 1.0f);
            return a + b;
        }

        public static float NoteToFrequency(int note)
        {
            return (float) (440.0 * Math.Pow(2, (note - 9) / 12.0f));
        }

        public static void ConvertBuffer(float[,] from, byte[] to)
        {
            const int bytesPerSample = 2;
            int channels = from.GetLength(0);
            int bufferSize = from.GetLength(1);

            // Make sure the buffer sizes are correct
            System.Diagnostics.Debug.Assert(to.Length == bufferSize * channels * bytesPerSample, "Buffer sizes are mismatched.");

            for (int i = 0; i < bufferSize; i++)
            {
                for (int c = 0; c < channels; c++)
                {
                    // First clamp the value to the [-1.0..1.0] range
                    float floatSample = MathHelper.Clamp(from[c, i], -1.0f, 1.0f);

                    // Convert it to the 16 bit [short.MinValue..short.MaxValue] range
                    short shortSample = (short)(floatSample >= 0.0f ? floatSample * short.MaxValue : floatSample * short.MinValue * -1);

                    // Calculate the right index based on the PCM format of interleaved samples per channel [L-R-L-R]
                    int index = i * channels * bytesPerSample + c * bytesPerSample;

                    // Store the 16 bit sample as two consecutive 8 bit values in the buffer with regard to endian-ness
                    if (!BitConverter.IsLittleEndian)
                    {
                        to[index] = (byte)(shortSample >> 8);
                        to[index + 1] = (byte)shortSample;
                    }
                    else
                    {
                        to[index] = (byte)shortSample;
                        to[index + 1] = (byte)(shortSample >> 8);
                    }
                }
            }
        }
    }
}
