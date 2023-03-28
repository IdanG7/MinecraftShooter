using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    public class Skeleton : Mob
    {
        private const float Tolerance = 0.5f;
        private const float RotationSpeed = 0.05f;
        private const int SpiralRotations = 4;

        private Texture2D texture;
        private Vector2 position;
        private Vector2 _center;
        private float _angle;
        private float _radius;
        private bool _reachedCenter;
        private bool _finishedSpiral;
        private int _rotationCount;

        private GraphicsDevice graphicsDevice;

        public Skeleton(ContentManager content, Texture2D texture, Vector2 position, float speed, GraphicsDevice graphicsDevice)
            : base(texture, position, 100, 4)
        {
            _center = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            _radius = Vector2.Distance(position, _center) * 6.5f; // decrease radius by 25%
            ;
            _angle = (float)Math.Atan2(position.Y - _center.Y, position.X - _center.X);
            _reachedCenter = false;
            _finishedSpiral = false;
            _rotationCount = 0;

            this.position = position;
            this.graphicsDevice = graphicsDevice;
            this.texture = content.Load<Texture2D>("Sized/Skeleton_64");
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            if (!_reachedCenter)
            {
                position.Y += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (position.Y >= _center.Y && position.Y <= _center.Y)
                {
                    _reachedCenter = true;
                    _radius = Vector2.Distance(position, _center);
                }
            }
            else if (_reachedCenter == true)
            {
                _angle += RotationSpeed;
                _radius -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                position = _center + _radius * new Vector2((float)Math.Cos(_angle), (float)Math.Sin(_angle));

                if (_angle >= MathHelper.TwoPi)
                {
                    _angle -= MathHelper.TwoPi;
                    _rotationCount++;
                }

                if (_rotationCount >= SpiralRotations)
                {
                    _finishedSpiral = true;
                }
            }
            else
            {
                position.X += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }


}
