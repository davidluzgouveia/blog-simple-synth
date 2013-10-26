using Microsoft.Xna.Framework.Graphics;
using SimpleSynth.Synthesizer;

namespace SimpleSynth
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class SimpleSynthGame : Game
    {
        public SimpleSynthGame()
        {
            GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _synth = new Synth();
            ApplyOscillator();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteFont = Content.Load<SpriteFont>("font");
            _piano = new Piano(_spriteFont, GraphicsDevice);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();

            int modifier = (_currentKeyboardState.IsKeyDown(Keys.LeftShift) || _currentKeyboardState.IsKeyDown(Keys.RightShift)) ? 10 : 1;

            if (_currentKeyboardState.IsKeyDown(Keys.Space) && _previousKeyboardState.IsKeyUp(Keys.Space))
            {
                NextOscillatorType();
            }

            if (_currentKeyboardState.IsKeyDown(Keys.Delete))
            {
                _synth.FadeInDuration -= 100 * modifier;
                if (_synth.FadeInDuration < 0)
                    _synth.FadeInDuration = 0;
            }

            if (_currentKeyboardState.IsKeyDown(Keys.Insert))
            {
                _synth.FadeInDuration += 100 * modifier;
            }

            if(_currentKeyboardState.IsKeyDown(Keys.PageDown))
            {
                _synth.FadeOutDuration -= 100 * modifier;
                if (_synth.FadeOutDuration < 0)
                    _synth.FadeOutDuration = 0;
            }

            if (_currentKeyboardState.IsKeyDown(Keys.PageUp))
            {
                _synth.FadeOutDuration += 100 * modifier;
            }

            CheckNoteTrigger(Keys.A, 0);
            CheckNoteTrigger(Keys.W, 1);
            CheckNoteTrigger(Keys.S, 2);
            CheckNoteTrigger(Keys.E, 3);
            CheckNoteTrigger(Keys.D, 4);
            CheckNoteTrigger(Keys.F, 5);
            CheckNoteTrigger(Keys.T, 6);
            CheckNoteTrigger(Keys.R, 6);
            CheckNoteTrigger(Keys.G, 7);
            CheckNoteTrigger(Keys.Y, 8);
            CheckNoteTrigger(Keys.H, 9);
            CheckNoteTrigger(Keys.U, 10);
            CheckNoteTrigger(Keys.J, 11);
            CheckNoteTrigger(Keys.K, 12);
            CheckNoteTrigger(Keys.O, 13);
            CheckNoteTrigger(Keys.L, 14);
            
            _previousKeyboardState = _currentKeyboardState;

            _synth.Update(gameTime);

            base.Update(gameTime);
        }

        private void CheckNoteTrigger(Keys key, int note)
        {
            if (_currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key))
            {
                _synth.NoteOn(note);
                _piano.NoteOn(note);
            }
            if (_currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key))
            {
                _synth.NoteOff(note);
                _piano.NoteOff(note);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_spriteFont, "Active Voices: " + _synth.ActiveVoicesCount, Vector2.Zero, Color.White);
            _spriteBatch.DrawString(_spriteFont, "Free Voices: " + _synth.FreeVoicesCount, new Vector2(0, 20), Color.White);
            _spriteBatch.DrawString(_spriteFont, "Registered Notes: " + _synth.KeyRegistryCount, new Vector2(0, 40), Color.White);
            _spriteBatch.DrawString(_spriteFont, "Oscillator: " + _oscillatorType, new Vector2(600, 0), Color.White);
            _spriteBatch.DrawString(_spriteFont, "Attack/FadeIn: " + (_synth.FadeInDuration == 0 ? "Disabled" : (((int)(1000 * _synth.FadeInDuration / 44100.0f)).ToString() + "ms")), new Vector2(300, 0), Color.White);
            _spriteBatch.DrawString(_spriteFont, "Release/FadeOut: " + (_synth.FadeOutDuration == 0 ? "Disabled" : (((int)(1000 * _synth.FadeOutDuration / 44100.0f)).ToString() + "ms")), new Vector2(300, 20), Color.White);
            _spriteBatch.DrawString(_spriteFont, "Creating a Basic Synth in XNA 4.0 - Part III (Sample)", new Vector2(110, 101), Color.Yellow, 0f, Vector2.Zero, new Vector2(1.3f), SpriteEffects.None, 0.0f );
            _spriteBatch.DrawString(_spriteFont, "Space: Change Oscillator  |  Insert and Delete: Attack  |  PageUp and PageDown: Release", new Vector2(0, 550), Color.White);
            _spriteBatch.DrawString(_spriteFont, "Source at: http://www.david-gouveia.com", new Vector2(0, 580), Color.Yellow);
            _spriteBatch.End();
            _piano.Draw(_spriteBatch);
            base.Draw(gameTime);
        }

        public void ApplyOscillator()
        {
            if (_oscillatorType == OscillatorTypes.Sine)
                _synth.Oscillator = Oscillator.Sine;
            else if (_oscillatorType == OscillatorTypes.Triangle)
                _synth.Oscillator = Oscillator.Triangle;
            else if (_oscillatorType == OscillatorTypes.Square)
                _synth.Oscillator = Oscillator.Square;
            else
                _synth.Oscillator = Oscillator.Sawtooth;
        }

        public void NextOscillatorType()
        {
            _oscillatorType = (OscillatorTypes)(((int)_oscillatorType + 1) % 4);
            ApplyOscillator();
        }

        private static void Main(string[] args)
        {
            using (SimpleSynthGame game = new SimpleSynthGame())
            {
                game.Run();
            }
        }

        private Synth _synth;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        private OscillatorTypes _oscillatorType = OscillatorTypes.Triangle;
        private SpriteFont _spriteFont;
        private SpriteBatch _spriteBatch;
        private Piano _piano;

        private enum OscillatorTypes
        {
            Sine,
            Triangle,
            Square,
            Sawtooth
        }
    }
}
