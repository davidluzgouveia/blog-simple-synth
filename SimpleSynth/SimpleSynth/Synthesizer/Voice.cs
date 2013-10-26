namespace SimpleSynth.Synthesizer
{
    public class Voice
    {
        private enum VoiceState { Attack, Sustain, Release }

        public Voice(Synth synth)
        {
            _synth = synth;
        }

        private VoiceState _state;

        public bool IsAlive { get; private set; }

        public void Start(float frequency)
        {
            _frequency = frequency;
            _time = 0.0f;
            _fadeMultiplier = 0.0f;

            _fadeCounter = 0;

            if (_synth.FadeInDuration == 0)
            {
                _state = VoiceState.Sustain;
            }
            else
            {
                _state = VoiceState.Attack;
            }

            IsAlive = true;
        }

        public void Stop()
        {
            if (_synth.FadeOutDuration == 0)
            {
                IsAlive = false;
            }
            else
            {
                _fadeCounter = (int)((1.0f - _fadeMultiplier) * _synth.FadeOutDuration);
                _state = VoiceState.Release;
            }
        }

        public void Process(float[,] workingBuffer)
        {
            if (IsAlive)
            {
                int samplesPerBuffer = workingBuffer.GetLength(1);
                for (int i = 0; i < samplesPerBuffer; i++)
                {
                    if (_state == VoiceState.Attack)
                    {
                        _fadeMultiplier = (float)_fadeCounter / _synth.FadeInDuration;

                        ++_fadeCounter;
                        if (_fadeCounter >= _synth.FadeInDuration)
                        {
                            _state = VoiceState.Sustain;
                        }
                    }
                    else if (_state == VoiceState.Release)
                    {
                        _fadeMultiplier = 1.0f - (float)_fadeCounter / _synth.FadeOutDuration;

                        ++_fadeCounter;
                        if (_fadeCounter >= _synth.FadeOutDuration)
                        {
                            IsAlive = false;
                            return;
                        }
                    }
                    else
                    {
                        _fadeMultiplier = 1.0f;
                    }

                    float sample = _synth.Oscillator(_frequency, _time);
                    workingBuffer[0, i] += sample * 0.2f * _fadeMultiplier;
                    _time += 1.0f / Synth.SampleRate;
                }
            }
        }

        private float _frequency;
        private float _time;
        private float _fadeMultiplier;
        private int _fadeCounter;
        private readonly Synth _synth;
    }
}