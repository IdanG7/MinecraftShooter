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
        private const float RotationSpeed = 0.015f;
        private const int SpiralRotations = 4;

        private int directionX;

        private Vector2 position;

        private Vector2 center;
        private double angle;
        private double radius;
        private bool reachedCenter;
        private bool finishedSpiral;
        private int rotationCount;
        public bool ToRemove;

        private List<Arrow> arrows;
        private Texture2D arrowTexture;

        int arrowDamage = 1;

        private float rateOfFireTimer;

        private float arrowSpeed = 5f;

        private GraphicsDevice graphicsDevice;

        public Skeleton(ContentManager content, Texture2D Texture, Vector2 position, float speed, GraphicsDevice graphicsDevice, int hp)
            : base(content.Load<Texture2D>("Sized/Skeleton_64"), position, 2, 4)
        {
            // Update the center position
            center = new Vector2(graphicsDevice.Viewport.Width / 2, position.Y + graphicsDevice.Viewport.Height / 2);

            // Set the starting position to the right side of the screen
            position = new Vector2(graphicsDevice.Viewport.Width - Texture.Width * 2, position.Y);

            radius = 100;
            angle = Math.PI * 0.5;

            reachedCenter = false;
            finishedSpiral = false;
            rotationCount = 0;

            directionX = 1;

            this.position = position;
            this.graphicsDevice = graphicsDevice;

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
            float maxRadius = Math.Min(center.X, graphicsDevice.Viewport.Height / 2) * 0.8f;

            if (!reachedCenter)
            {
                position.Y += Speed;

                if (Math.Abs(position.Y - center.Y) <= Tolerance)
                {
                    reachedCenter = true;
                }

                float dx = position.X - center.X;
                float dy = position.Y - center.Y;
                angle = (float)Math.Atan2(dy, dx); // Calculate the initial angle based on the current position
                radius = Math.Min((float)Math.Sqrt(dx * dx + dy * dy), maxRadius); // Calculate the initial radius based on the current position and limit it to maxRadius
            }
            else if (!finishedSpiral)
            {
                angle += 0.05;
                radius -= maxRadius / (4 * (float)Math.PI / 0.05f); // Subtract a fixed amount from radius so it finishes in 4 spirals

                if (radius <= 0)
                {
                    rotationCount++;
                    radius = 0;
                }

                if (rotationCount >= SpiralRotations)
                {
                    finishedSpiral = true;
                }

                position.X = center.X + (float)(radius * Math.Cos(angle)) - Texture.Width * 0.5f;
                position.Y = center.Y + (float)(radius * Math.Sin(angle)) - Texture.Height * 0.5f;
            }
            else
            {
                position.X += Speed * directionX;

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

        public bool HandleCollisionWithArrow(Arrow arrow)
        {
            if (arrow.BoundingBox.Intersects(BoundingBox) && !ToRemove)
            {
                // Handle the collision
                Hp -= arrow._damage;

                if (Hp <= 0)
                {
                    ToRemove = true;
                }

                return true; // Return true to indicate that a collision occurred and was handled
            }

            return false; // Return false to indicate no collision or handling
        }

        public override void Draw(SpriteBatch spriteBatch)
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

