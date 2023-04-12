using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GurevichI_PASS2
{
    public class Level
    {
        public int currentLevel = 1;

        public float SpawnTime { get { return spawnTime; } }
        public int MaxMobsOnScreen { get { return maxMobsOnScreen; } }
        public int TotalMobsInCurrentLevel { get { return totalMobsInCurrentLevel; } }

        private bool levelInitialized;
        private int totalMobsInCurrentLevel;
        private int maxMobsOnScreen;
        private float spawnTime;
        private ContentManager content;
        private GraphicsDevice graphicsDevice;
        private Player player;

        // Add textures
        private Texture2D villagerTexture;
        private Texture2D creeperTexture;
        private Texture2D skeletonTexture;
        private Texture2D pillagerTexture;
        private Texture2D endermanTexture;
        private Texture2D explodeTexture;

        // Add positions
        private Vector2 villagerPosition;
        private Vector2 creeperPosition;
        private Vector2 skeletonPosition;
        private Vector2 pillagerPosition;
        private Vector2 endermanPosition;

        // Add speed values
        private float villagerSpeed;
        private float creeperSpeed;
        private float skeletonSpeed;
        private float endermanSpeed;

        // Add health values
        private int villagerHp;
        private int creeperHp;
        private int skeletonHp;
        private int pillagerHp;
        private int endermanHp;

        public Level(ContentManager content, GraphicsDevice graphicsDevice, Player player)
        {
            this.content = content;
            this.graphicsDevice = graphicsDevice;
            this.player = player;

            // Load textures
            villagerTexture = content.Load<Texture2D>("Sized/Villager_64");
            creeperTexture = content.Load<Texture2D>("Sized/Creeper_64");
            skeletonTexture = content.Load<Texture2D>("Sized/Skeleton_64");
            pillagerTexture = content.Load<Texture2D>("Sized/Pillager_64");
            endermanTexture = content.Load<Texture2D>("SIzed/Enderman_64");
            explodeTexture = content.Load<Texture2D>("Sized/Explode_200");


            // Initialize level settings
            UpdateLevel();
        }

        public void UpdateLevel()
        {
            UpdateLevelSettings(currentLevel);
        }

        public void UpdateLevelSettings(int newLevel)
        {
            currentLevel = newLevel; // Add this line to update the current level
            levelInitialized = false;


            switch (currentLevel)
            {
                case 1:
                    totalMobsInCurrentLevel = 10;
                    maxMobsOnScreen = 1;
                    spawnTime = 2f;
                    break;
                case 2:
                    totalMobsInCurrentLevel = 15;
                    maxMobsOnScreen = 3;
                    spawnTime = 1.7f;
                    break;
                case 3:
                    totalMobsInCurrentLevel = 20;
                    maxMobsOnScreen = 3;
                    spawnTime = 1.3f;
                    break;
                case 4:
                    totalMobsInCurrentLevel = 25;
                    maxMobsOnScreen = 5;
                    spawnTime = 1.2f;
                    break;
                case 5:
                    totalMobsInCurrentLevel = 30;
                    maxMobsOnScreen = 3;
                    spawnTime = 1f;
                    break;
            }
        }

        public Mob CreateMobBasedOnLevel(int currentLevel, int randomValue, GraphicsDevice graphicsDevice)
        {
            Mob newMob = null;

            switch (currentLevel)
            {
                case 1:
                    /*  if (randomValue <= 70)
                      {

                          villagerPosition = new Vector2(-villagerTexture.Width, Game1.random.Next(0, graphicsDevice.Viewport.Height - villagerTexture.Height - player.texture.Height));
                          newMob = new Villager(content, villagerTexture, villagerPosition, villagerSpeed, graphicsDevice, villagerHp);
                      }
                      else if (randomValue <= 90)
                      {

                          creeperPosition = new Vector2(Game1.random.Next(0, graphicsDevice.Viewport.Width - creeperTexture.Width), -creeperTexture.Height);
                          newMob = new Creeper(content, creeperTexture, creeperPosition, player.Position, creeperSpeed, graphicsDevice, explodeTexture, creeperHp);
                      }*/
                    if (randomValue <= 100)
                    {
                        skeletonPosition = new Vector2(graphicsDevice.Viewport.Width / 2 - skeletonTexture.Width / 2, -skeletonTexture.Height);
                        newMob = new Skeleton(content, skeletonTexture, skeletonPosition, skeletonSpeed, graphicsDevice, skeletonHp);
                    }
                    break;

                case 2:
                    if (randomValue <= 50)
                    {
                        villagerPosition = new Vector2(-villagerTexture.Width, Game1.random.Next(0, graphicsDevice.Viewport.Height - villagerTexture.Height - player.texture.Height));
                        newMob = new Villager(content, villagerTexture, villagerPosition, villagerSpeed, graphicsDevice, villagerHp);
                    }
                    else if (randomValue <= 80)
                    {
                        creeperPosition = new Vector2(Game1.random.Next(0, graphicsDevice.Viewport.Width - creeperTexture.Width), -creeperTexture.Height);
                        newMob = new Creeper(content, creeperTexture, creeperPosition, player.Position, creeperSpeed, graphicsDevice, explodeTexture, creeperHp);
                    }
                    else
                    {
                        skeletonPosition = new Vector2(graphicsDevice.Viewport.Width / 2 - skeletonTexture.Width / 2, -skeletonTexture.Height);
                        newMob = new Skeleton(content, skeletonTexture, skeletonPosition, skeletonSpeed, graphicsDevice, skeletonHp);
                    }
                    break;

                case 3:
                    if (randomValue <= 40)
                    {
                        villagerPosition = new Vector2(-villagerTexture.Width, Game1.random.Next(0, graphicsDevice.Viewport.Height - villagerTexture.Height - player.texture.Height));
                        newMob = new Villager(content, villagerTexture, villagerPosition, villagerSpeed, graphicsDevice, villagerHp);
                    }
                    else if (randomValue <= 60)
                    {
                        creeperPosition = new Vector2(Game1.random.Next(0, graphicsDevice.Viewport.Width - creeperTexture.Width), -creeperTexture.Height);
                        newMob = new Creeper(content, creeperTexture, creeperPosition, player.Position, creeperSpeed, graphicsDevice, explodeTexture, creeperHp);
                    }
                    else if (randomValue <= 80)
                    {
                        skeletonPosition = new Vector2(graphicsDevice.Viewport.Width / 2 - skeletonTexture.Width / 2, -skeletonTexture.Height);
                        newMob = new Skeleton(content, skeletonTexture, skeletonPosition, skeletonSpeed, graphicsDevice, skeletonHp);
                    }
                    else
                    {
                        pillagerPosition = new Vector2(-pillagerTexture.Width, Game1.random.Next(graphicsDevice.Viewport.Height));
                        newMob = new Pillager(content, pillagerTexture, villagerPosition, villagerSpeed, graphicsDevice, pillagerHp);
                    }
                    break;

                case 4:
                    if (randomValue <= 50)
                    {
                        villagerPosition = new Vector2(-villagerTexture.Width, Game1.random.Next(0, graphicsDevice.Viewport.Height - villagerTexture.Height - player.texture.Height));
                        newMob = new Villager(content, villagerTexture, villagerPosition, villagerSpeed, graphicsDevice, villagerHp);
                    }
                    else if (randomValue <= 65)
                    {
                        creeperPosition = new Vector2(Game1.random.Next(0, graphicsDevice.Viewport.Width - creeperTexture.Width), -creeperTexture.Height);
                        newMob = new Creeper(content, creeperTexture, creeperPosition, player.Position, creeperSpeed, graphicsDevice, explodeTexture, creeperHp);
                    }
                    else if (randomValue <= 80)
                    {
                        skeletonPosition = new Vector2(graphicsDevice.Viewport.Width / 2 - skeletonTexture.Width / 2, -skeletonTexture.Height);
                        newMob = new Skeleton(content, skeletonTexture, skeletonPosition, skeletonSpeed, graphicsDevice, skeletonHp);
                    }
                    else if (randomValue <= 95)
                    {
                        pillagerPosition = new Vector2(-pillagerTexture.Width, Game1.random.Next(0, graphicsDevice.Viewport.Height - pillagerTexture.Height));
                        newMob = new Pillager(content, pillagerTexture, pillagerPosition, villagerSpeed, graphicsDevice, pillagerHp);
                    }
                    else
                    {
                        endermanPosition = new Vector2(Game1.random.Next(0, graphicsDevice.Viewport.Width - endermanTexture.Width), 0);
                        newMob = new Enderman(content, endermanTexture, endermanPosition, graphicsDevice, endermanHp, player);
                    }
                    break;



                case 5:
                    if (randomValue <= 10)
                    {
                        villagerPosition = new Vector2(-villagerTexture.Width, Game1.random.Next(0, graphicsDevice.Viewport.Height - villagerTexture.Height - player.texture.Height));
                        newMob = new Villager(content, villagerTexture, villagerPosition, villagerSpeed, graphicsDevice, villagerHp);
                    }
                    else if (randomValue <= 30)
                    {
                        creeperPosition = new Vector2(Game1.random.Next(0, graphicsDevice.Viewport.Width - creeperTexture.Width), -creeperTexture.Height);
                        newMob = new Creeper(content, creeperTexture, creeperPosition, player.Position, creeperSpeed, graphicsDevice, explodeTexture, creeperHp);
                    }

                    else if (randomValue <= 55)
                    {
                        skeletonPosition = new Vector2(graphicsDevice.Viewport.Width / 2 - skeletonTexture.Width / 2, -skeletonTexture.Height);
                        newMob = new Skeleton(content, skeletonTexture, skeletonPosition, skeletonSpeed, graphicsDevice, skeletonHp);
                    }
                    else if (randomValue <= 80)
                    {
                        pillagerPosition = new Vector2(-pillagerTexture.Width, Game1.random.Next(0, graphicsDevice.Viewport.Height - pillagerTexture.Height));
                        newMob = new Pillager(content, pillagerTexture, villagerPosition, villagerSpeed, graphicsDevice, pillagerHp);
                    }
                    else
                    {
                        endermanPosition = new Vector2(Game1.random.Next(0, graphicsDevice.Viewport.Width - endermanTexture.Width), 0);
                        newMob = new Enderman(content, endermanTexture, endermanPosition, graphicsDevice, endermanHp, player);
                    }
                    break;
            }
            return newMob;
        }
    }
}
