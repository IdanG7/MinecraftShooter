using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    public class Villager : Mob
    {
        private Texture2D texture;
        private Vector2 position;
        private float speed;
        private GraphicsDevice graphicsDevice;

        public Villager(ContentManager content, Texture2D texture, Vector2 position, float speed, GraphicsDevice graphicsDevice) : base(null, position, 1, 10)
        {
            this.texture = content.Load<Texture2D>("Sized/Villager_64");
            this.position = position;
            this.speed = (int)speed;
            this.graphicsDevice = graphicsDevice;


        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            // Move in a straight line at a medium-fast pace
            position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check if the villager is off the screen
            if (position.X < 0 || position.X + texture.Width > graphicsDevice.Viewport.Width)
            {
                // Reverse the villager's horizontal direction
                speed *= -1;

                // Make sure the villager is not off the screen
                position.X = MathHelper.Clamp(position.X, 0, graphicsDevice.Viewport.Width - texture.Width);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }




    }
}
