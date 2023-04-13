//Author: Idan Gurevich
//File Name: Enderman.cs
//Project Name: GurevichI_PASS2
//Creation Date: April 1, 2023
//Modified Date: April 11, 2023
//Description: A class representing the Enderman mob, which can teleport and inflict fear to the player on teleport.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    // Enderman class inherits from Mob class
    public class Enderman : Mob
    {
        private Vector2 position;
        private Vector2[] teleportPoints;
        private int currentPointIndex;
        private float teleportTimer;
        private const float TeleportDuration = 3f;
        public bool ToRemove;
        private Player player;
        public bool teleported = false;

        // Static teleport points shared by all Endermen
        private static Vector2[] sharedTeleportPoints;

        // Enderman constructor
        public Enderman(ContentManager content, Texture2D texture, Vector2 position, GraphicsDevice graphicsDevice, int hp, Player player) : base(content.Load<Texture2D>("Sized/Enderman_64"), position, 0, 5)
        {
            this.position = position;
            this.player = player;

            // Initialize sharedTeleportPoints if not already initialized
            if (sharedTeleportPoints == null)
            {
                sharedTeleportPoints = new Vector2[4];
                sharedTeleportPoints[0] = new Vector2(Game1.random.Next(0, graphicsDevice.Viewport.Width - texture.Width), 0);
                sharedTeleportPoints[1] = new Vector2(Game1.random.Next(0, graphicsDevice.Viewport.Width - texture.Width), Game1.random.Next(0, graphicsDevice.Viewport.Height - texture.Height - player.texture.Height));
                sharedTeleportPoints[2] = new Vector2(Game1.random.Next(0, graphicsDevice.Viewport.Width - texture.Width), Game1.random.Next(0, graphicsDevice.Viewport.Height - texture.Height - player.texture.Height));
                sharedTeleportPoints[3] = new Vector2(Game1.random.Next(0, graphicsDevice.Viewport.Width - texture.Width), Game1.random.Next(0, graphicsDevice.Viewport.Height - texture.Height - player.texture.Height));
            }

            // Scramble the order of the middle 3 points
            int[] indices = new int[] { 1, 2, 3 };
            ScrambleArray(indices);
            teleportPoints = new Vector2[5];
            teleportPoints[0] = sharedTeleportPoints[0];
            teleportPoints[1] = sharedTeleportPoints[indices[0]];
            teleportPoints[2] = sharedTeleportPoints[indices[1]];
            teleportPoints[3] = sharedTeleportPoints[indices[2]];

            currentPointIndex = 0;
            teleportTimer = TeleportDuration;
        }

        // Scramble an integer array
        private void ScrambleArray(int[] array)
        {
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int j = Game1.random.Next(i + 1);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        // Get the bounding box of the Enderman
        public new Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
            }
        }


        public override void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            // Update the teleport timer
            teleportTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Teleport the Enderman if the timer reaches 0
            if (teleportTimer <= 0)
            {
                teleportTimer = TeleportDuration;
                currentPointIndex++;

                // Teleport to the player's position if reached the last point
                if (currentPointIndex == 4)
                {
                    teleportPoints[4] = playerPosition - new Vector2(0, Texture.Height);
                }

                if (currentPointIndex < teleportPoints.Length)
                {
                    position = teleportPoints[currentPointIndex];
                    if (!teleported)
                    {
                        player.InduceFear();
                        Game1.endermanTeleport.CreateInstance().Play();
                        teleported = true;
                    }
                }
                else
                {
                    ToRemove = true;
                     Game1.endermanScream.CreateInstance().Play();
                }
            }
            else
            {
                teleported = false;
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
            if (BoundingBox.Intersects(arrow.BoundingBox))
            {
                Hp -= arrow.damage;

                if (Hp <= 0)
                {
                    ToRemove = true;
                }
                return true;
            }

            return false;
        }

        // Draw the Enderman
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, position, Color.White);
        }
    }
}