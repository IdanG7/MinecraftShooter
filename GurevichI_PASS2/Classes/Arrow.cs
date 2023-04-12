using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace GurevichI_PASS2
{
    public class Arrow
    {
        public Texture2D arrowTexture;
        public Vector2 position;
        public float speed;
        public int direction;
        public int damage;

        public Arrow(Texture2D arrowTexture, Vector2 position, float arrowSpeed, int direction, int damage)
        {
            this.arrowTexture = arrowTexture;
            this.position = position;
            speed = arrowSpeed;
            this.direction = direction;
            this.damage = damage;


        }

        public Vector2 GetCenter()
        {
            return new Vector2(position.X + arrowTexture.Width / 2, position.Y + arrowTexture.Height / 2);
        }

        public float GetRadius()
        {
            return Math.Min(arrowTexture.Width, arrowTexture.Height) / 2;
        }


        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, arrowTexture.Width, arrowTexture.Height); }
        }

        public void Update(GameTime gameTime)
        {
            position.Y += speed * direction;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(arrowTexture, position, Color.White);
        }
    }

}
