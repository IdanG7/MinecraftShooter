//Author: Idan Gurevich
//File Name: Arrow.cs
//Project Name: GurevichI_PASS2
//Creation Date: March 25, 2023
//Modified Date: April 12, 2023
//Description: Define the Arrow class which handles the behavior of the arrows fired by the player. The class
// contains methods for updating the arrow's position, checking for collisions with mobs, and
// removing the arrow if it has gone off-screen or hit a mob.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace GurevichI_PASS2
{
    // Define the Arrow class
    public class Arrow
    {
        // Declare class variables
        public Texture2D arrowTexture;
        public Vector2 position;
        public float speed;
        public int direction;
        public int damage;


        // Constructor
        public Arrow(Texture2D arrowTexture, Vector2 position, float arrowSpeed, int direction, int damage)
        {
            this.arrowTexture = arrowTexture;
            this.position = position;
            speed = arrowSpeed;
            this.direction = direction;
            this.damage = damage;
        }

        //Pre:arrowTexture and position
        //Post:After executing this code block, it will return a Vector2 object representing the center of the arrow object.
        //Desc: calculates the center point of the arrow object. 
        public Vector2 GetCenter()
        {
            return new Vector2(position.X + arrowTexture.Width / 2, position.Y + arrowTexture.Height / 2);
        }

        //Pre:arrowTexture
        //Post:After executing this code block, it will return a float value representing the radius of the arrow object.
        //Desc: calculates the radius of the arrow object.
        public float GetRadius()
        {
            return Math.Min(arrowTexture.Width, arrowTexture.Height) / 2;
        }

        // BoundingBox property
        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, arrowTexture.Width, arrowTexture.Height); }
        }

        // Update method
        public void Update(GameTime gameTime)
        {
            position.Y += speed * direction;
        }

        // Draw method
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(arrowTexture, position, Color.White);
        }
    }
}