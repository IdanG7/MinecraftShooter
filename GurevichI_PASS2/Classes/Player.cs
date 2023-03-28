using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GurevichI_PASS2
{
    public class Player
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        private float speed = 3f;
        public Player(Texture2D texture, Vector2 position, float speed)
        {
            Texture = texture;
            Position = position;
            this.speed = speed;
        }

        public void Update(KeyboardState keyboardState, int screenWidth)
        {
            Vector2 newPosition = Position;

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                newPosition.X -= speed;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                newPosition.X += speed;
            }

            newPosition.X = MathHelper.Clamp(newPosition.X, 0, screenWidth - Texture.Width);
            Position = newPosition;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}


