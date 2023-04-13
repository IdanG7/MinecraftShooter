// Author: Idan Gurevich
// File Name: Game1.cs
// Project Name: GurevichI_PASS2
// Creation Date: March 25, 2023
// Modified Date: April 12, 2023
// Description: This is the class which handles updating the mobs and supplying them with the needed variables, it is the parent class for all mobs.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    public class Mob
    {
        // Declare class variables
        public Texture2D Texture;
        public Vector2 Position;
        public float Speed;
        public int Hp;
        public int PointValue;
        public bool ToRemove { get; set; }
        public bool IsDead;
        private protected Rectangle rec;

        // Constructor
        public Mob(Texture2D texture, Vector2 position, int speed, int hp)
        {
            rec = new Rectangle(0, 0, 50, 50);
            Texture = texture;
            Position = position;
            Speed = speed;
            Hp = hp;
            IsDead = false;
            rec = new Rectangle(0, 0, 50, 50);
        }

        // Update method
        public virtual void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            rec.X = (int)Position.X;
            rec.Y = (int)Position.Y;
        }

        // Draw method
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        // BoundingBox property
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            }
        }

        //Pre: Arrow, graphicsDevice
        //Post: After executing this code block, it will always return true. It does not handle any collisions or modify any variables.
        //Desc: This code block is a virtual method called "HandleCollisionWithArrow", which is meant to be overridden by child classes that inherit from the base class that contains this method.
        //It takes in an "Arrow" object and a "GraphicsDevice" object as parameters,but the method does not handle any collisions or modify any variables. Instead, it returns a boolean value of true.
        public virtual bool HandleCollisionWithArrow(Arrow arrow, GraphicsDevice graphicsDevice)
        {
            return true;
        }
    }
}

