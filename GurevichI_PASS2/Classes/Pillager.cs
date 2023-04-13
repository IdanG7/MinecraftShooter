// Author: Idan Gurevich
// File Name: Game1.cs
// Project Name: GurevichI_PASS2
// Creation Date: March 25, 2023
// Modified Date: April 12, 2023
// Description: This class represents the Pillager enemy in the game. It inherits from the Mob class and
// implements its own unique behavior and attributes. It has a health point value, damage value, // and speed value, which are used to determine how the Pillager interacts with the game

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GurevichI_PASS2
{
    // Define the Pillager class, inheriting from Mob
    public class Pillager : Mob
    {
        // Declare a timer for when the Pillager is off the screen
        public float offScreenTimer;
        // Declare a variable for the Pillager's position
        private Vector2 position;
        // Declare a bool indicating whether the Pillager's shield is active
        private bool shieldActive;
        // Declare a variable for the shield texture
        Texture2D shieldTexture;

        // Declare other variables used in the class
        int baseline;
        const int SIN_AMP = 100;

        // Pillager constructor
        public Pillager(ContentManager Content, Texture2D texture, Vector2 position, float speed, GraphicsDevice graphicsDevice, int hp) : base(Content.Load<Texture2D>("Sized/Pillager_64"), position, 2, 2)
        {
            // Load the shield texture
            shieldTexture = Content.Load<Texture2D>("Sized/Shield_48");

            // Initialize the Pillager's properties
            this.position = position;
            shieldActive = true;

            // Set the baseline for the Pillager's vertical movement
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

            // Set the initial position of the Pillager's bounding rectangle
            rec.X = (int)position.X;
            rec.Y = (int)position.Y;

            // Initialize the off-screen timer
            offScreenTimer = 1;
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            // Move horizontally to the right
            position.X += Speed;
            // Update the vertical position based on a sine function
            position.Y = baseline + (int)(SIN_AMP * Math.Sin(0.03 * position.X));

            // Update the off-screen timer
            if (position.X < 0 || position.X > graphicsDevice.Viewport.Width || position.Y < 0 || position.Y > graphicsDevice.Viewport.Height)
            {
                offScreenTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                offScreenTimer = 1; // Reset the timer if the Pillager is on the screen
            }
        }

        //Pre: arrow, position, texture, and damage 
        //Post:After executing this subprogram, it will check whether the bounding box of the arrow intersects with the bounding box of the object. If there is a collision,
        //the Hp value of the object will be decreased by the damage value of the arrow. If the Hp value becomes less than or equal to 0,
        //the "ToRemove" bool of the object will be set to true. 
        //The new mob object will have its texture, position, speed, and health points set appropriately.
        //Desc: This method is used to handle collisions between an object and an arrow.
        public bool HandleCollisionWithArrow(Arrow arrow)
        {
            if (BoundingBox.Intersects(arrow.BoundingBox))
            {

                if (shieldActive) // If the shield is active, play the shield hit sound and deactivate the shield
                {
                    Game1.shieldHit.CreateInstance().Play();
                    shieldActive = false;
                }
                else // If the shield is not active, reduce the Pillager's HP
                {
                    Hp -= arrow.damage;

                    if (Hp <= 0) // If the Pillager's HP is 0 or below, mark it for removal
                    {
                        ToRemove = true;
                    }
                }
                return true;
            }

            return false;
        }

        // Define the bounding box for the Pillager
        public new Rectangle BoundingBox
        {
            get
            {
                // Return a rectangle representing the Pillager's bounding box
                return new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
            }
        }

        // Draw the Pillager and its shield (if active) to the screen
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the Pillager's texture at its current position
            spriteBatch.Draw(Texture, position, Color.White);

            // If the shield is active, draw the shield texture at the Pillager's position
            if (shieldActive)
            {
                spriteBatch.Draw(shieldTexture, position, Color.White);
            }
        }
    }
}