using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    public abstract class Mob
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public int Speed { get; set; }
        public int Hp { get; set; }
        public int PointValue { get; set; }
        public bool IsDead { get; set; }

        public Mob(Texture2D texture, Vector2 position, int speed, int hp)
        {
            Texture = texture;
            Position = position;
            Speed = speed;
            Hp = hp;
            IsDead = false;
        }

        public abstract void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice);

        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
