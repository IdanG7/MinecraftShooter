using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
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
        Texture2D dirtTexture;


        private List<Rectangle> grass1Rectangles = new List<Rectangle>();
        private List<Rectangle> grass2Rectangles = new List<Rectangle>();
        private List<Rectangle> cobblestoneRectangles = new List<Rectangle>();
        private List<Rectangle> dirtRectangles = new List<Rectangle>();


        List<Mob> mobs;

        Texture2D villagerTexture;
        Texture2D creeperTexture;
        Texture2D skeletonTexture;
        Texture2D pillagerTexture;
        Texture2D endermanTexture;
        Texture2D explodeTexture;

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

        private int skeletonHp;
        private int creeperHp;
        private int villagerHp;

        private bool levelInitialized = false;

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
            content = new ContentManager(Content.ServiceProvider, "content"); // initialize the content manager
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphicsDevice = GraphicsDevice;

            villagerTexture = Content.Load<Texture2D>("Sized/Villager_64");
            creeperTexture = Content.Load<Texture2D>("Sized/Creeper_64");
            explodeTexture = Content.Load<Texture2D>("Sized/Explode_200");
            skeletonTexture = Content.Load<Texture2D>("Sized/Skeleton_64");
            pillagerTexture = Content.Load<Texture2D>("Sized/Pillager_64");
            endermanTexture = Content.Load<Texture2D>("Sized/Enderman_64");
            playerTexture = Content.Load<Texture2D>("Sized/Steve_64");
            arrowTexture = Content.Load<Texture2D>("Sized/ArrowUp");
            cobblestoneTexture = Content.Load<Texture2D>("Sized/CobbleStone_64");
            grass2Texture = Content.Load<Texture2D>("Sized/Grass2_64");
            grass1Texture = Content.Load<Texture2D>("Sized/Grass1_64");
            dirtTexture = Content.Load<Texture2D>("Sized/Dirt_64");

            player = new Player(playerTexture, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - playerTexture.Height), 3f);

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

            player.Update(keyboardState, GraphicsDevice.Viewport.Width);

            if (keyboardState.IsKeyDown(Keys.Space) && fireRateTimer.Enabled == false)
            {

                var arrow = new Arrow(arrowTexture, player.Position, arrowSpeed, -1, 1);

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

            if (!levelInitialized)
            {
                UpdateLevelSettings(currentLevel);
                levelInitialized = true;
            }

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
            // Check for collisions between player arrows and mobs
            for (int i = arrows.Count - 1; i >= 0; i--)
            {
                for (int j = mobs.Count - 1; j >= 0; j--)
                {
                    // Check if the current mob is a Creeper
                    if (mobs[j] is Creeper)
                    {
                        Creeper creeper = (Creeper)mobs[j];
                        // Check for collision between the arrow and the Creeper
                        if (arrows[i].BoundingBox.Intersects(creeper.BoundingBox))
                        {
                            // Handle the collision
                            if (!creeper.Exploded)
                            {
                                // Subtract the arrow's damage from the Creeper's health
                                creeper.Hp -= arrows[i]._damage;

                                if (creeper.Hp <= 0)
                                {
                                    // Store the Death Position
                                    creeper.DeathPosition = creeper.position;

                                    // Creeper is dead, make it explode
                                    creeper.Exploded = true;

                                    //Store the Death Position
                                    creeper.DeathPosition = creeper.position;

                                    // Creeper is dead, make it explode
                                    creeper.Exploded = true;
                                    mobs.RemoveAt(j);



                                    // Get the explosion radius
                                    int explosionRadius = 100;

                                    for (int g1 = grass1Rectangles.Count - 1; g1 >= 0; g1--)
                                    {
                                        if (Vector2.Distance(creeper.DeathPosition, new Vector2(grass1Rectangles[g1].X, grass1Rectangles[g1].Y)) <= explosionRadius)
                                        {
                                            dirtRectangles.Add(new Rectangle(grass1Rectangles[g1].X, grass1Rectangles[g1].Y, dirtTexture.Width, dirtTexture.Height));
                                            grass1Rectangles.RemoveAt(g1);
                                        }
                                    }

                                    for (int g2 = grass2Rectangles.Count - 1; g2 >= 0; g2--)
                                    {
                                        if (Vector2.Distance(creeper.DeathPosition, new Vector2(grass2Rectangles[g2].X, grass2Rectangles[g2].Y)) <= explosionRadius)
                                        {
                                            dirtRectangles.Add(new Rectangle(grass2Rectangles[g2].X, grass2Rectangles[g2].Y, dirtTexture.Width, dirtTexture.Height));
                                            grass2Rectangles.RemoveAt(g2);
                                        }
                                    }

                                    for (int c = cobblestoneRectangles.Count - 1; c >= 0; c--)
                                    {
                                        if (Vector2.Distance(creeper.DeathPosition, new Vector2(cobblestoneRectangles[c].X, cobblestoneRectangles[c].Y)) <= explosionRadius)
                                        {
                                            dirtRectangles.Add(new Rectangle(cobblestoneRectangles[c].X, cobblestoneRectangles[c].Y, dirtTexture.Width, dirtTexture.Height));
                                            cobblestoneRectangles.RemoveAt(c);
                                        }
                                    }

                                }

                                // Remove the arrow that collided with the Creeper
                                arrows.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    else if (mobs[j] is Villager)
                    {
                        Villager villager = (Villager)mobs[j];
                        if (arrows[i].BoundingBox.Intersects(villager.BoundingBox))
                        {
                            villager.Hp -= arrows[i]._damage;

                            if (villager.Hp <= 0)
                            {
                                mobs.RemoveAt(j);
                            }
                            arrows.RemoveAt(i);
                            break;
                        }
                    }
                    else if (mobs[j] is Skeleton)
                    {
                        Skeleton skeleton = (Skeleton)mobs[j];
                        if (arrows[i].BoundingBox.Intersects(skeleton.BoundingBox))
                        {
                            skeleton.Hp -= arrows[i]._damage;

                            if (skeleton.Hp <= 0)
                            {
                                mobs.RemoveAt(j);
                            }
                            arrows.RemoveAt(i);
                            break;
                        }
                    }
                }
            }


            for (int j = mobs.Count - 1; j >= 0; j--)
            {
                if (mobs[j] is Creeper creeper && creeper.ToRemove)
                {
                    mobs.RemoveAt(j);
                }
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

            foreach (Rectangle dirtRect in dirtRectangles)
            {
                spriteBatch.Draw(dirtTexture, dirtRect, Color.White);
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

        private void UpdateLevelSettings(int currentLevel)
        {
            levelInitialized = false;

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

        private Mob CreateMobBasedOnLevel(int currentLevel, int randomValue, GraphicsDevice graphicsDevice)
        {
            Mob newMob = null;

            switch (currentLevel)
            {
                case 1:
                    if (randomValue <= 70)
                    {

                        villagerPosition = new Vector2(-villagerTexture.Width, random.Next(0, GraphicsDevice.Viewport.Height - villagerTexture.Height));
                        newMob = new Villager(content, villagerPosition, villagerSpeed, graphicsDevice, villagerHp);
                    }
                    else if (randomValue <= 90)
                    {

                        creeperPosition = new Vector2(random.Next(0, graphicsDevice.Viewport.Width - creeperTexture.Width), -creeperTexture.Height);
                        newMob = new Creeper(content, creeperTexture, creeperPosition, player.Position, creeperSpeed, graphicsDevice, explodeTexture, creeperHp);
                    }
                    else
                    {

                        skeletonPosition = new Vector2(graphicsDevice.Viewport.Width / 2 - skeletonTexture.Width / 2, -skeletonTexture.Height);
                        newMob = new Skeleton(content, skeletonTexture, skeletonPosition, skeletonSpeed, graphicsDevice, skeletonHp);
                    }
                    break;
            }
            return newMob;
        }
    }
}