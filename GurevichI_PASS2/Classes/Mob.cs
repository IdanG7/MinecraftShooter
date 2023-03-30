using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    public class Mob
    {
        public Texture2D Texture;
        public Vector2 Position;
        public int Speed;
        public int Hp;
        public int PointValue;
        public bool IsDead;

        public Mob(Texture2D texture, Vector2 position, int speed, int hp)
        {
            Texture = texture;
            Position = position;
            Speed = speed;
            Hp = hp;
            IsDead = false;
        }

        public virtual void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {

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

    }
}
