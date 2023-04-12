using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
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

        public Enderman(ContentManager content, Texture2D texture, Vector2 position, GraphicsDevice graphicsDevice, int hp, Player player) : base(content.Load<Texture2D>("Sized/Enderman_64"), position, 0, 5)
        {
            this.position = position;
            this.player = player;

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

        public new Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
            }
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice)
        {
            teleportTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (teleportTimer <= 0)
            {
                teleportTimer = TeleportDuration;
                currentPointIndex++;

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
                        teleported = true;
                    }
                }
                else
                {
                    ToRemove = true;
                }
            }
            else
            {
                teleported = false;
            }
        }


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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, position, Color.White);
        }


    }
}
