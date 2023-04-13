//Author: Idan Gurevich
//File Name: Game1.CS
//Project Name: GurevichI_PASS2
//Creation Date: March 25, 2023
//Modified Date: April 12, 2023
//Description: This class represents the player object and handles the player's movement and collision

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GurevichI_PASS2
{
    // Define Player class
    public class Player
    {
        // Declare class variables
        public Texture2D texture;
        public Vector2 position;
        public float Speed = 3f;
        public float fearTimer;
        public const float FearDuration = 0.5f;

        public Player(Texture2D texture, Vector2 position, float speed)
        {
            this.texture = texture;
            this.position = position;
            this.Speed = speed;
            fearTimer = 0f;

            // Initialize properties for the shop functionality
            Speed = speed;
        }

        // Handle collision with Skeleton arrows
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

        // BoundingBox property
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            }
        }

        //Pre:texture and position
        //Post:After executing this code block, it will return a Vector2 object representing the center of the player object.
        //Desc: calculates the center point of the player object. 
        public Vector2 GetCenter()
        {
            return new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
        }

        //Pre:Texture
        //Post:After executing this code block, it will return a float value representing the radius of the player object.
        //Desc: calculates the radius of the player object.
        public float GetRadius()
        {
            return Math.Min(texture.Width, texture.Height) / 2;
        }

        public void Update(KeyboardState keyboardState, GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            //Checks if the fearTimer is greater than 0. If it is, the timer is decreased by the elapsed game time and the method returns without continuing with the code below.
            if (fearTimer > 0)
            {
                fearTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }
            //Creates a new Vector2 called newPosition with the same value as the position variable.
            Vector2 newPosition = position;

            //Checks the state of the Left and Right keys of the keyboard. If Left is pressed, the X value of newPosition is decreased by Speed. If Right is pressed, the X value is increased by Speed.
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                newPosition.X -= Speed; 
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                newPosition.X += Speed;
            }

            //Clamps the X value of newPosition to be within the range of 0 to graphicsDevice.Viewport.Width - texture.Width. This ensures that the object stays within the bounds of the game screen.
            newPosition.X = MathHelper.Clamp(newPosition.X, 0, graphicsDevice.Viewport.Width - texture.Width);
            position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        //Pre:FearDuration
        //Post:Boolean Value (True)
        //Desc: sets the "fearTimer" variable to the value of "FearDuration" and returns true. The purpose of this method is to induce fear in a character in the game. 
        public bool InduceFear()
        {
            fearTimer = FearDuration;
            return true;
        }
    }
}


