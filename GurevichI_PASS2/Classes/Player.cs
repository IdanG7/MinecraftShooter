using System;
using System.Collections.Generic;
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
        public float Speed = 3f;
        public float fearTimer;
        public const float FearDuration = 0.5f;




        public Player(Texture2D texture, Vector2 position, float speed)
        {
            this.texture = texture;
            Position = position;
            this.Speed = speed;
            fearTimer = 0f;

            // Initialize properties for the shop functionality

            Speed = speed;
        }

        public bool HandleCollisionWithSkeletonArrows(List<Arrow> skeletonArrows)
        {
            foreach (Arrow arrow in skeletonArrows)
            {
                if (arrow.BoundingBox.Intersects(BoundingBox))
                {
                    return true;
                }
            }

            return false;
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            }
        }

        public Vector2 GetCenter()
        {
            return new Vector2(Position.X + texture.Width / 2, Position.Y + texture.Height / 2);
        }

        public float GetRadius()
        {
            return Math.Min(texture.Width, texture.Height) / 2;
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
                newPosition.X -= Speed; // Update to use the Speed property
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                newPosition.X += Speed; // Update to use the Speed property
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
