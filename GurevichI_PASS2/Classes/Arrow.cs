using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace GurevichI_PASS2
{
    public class Arrow
    {

        public Texture2D _arrowTexture { get; private set; }
        public Vector2 _position { get; set; }

        private float _speed;


        public Arrow(Texture2D arrowTexture, Vector2 position, float speed)
        {
            _arrowTexture = arrowTexture;
            _position = position;
            _speed = speed;
        }

        public void Update(GameTime gameTime)
        {
            Vector2 newPosition = _position;
            newPosition.Y -= _speed;
            _position = newPosition;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_arrowTexture, _position, Color.White);
        }
        public void SetPosition(Vector2 newPosition)
        {
            _position = newPosition;
        }

    }
}
