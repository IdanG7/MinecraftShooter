//Author: Idan Gurevich
//File Name: Game1.CS
//Project Name: GurevichI_PASS2
//Creation Date: March 25, 2023
//Modified Date: April 12, 2023
//This class represents the Skeleton enemy, which can move around and shoot arrows at the player. It handles collision detection with the player and arrows,
//keeps track of its health points, and updates its state during gameplay.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GurevichI_PASS2
{
    public class Skeleton : Mob
    {
        //Set const variables
        private const int SpiralRotations = 4;
        private const int ArrowDamage = 1;
        private const float RotationSpeed = 0.015f;
        private const float ArrowSpeed = 5f;
        private const float RateOfFireCooldown = 1.5f;

        //Movement and spiral variables
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
        public float maxRadius;

        //Arrow lists, position and arrow texture
        public List<Arrow> skeletonArrows;
        private Texture2D downArrowTexture;
        private Vector2 arrowPosition;

        //Timer
        private float rateOfFireTimer;

        private GraphicsDevice graphicsDevice;

        //Constructor
        public Skeleton(ContentManager content, Texture2D Texture, Vector2 position, float speed, GraphicsDevice graphicsDevice, int hp)
            : base(content.Load<Texture2D>("Sized/Skeleton_64"), position, (int)3, 4)
        {
            // Update the center position
            center = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);

            // Set the starting position to the right side of the screen
            position = new Vector2(graphicsDevice.Viewport.Width - 150 - Texture.Width * 2, position.Y);

            //Set bools for spiral and center
            reachedCenter = false;
            finishedSpiral = false;
            rotationCount = 0;

            //set directions
            directionX = 1;

            this.position = position;
            this.graphicsDevice = graphicsDevice;

            //Set the offscreentimer to 1
            offScreenTimer = 1;

            //Load the downarrow texture for the skeleton
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
            //Set max radius so it doesnt go off screen
            maxRadius = Math.Max(center.X, graphicsDevice.Viewport.Height / 2);

            if (!reachedCenter)
            {
                //set the y position to the skeleton speed
                position.Y += Speed;

                //when skeleton reaches center start spirling
                if (position.Y >= center.Y)
                {
                    reachedCenter = true;
                    angle = 0;
                    radius = 150;
                }
            }
            else if (!finishedSpiral)
            {
                //set the angle to the rotationspeed times the speed of the skeleton and set radius to 0.35 so that it completes in 4 spirals
                angle += RotationSpeed * Speed;
                radius -= 0.35f;

                //When it passes the middle point add 1 to the rotation count
                if (radius <= 0)
                {
                    rotationCount++;
                    radius = 0;
                }

                //Once there are 4 rotations finis the spiral
                if (rotationCount >= SpiralRotations)
                {
                    finishedSpiral = true;
                }

                //calculate the position of the skeelton while its in the path around the center point
                position.X = center.X + (float)(radius * Math.Cos(angle)) - Texture.Width * 0.5f;
                position.Y = center.Y + (float)(radius * Math.Sin(angle)) - Texture.Height * 0.5f;
            }
            else
            {
                //once done make it go right
                position.X += Speed * directionX;
                if (position.X <= 0)
                {
                    position.X = 0;
                    directionX = 1;
                }
                // if skeleton off screen start countdown
                if (position.X < 0 || position.X > graphicsDevice.Viewport.Width || position.Y < 0 || position.Y > graphicsDevice.Viewport.Height)
                {
                    offScreenTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    // Reset the timer if the skeleton is on the screen
                    offScreenTimer = 1;
                }
            }

            // Fire arrows
            rateOfFireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Check the skeleton firerate
            if (rateOfFireTimer >= RateOfFireCooldown)
            {
                //set the arrow position and object
                arrowPosition = new Vector2(position.X + Texture.Width / 2, position.Y);
                Arrow arrow = new Arrow(downArrowTexture, position, ArrowSpeed, 1, ArrowDamage);

                //add a arrow to the list
                skeletonArrows.Add(arrow);
                rateOfFireTimer = 0;
            }

            // Update arrows so that when the arrow is off screen remove it form list
            for (int i = skeletonArrows.Count - 1; i >= 0; i--)
            {
                skeletonArrows[i].Update(gameTime);
                if (skeletonArrows[i].position.Y < 0 || skeletonArrows[i].position.Y > graphicsDevice.Viewport.Height)
                {
                    skeletonArrows.RemoveAt(i);
                }
            }
        }

        //Pre: arrow, position, texture, and damage 
        //Post:After executing this subprogram, it will check whether the bounding box of the arrow intersects with the bounding box of the object. If there is a collision,
        //the Hp value of the object will be decreased by the damage value of the arrow. If the Hp value becomes less than or equal to 0,
        //the "ToRemove" bool of the object will be set to true. 
        //The new mob object will have its texture, position, speed, and health points set appropriately.
        //Desc: This method is used to handle collisions between an object and an arrow.
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

        //Pre: skeletonArrows, GetCenter() and GetRadius()
        //Post:After executing this code block, it will loop through all the arrows in the "skeletonArrows" list, and for each arrow, it will check if it intersects with the player object.
        //If there is an intersection, the arrow will be removed from the list, and the code block will return a Boolean value to indicate that a collision occurred.
        //Desc:This code block is used to handle collisions between the player and arrows fired by a skeleton. 
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