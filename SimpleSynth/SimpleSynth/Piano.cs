using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleSynth
{
    public class Piano
    {
        public Vector2 Position;
        public Vector2 Size;

        public Piano(SpriteFont font, GraphicsDevice graphicsDevice)
        {
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            _keys = new bool[MaxNotes];
            for (int i = 0; i < MaxNotes; i++)
            {
                _keys[i] = false;
            }

            _font = font;

            Position = new Vector2(50, 150);
            Size = new Vector2(600, 339);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int whiteKeyWidth = (int) (Size.X / 8);
            int blackKeyWidth = (int) (whiteKeyWidth / 1.8f);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);


            for (int i = 0; i < MaxNotes; i++)
            {
                int m = (i / 12);
                int n = i % 12;

                // White Note
                if(n == 0 || n == 2 || n == 4 || n == 5 || n == 7 || n == 9 || n == 11)
                {
                    if(n == 2)
                    {
                        n = 1;
                    }
                    else if(n == 4)
                    {
                        n = 2;
                    }
                    else if(n == 5)
                    {
                        n = 3;
                    }
                    else if(n == 7)
                    {
                        n = 4;
                    }
                    else if(n == 9)
                    {
                        n = 5;
                    }
                    else if(n == 11)
                    {
                        n = 6;
                    }

                    FillRectangle(spriteBatch, Position.X + (n + m * 7) * whiteKeyWidth, Position.Y, whiteKeyWidth, Size.Y, Color.Black, 1.0f);
                    FillRectangle(spriteBatch, Position.X + (n + m * 7) * whiteKeyWidth + 1, Position.Y + 1, whiteKeyWidth - 2, Size.Y - 2, _keys[i] ? Color.Yellow : Color.White, 0.9f);

                    spriteBatch.DrawString(_font, _letters[i], new Vector2(Position.X + (n + m * 7) * whiteKeyWidth + whiteKeyWidth/2, Position.Y + Size.Y - 50), Color.Black);
                }
                // Black Key
                else
                {
                    if (n == 1)
                    {
                        n = 0;
                    }
                    else if (n == 3)
                    {
                        n = 1;
                    }
                    else if (n == 6)
                    {
                        n = 3;
                    }
                    else if (n == 8)
                    {
                        n = 4;
                    }
                    else if (n == 10)
                    {
                        n = 5;
                    }

                    FillRectangle(spriteBatch, Position.X + (n + m * 7) * whiteKeyWidth + whiteKeyWidth - blackKeyWidth / 2, Position.Y, blackKeyWidth, Size.Y / 2, Color.Black, 0.8f);
                    FillRectangle(spriteBatch, Position.X + (n + m * 7) * whiteKeyWidth + whiteKeyWidth - blackKeyWidth / 2 + 1, Position.Y + 1, blackKeyWidth - 2, Size.Y / 2 - 2, _keys[i] ? Color.Yellow : Color.Black, 0.7f);

                    spriteBatch.DrawString(_font, _letters[i], new Vector2(Position.X + (n + m * 7) * whiteKeyWidth + whiteKeyWidth - 6, Position.Y), Color.White);
                }
            }
            spriteBatch.End();
        }

        public void NoteOn(int note)
        {
            if(note < _keys.Length)
            _keys[note] = true;
        }

        public void NoteOff(int note)
        {
            if (note < _keys.Length)
            _keys[note] = false;
        }

        private void FillRectangle(SpriteBatch spriteBatch, float x, float y, float w, float h, Color color, float depth)
        {
            spriteBatch.Draw(_pixel, new Rectangle((int)System.Math.Round(x), (int)System.Math.Round(y), (int)System.Math.Round(w), (int)System.Math.Round(h)), null, color, 0.0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        private string[] _letters = new[] { "A", "W", "S", "E", "D", "F", "T", "G", "Y", "H", "U", "J", "K", "O", "L"};
        private readonly Texture2D _pixel;
        private bool[] _keys;
        private const int MaxNotes = 15;
        private SpriteFont _font;
    }
}
