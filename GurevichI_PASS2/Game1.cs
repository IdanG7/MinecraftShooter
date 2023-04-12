using System;
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
        Song resultMusic;

        private Player player;
        private Level level;
        public Arrow arrow;
        public Skeleton skeleton;

        int arrowDamage;

        private int playerScore = 500;
        private int[] levelScores = new int[5];
        private int currentLevel;
        private int killsInCurrentLevel;
        private int shotsFired;
        private int shotsHit;

        double hitPercentage;
        string scoreTextGame;
        string levelScoreText;
        string continueMessage;
        string scoreText;
        string instructionHeader;
        string moveInstructions;
        string shootInstructions;
        string goalInstructions;

        List<Arrow> arrows;
        List<Mob> mobs;
        List<Vector2> iconPositions = new List<Vector2>();


        List<Rectangle> grass1Rectangles = new List<Rectangle>();
        List<Rectangle> grass2Rectangles = new List<Rectangle>();
        List<Rectangle> cobblestoneRectangles = new List<Rectangle>();
        List<Rectangle> dirtRectangles = new List<Rectangle>();
        Rectangle semiTransparentBackgroundRec;
        Rectangle smallSemiTransparentBackgroundRec;

        Rectangle titleRect;
        Rectangle button1Rect;
        Rectangle button2Rect;
        Rectangle button3Rect;
        Rectangle continueButtonRect;
        Rectangle speedBuffRect;
        Rectangle pointsBuffRect;
        Rectangle damageBuffRect;
        Rectangle rateOfFireRect;
        Rectangle popupBackgroundRect;

        Vector2 titlePosition;
        Vector2 button1Position;
        Vector2 button2Position;
        Vector2 button3Position;
        Vector2 levelScoresPosition;
        Vector2 statsPosition;
        Vector2 scoreTextPosition;
        Vector2 continueButtonPosition;
        Vector2 scoreTextPositionGame;
        Vector2 tempLevelScoresPosition;
        Vector2 instructionHeaderPosition;
        Vector2 moveInstructionsPos;
        Vector2 shootInstructionsPos;
        Vector2 goalInstructionsPos;
        Vector2 rateofFirePos;
        Vector2 pointsBuffPos;
        Vector2 speedBuffPos;
        Vector2 damageBuffPos;
        Vector2 buttonOrigin;
        Vector2 continueMessagePosition;
        Vector2 itemFrame1Pos;
        Vector2 itemFrame2Pos;
        Vector2 itemFrame3Pos;
        Vector2 itemFrame4Pos;
        Vector2 popupTextPosition;

        Texture2D playerTexture;
        Texture2D cobblestoneTexture;
        Texture2D grass1Texture;
        Texture2D grass2Texture;
        Texture2D dirtTexture;
        Texture2D background;
        Texture2D shopBackground;
        Texture2D button;
        Texture2D menuTitle;
        Texture2D shopTitle;
        Texture2D arrowTexture;
        Texture2D semiTransparentBackground;
        Texture2D traderTexture;
        Texture2D speedBuff;
        Texture2D damageBuff;
        Texture2D pointsBuff;
        Texture2D rateOfFireBuff;
        Texture2D speedBuffIcon;
        Texture2D damageBuffIcon;
        Texture2D pointsBuffIcon;
        Texture2D rateOfFireBuffIcon;
        Texture2D itemFrame;
        Texture2D pixel;

        int boxWidth;
        int boxHeight;
        int box2Height;
        int boxX;
        int boxY;
        int box2Y;

        bool speedBuffActive = false;
        bool damageBuffActive = false;
        bool rateOfFireBuffActive = false;
        bool pointsBuffActive = false;


        private bool showPopup = false;
        private string popupText = "";
        private Vector2 popupPosition;
        private Color popupColor = Color.White;

        int speedBuffCost = 100;
        int damageBuffCost = 200;
        int rateOfFireBuffCost = 300;
        int pointsBuffCost = 500;


        float horizontalPadding;
        float verticalPadding;
        float centerX;
        float centerY;
        float iconPadding = 10.0f;
        float buttonScale = 0.8f;

        SpriteFont mainFont;
        SpriteFont statsFont;
        SpriteFont smallText;

        int popupBackgroundWidth = 350; // Adjust this value according to your desired width
        int popupBackgroundHeight = 100; // Adjust this value according to your desired height

        Color button1Color = Color.White;
        Color button2Color = Color.White;
        Color button3Color = Color.White;
        Color continueButtonColor = Color.White;

        float elapsedSpawnTime = 0f;
        int totalMobsInCurrentLevel;
        int maxMobsOnScreen;
        float spawnTime;
        bool levelInitialized = false;

        float arrowSpeed = 5f;

        Timer fireRateTimer;
        private double initialFireRateInterval = 333;

        private float resultsScreenDelay = 0f;
        private float resultsScreenDelayDuration = 1f; // Duration in seconds


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            arrows = new List<Arrow>();

            fireRateTimer = new Timer(initialFireRateInterval);
            fireRateTimer.Elapsed += (sender, args) => fireRateTimer.Enabled = false;

            mobs = new List<Mob>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            content = new ContentManager(Content.ServiceProvider, "content"); // initialize the content manager
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphicsDevice = GraphicsDevice;

            boxWidth = (int)(GraphicsDevice.Viewport.Width * 1);
            boxHeight = (int)(GraphicsDevice.Viewport.Height * 0.8);
            box2Height = (int)(GraphicsDevice.Viewport.Height * 0.8);
            boxX = (GraphicsDevice.Viewport.Width - boxWidth) / 2;
            boxY = (GraphicsDevice.Viewport.Height - boxHeight) / 2;
            box2Y = (int)(centerY + (box2Height / 2 - 25) + (verticalPadding * 0.2f));


            horizontalPadding = GraphicsDevice.Viewport.Width * 0.1f;
            verticalPadding = GraphicsDevice.Viewport.Height * 0.1f;
            centerX = GraphicsDevice.Viewport.Width / 2;
            centerY = GraphicsDevice.Viewport.Height / 2;


            // Load font
            mainFont = content.Load<SpriteFont>("Fonts/MainFont");
            statsFont = content.Load<SpriteFont>("Fonts/StatFont");
            smallText = content.Load<SpriteFont>("Fonts/SmallText");

            // Load player-related assets
            playerTexture = content.Load<Texture2D>("Sized/Nacho_64");
            arrowTexture = content.Load<Texture2D>("Sized/ArrowUp");

            // Load level-related assets
            cobblestoneTexture = content.Load<Texture2D>("Sized/CobbleStone_64");
            grass2Texture = content.Load<Texture2D>("Sized/Grass2_64");
            grass1Texture = content.Load<Texture2D>("Sized/Grass1_64");
            dirtTexture = content.Load<Texture2D>("Sized/Dirt_64");
            speedBuffIcon = content.Load<Texture2D>("Sized/IconSpeed_32");
            rateOfFireBuffIcon = content.Load<Texture2D>("Sized/IconFireRate_32");
            damageBuffIcon = content.Load<Texture2D>("Sized/IconDamage_32");
            pointsBuffIcon = content.Load<Texture2D>("Sized/IconPoints_32");


            // Load menu-related assets
            background = content.Load<Texture2D>("Sized/MenuBG1");
            shopBackground = content.Load<Texture2D>("Raw/Dirt");
            button = content.Load<Texture2D>("Sized/Button");
            menuTitle = content.Load<Texture2D>("Sized/Title");
            shopTitle = content.Load<Texture2D>("Sized/ShopTitle");
            semiTransparentBackground = content.Load<Texture2D>("Sized/BlackBackground");
            speedBuff = content.Load<Texture2D>("Sized/Boots");
            rateOfFireBuff = content.Load<Texture2D>("Sized/Bow_64");
            damageBuff = content.Load<Texture2D>("Sized/Sword");
            pointsBuff = content.Load<Texture2D>("Sized/Bottle");
            itemFrame = content.Load<Texture2D>("Sized/itemframe");

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            buttonOrigin = new Vector2(button.Width / 2, button.Height / 2);
            titlePosition = new Vector2(GraphicsDevice.Viewport.Width / 2 - menuTitle.Width / 2, -25);
            titleRect = new Rectangle(graphicsDevice.Viewport.Width / 2 - menuTitle.Width / 2, graphicsDevice.Viewport.Height / 4, menuTitle.Width, menuTitle.Height);
            button1Position = new Vector2(graphicsDevice.Viewport.Width / 2 - button.Width / 2 - 10, titleRect.Bottom - 200);
            button2Position = new Vector2(graphicsDevice.Viewport.Width / 2 - button.Width - 10, button1Position.Y + button.Height + 10);
            button3Position = new Vector2(graphicsDevice.Viewport.Width / 2 + 10, button1Position.Y + button.Height + 10);
            continueButtonPosition = new Vector2(centerX - button.Width / 2, GraphicsDevice.Viewport.Height - verticalPadding * 2.5f);
            continueMessagePosition = new Vector2(centerX - mainFont.MeasureString("Press Enter to Continue").X / 2f, GraphicsDevice.Viewport.Height - verticalPadding * 2.5f);

            Vector2 rateOfFireBuffCenter = new Vector2(rateOfFireBuff.Width / 2, rateOfFireBuff.Height / 2);
            Vector2 speedBuffCenter = new Vector2(speedBuff.Width / 2, speedBuff.Height / 2);
            Vector2 pointsBuffCenter = new Vector2(pointsBuff.Width / 2, pointsBuff.Height / 2);
            Vector2 damageBuffCenter = new Vector2(damageBuff.Width / 2, damageBuff.Height / 2);



            itemFrame1Pos = new Vector2(centerX - horizontalPadding * 4.5f, centerY - verticalPadding * 2.5f);
            itemFrame2Pos = new Vector2(centerX + horizontalPadding * 3f, centerY - verticalPadding * 2.5f);
            itemFrame3Pos = new Vector2(centerX - horizontalPadding * 4.5f, centerY + verticalPadding * 2f);
            itemFrame4Pos = new Vector2(centerX + horizontalPadding * 3f, centerY + verticalPadding * 2f);


            rateofFirePos = itemFrame1Pos + rateOfFireBuffCenter;
            speedBuffPos = itemFrame2Pos + speedBuffCenter;
            pointsBuffPos = itemFrame3Pos + pointsBuffCenter;
            damageBuffPos = itemFrame4Pos + damageBuffCenter;

            iconPositions.Add(new Vector2(GraphicsDevice.Viewport.Width - speedBuffIcon.Width - iconPadding, GraphicsDevice.Viewport.Height - speedBuffIcon.Height - iconPadding));
            iconPositions.Add(new Vector2(GraphicsDevice.Viewport.Width - damageBuffIcon.Width - iconPadding, GraphicsDevice.Viewport.Height - speedBuffIcon.Height - damageBuffIcon.Height - iconPadding * 2));
            iconPositions.Add(new Vector2(GraphicsDevice.Viewport.Width - pointsBuffIcon.Width - iconPadding, GraphicsDevice.Viewport.Height - speedBuffIcon.Height - damageBuffIcon.Height - pointsBuffIcon.Height - iconPadding * 3));
            iconPositions.Add(new Vector2(GraphicsDevice.Viewport.Width - rateOfFireBuffIcon.Width - iconPadding, GraphicsDevice.Viewport.Height - speedBuffIcon.Height - damageBuffIcon.Height - pointsBuffIcon.Height - rateOfFireBuffIcon.Height - iconPadding * 4));

            button1Rect = new Rectangle((int)button1Position.X, (int)button1Position.Y, button.Width, button.Height);
            button2Rect = new Rectangle((int)button2Position.X, (int)button2Position.Y, button.Width, button.Height);
            button3Rect = new Rectangle((int)button3Position.X, (int)button3Position.Y, button.Width, button.Height);
            continueButtonRect = new Rectangle((int)continueButtonPosition.X, (int)continueButtonPosition.Y, button.Width, button.Height);

            rateOfFireRect = new Rectangle((int)rateofFirePos.X, (int)rateofFirePos.Y, rateOfFireBuff.Width, rateOfFireBuff.Height);
            speedBuffRect = new Rectangle((int)speedBuffPos.X, (int)speedBuffPos.Y, speedBuff.Width, speedBuff.Height);
            pointsBuffRect = new Rectangle((int)pointsBuffPos.X, (int)pointsBuffPos.Y, pointsBuff.Width, pointsBuff.Height);
            damageBuffRect = new Rectangle((int)damageBuffPos.X, (int)damageBuffPos.Y, damageBuff.Width, damageBuff.Height);

            semiTransparentBackgroundRec = new Rectangle(boxX, boxY, boxWidth, boxHeight);
            smallSemiTransparentBackgroundRec = new Rectangle(boxX, box2Y, boxWidth, box2Height);

            popupBackgroundRect = new Rectangle(
                (GraphicsDevice.Viewport.Width - popupBackgroundWidth) / 2,
                (GraphicsDevice.Viewport.Height - popupBackgroundHeight) / 2,
                popupBackgroundWidth,
                popupBackgroundHeight
            );

            levelScoresPosition = new Vector2(horizontalPadding, verticalPadding * 3);
            statsPosition = new Vector2(GraphicsDevice.Viewport.Width - horizontalPadding * 3, verticalPadding * 3);
            scoreTextPosition = new Vector2(centerX - mainFont.MeasureString("SCORE : ").X / 2f, verticalPadding);
            scoreTextPositionGame = new Vector2();
            instructionHeaderPosition = new Vector2(centerX - mainFont.MeasureString("INSTRUCTIONS").X / 2f, verticalPadding);
            moveInstructionsPos = new Vector2(horizontalPadding, verticalPadding * 3f);
            goalInstructionsPos = new Vector2(centerX - mainFont.MeasureString("Kill all the mobs before they go off the screen!").X / 4f, GraphicsDevice.Viewport.Height - verticalPadding * 4f);
            shootInstructionsPos = new Vector2(horizontalPadding * -0.01f, verticalPadding * 4f);
            popupPosition = new Vector2(
                            popupBackgroundRect.X + (popupBackgroundRect.Width / 2) - (mainFont.MeasureString(popupText).X / 2),
                            popupBackgroundRect.Y + (popupBackgroundRect.Height / 2) - (mainFont.MeasureString(popupText).Y / 2));
            popupTextPosition = new Vector2(
                   popupBackgroundRect.X + (popupBackgroundRect.Width / 12f) - (mainFont.MeasureString(popupText).X / 2),
                   popupBackgroundRect.Y + (popupBackgroundRect.Height / 4.5f) - (mainFont.MeasureString(popupText).Y / 2)
               );


            player = new Player(playerTexture, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - playerTexture.Height), 3f);
            level = new Level(content, graphicsDevice, player);

            // Load background music
            backgroundMusic = content.Load<Song>("Music/Gameplay");
            menuMusic = content.Load<Song>("Music/Menu");
            resultMusic = content.Load<Song>("Music/Results");

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
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            switch (currentGameState)
            {
                case MENU:
                    if (button1Rect.Contains(mouseState.Position))
                    {
                        button1Color = Color.Yellow;

                    }
                    else
                    {
                        button1Color = Color.White;

                    }

                    if (button2Rect.Contains(mouseState.Position))
                    {
                        button2Color = Color.Yellow;

                    }
                    else
                    {
                        button2Color = Color.White;

                    }

                    if (button3Rect.Contains(mouseState.Position))
                    {
                        button3Color = Color.Yellow;

                    }
                    else
                    {
                        button3Color = Color.White;

                    }

                    // Handle button clicks
                    if (mouseState.LeftButton == ButtonState.Pressed && button1Rect.Contains(mouseState.Position))
                    {
                        currentGameState = INSTRUCTIONS;
                    }
                    if (mouseState.LeftButton == ButtonState.Pressed && button2Rect.Contains(mouseState.Position))
                    {
                        currentGameState = SHOP;
                    }
                    if (mouseState.LeftButton == ButtonState.Pressed && button3Rect.Contains(mouseState.Position))
                    {
                        Environment.Exit(0);
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

                case INSTRUCTIONS:

                    if (mouseState.LeftButton == ButtonState.Pressed && continueButtonRect.Contains(mouseState.Position))
                    {
                        currentGameState = GAMEPLAY;
                    }


                    if (continueButtonRect.Contains(mouseState.Position))
                    {
                        continueButtonColor = Color.Yellow;

                    }
                    else
                    {
                        continueButtonColor = Color.White;

                    }
                    break;

                case GAMEPLAY:

                    player.Update(keyboardState, graphicsDevice, gameTime);

                    level.UpdateLevel();

                    if (keyboardState.IsKeyDown(Keys.Space) && fireRateTimer.Enabled == false && player.fearTimer <= 0)
                    {
                        if (damageBuffActive)
                        {
                            arrowDamage = 3;
                        }
                        else
                        {
                            arrowDamage = 1;
                        }

                        Arrow arrow = new Arrow(arrowTexture, player.Position, arrowSpeed, -1, arrowDamage);

                        arrows.Add(arrow);
                        fireRateTimer.Enabled = true;
                        shotsFired++;
                    }


                    for (int i = arrows.Count - 1; i >= 0; i--)
                    {
                        arrows[i].Update(gameTime);

                        if (arrows[i].position.Y + arrows[i].arrowTexture.Height <= 0)
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
                        else if (mobs[j] is Pillager pillager)
                        {
                            if (pillager.offScreenTimer <= 0)
                            {

                                mobs.RemoveAt(j);
                            }
                        }
                        else if (mobs[j] is Skeleton skeleton)
                        {
                            if (skeleton.offScreenTimer <= 0)
                            {

                                mobs.RemoveAt(j);
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
                                shotsHit++;
                                arrows.RemoveAt(i);
                                break;
                            }
                            else if (mobs[j] is Villager villager && villager.HandleCollisionWithArrow(arrows[i]))
                            {
                                arrows.RemoveAt(i);
                                shotsHit++;
                                break;
                            }

                            if (mobs[j] is Skeleton skeleton && skeleton.HandleCollisionWithArrow(arrows[i]))
                            {
                                arrows.RemoveAt(i);
                                shotsHit++;
                                break;
                            }


                            else if (mobs[j] is Pillager pillager && pillager.HandleCollisionWithArrow(arrows[i]))
                            {
                                arrows.RemoveAt(i);
                                shotsHit++;
                                break;
                            }
                            else if (mobs[j] is Enderman enderman && enderman.HandleCollisionWithArrow(arrows[i]))
                            {
                                arrows.RemoveAt(i);
                                shotsHit++;
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < mobs.Count; i++)
                    {
                        if (mobs[i] is Skeleton skeleton)
                        {
                            if (skeleton.CheckPlayerCollisionWithSkeletonArrows(player))
                            {
                                player.Speed = 3f;
                                fireRateTimer.Interval = initialFireRateInterval;

                                damageBuffActive = false;
                                speedBuffActive = false;
                                rateOfFireBuffActive = false;
                                pointsBuffActive = false;
                            }
                        }
                    }

                    for (int j = mobs.Count - 1; j >= 0; j--)
                    {
                        if (mobs[j] is Creeper creeper && creeper.ToRemove)
                        {
                            mobs.RemoveAt(j);

                            if (creeper.Hp <= 0)
                            {
                                if (pointsBuffActive)
                                {
                                    playerScore += 80;
                                }
                                else
                                {
                                    playerScore += 40;
                                }

                                killsInCurrentLevel++;
                            }
                            else if (creeper.Exploded)
                            {
                                playerScore -= 40;
                            }
                        }
                        else if (mobs[j] is Skeleton skeleton && skeleton.ToRemove)
                        {
                            mobs.RemoveAt(j);

                            if (pointsBuffActive)
                            {
                                playerScore += 50;
                            }
                            else
                            {
                                playerScore += 25;
                            }

                            if (skeleton.Hp <= 0)
                            {
                                killsInCurrentLevel++;
                            }
                        }
                        else if (mobs[j] is Pillager pillager && pillager.ToRemove)
                        {
                            mobs.RemoveAt(j);

                            if (pointsBuffActive)
                            {
                                playerScore += 50;
                            }
                            else
                            {
                                playerScore += 25;
                            }

                            if (pillager.Hp <= 0)
                            {
                                killsInCurrentLevel++;
                            }

                        }
                        else if (mobs[j] is Enderman enderman && enderman.ToRemove)
                        {
                            mobs.RemoveAt(j);

                            if (enderman.Hp <= 0)
                            {
                                if (pointsBuffActive)
                                {
                                    playerScore += 200;
                                }
                                else
                                {
                                    playerScore += 10;
                                }

                                killsInCurrentLevel++;

                            }
                        }
                        else if (mobs[j] is Villager villager && villager.ToRemove)
                        {
                            mobs.RemoveAt(j);

                            if (pointsBuffActive)
                            {
                                playerScore += 20;
                            }
                            else
                            {
                                playerScore += 10;
                            }

                            if (villager.Hp <= 0)
                            {
                                killsInCurrentLevel++;

                            }
                        }
                    }


                    if (totalMobsInCurrentLevel == 0 && mobs.Count == 0)
                    {
                        resultsScreenDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;

                        if (resultsScreenDelay >= resultsScreenDelayDuration)
                        {
                            currentGameState = RESULTS;
                            resultsScreenDelay = 0f; // Reset the delay for the next time
                        }
                    }
                    else
                    {
                        resultsScreenDelay = 0f; // Reset the delay if the level is not complete
                    }


                    if (currentGameState == GAMEPLAY)
                    {
                        if (MediaPlayer.State != MediaState.Playing)
                        {
                            MediaPlayer.IsRepeating = true;
                            //MediaPlayer.Play(backgroundMusic);
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

                    if (currentGameState == RESULTS)
                    {
                        if (MediaPlayer.State != MediaState.Playing)
                        {
                            MediaPlayer.IsRepeating = true;
                            //  MediaPlayer.Play(resultMusic);
                        }
                    }
                    else
                    {
                        MediaPlayer.Stop();
                    }


                    currentLevel = level.currentLevel;
                    levelScores[currentLevel - 1] = playerScore;

                    if (shotsFired > 0)
                    {
                        hitPercentage = (double)shotsHit / shotsFired * 100;
                    }
                    else
                    {
                        hitPercentage = 0;
                    }

                    // Check for user input to proceed to the next level
                    if (keyboardState.IsKeyDown(Keys.Enter))
                    {
                        // Reset variables for the next level
                        killsInCurrentLevel = 0;
                        shotsFired = 0;
                        shotsHit = 0;

                        currentGameState = SHOP;
                        StartNextLevel();
                    }
                    break;

                case SHOP:

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        // Check for clicks on the Speed Buff rectangle and purchase conditions
                        if (!speedBuffActive && playerScore >= speedBuffCost && speedBuffRect.Contains(mouseState.Position))
                        {
                            speedBuffActive = true;
                            playerScore -= speedBuffCost;
                            player.Speed *= 2;
                        }

                        // Check for clicks on the Damage Buff rectangle and purchase conditions
                        if (!damageBuffActive && playerScore >= damageBuffCost && damageBuffRect.Contains(mouseState.Position))
                        {
                            damageBuffActive = true;
                            playerScore -= damageBuffCost;
                            arrowDamage = 3;
                        }

                        // Check for clicks on the Rate of Fire Buff rectangle and purchase conditions
                        if (!rateOfFireBuffActive && playerScore >= rateOfFireBuffCost && rateOfFireRect.Contains(mouseState.Position))
                        {
                            rateOfFireBuffActive = true;
                            playerScore -= rateOfFireBuffCost;
                            fireRateTimer.Interval = initialFireRateInterval / 2;
                        }


                        // Check for clicks on the Points Buff rectangle and purchase conditions
                        if (!pointsBuffActive && playerScore >= pointsBuffCost && pointsBuffRect.Contains(mouseState.Position))
                        {
                            pointsBuffActive = true;
                            playerScore -= pointsBuffCost;
                        }
                    }
                    else
                    {
                        showPopup = false;
                    }

                    // Show popup when hovering over the Rate of Fire Buff rectangle
                    if (rateOfFireRect.Contains(mouseState.Position))
                    {
                        showPopup = true;
                        popupText = $"Rate of Fire Upgrade\nPrice: {rateOfFireBuffCost}";


                        if (playerScore >= rateOfFireBuffCost && !rateOfFireBuffActive)
                        {
                            popupColor = Color.Green;
                        }
                        else
                        {
                            popupColor = Color.Red;
                        }
                    }
                    // Show popup when hovering over the Speed Buff rectangle
                    else if (speedBuffRect.Contains(mouseState.Position))
                    {
                        showPopup = true;
                        popupText = $"Speed Upgrade\nPrice: {speedBuffCost}";

                        if (playerScore >= speedBuffCost && !speedBuffActive)
                        {
                            popupColor = Color.Green;
                        }
                        else
                        {
                            popupColor = Color.Red;
                        }
                    }
                    // Show popup when hovering over the Damage Buff rectangle
                    else if (damageBuffRect.Contains(mouseState.Position))
                    {
                        showPopup = true;
                        popupText = $"Damage Upgrade\nPrice: {damageBuffCost}";

                        if (playerScore >= damageBuffCost && !damageBuffActive)
                        {
                            popupColor = Color.Green;
                        }
                        else
                        {
                            popupColor = Color.Red;
                        }
                    }
                    // Show popup when hovering over the Points Buff rectangle
                    else if (pointsBuffRect.Contains(mouseState.Position))
                    {
                        showPopup = true;
                        popupText = $"Points Upgrade\nPrice: {pointsBuffCost}";

                        if (playerScore >= pointsBuffCost && !pointsBuffActive)
                        {
                            popupColor = Color.Green;
                        }
                        else
                        {
                            popupColor = Color.Red;
                        }
                    }
                    else
                    {
                        showPopup = false;
                    }

                    // Check for click on a "Continue" or "Exit Shop" button
                    // and change the game state back to the pre-level instructions or the next level
                    if (mouseState.LeftButton == ButtonState.Pressed && continueButtonRect.Contains(mouseState.Position))
                    {
                        currentGameState = GAMEPLAY;
                    }
                    // Update active buffs display and behavior (deactivate on player hit by Skeleton arrow)
                    // Check for player hit by Skeleton arrow and deactivate buffs accordingly



                    if (continueButtonRect.Contains(mouseState.Position))
                    {
                        continueButtonColor = Color.Yellow;

                    }
                    else
                    {
                        continueButtonColor = Color.White;

                    }

                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            switch (currentGameState)
            {

                case MENU:
                    GraphicsDevice.Clear(Color.Green);

                    spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    spriteBatch.Draw(menuTitle, titlePosition, Color.White);

                    spriteBatch.Draw(button, button1Position + buttonOrigin, null, Color.White, 0, buttonOrigin, buttonScale, SpriteEffects.None, 0);
                    spriteBatch.DrawString(mainFont, "PLAY GAME", button1Position + new Vector2(button.Width / 2, button.Height / 2) - mainFont.MeasureString("PLAY GAME") / 2, button1Color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

                    spriteBatch.Draw(button, button2Position + buttonOrigin, null, Color.White, 0, buttonOrigin, buttonScale, SpriteEffects.None, 0);
                    spriteBatch.DrawString(mainFont, "SHOP", button2Position + new Vector2(button.Width / 2, button.Height / 2) - mainFont.MeasureString("SHOP") / 2, button2Color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

                    spriteBatch.Draw(button, button3Position + buttonOrigin, null, Color.White, 0, buttonOrigin, buttonScale, SpriteEffects.None, 0);
                    spriteBatch.DrawString(mainFont, "EXIT", button3Position + new Vector2(button.Width / 2, button.Height / 2) - mainFont.MeasureString("EXIT") / 2, button3Color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    break;

                case GAMEPLAY:
                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    DrawLevelScreen();
                    DrawItemsOnScreen();
                    DrawIcons();



                    scoreTextGame = "Score: " + playerScore.ToString();

                    spriteBatch.DrawString(statsFont, scoreTextGame, scoreTextPositionGame, Color.White);


                    break;

                case STATS:
                    // Draw logic for STATS state
                    break;

                case INSTRUCTIONS:
                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    DrawLevelScreen();

                    spriteBatch.Draw(semiTransparentBackground, semiTransparentBackgroundRec, Color.White * 0.4f);

                    instructionHeader = "INSTRUCTIONS";
                    spriteBatch.DrawString(mainFont, instructionHeader, instructionHeaderPosition, Color.Yellow);

                    moveInstructions = "MOVE: Left and Right arrow keys";
                    spriteBatch.DrawString(smallText, moveInstructions, moveInstructionsPos, Color.White);
                    shootInstructions = "SHOOT: Spacebar";
                    spriteBatch.DrawString(smallText, shootInstructions, shootInstructionsPos, Color.White);
                    goalInstructions = "Kill all the mobs before they go off the screen!";
                    spriteBatch.DrawString(smallText, goalInstructions, goalInstructionsPos, Color.White);

                    spriteBatch.Draw(button, continueButtonPosition + buttonOrigin, null, Color.White, 0, buttonOrigin, buttonScale, SpriteEffects.None, 0);
                    spriteBatch.DrawString(mainFont, "CONTINUE", continueButtonPosition + new Vector2(button.Width / 2, button.Height / 2) - mainFont.MeasureString("CONTINUE") / 2, continueButtonColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);


                    break;

                case RESULTS:
                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    DrawLevelScreen();

                    spriteBatch.Draw(semiTransparentBackground, semiTransparentBackgroundRec, Color.White * 0.4f);

                    // Draw score on top
                    scoreText = "SCORE: " + playerScore.ToString();
                    spriteBatch.DrawString(mainFont, scoreText, scoreTextPosition, Color.Yellow, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    tempLevelScoresPosition = levelScoresPosition;
                    // Draw level scores on the left side

                    for (int i = 0; i < levelScores.Length; i++)
                    {
                        levelScoreText = $"Level {i + 1}: {levelScores[i]}";
                        spriteBatch.DrawString(statsFont, levelScoreText, tempLevelScoresPosition, Color.White);
                        tempLevelScoresPosition.Y += statsFont.LineSpacing;
                    }

                    // Draw Level, Kills, ShotsFired, ShotsHit, and Hit% on the right side
                    Vector2 tempStatsPosition = statsPosition;
                    string levelText = "Level: " + currentLevel;
                    spriteBatch.DrawString(statsFont, levelText, tempStatsPosition, Color.White);

                    tempStatsPosition.Y += statsFont.LineSpacing;
                    string killsText = "Kills: " + killsInCurrentLevel;
                    spriteBatch.DrawString(statsFont, killsText, tempStatsPosition, Color.White);

                    tempStatsPosition.Y += statsFont.LineSpacing;
                    string shotsFiredText = "Shots Fired: " + shotsFired;
                    spriteBatch.DrawString(statsFont, shotsFiredText, tempStatsPosition, Color.White);

                    tempStatsPosition.Y += statsFont.LineSpacing;
                    string shotsHitText = "Shots Hit: " + shotsHit;
                    spriteBatch.DrawString(statsFont, shotsHitText, tempStatsPosition, Color.White);

                    tempStatsPosition.Y += statsFont.LineSpacing;
                    string hitPercentageText = "Hit%: " + hitPercentage.ToString("0.00") + "%";
                    spriteBatch.DrawString(statsFont, hitPercentageText, tempStatsPosition, Color.White);

                    string continueMessage = "Press Enter To Continue";
                    spriteBatch.DrawString(mainFont, continueMessage, continueMessagePosition, Color.White);

                    break;

                case SHOP:
                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    spriteBatch.Draw(shopBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    spriteBatch.Draw(shopTitle, titlePosition, Color.White);

                    scoreTextGame = "Score: " + playerScore.ToString();

                    spriteBatch.DrawString(statsFont, scoreTextGame, scoreTextPositionGame, Color.White);

                    // Draw the shop item

                    spriteBatch.Draw(itemFrame, itemFrame1Pos, Color.White);
                    spriteBatch.Draw(itemFrame, itemFrame2Pos, Color.White);
                    spriteBatch.Draw(itemFrame, itemFrame3Pos, Color.White);
                    spriteBatch.Draw(itemFrame, itemFrame4Pos, Color.White);

                    spriteBatch.Draw(rateOfFireBuff, rateofFirePos, Color.White);
                    spriteBatch.Draw(pointsBuff, pointsBuffPos, Color.White);
                    spriteBatch.Draw(speedBuff, speedBuffPos, Color.White);
                    spriteBatch.Draw(damageBuff, damageBuffPos, Color.White);
                    if (showPopup)
                    {
                        spriteBatch.Draw(pixel, popupBackgroundRect, Color.Black * 0.8f);
                        spriteBatch.DrawString(statsFont, popupText, popupTextPosition, popupColor);
                    }
                    continueButtonPosition = new Vector2(centerX - button.Width / 2, box2Y + box2Height - verticalPadding - button.Height);
                    spriteBatch.Draw(button, continueButtonPosition + buttonOrigin, null, Color.White, 0, buttonOrigin, buttonScale, SpriteEffects.None, 0);
                    spriteBatch.DrawString(mainFont, "CONTINUE", continueButtonPosition + new Vector2(button.Width / 2, button.Height / 2) - mainFont.MeasureString("CONTINUE") / 2, continueButtonColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    break;

            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawLevelScreen()
        {
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

        }

        private void DrawItemsOnScreen()
        {
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

            foreach (Enderman enderman in mobs.OfType<Enderman>())
            {
                enderman.Draw(spriteBatch);
            }

            player.Draw(spriteBatch);
        }
        private void DrawIcons()
        {
            // Draw the Speed Buff icon
            Color speedBuffColor;
            if (speedBuffActive)
            {
                speedBuffColor = Color.White;
            }
            else
            {
                speedBuffColor = new Color(255, 255, 255, 128);
            }
            spriteBatch.Draw(speedBuffIcon, iconPositions[0], speedBuffColor);

            // Draw the Damage Buff icon
            Color damageBuffColor;
            if (damageBuffActive)
            {
                damageBuffColor = Color.White;
            }
            else
            {
                damageBuffColor = new Color(255, 255, 255, 128);
            }
            spriteBatch.Draw(damageBuffIcon, iconPositions[1], damageBuffColor);

            Color pointsBuffColor;
            if (pointsBuffActive)
            {
                pointsBuffColor = Color.White;
            }
            else
            {
                pointsBuffColor = new Color(255, 255, 255, 128);
            }
            spriteBatch.Draw(pointsBuffIcon, iconPositions[2], pointsBuffColor);

            // Draw the Rate of Fire Buff icon
            Color rateOfFireBuffColor;
            if (rateOfFireBuffActive)
            {
                rateOfFireBuffColor = Color.White;
            }
            else
            {
                rateOfFireBuffColor = new Color(255, 255, 255, 128);
            }
            spriteBatch.Draw(rateOfFireBuffIcon, iconPositions[3], rateOfFireBuffColor);

        }

        private void StartNextLevel()
        {
            level.currentLevel++;
            levelInitialized = false;
        }
    }
}