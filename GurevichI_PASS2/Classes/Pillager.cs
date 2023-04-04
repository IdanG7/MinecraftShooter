using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    public class Pillager : Mob
    {
        public float offScreenTimer;
        private Vector2 position;
        private bool shieldActive;
        public bool ToRemove;

        int baseline;
        const int SIN_AMP = 100;



        public Pillager(ContentManager content, Texture2D texture, Vector2 position, float speed, GraphicsDevice graphicsDevice, int hp) : base(content.Load<Texture2D>("Sized/Pillager_64"), position, 2, 2)
        {
            this.position = position;
            shieldActive = true;

            baseline = Game1.random.Next(SIN_AMP, graphicsDevice.Viewport.Height - SIN_AMP - rec.Height * 2);
            position.Y = baseline;

            // Spawn the Pillager offscreen to the left or right based on the random condition
            if (Game1.random.Next(0, 100) < 50)
            {
                position.X = -rec.Width;
            }
            else
            {
                position.X = graphicsDevice.Viewport.Width;
            }

            rec.X = (int)position.X;
            rec.Y = (int)position.Y;

            offScreenTimer = 1;
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            // Move horizontally to the right
            position.X += Speed;
            position.Y = baseline + (int)(SIN_AMP * Math.Sin(0.03 * position.X));

            // Update offScreenTimer
            if (position.X < 0 || position.X > graphicsDevice.Viewport.Width || position.Y < 0 || position.Y > graphicsDevice.Viewport.Height)
            {
                offScreenTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                offScreenTimer = 1; // Reset the timer if the Pillager is on the screen
            }
        }

        public bool HandleCollisionWithArrow(Arrow arrow)
        {
            if (BoundingBox.Intersects(arrow.BoundingBox))
            {
                if (shieldActive)
                {
                    shieldActive = false;
                }
                else
                {
                    Hp -= arrow._damage;

                    if (Hp <= 0)
                    {
                        ToRemove = true;
                    }
                }
                return true;
            }

            return false;
        }

        public new Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
            }
        }

        public void Draw(SpriteBatch spriteBatch, ContentManager Content)
        {
            spriteBatch.Draw(Texture, position, Color.White);

            if (shieldActive)
            {
                // Assuming you have a "Sized/Shield_64" texture
                spriteBatch.Draw(Content.Load<Texture2D>("Sized/Shield_48"), position, Color.White);
            }
        }
    }
}
