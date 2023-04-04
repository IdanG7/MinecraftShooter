using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    public class Villager : Mob
    {
        public float offScreenTimer;
        private Vector2 position;
        private Texture2D texture;
        private GraphicsDevice graphicsDevice;

        public Villager(ContentManager content, Texture2D texture, Vector2 position, float speed, GraphicsDevice graphicsDevice, int hp) : base(texture, position, (int)4.5f, 1)
        {
            this.position = position;
            this.graphicsDevice = graphicsDevice;
            this.texture = content.Load<Texture2D>("Sized/Villager_64");
            offScreenTimer = 5f;


        }

        // Get the bounding box of the Villager
        public new Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
            }
        }

        // Update the Villager's position and handle off-screen timer
        public override void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            position.X += Speed;
            if (Position.X < 0 || Position.X > graphicsDevice.Viewport.Width || Position.Y < 0 || Position.Y > graphicsDevice.Viewport.Height)
            {
                offScreenTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                offScreenTimer = 5f;
            }
        }

        // Check for arrow collisions
        public bool HandleCollisionWithArrow(Arrow arrow)
        {
            if (BoundingBox.Intersects(arrow.BoundingBox))
            {
                Hp -= arrow._damage;
                if (Hp <= 0)
                {
                    ToRemove = true;
                    return true;
                }
            }
            return false;
        }

        // Draw the Villager
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, position, Color.White);
        }
    }
}
