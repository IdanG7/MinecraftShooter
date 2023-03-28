using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    public class Skeleton : Mob
    {
        private const float Tolerance = 0.5f;
        private const float RotationSpeed = 0.025f;
        private const int SpiralRotations = 4;

        private int directionX;

        private Vector2 position;

        private Vector2 center;
        private float angle;
        private float radius;
        private bool reachedCenter;
        private bool finishedSpiral;
        private int rotationCount;

        private int _hp;

        private List<Arrow> arrows;
        private Texture2D arrowTexture;

        int arrowDamage = 1;

        private float rateOfFireTimer;

        private float arrowSpeed = 5f;

        private GraphicsDevice graphicsDevice;

        public Skeleton(ContentManager content, Texture2D Texture, Vector2 position, float speed, GraphicsDevice graphicsDevice, int hp)
            : base(content.Load<Texture2D>("Sized/Skeleton_64"), position, 1, hp)
        {
            center = new Vector2(position.X, position.Y + graphicsDevice.Viewport.Height / 2);

            radius = Vector2.Distance(position, center) * 0.75f;
            angle = (float)Math.Atan2(position.Y - center.Y, position.X - center.X);
            reachedCenter = false;
            finishedSpiral = false;
            rotationCount = 0;

            directionX = 1;

            this.position = position;
            this.graphicsDevice = graphicsDevice;

            this._hp = hp;

            arrowTexture = content.Load<Texture2D>("Sized/ArrowDown");

            // Initialize the arrows list
            arrows = new List<Arrow>();
        }

        public new Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
            }
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            if (!reachedCenter)
            {
                position.Y += Speed;

                if (Math.Abs(position.Y - center.Y) <= Tolerance)
                {
                    reachedCenter = true;
                }
            }
            else if (!finishedSpiral)
            {
                angle += RotationSpeed;
                position = center + radius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                if (angle >= MathHelper.TwoPi)
                {
                    angle -= MathHelper.TwoPi;
                    rotationCount++;
                }

                if (rotationCount >= SpiralRotations)
                {
                    finishedSpiral = true;
                }
            }
            else
            {
                position.X += Speed * directionX * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (position.X <= 0)
                {
                    position.X = 0;
                    directionX = 1;
                }
                else if (position.X + Texture.Width >= graphicsDevice.Viewport.Width)
                {
                    position.X = graphicsDevice.Viewport.Width - Texture.Width;
                    directionX = -1;
                }
            }

            rateOfFireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (rateOfFireTimer >= 1.5f)
            {
                Vector2 arrowPosition = new Vector2(position.X + Texture.Width / 2, position.Y);
                Arrow arrow = new Arrow(arrowTexture, position, arrowSpeed, 1, arrowDamage);

                arrows.Add(arrow);
                rateOfFireTimer = 0;
            }

            for (int i = arrows.Count - 1; i >= 0; i--)
            {
                arrows[i].Update(gameTime);
                if (arrows[i]._position.Y < 0 || arrows[i]._position.Y > graphicsDevice.Viewport.Height)
                {
                    arrows.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the skeleton
            spriteBatch.Draw(Texture, position, Color.White);

            // Draw the arrows
            foreach (Arrow arrow in arrows)
            {
                arrow.Draw(spriteBatch);
            }
        }
    }
}



