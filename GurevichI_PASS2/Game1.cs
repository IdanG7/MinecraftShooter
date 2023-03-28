using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using GurevichI_PASS2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GurevichI_PASS2
{
    public class Game1 : Game
    {
        private Timer fireRateTimer;

        private Random random = new Random();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private ContentManager content;

        Player player;

        private Vector2 villagerPosition;
        private float villagerSpeed;

        Texture2D playerTexture;

        List<Arrow> arrows;
        Texture2D arrowTexture;
        float arrowSpeed = 5f;

        Texture2D cobblestoneTexture;
        Texture2D grass1Texture;
        Texture2D grass2Texture;

        private List<Rectangle> grass1Rectangles = new List<Rectangle>();
        private List<Rectangle> grass2Rectangles = new List<Rectangle>();
        private List<Rectangle> cobblestoneRectangles = new List<Rectangle>();

        List<Mob> mobs;

        Texture2D villagerTexture;
        Texture2D creeperTexture;
        Texture2D skeletonTexture;
        Texture2D pillagerTexture;
        Texture2D endermanTexture;

        private Vector2 skeletonPosition;
        private Vector2 creeperPosition;
        private Vector2 endermanPosition;
        private Vector2 pillagerPosition;

        private int currentLevel = 1;
        private float elapsedSpawnTime = 0f;

        private int totalMobsInCurrentLevel;
        private int maxMobsOnScreen;
        private float spawnTime;

        private float creeperSpeed;

        private float skeletonSpeed;

        private GraphicsDevice graphicsDevice;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            arrows = new List<Arrow>();

            fireRateTimer = new Timer(300);
            fireRateTimer.Elapsed += (sender, args) => fireRateTimer.Enabled = false;

            mobs = new List<Mob>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            content = new ContentManager(Content.ServiceProvider, "Content"); // initialize the content manager
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphicsDevice = GraphicsDevice;

            villagerTexture = Content.Load<Texture2D>("Sized/Villager_64");
            creeperTexture = Content.Load<Texture2D>("Sized/Creeper_64");
            skeletonTexture = Content.Load<Texture2D>("Sized/Skeleton_64");
            // pillagerTexture = Content.Load<Texture2D>("Sized/Pillager_64");
            // endermanTexture = Content.Load<Texture2D>("Sized/Enderman_64");
            playerTexture = Content.Load<Texture2D>("Sized/Steve_64");

            player = new Player(playerTexture, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - playerTexture.Height), 3f);

            villagerPosition = new Vector2(-villagerTexture.Width, random.Next(0, GraphicsDevice.Viewport.Height - villagerTexture.Height));
            //  skeletonPosition = new Vector2(GraphicsDevice.Viewport.Width / 2 - skeletonTexture.Width / 2, -skeletonTexture.Height);


            villagerSpeed = 100f;
            creeperSpeed = 1.5f;
            skeletonSpeed = 100f;


            arrowTexture = Content.Load<Texture2D>("Sized/ArrowUp");

            cobblestoneTexture = Content.Load<Texture2D>("Sized/CobbleStone_64");
            grass2Texture = Content.Load<Texture2D>("Sized/Grass2_64");
            grass1Texture = Content.Load<Texture2D>("Sized/Grass1_64");

            for (int x = 0; x < GraphicsDevice.Viewport.Width; x += grass2Texture.Width)
            {
                for (int y = 0; y < GraphicsDevice.Viewport.Height; y += grass2Texture.Height)
                {
                    if (random.Next(0, 3) == 0)
                    {
                        grass1Rectangles.Add(new Rectangle(x, y, grass1Texture.Width, grass1Texture.Height));
                    }
                    else if (random.Next(0, 3) == 1)
                    {
                        grass2Rectangles.Add(new Rectangle(x, y, grass2Texture.Width, grass2Texture.Height));
                    }
                    else
                    {
                        cobblestoneRectangles.Add(new Rectangle(x, y, cobblestoneTexture.Width, cobblestoneTexture.Height));
                    }
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            player.Update(keyboardState, (float)gameTime.ElapsedGameTime.TotalSeconds, GraphicsDevice.Viewport.Width);

            if (keyboardState.IsKeyDown(Keys.Space) && fireRateTimer.Enabled == false)
            {
                Arrow arrow = new Arrow(arrowTexture, player.Position, arrowSpeed);
                arrows.Add(arrow);

                fireRateTimer.Enabled = true;
            }

            for (int i = arrows.Count - 1; i >= 0; i--)
            {
                arrows[i].Update(gameTime);

                if (arrows[i]._position.Y + arrows[i]._arrowTexture.Height <= 0)
                {
                    arrows.RemoveAt(i);
                }

            }

            UpdateLevelSettings();

            elapsedSpawnTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedSpawnTime >= spawnTime && mobs.Count < maxMobsOnScreen && totalMobsInCurrentLevel > 0)
            {
                Mob newMob = null;
                int randomValue = random.Next(1, 101);

                newMob = CreateMobBasedOnLevel(currentLevel, randomValue, graphicsDevice);

                if (newMob != null)
                {
                    mobs.Add(newMob);
                    totalMobsInCurrentLevel--;
                }

                elapsedSpawnTime = 0f;
            }


            for (int i = mobs.Count - 1; i >= 0; i--)
            {
                mobs[i].Update(gameTime, player.Position, GraphicsDevice);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (Rectangle grass1Rect in grass1Rectangles)
            {
                spriteBatch.Draw(grass1Texture, grass1Rect, Color.White);
            }

            foreach (Rectangle grass2Rect in grass2Rectangles)
            {
                spriteBatch.Draw(grass2Texture, grass2Rect, Color.White);
            }

            foreach (Rectangle cobblestoneRect in cobblestoneRectangles)
            {
                spriteBatch.Draw(cobblestoneTexture, cobblestoneRect, Color.White);
            }

            player.Draw(spriteBatch);

            foreach (Arrow arrow in arrows)
            {
                arrow.Draw(spriteBatch);
            }

            foreach (Villager villager in mobs.OfType<Villager>())
            {
                villager.Draw(spriteBatch);
            }

            foreach (Creeper creeper in mobs.OfType<Creeper>())
            {
                creeper.Draw(spriteBatch);
            }


            foreach (Skeleton skeleton in mobs.OfType<Skeleton>())
            {
                skeleton.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdateLevelSettings()
        {
            switch (currentLevel)
            {
                case 1:
                    totalMobsInCurrentLevel = 10;
                    maxMobsOnScreen = 2;
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

        private Mob CreateMobBasedOnLevel(int level, int randomValue, GraphicsDevice graphicsDevice)
        {
            Mob newMob = null;

            switch (currentLevel)
            {
                case 1:
                    /*  if (randomValue <= 70)
                      {
                          villagerPosition = new Vector2(-villagerTexture.Width, random.Next(0, GraphicsDevice.Viewport.Height - villagerTexture.Height));
                          newMob = new Villager(content, villagerTexture, villagerPosition, villagerSpeed, graphicsDevice);
                      }
                      else if (randomValue <= 90)
                      {
                          creeperPosition = new Vector2(25, 25);
                          newMob = new Creeper(content, creeperTexture, creeperPosition, player.Position, creeperSpeed, graphicsDevice);
                      }*/
                    if (randomValue <= 70)
                    {
                        skeletonPosition = new Vector2(GraphicsDevice.Viewport.Width / 2 - skeletonTexture.Width / 2, -skeletonTexture.Height - 50);

                        newMob = new Skeleton(content, skeletonTexture, skeletonPosition, skeletonSpeed, graphicsDevice);
                    }
                    break;

                    // Add the other cases here

            }
            return newMob;
        }



    }

}