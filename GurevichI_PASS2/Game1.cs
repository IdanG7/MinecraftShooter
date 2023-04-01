﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace GurevichI_PASS2
{
    public class Game1 : Game
    {
        private const int MENU = 0;
        private const int GAMEPLAY = 1;
        private const int STATS = 2;
        private const int INSTRUCTIONS = 3;
        private const int RESULTS = 4;
        private const int SHOP = 5;

        public int currentGameState = MENU;

        public static Random random = new Random();

        private GraphicsDeviceManager graphics;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;
        private ContentManager content;

        Song backgroundMusic;
        Song menuMusic;


        private Player player;
        private Level level;


        List<Arrow> arrows;
        List<Mob> mobs;

        List<Rectangle> grass1Rectangles = new List<Rectangle>();
        List<Rectangle> grass2Rectangles = new List<Rectangle>();
        List<Rectangle> cobblestoneRectangles = new List<Rectangle>();
        List<Rectangle> dirtRectangles = new List<Rectangle>();

        Rectangle titleRect;
        Rectangle button1Rect;
        Rectangle button2Rect;
        Rectangle button3Rect;

        Vector2 titlePosition;
        Vector2 button1Position;
        Vector2 button2Position;
        Vector2 button3Position;
        Vector2 playerPosition;

        public Texture2D playerTexture;
        public Texture2D villagerTexture;
        public Texture2D creeperTexture;
        public Texture2D skeletonTexture;
        public Texture2D pillagerTexture;
        public Texture2D endermanTexture;
        public Texture2D explodeTexture;
        public Texture2D cobblestoneTexture;
        public Texture2D grass1Texture;
        public Texture2D grass2Texture;
        public Texture2D dirtTexture;
        public Texture2D background;
        public Texture2D button;
        public Texture2D menuTitle;
        public Texture2D arrowTexture;
        public Texture2D shieldTexture;

        SpriteFont mainFont;

        float elapsedSpawnTime = 0f;
        int totalMobsInCurrentLevel;
        int maxMobsOnScreen;
        float spawnTime;
        bool levelInitialized = false;

        float arrowSpeed = 5f;

        Timer fireRateTimer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            arrows = new List<Arrow>();

            fireRateTimer = new Timer(333);
            fireRateTimer.Elapsed += (sender, args) => fireRateTimer.Enabled = false;

            mobs = new List<Mob>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            content = new ContentManager(Content.ServiceProvider, "content"); // initialize the content manager
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphicsDevice = GraphicsDevice;

            // Load textures
            villagerTexture = content.Load<Texture2D>("Sized/Villager_64");
            creeperTexture = content.Load<Texture2D>("Sized/Creeper_64");
            explodeTexture = content.Load<Texture2D>("Sized/Explode_200");
            skeletonTexture = content.Load<Texture2D>("Sized/Skeleton_64");
            pillagerTexture = content.Load<Texture2D>("Sized/Pillager_64");
            endermanTexture = content.Load<Texture2D>("Sized/Enderman_64");

            shieldTexture = content.Load<Texture2D>("Sized/Shield_48");

            // Load font
            mainFont = content.Load<SpriteFont>("Fonts/MinecraftMain");

            // Load player-related assets
            playerTexture = content.Load<Texture2D>("Sized/Nacho_64");
            arrowTexture = content.Load<Texture2D>("Sized/ArrowUp");
            player = new Player(playerTexture, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - playerTexture.Height), 3f);

            // Load level-related assets
            cobblestoneTexture = content.Load<Texture2D>("Sized/CobbleStone_64");
            grass2Texture = content.Load<Texture2D>("Sized/Grass2_64");
            grass1Texture = content.Load<Texture2D>("Sized/Grass1_64");
            dirtTexture = content.Load<Texture2D>("Sized/Dirt_64");
            level = new Level(content, graphicsDevice, player);

            // Load menu-related assets
            background = content.Load<Texture2D>("Sized/MenuBG1");
            button = content.Load<Texture2D>("Sized/Button");
            menuTitle = content.Load<Texture2D>("Sized/Title");

            titlePosition = new Vector2(GraphicsDevice.Viewport.Width / 2 - menuTitle.Width / 2, -25);
            titleRect = new Rectangle(graphicsDevice.Viewport.Width / 2 - menuTitle.Width / 2, graphicsDevice.Viewport.Height / 4, menuTitle.Width, menuTitle.Height);
            button1Position = new Vector2(graphicsDevice.Viewport.Width / 2 - button.Width / 2 - 10, titleRect.Bottom - 200);
            button2Position = new Vector2(graphicsDevice.Viewport.Width / 2 - button.Width - 10, button1Position.Y + button.Height + 10);
            button3Position = new Vector2(graphicsDevice.Viewport.Width / 2 + 10, button1Position.Y + button.Height + 10);
            button1Rect = new Rectangle((int)button1Position.X, (int)button1Position.Y, button.Width, button.Height);
            button2Rect = new Rectangle((int)button2Position.X, (int)button2Position.Y, button.Width, button.Height);
            button3Rect = new Rectangle((int)button3Position.X, (int)button3Position.Y, button.Width, button.Height);

            // Load background music
            backgroundMusic = content.Load<Song>("Audio/Music/Gameplay");
            menuMusic = content.Load<Song>("Audio/Music/Menu");

            // Generate grass and cobblestone
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
            switch (currentGameState)
            {
                case MENU:
                    MouseState mouseState = Mouse.GetState();
                    button1Rect = new Rectangle((int)button1Position.X, (int)button1Position.Y, button.Width, button.Height);
                    if (mouseState.LeftButton == ButtonState.Pressed && button1Rect.Contains(mouseState.Position))
                    {
                        currentGameState = GAMEPLAY;
                    }

                    if (currentGameState == MENU)
                    {
                        if (MediaPlayer.State != MediaState.Playing)
                        {
                            MediaPlayer.IsRepeating = true;
                            MediaPlayer.Play(menuMusic);
                        }
                    }
                    else
                    {
                        MediaPlayer.Stop();
                    }

                    break;

                case GAMEPLAY:
                    KeyboardState keyboardStateGame = Keyboard.GetState();

                    player.Update(keyboardStateGame, graphicsDevice);

                    level.UpdateLevel();

                    if (keyboardStateGame.IsKeyDown(Keys.Space) && fireRateTimer.Enabled == false)
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
                        level.UpdateLevel();
                        spawnTime = level.SpawnTime;
                        maxMobsOnScreen = level.MaxMobsOnScreen;
                        totalMobsInCurrentLevel = level.TotalMobsInCurrentLevel;
                        levelInitialized = true;
                    }


                    elapsedSpawnTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (elapsedSpawnTime >= spawnTime && mobs.Count < maxMobsOnScreen && totalMobsInCurrentLevel > 0)
                    {
                        Mob newMob = null;
                        int randomValue = random.Next(1, 101);

                        newMob = level.CreateMobBasedOnLevel(level.currentLevel, randomValue, GraphicsDevice);

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

                    for (int j = mobs.Count - 1; j >= 0; j--)
                    {
                        if (mobs[j] is Villager villager)
                        {
                            if (villager.offScreenTimer <= 0)
                            {
                                mobs.RemoveAt(j);
                            }
                        }
                    }

                    for (int i = mobs.Count - 1; i >= 0; i--)
                    {
                        if (mobs[i] is Pillager pillager)
                        {
                            if (pillager.offScreenTimer <= 0)
                            {
                                mobs.RemoveAt(i);
                            }
                        }
                    }


                    // Check for collisions between player arrows and mobs
                    for (int i = arrows.Count - 1; i >= 0; i--)
                    {
                        for (int j = mobs.Count - 1; j >= 0; j--)
                        {
                            if (mobs[j] is Creeper creeper && creeper.HandleCollisionWithArrow(arrows[i], GraphicsDevice, grass1Rectangles, grass2Rectangles, cobblestoneRectangles, dirtRectangles, dirtTexture))
                            {
                                // Remove the arrow that collided with the Creeper
                                arrows.RemoveAt(i);
                                break;
                            }
                            else if (mobs[j] is Villager villager && villager.HandleCollisionWithArrow(arrows[i]))
                            {
                                mobs.RemoveAt(j);
                                arrows.RemoveAt(i);
                                break;
                            }

                            else if (mobs[j] is Skeleton skeleton && skeleton.HandleCollisionWithArrow(arrows[i]))
                            {
                                arrows.RemoveAt(i);
                                break;
                            }
                            else if (mobs[j] is Pillager pillager && pillager.HandleCollisionWithArrow(arrows[i]))
                            {
                                arrows.RemoveAt(i);
                                break;
                            }

                        }
                    }


                    for (int j = mobs.Count - 1; j >= 0; j--)
                    {
                        if (mobs[j] is Creeper creeper && creeper.ToRemove)
                        {
                            mobs.RemoveAt(j);
                        }
                        else if (mobs[j] is Skeleton skeleton && skeleton.ToRemove)
                        {
                            mobs.RemoveAt(j);
                        }
                        else if (mobs[j] is Pillager pillager && pillager.ToRemove)
                        {
                            mobs.RemoveAt(j);
                        }
                    }

                    if (currentGameState == GAMEPLAY)
                    {
                        if (MediaPlayer.State != MediaState.Playing)
                        {
                            MediaPlayer.IsRepeating = true;
                            MediaPlayer.Play(backgroundMusic);
                        }
                    }
                    else
                    {
                        MediaPlayer.Stop();
                    }

                    break;

                case STATS:
                    // Update logic for STATS state
                    break;
                case RESULTS:
                    // Update logic for RESULTS state
                    break;
                case SHOP:
                    // Update logic for SHOP state
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            switch (currentGameState)
            {
                case MENU:
                    GraphicsDevice.Clear(Color.Green);

                    spriteBatch.Begin();

                    spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    spriteBatch.Draw(menuTitle, titlePosition, Color.White);

                    float buttonScale = 0.8f; // Adjust this value to change the button size
                    Vector2 buttonOrigin = new Vector2(button.Width / 2, button.Height / 2);

                    spriteBatch.Draw(button, button1Position + buttonOrigin, null, Color.White, 0, buttonOrigin, buttonScale, SpriteEffects.None, 0);
                    spriteBatch.DrawString(mainFont, "PLAY GAME", button1Position + new Vector2(button.Width / 2, button.Height / 2) - mainFont.MeasureString("PLAY GAME") * 2.5f / 2, Color.Black, 0, Vector2.Zero, 2.5f, SpriteEffects.None, 0);

                    spriteBatch.Draw(button, button2Position + buttonOrigin, null, Color.White, 0, buttonOrigin, buttonScale, SpriteEffects.None, 0);
                    spriteBatch.Draw(button, button3Position + buttonOrigin, null, Color.White, 0, buttonOrigin, buttonScale, SpriteEffects.None, 0);

                    spriteBatch.End();

                    break;

                case GAMEPLAY:

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

                    foreach (Pillager pillager in mobs.OfType<Pillager>())
                    {
                        pillager.Draw(spriteBatch, content);
                    }

                    spriteBatch.End();

                    break;
                case STATS:
                    // Draw logic for STATS state
                    break;
                case INSTRUCTIONS:
                    // Draw logic for INSTRUCTIONS state
                    break;
                case RESULTS:
                    // Draw logic for RESULTS state
                    break;
                case SHOP:
                    // Draw logic for SHOP state
                    break;
            }

            base.Draw(gameTime);
        }

    }
}