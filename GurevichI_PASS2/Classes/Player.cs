using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GurevichI_PASS2
{
    public class Player
    {
        public Texture2D texture;
        public Vector2 Position;
        public float speed = 3f;
        public float fearTimer;
        public const float FearDuration = 0.5f;

        public Player(Texture2D texture, Vector2 position, float speed)
        {
            this.texture = texture;
            Position = position;
            this.speed = speed;
            fearTimer = 0f;
        }

        public void Update(KeyboardState keyboardState, GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            if (fearTimer > 0)
            {
                fearTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }

            Vector2 newPosition = Position;

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                newPosition.X -= speed;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                newPosition.X += speed;
            }

            newPosition.X = MathHelper.Clamp(newPosition.X, 0, graphicsDevice.Viewport.Width - texture.Width);
            Position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }

        public bool InduceFear()
        {
            fearTimer = FearDuration;
            return true;

        }
    }
}
