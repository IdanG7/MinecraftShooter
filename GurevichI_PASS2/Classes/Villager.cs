using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    public class Villager : Mob
    {
        private Vector2 position;
        private float speed;
        private GraphicsDevice graphicsDevice;
        private int _hp;

        public Villager(ContentManager content, Vector2 position, float speed, GraphicsDevice graphicsDevice, int hp) : base(content.Load<Texture2D>("Sized/Villager_64"), position, 2, hp)
        {
            this.position = position;
            this.speed = (int)speed;
            this.graphicsDevice = graphicsDevice;
            this.Hp = _hp;

        }

        public new Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
            }
        }


        public override void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            // Move in a straight line at a medium-fast pace
            position.X += speed;

            // Check if the villager is off the screen
            if (position.X < 0 || position.X + Texture.Width > graphicsDevice.Viewport.Width)
            {
                // Reverse the villager's horizontal direction
                speed *= -1;

                // Make sure the villager is not off the screen
                position.X = MathHelper.Clamp(position.X, 0, graphicsDevice.Viewport.Width - Texture.Width);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, position, Color.White);
        }
    }
}
