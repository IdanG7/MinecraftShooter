﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    public class Creeper : Mob
    {
        public Texture2D texture;
        public Vector2 position;
        private Vector2 playerPosition;
        private GraphicsDevice graphicsDevice;
        public Texture2D explosionTexture;
        public Vector2 explosionPosition;

        private int _hp;

        public bool Exploded { get; set; }
        public bool ToRemove { get; set; }
        public Vector2 DeathPosition { get; set; }


        private float explosionDuration = 0.5f; // Duration of the explosion in seconds
        private float explosionTimer = 0f;

        public Creeper(ContentManager content, Texture2D texture, Vector2 position, Vector2 playerPosition, float speed, GraphicsDevice graphicsDevice, Texture2D explosionTexture, int hp)
    : base(texture, position, 2, hp)
        {
            PointValue = 40;
            this.position = position;
            this.playerPosition = playerPosition;
            this.graphicsDevice = graphicsDevice;
            this.texture = content.Load<Texture2D>("Sized/Creeper_64");
            this.explosionTexture = explosionTexture;
            this._hp = hp;
            explosionPosition = new Vector2(position.X, position.Y);

            Exploded = false;
            ToRemove = false;
        }

        public new int Hp
        {
            get { return _hp; }
            set { _hp = value; }
        }



        public new Rectangle BoundingBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }
        }



        public override void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            if (!Exploded)
            {
                // Calculate the direction vector towards the player
                Vector2 direction = playerPosition - position;
                direction.Normalize();

                // Move the creeper towards the player at the given speed
                position += direction * Speed;
                explosionPosition = position;

                // Check if the Creeper reached the bottom of the screen
                if (position.Y + texture.Height >= graphicsDevice.Viewport.Height)
                {
                    Exploded = true;
                }
            }
            else
            {
                // Update the explosion timer
                explosionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Check if the explosion duration has passed
                if (explosionTimer >= explosionDuration)
                {
                    ToRemove = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Exploded)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
            else if (!ToRemove)
            {
                // Draw explosion texture
                spriteBatch.Draw(explosionTexture, explosionPosition, Color.White);
            }
        }




    }
}
