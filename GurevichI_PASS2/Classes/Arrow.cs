using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace GurevichI_PASS2
{
    public class Arrow
    {
        public Texture2D _arrowTexture;
        public Vector2 _position;
        public float _speed;
        public int _direction;
        public int _damage;

        public Arrow(Texture2D arrowTexture, Vector2 position, float arrowSpeed, int direction, int damage)
        {
            _arrowTexture = arrowTexture;
            _position = position;
            _speed = arrowSpeed;
            _direction = direction;
            _damage = damage;

        }

        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, _arrowTexture.Width, _arrowTexture.Height); }
        }

        public void Update(GameTime gameTime)
        {
            _position.Y += _speed * _direction;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_arrowTexture, _position, Color.White);
        }
    }

}
