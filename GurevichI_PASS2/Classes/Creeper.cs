using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    public class Creeper : Mob
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 playerPosition;
        private GraphicsDevice graphicsDevice;

        public Creeper(ContentManager content, Texture2D texture, Vector2 position, Vector2 playerPosition, float speed, GraphicsDevice graphicsDevice)
            : base(texture, position, 2, 1)
        {
            PointValue = 40;
            this.position = position;
            this.playerPosition = playerPosition;
            this.graphicsDevice = graphicsDevice;
            this.texture = content.Load<Texture2D>("Sized/Creeper_64");
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            // Calculate the direction vector towards the player
            Vector2 direction = playerPosition - position;
            direction.Normalize();

            // Move the creeper towards the player at the given speed
            position += direction * Speed;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }


    }
}
