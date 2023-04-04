using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    public class Mob
    {
        public Texture2D Texture;
        public Vector2 Position;
        public float Speed;
        public int Hp;
        public int PointValue;
        public bool ToRemove { get; set; }
        public bool IsDead;
        private protected Rectangle rec;

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

        public virtual void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            rec.X = (int)Position.X;
            rec.Y = (int)Position.Y;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            }
        }

        public virtual bool HandleCollisionWithArrow(Arrow arrow, GraphicsDevice graphicsDevice)
        {
            return true;
        }

    }
}
