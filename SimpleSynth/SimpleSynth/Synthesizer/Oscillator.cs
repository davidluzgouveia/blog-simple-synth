namespace SimpleSynth.Synthesizer
{
    using System;

    public delegate float OscillatorDelegate(float frequency, float time);

    public static class Oscillator
    {
        public static float Sine(float frequency, float time)
        {
            return (float) Math.Sin(frequency * time * 2 * Math.PI);
        }

        public static float Square(float frequency, float time)
        {
            return Sine(frequency, time) >= 0 ? 1.0f : -1.0f;
        }

        public static float Sawtooth(float frequency, float time)
        {
            return (float) (2 * (time * frequency - Math.Floor(time * frequency + 0.5)));
        }

        public static float Triangle(float frequency, float time)
        {
            return Math.Abs(Sawtooth(frequency, time)) * 2.0f - 1.0f;
        }
    }
}
