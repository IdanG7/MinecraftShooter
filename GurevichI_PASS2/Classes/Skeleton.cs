using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    public class Skeleton : Mob
    {
        private const float Tolerance = 0f;
        private const float RotationSpeed = 0.015f;
        private const int SpiralRotations = 4;

        private const int ArrowDamage = 1;
        private const float ArrowSpeed = 5f;
        private const float RateOfFireCooldown = 1.5f;

        private int directionX;

        private Vector2 position;

        private Vector2 center;
        private double angle;
        private double radius;
        private bool reachedCenter;
        private bool finishedSpiral;
        private int rotationCount;
        public bool ToRemove;
        public float offScreenTimer;


        public List<Arrow> skeletonArrows;
        private Texture2D downArrowTexture;

        private float rateOfFireTimer;

        private GraphicsDevice graphicsDevice;

        public Skeleton(ContentManager content, Texture2D Texture, Vector2 position, float speed, GraphicsDevice graphicsDevice, int hp)
            : base(content.Load<Texture2D>("Sized/Skeleton_64"), position, (int)2f, 4)
        {
            // Update the center position

            center = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);

            // Set the starting position to the right side of the screen
            position = new Vector2(graphicsDevice.Viewport.Width - 150 - Texture.Width * 2, position.Y);


            reachedCenter = false;
            finishedSpiral = false;
            rotationCount = 0;

            directionX = 1;

            this.position = position;
            this.graphicsDevice = graphicsDevice;

            offScreenTimer = 1;

            downArrowTexture = content.Load<Texture2D>("Sized/ArrowDown");

            // Initialize the arrows list
            skeletonArrows = new List<Arrow>();
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
            float maxRadius = Math.Max(center.X, graphicsDevice.Viewport.Height / 2);

            if (!reachedCenter)
            {
                position.Y += Speed;

                if (Math.Abs(position.Y - center.Y) <= Tolerance)
                {
                    reachedCenter = true;
                }


                angle = 0;
                radius = 150;
            }
            else if (!finishedSpiral)
            {
                angle += 0.05;
                radius -= 0.35;

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
                // Update offScreenTimer
                if (position.X < 0 || position.X > graphicsDevice.Viewport.Width || position.Y < 0 || position.Y > graphicsDevice.Viewport.Height)
                {
                    offScreenTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    offScreenTimer = 1; // Reset the timer if the Pillager is on the screen
                }
            }


            rateOfFireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (rateOfFireTimer >= RateOfFireCooldown)
            {
                Vector2 arrowPosition = new Vector2(position.X + Texture.Width / 2, position.Y);
                Arrow arrow = new Arrow(downArrowTexture, position, ArrowSpeed, 1, ArrowDamage);

                skeletonArrows.Add(arrow);
                rateOfFireTimer = 0;
            }

            for (int i = skeletonArrows.Count - 1; i >= 0; i--)
            {
                skeletonArrows[i].Update(gameTime);
                if (skeletonArrows[i].position.Y < 0 || skeletonArrows[i].position.Y > graphicsDevice.Viewport.Height)
                {
                    skeletonArrows.RemoveAt(i);
                }
            }
        }

        public bool HandleCollisionWithArrow(Arrow arrow)
        {
            if (arrow.BoundingBox.Intersects(BoundingBox) && !ToRemove)
            {
                // Handle the collision
                Hp -= arrow.damage;

                if (Hp <= 0)
                {
                    ToRemove = true;
                }

                return true; // Return true to indicate that a collision occurred and was handled
            }

            return false; // Return false to indicate no collision or handling
        }

        public bool CheckPlayerCollisionWithSkeletonArrows(Player player)
        {
            for (int i = skeletonArrows.Count - 1; i >= 0; i--)
            {
                Vector2 arrowCenter = skeletonArrows[i].GetCenter();
                Vector2 playerCenter = player.GetCenter();
                float combinedRadius = skeletonArrows[i].GetRadius() + player.GetRadius();

                if (Vector2.DistanceSquared(arrowCenter, playerCenter) <= combinedRadius * combinedRadius)
                {
                    // Remove the arrow that collided with the player
                    skeletonArrows.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the skeleton
            spriteBatch.Draw(Texture, position, Color.White);

            // Draw the arrows
            foreach (Arrow arrow in skeletonArrows)
            {
                arrow.Draw(spriteBatch);
            }
        }
    }
}