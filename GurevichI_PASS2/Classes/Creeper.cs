//Author: Idan Gurevich
//File Name: Creeper.cs
//Project Name: GurevichI_PASS2
//Creation Date: March 25, 2023
//Modified Date: April 12, 2023
//Description: Create a class representing the Creeper enemy, including its properties, behaviors, and collision detection with arrows.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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
        public int explosionRadius = 100;

        public bool Exploded { get; private set; }
        public bool ToRemove { get; private set; }
        private Vector2 DeathPosition;

        private float explosionDuration = 0.5f;
        private float explosionTimer = 0f;

        public Creeper(ContentManager content, Texture2D texture, Vector2 position, Vector2 playerPosition, float speed, GraphicsDevice graphicsDevice, Texture2D explosionTexture, int hp)
            : base(texture, position, (int)3f, 3)
        {

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

        public Vector2 GetCenter()
        {
            return new Vector2(position.X + Texture.Width / 2, position.Y + Texture.Height / 2);
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            if (!Exploded)
            {
                // Calculate the direction to the player and normalize it
                Vector2 direction = playerPosition - position;
                direction.Normalize();

                // Move the creeper in the direction of the player at the speed of the creeper
                position += direction * Speed;

                // Set the explosion position to the current position of the creeper
                explosionPosition = position;

                // Check if the creeper has reached the bottom of the screen
                if (position.Y + texture.Height >= graphicsDevice.Viewport.Height)
                {
                    // If so, set the creeper to exploded and set the death position
                    Exploded = true;
                    DeathPosition = position;
                }
            }
            else
            {
                // Update the explosion position to the death position of the creeper
                explosionPosition = DeathPosition;

                // Update the explosion timer
                explosionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // If the explosion timer has reached the explosion duration, set the creeper to be removed
                if (explosionTimer >= explosionDuration)
                {
                    ToRemove = true;
                }
            }
        }


        //Pre: arrow, position, texture, and damage 
        //Post:After executing this subprogram, it will check whether the bounding box of the arrow intersects with the bounding box of the object. If there is a collision,
        //the Hp value of the object will be decreased by the damage value of the arrow. If the Hp value becomes less than or equal to 0,
        //the "ToRemove" bool of the object will be set to true. 
        //The new mob object will have its texture, position, speed, and health points set appropriately.
        //Desc: This method is used to handle collisions between an object and an arrow.
        public bool HandleCollisionWithArrow(Arrow arrow, GraphicsDevice graphicsDevice, List<Rectangle> grass1Rectangles, List<Rectangle> grass2Rectangles, List<Rectangle> cobblestoneRectangles, List<Rectangle> dirtRectangles, Texture2D dirtTexture)
        {
            if (arrow.BoundingBox.Intersects(BoundingBox) && !Exploded)
            {
                //If the above check is true, this line decreases the health points (Hp) of the Creeper by the damage value of the arrow.
                Hp -= arrow.damage;

                //if the Creeper's health points are less than or equal to 0, these lines set the DeathPosition of the Creeper to its current position,

                if (Hp <= 0)
                {
                    DeathPosition = position;
                    Exploded = true;

                    // loop through all the grass1Rectangles,grass2Rectangles and cobblestoneRectangle and check if their distance from the DeathPosition of the Creeper is less than or equal to the explosion radius.
                    // If it is, then a dirt rectangle is created and added to the dirtRectangles list, and the corresponding grass1Rectangle is removed.
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
                // Return true to indicate that a collision occurred and was handled
                return true;
            }
            // Return false to indicate no collision or handling
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Exploded)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
            else if (!ToRemove)
            {
                // Create a destination rectangle with the same size as the Creeper texture
                Rectangle destinationRect = new Rectangle((int)explosionPosition.X, (int)explosionPosition.Y, (int)(texture.Width * 1.5f), (int)(texture.Height * 1.5f));

                // Draw the explosion texture scaled to the destination rectangle
                spriteBatch.Draw(explosionTexture, destinationRect, Color.White);
            }
        }
    }
}

