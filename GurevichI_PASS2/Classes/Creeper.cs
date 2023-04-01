using System.Collections.Generic;
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
        private Texture2D explosionTexture;
        private Vector2 explosionPosition;

        public bool Exploded { get; private set; }
        public bool ToRemove { get; private set; }
        private Vector2 DeathPosition;

        private float explosionDuration = 0.5f;
        private float explosionTimer = 0f;

        public Creeper(ContentManager content, Texture2D texture, Vector2 position, Vector2 playerPosition, float speed, GraphicsDevice graphicsDevice, Texture2D explosionTexture, int hp)
            : base(texture, position, (int)3f, 3)
        {
            PointValue = 40;
            this.position = position;
            this.playerPosition = playerPosition;
            this.graphicsDevice = graphicsDevice;
            this.texture = content.Load<Texture2D>("Sized/Creeper_64");
            this.explosionTexture = explosionTexture;
            explosionPosition = new Vector2(position.X, position.Y);

            Exploded = false;
            ToRemove = false;
        }

        public new Rectangle BoundingBox => new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

        public override void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            if (!Exploded)
            {
                Vector2 direction = playerPosition - position;
                direction.Normalize();

                position += direction * Speed;
                explosionPosition = position;

                if (position.Y + texture.Height >= graphicsDevice.Viewport.Height)
                {
                    Exploded = true;
                    DeathPosition = position;
                }
            }
            else
            {
                explosionPosition = DeathPosition;
                explosionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (explosionTimer >= explosionDuration)
                {
                    ToRemove = true;
                }
            }
        }

        public bool HandleCollisionWithArrow(Arrow arrow, GraphicsDevice graphicsDevice, List<Rectangle> grass1Rectangles, List<Rectangle> grass2Rectangles, List<Rectangle> cobblestoneRectangles, List<Rectangle> dirtRectangles, Texture2D dirtTexture)
        {
            if (arrow.BoundingBox.Intersects(BoundingBox) && !Exploded)
            {
                Hp -= arrow._damage;

                if (Hp <= 0)
                {
                    DeathPosition = position;
                    Exploded = true;

                    int explosionRadius = 100;

                    for (int g1 = grass1Rectangles.Count - 1; g1 >= 0; g1--)
                    {
                        if (Vector2.Distance(DeathPosition, new Vector2(grass1Rectangles[g1].X, grass1Rectangles[g1].Y)) <= explosionRadius)
                        {
                            dirtRectangles.Add(new Rectangle(grass1Rectangles[g1].X, grass1Rectangles[g1].Y, dirtTexture.Width, dirtTexture.Height));
                            grass1Rectangles.RemoveAt(g1);
                        }
                    }

                    for (int g2 = grass2Rectangles.Count - 1; g2 >= 0; g2--)
                    {
                        if (Vector2.Distance(DeathPosition, new Vector2(grass2Rectangles[g2].X, grass2Rectangles[g2].Y)) <= explosionRadius)
                        {
                            dirtRectangles.Add(new Rectangle(grass2Rectangles[g2].X, grass2Rectangles[g2].Y, dirtTexture.Width, dirtTexture.Height));
                            grass2Rectangles.RemoveAt(g2);
                        }
                    }
                    for (int c = cobblestoneRectangles.Count - 1; c >= 0; c--)
                    {
                        if (Vector2.Distance(DeathPosition, new Vector2(cobblestoneRectangles[c].X, cobblestoneRectangles[c].Y)) <= explosionRadius)
                        {
                            dirtRectangles.Add(new Rectangle(cobblestoneRectangles[c].X, cobblestoneRectangles[c].Y, dirtTexture.Width, dirtTexture.Height));
                            cobblestoneRectangles.RemoveAt(c);
                        }
                    }
                }

                return true; // Return true to indicate that a collision occurred and was handled
            }

            return false; // Return false to indicate no collision or handling
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Exploded)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
            else if (!ToRemove)
            {
                // Draw explosion texture
                spriteBatch.Draw(explosionTexture, explosionPosition, Color.White);
            }
        }
    }
}

