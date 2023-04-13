//Author: Idan Gurevich
//File Name: Game1.CS
//Project Name: GurevichI_PASS2
//Creation Date: March 25, 2023
//Modified Date: April 12, 2023
//Description: This driver class is responsible for updating the game state and handling user input.
//It contains methods for updating the game in different states, such as gameplay, stats, shop, and menu.
//The driver class also includes code for displaying buttons and handling button clicks.



using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;


namespace GurevichI_PASS2
{
    public class Game1 : Game
    {
        //constants used for positioning and sizing in the game
        private const float INSTRUCTION_HEADER_VERTICAL = 1f;
        private const float MOVE_INSTRUCTION_HEADER_VERTICAL = 3f;
        private const float GOAL_INSTRUCTION_HEADER_VERTICAL = 4f;
        private const float SHOOT_INSTRUCTION_HEADER_VERTICAL = 2f;
        private const float HORIZONTAL_PADDING_FACTOR = 0.1f;
        private const float VERTICAL_PADDING_FACTOR = 0.1f;
        private const int ICON_PADDING = 10;
        private const int BUTTON_SPACING = 10;
        private const int BUTTON_WIDTH_OFFSET = 50;
        private const int TITLE_BOTTOM_OFFSET = 200;
        private const float BUTTON_SCALE = 0.8f;
        private const int POPUP_BACKGROUND_WIDTH = 350;
        private const int POPUP_BACKGROUND_HEIGHT = 100;

        //Keyboard and mouse state
        KeyboardState keyboardState;
        MouseState mouseState;

        //Buff Costs
        private const int SPEED_BUFF_COST = 100;
        private const int DAMAGE_BUFF_COST = 200;
        private const int RATE_OF_FIRE_BUFF_COST = 300;
        private const int POINTS_COST = 500;

        //public varaibles
        public static SoundEffect endermanTeleport;
        public static SoundEffect endermanScream;
        public static SoundEffect shieldHit;
        public static SoundEffect explode;
        public int currentGameState = MENU;
        public static Random random = new Random();
        public Arrow arrow;
        public Skeleton skeleton;

        //Gamestates
        private const int MENU = 0;
        private const int GAMEPLAY = 1;
        private const int STATS = 2;
        private const int INSTRUCTIONS = 3;
        private const int RESULTS = 4;
        private const int SHOP = 5;

        //Monogame Managers
        private GraphicsDeviceManager graphics;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;
        private ContentManager content;

        //Background Musics
        private Song backgroundMusic;
        private Song menuMusic;
        private Song resultMusic;

        //Sound effects
        private SoundEffect buyAccept;
        private SoundEffect bowShoot;
        private SoundEffect arrowHit;
        private SoundEffect buttonClick;

        //Classes
        private Player player;
        private Level level;

        //Arrow Damage and speed
        private int arrowDamage;
        private float arrowSpeed = 5f;

        //Stats
        private int playerScore = 0;
        private int[] levelScores = new int[5];
        private int currentLevel;
        private int killsInCurrentLevel;
        private int shotsFired;
        private int shotsFiredInRound;
        private int shotsHit;
        private int shotsHitInRound;
        private double hitPercentage;
        private int highScore;
        private int gamesPlayed;
        private float topHitPercentage;
        private int totalVillagersKilled;
        private int totalCreepersKilled;
        private int totalSkeletonsKilled;
        private int totalPillagersKilled;
        private int totalEndermenKilled;

        private int totalKills;
        private float allTimeHitPercentage;
        private float avgShotsPerGame;
        private float avgKillsPerGame;

        //Strings for ingame readiblity
        private string scoreTextGame;
        private string levelScoreText;
        private string continueMessage = "Press Enter To Continue";
        private string continueMessageResult = "Press Space To Continue";
        private string scoreText;
        private string instructionHeader;
        private string moveInstructions;
        private string shootInstructions;
        private string goalInstructions;
        private string popupText = "";

        //Stats file name
        private string statsFilename = "Statistics";

        //Lists for mobs, arrows, icon positions and level items
        private List<Arrow> arrows;
        private List<Mob> mobs;
        private List<Vector2> iconPositions = new List<Vector2>();
        private List<Rectangle> grass1Rectangles = new List<Rectangle>();
        private List<Rectangle> grass2Rectangles = new List<Rectangle>();
        private List<Rectangle> cobblestoneRectangles = new List<Rectangle>();
        private List<Rectangle> dirtRectangles = new List<Rectangle>();

        //Rectangles for GUI
        private Rectangle titleRect;
        private Rectangle button1Rect;
        private Rectangle button2Rect;
        private Rectangle button3Rect;
        private Rectangle continueButtonRect;
        private Rectangle continueButtonShopRect;
        private Rectangle speedBuffRect;
        private Rectangle pointsBuffRect;
        private Rectangle damageBuffRect;
        private Rectangle rateOfFireRect;
        private Rectangle popupBackgroundRect;
        private Rectangle semiTransparentBackgroundRec;

        //All positions for everything in the game
        private Vector2 titlePosition;
        private Vector2 button1Position;
        private Vector2 button2Position;
        private Vector2 button3Position;
        private Vector2 levelScoresPosition;
        private Vector2 statsPosition;
        private Vector2 scoreTextPosition;
        private Vector2 continueButtonPosition;
        private Vector2 scoreTextPositionGame;
        private Vector2 scoreTextPositionShop;
        private Vector2 tempLevelScoresPosition;
        private Vector2 instructionHeaderPosition;
        private Vector2 moveInstructionsPos;
        private Vector2 shootInstructionsPos;
        private Vector2 goalInstructionsPos;
        private Vector2 rateofFirePos;
        private Vector2 pointsBuffPos;
        private Vector2 speedBuffPos;
        private Vector2 damageBuffPos;
        private Vector2 rateOfFireBuffCenter;
        private Vector2 speedBuffCenter;
        private Vector2 pointsBuffCenter;
        private Vector2 damageBuffCenter;
        private Vector2 buttonOrigin;
        private Vector2 continueMessagePosition;
        private Vector2 continueMessageResultPosition;
        private Vector2 continueMessagePositionStats;
        private Vector2 continueButtonPositionShop;
        private Vector2 itemFrame1Pos;
        private Vector2 itemFrame2Pos;
        private Vector2 itemFrame3Pos;
        private Vector2 itemFrame4Pos;
        private Vector2 popupTextPosition;
        private Vector2 popupPosition;

        //All loadable textures
        private Texture2D playerTexture;
        private Texture2D cobblestoneTexture;
        private Texture2D grass1Texture;
        private Texture2D grass2Texture;
        private Texture2D dirtTexture;
        private Texture2D background;
        private Texture2D shopBackground;
        private Texture2D button;
        private Texture2D menuTitle;
        private Texture2D shopTitle;
        private Texture2D statsTitle;
        private Texture2D arrowTexture;
        private Texture2D semiTransparentBackground;
        private Texture2D speedBuff;
        private Texture2D damageBuff;
        private Texture2D pointsBuff;
        private Texture2D rateOfFireBuff;
        private Texture2D speedBuffIcon;
        private Texture2D damageBuffIcon;
        private Texture2D pointsBuffIcon;
        private Texture2D rateOfFireBuffIcon;
        private Texture2D itemFrame;
        private Texture2D pixel;

        //Box Dimensions
        private int boxWidth;
        private int boxHeight;
        private int box2Height;
        private int boxX;
        private int boxY;
        private int box2Y;
        //Buff Bools
        private bool speedBuffActive = false;
        private bool damageBuffActive = false;
        private bool rateOfFireBuffActive = false;
        private bool pointsBuffActive = false;
        private bool statsUpdated = false;

        //Shop popup bool
        private bool showPopup = false;

        //Color Variable for the popup
        private Color popupColor = Color.White;

        //Padding, scales and differnt positions
        private float horizontalPadding;
        private float verticalPadding;
        private float centerX;
        private float centerY;
        private float startX;
        private float startY;
        private float startXRight;
        private float lineHeight;

        //Font Variables
        private SpriteFont mainFont;
        private SpriteFont statsFont;
        private SpriteFont smallText;

        //Colors for buttons
        private Color button1Color = Color.White;
        private Color button2Color = Color.White;
        private Color button3Color = Color.White;
        private Color continueButtonColor = Color.White;

        //Level Variables
        private float elapsedSpawnTime = 0f;
        private int totalMobsInCurrentLevel;
        private int maxMobsOnScreen;
        private float spawnTime;
        private bool levelInitialized = false;

        //Timer Variables
        private Timer fireRateTimer;
        private double initialFireRateInterval = 333;
        private float resultsScreenDelay = 0f;
        private float resultsScreenDelayDuration = 1f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //Initialize list for arrows
            arrows = new List<Arrow>();

            //Initialize timer for the fire rate
            fireRateTimer = new Timer(initialFireRateInterval);
            fireRateTimer.Elapsed += (sender, args) => fireRateTimer.Enabled = false;

            //Initialize mob list
            mobs = new List<Mob>();

            //Set the Volume for the music and sound effects
            SoundEffect.MasterVolume = 1f;
            MediaPlayer.Volume = 0.6f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            content = new ContentManager(Content.ServiceProvider, "content"); // initialize the content manager
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphicsDevice = GraphicsDevice;

            //Call all methods for the loadcontent method which provide for the whole code
            InitializeVariables();
            LoadFonts();
            LoadPlayerRelatedAssets();
            LoadLevelRelatedAssets();
            LoadMenuRelatedAssets();
            LoadSoundRelatedAssets();
            SetupLineHeightAndPixel(statsFont, graphicsDevice);
            SetupButtonOriginsAndTitlePositions(button, menuTitle, graphicsDevice);
            SetupButtonPositions(button, titleRect, graphicsDevice);
            SetupOtherPositions();
            GenerateBackground();

            //Import stats
            ImportStatisticsToFile();

            //Load the Player and Level for the update method
            player = new Player(playerTexture, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - playerTexture.Height), 3f);
            level = new Level(content, graphicsDevice, player);

        }

        protected override void Update(GameTime gameTime)
        {
            //Set the keyboard and mouse state for the whole update method
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            switch (currentGameState)
            {
                case MENU:
                    //Call on the updatemenu method
                    UpdateMenu(mouseState);
                    break;

                case INSTRUCTIONS:
                    //Call on the updateinstructions method
                    UpdateInstructions(mouseState);
                    break;

                case GAMEPLAY:
                    //Call on the updateGameplay method
                    UpdateGamePlay(keyboardState, gameTime);
                    break;

                case RESULTS:
                    //call on the updateresults method
                    UpdateResults(keyboardState);
                    break;

                case STATS:
                    //call on the updatestats method
                    UpdateStats(keyboardState);
                    break;

                case SHOP:
                    //call on the updateShop Method
                    UpdateShop(keyboardState, mouseState);
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
                    //Clear the screen
                    GraphicsDevice.Clear(Color.Green);

                    //Draw background and menu title
                    spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    spriteBatch.Draw(menuTitle, titlePosition, Color.White);

                    //Draw the first button saying play game
                    spriteBatch.Draw(button, button1Position + buttonOrigin, null, Color.White, 0, buttonOrigin, BUTTON_SCALE, SpriteEffects.None, 0);
                    spriteBatch.DrawString(mainFont, "PLAY GAME", button1Position + new Vector2(button.Width / 2, button.Height / 2) - mainFont.MeasureString("PLAY GAME") / 2, button1Color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

                    //Draw the second button saying stats
                    spriteBatch.Draw(button, button2Position + buttonOrigin, null, Color.White, 0, buttonOrigin, BUTTON_SCALE, SpriteEffects.None, 0);
                    spriteBatch.DrawString(mainFont, "STATS", button2Position + new Vector2(button.Width / 2, button.Height / 2) - mainFont.MeasureString("STATS") / 2, button2Color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

                    //Draw the third button saying Exit
                    spriteBatch.Draw(button, button3Position + buttonOrigin, null, Color.White, 0, buttonOrigin, BUTTON_SCALE, SpriteEffects.None, 0);
                    spriteBatch.DrawString(mainFont, "EXIT", button3Position + new Vector2(button.Width / 2, button.Height / 2) - mainFont.MeasureString("EXIT") / 2, button3Color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    break;

                case GAMEPLAY:
                    //Clear the screen
                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    //Call on the Level screen method, itemscreen method and icon to draw the gameplay screen
                    DrawLevelScreen();
                    DrawItemsOnScreen();
                    DrawIcons();

                    //Display the score at the top right of the screen
                    scoreTextGame = "Score: " + playerScore.ToString();
                    spriteBatch.DrawString(statsFont, scoreTextGame, scoreTextPositionGame, Color.Yellow);
                    break;

                case STATS:
                    //Clear the screen
                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    //Draw the shop background with the stats tile 
                    spriteBatch.Draw(shopBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    spriteBatch.Draw(statsTitle, titlePosition, Color.White);

                    // Draw the statistics on the left side
                    spriteBatch.DrawString(statsFont, $"High Score: {highScore}", new Vector2(startX, startY), Color.White);
                    spriteBatch.DrawString(statsFont, $"Games Played: {gamesPlayed}", new Vector2(startX, startY + lineHeight), Color.White);
                    spriteBatch.DrawString(statsFont, $"Shots Fired: {shotsFired}", new Vector2(startX, startY + lineHeight * 2), Color.White);
                    spriteBatch.DrawString(statsFont, $"Shots Hit: {shotsHit}", new Vector2(startX, startY + lineHeight * 3), Color.White);
                    spriteBatch.DrawString(statsFont, $"Top Hit %: {topHitPercentage}", new Vector2(startX, startY + lineHeight * 4), Color.White);
                    spriteBatch.DrawString(statsFont, $"Average Shots: {avgShotsPerGame}", new Vector2(startX, startY + lineHeight * 5), Color.White);
                    spriteBatch.DrawString(statsFont, $"Average Kills: {avgKillsPerGame}", new Vector2(startX, startY + lineHeight * 6), Color.White);

                    // Draw the statistics on the right side
                    spriteBatch.DrawString(statsFont, $"Total Villagers Killed: {totalVillagersKilled}", new Vector2(startXRight, startY), Color.White);
                    spriteBatch.DrawString(statsFont, $"Total Creepers Killed: {totalCreepersKilled}", new Vector2(startXRight, startY + lineHeight * 1), Color.White);
                    spriteBatch.DrawString(statsFont, $"Total Skeletons Killed: {totalSkeletonsKilled}", new Vector2(startXRight, startY + lineHeight * 2), Color.White);
                    spriteBatch.DrawString(statsFont, $"Total Pillagers Killed: {totalPillagersKilled}", new Vector2(startXRight, startY + lineHeight * 3), Color.White);
                    spriteBatch.DrawString(statsFont, $"Total Enderman Killed: {totalEndermenKilled}", new Vector2(startXRight, startY + lineHeight * 4), Color.White);
                    spriteBatch.DrawString(statsFont, $"Total Kills: {totalKills}", new Vector2(startXRight, startY + lineHeight * 5), Color.White);
                    spriteBatch.DrawString(statsFont, $"All-Time Hit %: {allTimeHitPercentage}", new Vector2(startXRight, startY + lineHeight * 6), Color.White);

                    //Draw the continue message at the bottom of the screen
                    spriteBatch.DrawString(mainFont, continueMessage, continueMessagePositionStats, Color.White);
                    break;

                case INSTRUCTIONS:
                    //Clear screen
                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    //Draw the level screen as the background for the instructions
                    DrawLevelScreen();

                    //Draw a semi-transparent rectangle on top of the background
                    spriteBatch.Draw(semiTransparentBackground, semiTransparentBackgroundRec, Color.White * 0.4f);

                    //Draw Header
                    instructionHeader = "INSTRUCTIONS";
                    spriteBatch.DrawString(mainFont, instructionHeader, instructionHeaderPosition, Color.Yellow);

                    //Draw instructions for user
                    moveInstructions = "MOVE: Left and Right arrow keys";
                    spriteBatch.DrawString(smallText, moveInstructions, moveInstructionsPos, Color.White);
                    shootInstructions = "SHOOT: Spacebar";
                    spriteBatch.DrawString(smallText, shootInstructions, shootInstructionsPos, Color.White);
                    goalInstructions = "Kill all the mobs before they go off the screen!";
                    spriteBatch.DrawString(smallText, goalInstructions, goalInstructionsPos, Color.White);

                    //Draw the continue button at the bottom of the screen
                    spriteBatch.Draw(button, continueButtonPosition + buttonOrigin, null, Color.White, 0, buttonOrigin, BUTTON_SCALE, SpriteEffects.None, 0);
                    spriteBatch.DrawString(mainFont, "CONTINUE", continueButtonPosition + new Vector2(button.Width / 2, button.Height / 2) - mainFont.MeasureString("CONTINUE") / 2, continueButtonColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    break;

                case RESULTS:
                    //Clear screen
                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    //Draw the level screeen as the background for the instructions
                    DrawLevelScreen();

                    //Draw a semi-transparent rectangle on top of the background
                    spriteBatch.Draw(semiTransparentBackground, semiTransparentBackgroundRec, Color.White * 0.4f);

                    // Draw score on top of the screen
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
                    spriteBatch.DrawString(statsFont, levelText, statsPosition, Color.White);

                    tempStatsPosition.Y += statsFont.LineSpacing;
                    string killsText = "Kills: " + killsInCurrentLevel;
                    spriteBatch.DrawString(statsFont, killsText, tempStatsPosition, Color.White);

                    tempStatsPosition.Y += statsFont.LineSpacing;
                    string shotsFiredText = "Shots Fired: " + shotsFiredInRound;
                    spriteBatch.DrawString(statsFont, shotsFiredText, tempStatsPosition, Color.White);

                    tempStatsPosition.Y += statsFont.LineSpacing;
                    string shotsHitText = "Shots Hit: " + shotsHitInRound;
                    spriteBatch.DrawString(statsFont, shotsHitText, tempStatsPosition, Color.White);

                    tempStatsPosition.Y += statsFont.LineSpacing;
                    string hitPercentageText = "Hit%: " + hitPercentage.ToString("0.00") + "%";
                    spriteBatch.DrawString(statsFont, hitPercentageText, tempStatsPosition, Color.White);

                    //Draw the continue message as the bottom of the screen
                    spriteBatch.DrawString(mainFont, continueMessageResult, continueMessagePosition, Color.White);
                    break;

                case SHOP:
                    //Clear screen
                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    //Draw the shop background and title on top of it 
                    spriteBatch.Draw(shopBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    spriteBatch.Draw(shopTitle, titlePosition, Color.White);

                    //Draw the player score to inform player what they can purchase
                    scoreTextGame = "Score: " + playerScore.ToString();
                    spriteBatch.DrawString(mainFont, scoreTextGame, scoreTextPositionShop, Color.White);

                    // Draw the shop item on the item frames
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
                        //If the popup is true draw the black box in the middle of the screen with the related text on it
                        spriteBatch.Draw(pixel, popupBackgroundRect, Color.Black * 0.8f);
                        spriteBatch.DrawString(statsFont, popupText, popupTextPosition, popupColor);
                    }

                    //Draw the continue message at the bottom of the screen
                    spriteBatch.DrawString(statsFont, continueMessage, continueButtonPositionShop, Color.White);
                    break;

            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        //Pre: None
        //Post: None
        //Desc: This subprogram generates a random background image for a game by filling the screen with grass and cobblestone textures. It doesn't take any input parameters or return any values.
        private void GenerateBackground()
        {
            // Generate grass and cobblestone at a random rate
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

        //Pre: grass1Rectangles, grass2Rectangles, cobblestoneRectangles, dirtRectangles, spriteBatch, grass1Texture, grass2Texture, cobblestoneTexture and dirtTexture
        //Post: None
        //Desc: This subprogram draws rectangles related to the levels background image by iterating over the lists of rectangles that were generated by the previous subprogram.
        private void DrawLevelScreen()
        {
            //Draw all the rects relating to the levels background
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

        //Pre: arrows, mobs, player and spritebatch
        //Post: None
        //Desc: This subprogram draws the game objects on the screen, including arrows, mobs, and the player.
        private void DrawItemsOnScreen()
        {
            //Draw the arrow which is spawned when space is pressed
            foreach (Arrow arrow in arrows)
            {
                arrow.Draw(spriteBatch);
            }

            //Draw each mob which is inherited from the mob class onto the screen
            foreach (Mob mob in mobs)
            {
                mob.Draw(spriteBatch);

            }

            //Draw the player on the screen
            player.Draw(spriteBatch);
        }

        //Pre: spriteBatch, speedBuffIcon, damageBuffIcon, pointsBuffIcon, rateOfFIreBuffIcon, speedBuffActive, damageBuffActive, pointsBuffActive, rateOfFireBuffActive and iconPositions
        //Post: None
        //Desc: This subprogram draws buff icons on the screen based on whether the corresponding buff is active or not.
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

        //Pre: level and levelInitialized
        //Post: None
        //Desc: This subprogram advances the game to the next level by incrementing the currentLevel variable of the level object and setting levelInitialized to false
        private void StartNextLevel()
        {
            //Add 1 to current level
            level.currentLevel++;

            //Level ready = false so when ready it doesn't skip one
            levelInitialized = false;
        }

        //Pre: statsFilename
        //Post: None
        //Desc: This subprogram writes the game statistics to a file using the File.WriteAllLines method.
        //It creates a new string array with the game statistics converted to strings and writes this array as a single line in the specified file.
        private void ExportStatisticsToFile()
        {
            //Write all the variables into a file to export
            File.WriteAllLines(statsFilename, new string[]{ highScore.ToString(), gamesPlayed.ToString(),
        shotsFired.ToString(), shotsHit.ToString(), topHitPercentage.ToString(), totalVillagersKilled.ToString(),
        totalCreepersKilled.ToString(), totalSkeletonsKilled.ToString(), totalPillagersKilled.ToString(),
        totalEndermenKilled.ToString()});
        }

        //Pre: statsFilename
        //Post: None
        //Desc:This subprogram reads the game statistics from a file using the File.ReadAllLines method.
        //It reads each line of the file into a string array, then parses each value from the array into the corresponding variable type 
        private void ImportStatisticsToFile()
        {
            try
            {
                // Inputting all the different variables and statistics into the statistics file
                string[] lines = File.ReadAllLines(statsFilename);
                highScore = int.Parse(lines[0]);
                gamesPlayed = int.Parse(lines[1]);
                shotsFired = int.Parse(lines[2]);
                shotsHit = int.Parse(lines[3]);
                topHitPercentage = float.Parse(lines[4]);
                totalVillagersKilled = int.Parse(lines[5]);
                totalCreepersKilled = int.Parse(lines[6]);
                totalSkeletonsKilled = int.Parse(lines[7]);
                totalPillagersKilled = int.Parse(lines[8]);
                totalEndermenKilled = int.Parse(lines[9]);
            }
            catch (Exception)
            {
                // Handle the exception, e.g., log the error or display a message to the user
            }
        }

        //Pre: Mousestate and Gamestates
        //Post: None
        //Desc: This subprogram updates the menu screen by changing the color of buttons depending on the mouse position. If a button is clicked, the game switches to a corresponding state and plays a sound. 
        private void UpdateMenu(MouseState mouseState)
        {
            // Check if the mouse position is within the bounds of button1, and change its color accordingly
            if (button1Rect.Contains(mouseState.Position))
            {
                button1Color = Color.Yellow;
            }
            else
            {
                button1Color = Color.White;
            }
            // Check if the mouse position is within the bounds of button2, and change its color accordingly
            if (button2Rect.Contains(mouseState.Position))
            {
                button2Color = Color.Yellow;
            }
            else
            {
                button2Color = Color.White;
            }
            // Check if the mouse position is within the bounds of button3, and change its color accordingly
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
                // Play a sound effect for the button click
                buttonClick.CreateInstance().Play();

                // Change the game state to the instructions screen
                currentGameState = INSTRUCTIONS;
            }
            if (mouseState.LeftButton == ButtonState.Pressed && button2Rect.Contains(mouseState.Position))
            {
                // Play a sound effect for the button click
                buttonClick.CreateInstance().Play();

                // Change the game state to the stats screen
                currentGameState = STATS;
            }
            if (mouseState.LeftButton == ButtonState.Pressed && button3Rect.Contains(mouseState.Position))
            {
                // Exit the game if the user clicks the exit button
                Environment.Exit(0);
            }

            // Check the current game state and play or stop the menu music accordingly
            if (currentGameState == MENU)
            {
                // If the menu music is not currently playing, start playing it on repeat
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(menuMusic);
                }
            }
            else
            {
                // Stop the menu music if the game is not in the menu state
                MediaPlayer.Stop();
            }
        }

        //Pre: mousestate, continuebuttonRect, button click and gamestate
        //Post: None
        //Desc: This subprogram updates the insturctions screen by changing button colors and gamestate, aswell as sound effects
        private void UpdateInstructions(MouseState mouseState)
        {

            // Check if the left mouse button is pressed and the mouse is within the bounds of the continue button, and change the game state to gameplay
            if (mouseState.LeftButton == ButtonState.Pressed && continueButtonRect.Contains(mouseState.Position))
            {
                // Play a sound effect for the button click
                buttonClick.CreateInstance().Play();

                // Change the game state to gameplay
                currentGameState = GAMEPLAY;
            }

            // Check if the mouse is within the bounds of the continue button, and change its color accordingly
            if (continueButtonRect.Contains(mouseState.Position))
            {
                continueButtonColor = Color.Yellow;
            }
            else
            {
                continueButtonColor = Color.White;
            }
        }

        //Pre: None
        //Post: None
        //Desc:Update the main gameplay 
        private void UpdateGamePlay(KeyboardState keyboardState, GameTime gameTime)
        {
            // Update player state based on input, graphics, and time
            player.Update(keyboardState, graphicsDevice, gameTime);

            // Update the level state
            level.UpdateLevel();

            // Set player score to 0 if it's negative
            playerScore = Math.Max(playerScore, 0);

            // Check if spacebar is pressed, fire rate timer is disabled, and fear timer is not active
            if (keyboardState.IsKeyDown(Keys.Space) && fireRateTimer.Enabled == false && player.fearTimer <= 0)
            {
                // Play bow shooting sound
                bowShoot.CreateInstance().Play();

                // Set arrow damage to 3 if buff is active
                if (damageBuffActive)
                {
                    arrowDamage = 3;
                }
                // Set arrow damage to 1 if buff is not active
                else
                {
                    arrowDamage = 1;
                }

                // Create a new Arrow object with properties set
                Arrow arrow = new Arrow(arrowTexture, player.position, arrowSpeed, -1, arrowDamage);

                // Add the new arrow to the arrows list
                arrows.Add(arrow);

                // Enable the fire rate timer
                fireRateTimer.Enabled = true;

                // Increment shots fired in the current round and overall
                shotsFiredInRound++;
                shotsFired++;
            }
            // Update arrow position and properties
            for (int i = arrows.Count - 1; i >= 0; i--)
            {
                arrows[i].Update(gameTime);

                if (arrows[i].position.Y + arrows[i].arrowTexture.Height <= 0)
                {
                    arrows.RemoveAt(i);
                }
            }


            // Check if level is not initialized
            if (!levelInitialized)
            {
                // Update level, spawn time, max mobs on screen, and total mobs in current level
                level.UpdateLevel();
                spawnTime = level.SpawnTime;
                maxMobsOnScreen = level.MaxMobsOnScreen;
                totalMobsInCurrentLevel = level.TotalMobsInCurrentLevel;
                levelInitialized = true;
            }

            // Update elapsed spawn time
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
                mobs[i].Update(gameTime, player.position, GraphicsDevice);
            }


            // Check if a mob goes off-screen and remove it from the mobs list
            for (int j = mobs.Count - 1; j >= 0; j--)
            {
                // Check if the mob is a Villager and remove it if off-screen timer is expired
                if (mobs[j] is Villager villager)
                {
                    if (villager.offScreenTimer <= 0)
                    {
                        mobs.RemoveAt(j);
                    }
                }
                // Check if the mob is a Pillager and remove it if off-screen timer is expired
                else if (mobs[j] is Pillager pillager)
                {
                    if (pillager.offScreenTimer <= 0)
                    {
                        mobs.RemoveAt(j);
                    }
                }
                // Check if the mob is a Skeleton and remove it if off-screen timer is expired
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
                    // Check for collision with Creeper, handle it, and remove the arrow
                    if (mobs[j] is Creeper creeper && creeper.HandleCollisionWithArrow(arrows[i], GraphicsDevice, grass1Rectangles, grass2Rectangles, cobblestoneRectangles, dirtRectangles, dirtTexture))
                    {
                        arrows.RemoveAt(i);
                        shotsHitInRound++;
                        shotsHit++;
                        break;
                    }
                    // Check for collision with Villager, handle it, and remove the arrow
                    else if (mobs[j] is Villager villager && villager.HandleCollisionWithArrow(arrows[i]))
                    {
                        arrows.RemoveAt(i);
                        shotsHitInRound++;
                        shotsHit++;
                        break;
                    }
                    // Check for collision with Skeleton, handle it, and remove the arrow
                    else if (mobs[j] is Skeleton skeleton && skeleton.HandleCollisionWithArrow(arrows[i]))
                    {
                        arrows.RemoveAt(i);
                        shotsHitInRound++;
                        shotsHit++;
                        break;
                    }
                    // Check for collision with Pillager, handle it, and remove the arrow
                    else if (mobs[j] is Pillager pillager && pillager.HandleCollisionWithArrow(arrows[i]))
                    {
                        arrows.RemoveAt(i);
                        shotsHitInRound++;
                        shotsHit++;
                        break;
                    }
                    // Check for collision with Enderman, handle it, and remove the arrow
                    else if (mobs[j] is Enderman enderman && enderman.HandleCollisionWithArrow(arrows[i]))
                    {
                        arrows.RemoveAt(i);
                        shotsHitInRound++;
                        shotsHit++;
                        break;
                    }
                }
            }

            // Check for player collision with Skeleton arrows
            for (int i = 0; i < mobs.Count; i++)
            {
                if (mobs[i] is Skeleton skeleton)
                {
                    if (skeleton.CheckPlayerCollisionWithSkeletonArrows(player))
                    {
                        // Play arrow hit sound and reset player properties
                        arrowHit.CreateInstance().Play();
                        player.Speed = 3f;
                        fireRateTimer.Interval = initialFireRateInterval;

                        // Deactivate all buffs
                        damageBuffActive = false;
                        speedBuffActive = false;
                        rateOfFireBuffActive = false;
                        pointsBuffActive = false;

                        // Decrease player score
                        playerScore -= 10;
                    }
                }
            }

            // Loop through mobs in reverse to handle removal correctly
            for (int j = mobs.Count - 1; j >= 0; j--)
            {
                // Check if mob is a Creeper and marked for removal
                if (mobs[j] is Creeper creeper && creeper.ToRemove)
                {
                    // Remove the mob from the list
                    mobs.RemoveAt(j);
                    explode.CreateInstance().Play();
                    // Check if Creeper exploded
                    if (creeper.Exploded)
                    {
                        // Apply score penalty or bonus based on distance to explosion
                        if (Vector2.Distance(player.GetCenter(), creeper.GetCenter()) <= creeper.explosionRadius)
                        {
                            playerScore -= 40;
                        }
                        else
                        {
                            // Apply score bonus based on active buffs
                            if (pointsBuffActive)
                            {
                                playerScore += 80;
                            }
                            else
                            {
                                playerScore += 40;
                            }
                            totalCreepersKilled++;
                            killsInCurrentLevel++;
                        }
                    }
                }
                // Check if mob is a Skeleton and marked for removal
                else if (mobs[j] is Skeleton skeleton && skeleton.ToRemove)
                {
                    // Remove the mob from the list
                    mobs.RemoveAt(j);

                    // Apply score bonus based on active buffs
                    if (pointsBuffActive)
                    {
                        playerScore += 50;
                    }
                    else
                    {
                        playerScore += 25;
                    }
                    // Update kill statistics if the Skeleton's HP is depleted
                    if (skeleton.Hp <= 0)
                    {
                        totalSkeletonsKilled++;
                        killsInCurrentLevel++;
                    }
                }
                // Check if mob is a Pillager and marked for removal
                else if (mobs[j] is Pillager pillager && pillager.ToRemove)
                {
                    // Remove the mob from the list
                    mobs.RemoveAt(j);

                    // Apply score bonus based on active buffs
                    if (pointsBuffActive)
                    {
                        playerScore += 50;
                    }
                    else
                    {
                        playerScore += 25;
                    }
                    // Update kill statistics if the Pillager's HP is depleted
                    if (pillager.Hp <= 0)
                    {
                        totalPillagersKilled++;
                        killsInCurrentLevel++;
                    }
                }
                // Check if mob is an Enderman and marked for removal
                else if (mobs[j] is Enderman enderman && enderman.ToRemove)
                {
                    // Remove the mob from the list
                    mobs.RemoveAt(j);

                    // Update kill statistics if the Enderman's HP is depleted
                    if (enderman.Hp <= 0)
                    {
                        // Apply score bonus based on active buffs
                        if (pointsBuffActive)
                        {
                            playerScore += 200;
                        }
                        else
                        {
                            playerScore += 10;
                        }
                        totalEndermenKilled++;
                        killsInCurrentLevel++;
                    }
                }
                // Check if mob is a Villager and marked for removal
                else if (mobs[j] is Villager villager && villager.ToRemove)
                {
                    // Remove the mob from the list
                    mobs.RemoveAt(j);

                    // Apply score bonus based on active buffs
                    if (pointsBuffActive)
                    {
                        playerScore += 20;
                    }
                    else
                    {
                        playerScore += 10;
                    }
                    // Update kill statistics if the Villager's HP is depleted
                    if (villager.Hp <= 0)
                    {
                        totalVillagersKilled++;
                        killsInCurrentLevel++;
                    }
                }
            }

            // Check if level is complete (all mobs killed and no mobs left)
            if (totalMobsInCurrentLevel == 0 && mobs.Count == 0)
            {

                // Increment results screen delay
                resultsScreenDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;

                //Remove all arrows left on screen
                for (int i = arrows.Count - 1; i >= 0; i--)
                {
                    arrows.RemoveAt(i);
                }
                // Transition to results screen after delay duration
                if (resultsScreenDelay >= resultsScreenDelayDuration)
                {
                    currentGameState = RESULTS;
                    resultsScreenDelay = 0f; // Reset the delay for the next time
                }
            }
            else
            {
                // Reset the delay if the level is not complete
                resultsScreenDelay = 0f;
            }

            // Check if the current game state is GAMEPLAY
            if (currentGameState == GAMEPLAY)
            {
                // Play background music if not playing
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(backgroundMusic);
                }
            }
            else if (currentGameState != GAMEPLAY)
            {
                // Stop background music if not in gameplay state
                MediaPlayer.Stop();
            }

            // Export game statistics to a file
            ExportStatisticsToFile();
        }

        //Pre: None
        //Post: None
        //Desc: update the main results
        private void UpdateResults(KeyboardState keyboardState)
        {

            // Store current level and update level scores
            currentLevel = level.currentLevel;
            levelScores[currentLevel - 1] = playerScore;

            // Calculate hit percentage
            if (shotsFiredInRound > 0)
            {
                hitPercentage = (double)shotsHitInRound / shotsFiredInRound * 100;
            }
            else
            {
                hitPercentage = 0;
            }

            // Check for user input to proceed to the next level
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                // Reset variables for the next level
                killsInCurrentLevel = 0;
                shotsFiredInRound = 0;
                shotsHitInRound = 0;

                // Check if the current level is within the level limit (1-5)
                if (level.currentLevel <= 5)
                {
                    currentGameState = SHOP;
                    StartNextLevel();
                }

                // If the current level is greater than 5, switch to STATS game state
                if (level.currentLevel > 5)
                {
                    currentGameState = STATS;
                }
            }

            // Check if the current game state is RESULTS
            if (currentGameState == RESULTS)
            {
                // Play results music if not playing
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(resultMusic);
                }
            }
            else if (currentGameState != RESULTS)
            {
                // Stop results music if not in results state
                MediaPlayer.Stop();
            }
        }

        //Pre: None
        //Post: None
        //Desc: update the main stats
        private void UpdateStats(KeyboardState keyboardState)
        {
            // Update statistics if the current level is greater than 5 and stats haven't been updated yet
            if (level.currentLevel > 5 && !statsUpdated)
            {
                gamesPlayed++;

                // Update high score if necessary
                if (highScore > playerScore)
                {
                    highScore = playerScore;
                }

                // Calculate total kills and other statistics
                totalKills = totalEndermenKilled + totalCreepersKilled + totalPillagersKilled + totalSkeletonsKilled + totalVillagersKilled;
                allTimeHitPercentage = (int)Math.Round((double)(100 * shotsHit) / shotsFired);
                avgShotsPerGame = shotsFired / gamesPlayed;
                avgKillsPerGame = totalKills / gamesPlayed;

                // Calculate hit percentage for the current game
                if (shotsFired > 0)
                {
                    hitPercentage = (int)Math.Round((double)(100 * shotsHit) / shotsFired);
                }
                else
                {
                    hitPercentage = 0;
                }

                // Update top hit percentage and top score if necessary
                if (hitPercentage > topHitPercentage)
                {
                    topHitPercentage = (float)hitPercentage;
                }
                if (playerScore > highScore)
                {
                    highScore = playerScore;
                }

                // Set statsUpdated to true, export statistics to file, and restart the game
                statsUpdated = true;
                ExportStatisticsToFile();
                RestartGame();
            }

            // Check if the user pressed Enter
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                // Change the game state to MENU
                currentGameState = MENU;
            }

        }

        //Pre: None
        //Post: None
        //Desc: update the main shop
        private void UpdateShop(KeyboardState keyboardState, MouseState mouseState)
        {
            // Check if the left mouse button is pressed
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                // Purchase speed buff if clicked on its rectangle and conditions are met
                if (!speedBuffActive && playerScore >= SPEED_BUFF_COST && speedBuffRect.Contains(mouseState.Position))
                {
                    buyAccept.CreateInstance().Play();
                    speedBuffActive = true;
                    playerScore -= SPEED_BUFF_COST;
                    player.Speed *= 2;
                }
                // Purchase damage buff if clicked on its rectangle and conditions are met
                if (!damageBuffActive && playerScore >= DAMAGE_BUFF_COST && damageBuffRect.Contains(mouseState.Position))
                {
                    buyAccept.CreateInstance().Play();
                    damageBuffActive = true;
                    playerScore -= DAMAGE_BUFF_COST;
                    arrowDamage = 3;
                }
                // Purchase rate of fire buff if clicked on its rectangle and conditions are met
                if (!rateOfFireBuffActive && playerScore >= RATE_OF_FIRE_BUFF_COST && rateOfFireRect.Contains(mouseState.Position))
                {
                    buyAccept.CreateInstance().Play();
                    rateOfFireBuffActive = true;
                    playerScore -= RATE_OF_FIRE_BUFF_COST;
                    fireRateTimer.Interval = initialFireRateInterval / 2;
                }
                // Purchase points buff if clicked on its rectangle and conditions are met
                if (!pointsBuffActive && playerScore >= POINTS_COST && pointsBuffRect.Contains(mouseState.Position))
                {
                    buyAccept.CreateInstance().Play();
                    pointsBuffActive = true;
                    playerScore -= POINTS_COST;
                }
            }
            else
            {
                showPopup = false;
            }

            // Show popup for each buff when the mouse hovers over their respective rectangles
            if (rateOfFireRect.Contains(mouseState.Position))
            {
                showPopup = true;
                popupText = $"Rate of Fire Upgrade\nPrice: {RATE_OF_FIRE_BUFF_COST}";
                popupColor = playerScore >= RATE_OF_FIRE_BUFF_COST && !rateOfFireBuffActive ? Color.Green : Color.Red;
            }
            else if (speedBuffRect.Contains(mouseState.Position))
            {
                showPopup = true;
                popupText = $"Speed Upgrade\nPrice: {SPEED_BUFF_COST}";
                popupColor = playerScore >= SPEED_BUFF_COST && !speedBuffActive ? Color.Green : Color.Red;
            }
            else if (damageBuffRect.Contains(mouseState.Position))
            {
                showPopup = true;
                popupText = $"Damage Upgrade\nPrice: {DAMAGE_BUFF_COST}";
                popupColor = playerScore >= DAMAGE_BUFF_COST && !damageBuffActive ? Color.Green : Color.Red;
            }
            else if (pointsBuffRect.Contains(mouseState.Position))
            {
                showPopup = true;
                popupText = $"Points Upgrade\nPrice: {POINTS_COST}";
                popupColor = playerScore >= POINTS_COST && !pointsBuffActive ? Color.Green : Color.Red;
            }
            else
            {
                showPopup = false;
            }

            // Change the game state back to the pre-level instructions or the next level if the Enter key is pressed
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                currentGameState = GAMEPLAY;
            }

        }

        //Pre: None
        //Post: None
        //Desc: Restarts the game to its base value for a game reset
        private void RestartGame()
        {
            killsInCurrentLevel = 0;
            shotsFiredInRound = 0;
            shotsHitInRound = 0;
            level.currentLevel = 1;

            for (int i = 0; i < levelScores.Length; i++)
            {
                levelScores[i] = 0;
            }

        }

        //Pre: None
        //Post: None
        //Desc: Initialize the variables for padding and positioning
        private void InitializeVariables()
        {
            boxWidth = (int)(GraphicsDevice.Viewport.Width * 1);
            boxHeight = (int)(GraphicsDevice.Viewport.Height * 0.8);
            box2Height = (int)(GraphicsDevice.Viewport.Height * 0.8);
            boxX = (GraphicsDevice.Viewport.Width - boxWidth) / 2;
            boxY = (GraphicsDevice.Viewport.Height - boxHeight) / 2;
            box2Y = (int)(centerY + (box2Height / 2 - 25) + (verticalPadding * 0.2f));

            horizontalPadding = GraphicsDevice.Viewport.Width * HORIZONTAL_PADDING_FACTOR;
            verticalPadding = GraphicsDevice.Viewport.Height * VERTICAL_PADDING_FACTOR;
            centerX = GraphicsDevice.Viewport.Width / 2;
            centerY = GraphicsDevice.Viewport.Height / 2;

            startX = boxX + horizontalPadding;
            startY = boxY + verticalPadding * 2.5f;
            startXRight = boxX + boxWidth - horizontalPadding * 5f;
        }

        //Pre: None
        //Post: None
        //Desc: Loading fonts
        public void LoadFonts()
        {
            //Load all fonts
            mainFont = content.Load<SpriteFont>("Fonts/MainFont");
            statsFont = content.Load<SpriteFont>("Fonts/StatFont");
            smallText = content.Load<SpriteFont>("Fonts/SmallText");
        }

        //Pre: None
        //Post: None
        //Desc: Loading textures for player and player related items
        private void LoadPlayerRelatedAssets()
        {
            //Load player and arrow texture
            playerTexture = content.Load<Texture2D>("Sized/Nacho_64");
            arrowTexture = content.Load<Texture2D>("Sized/ArrowUp");
        }

        //Pre: None
        //Post: None
        //Desc: Loading textures for level
        private void LoadLevelRelatedAssets()
        {
            //Load the cobble, grass and dirt textures
            cobblestoneTexture = content.Load<Texture2D>("Sized/CobbleStone_64");
            grass2Texture = content.Load<Texture2D>("Sized/Grass2_64");
            grass1Texture = content.Load<Texture2D>("Sized/Grass1_64");
            dirtTexture = content.Load<Texture2D>("Sized/Dirt_64");

            //Load the icons for the game screen
            speedBuffIcon = content.Load<Texture2D>("Sized/IconSpeed_32");
            rateOfFireBuffIcon = content.Load<Texture2D>("Sized/IconFireRate_32");
            damageBuffIcon = content.Load<Texture2D>("Sized/IconDamage_32");
            pointsBuffIcon = content.Load<Texture2D>("Sized/IconPoints_32");
        }

        //Pre: None
        //Post: None
        //Desc: Loading textures for menu items
        private void LoadMenuRelatedAssets()
        {
            //Load the backgrounds
            background = content.Load<Texture2D>("Sized/MenuBG1");
            shopBackground = content.Load<Texture2D>("Raw/Dirt");

            //Load the button texture
            button = content.Load<Texture2D>("Sized/Button");

            //Load the titles for the gamestates
            menuTitle = content.Load<Texture2D>("Sized/Title");
            shopTitle = content.Load<Texture2D>("Sized/ShopTitle");
            statsTitle = content.Load<Texture2D>("Sized/StatsTitle");

            //Load a black screeen for the transparent background
            semiTransparentBackground = content.Load<Texture2D>("Sized/BlackBackground");

            //Load buff icons and item frame
            speedBuff = content.Load<Texture2D>("Sized/Boots");
            rateOfFireBuff = content.Load<Texture2D>("Sized/Bow_64");
            damageBuff = content.Load<Texture2D>("Sized/Sword");
            pointsBuff = content.Load<Texture2D>("Sized/Bottle");
            itemFrame = content.Load<Texture2D>("Sized/itemframe");
        }

        //Pre: None
        //Post: After executing this subprogram, the various sound assets such as background music, menu music, sound effects, etc. will be loaded into the respective variables.
        //Desc: loading sound assets
        private void LoadSoundRelatedAssets()
        {
            // Load background music
            backgroundMusic = content.Load<Song>("Music/Gameplay");
            menuMusic = content.Load<Song>("Music/Menu");
            resultMusic = content.Load<Song>("Music/Results");

            //Load Sound Effects
            buyAccept = content.Load<SoundEffect>("Sounds/buyaccept");
            bowShoot = content.Load<SoundEffect>("Sounds/BowShoot");
            arrowHit = content.Load<SoundEffect>("Sounds/ArrowImpact");
            endermanTeleport = content.Load<SoundEffect>("Sounds/EndermanTeleport");
            endermanScream = content.Load<SoundEffect>("Sounds/EndermanScream");
            shieldHit = content.Load<SoundEffect>("Sounds/ShieldHit");
            explode = content.Load<SoundEffect>("Sounds/Explode");
            buttonClick = content.Load<SoundEffect>("Sounds/button");
        }

        //Pre: None
        //Post: None
        //Desc: Sets lineheight and makes the texture for a pixel
        private void SetupLineHeightAndPixel(SpriteFont statsFont, GraphicsDevice graphicsDevice)
        {
            //Set the line spacing
            lineHeight = statsFont.LineSpacing * 1f;

            //Load Pixel and set the color
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        //Pre: None
        //Post: None
        //Desc: Setup positions and recs for buttons and titles
        private void SetupButtonOriginsAndTitlePositions(Texture2D button, Texture2D menuTitle, GraphicsDevice graphicsDevice)
        {
            //Set the button orignal position
            buttonOrigin = new Vector2(button.Width / 2, button.Height / 2);

            //Set the position for the title and the rectangle
            titlePosition = new Vector2(graphicsDevice.Viewport.Width / 2 - menuTitle.Width / 2, -25);
            titleRect = new Rectangle(graphicsDevice.Viewport.Width / 2 - menuTitle.Width / 2, graphicsDevice.Viewport.Height / 4, menuTitle.Width, menuTitle.Height);
        }

        //Pre: None
        //Post: None
        //Desc: Setup the positions for buttons
        private void SetupButtonPositions(Texture2D button, Rectangle titleRect, GraphicsDevice graphicsDevice)
        {
            //Set all button positions
            button1Position = new Vector2(graphicsDevice.Viewport.Width / 2 - button.Width / 2 - BUTTON_SPACING / 2, titleRect.Bottom - TITLE_BOTTOM_OFFSET);
            button2Position = new Vector2(graphicsDevice.Viewport.Width / 2 - button.Width - BUTTON_SPACING, button1Position.Y + button.Height + BUTTON_SPACING);
            button3Position = new Vector2(graphicsDevice.Viewport.Width / 2 + BUTTON_SPACING, button1Position.Y + button.Height + BUTTON_SPACING);
            continueButtonPosition = new Vector2(centerX - button.Width / 2, GraphicsDevice.Viewport.Height - verticalPadding * 3.5f);

            //Define all button recs
            button1Rect = new Rectangle((int)button1Position.X, (int)button1Position.Y, button.Width - BUTTON_WIDTH_OFFSET, button.Height);
            button2Rect = new Rectangle((int)button2Position.X, (int)button2Position.Y, button.Width - BUTTON_WIDTH_OFFSET, button.Height);
            button3Rect = new Rectangle((int)button3Position.X, (int)button3Position.Y, button.Width - BUTTON_WIDTH_OFFSET, button.Height);
            continueButtonRect = new Rectangle((int)continueButtonPosition.X, (int)continueButtonPosition.Y, button.Width - 50, button.Height);
            continueButtonShopRect = new Rectangle((int)continueButtonPositionShop.X, (int)continueButtonPositionShop.Y, button.Width, button.Height);

        }

        //Pre: None
        //Post: None
        //Desc:Setup all other position and recs
        private void SetupOtherPositions()
        {
            //Set position for the continue messages and center them
            continueMessagePosition = new Vector2(centerX - mainFont.MeasureString("Press Enter to Continue").X / 2f, GraphicsDevice.Viewport.Height - verticalPadding * 3f);
            continueMessageResultPosition = new Vector2(centerX - mainFont.MeasureString("Press SPACE to Continue").X / 2f, GraphicsDevice.Viewport.Height - verticalPadding * 3f);
            continueMessagePositionStats = new Vector2(centerX - mainFont.MeasureString("Press Enter to Continue").X / 2f, GraphicsDevice.Viewport.Height - verticalPadding * 1.5f);
            continueButtonPositionShop = new Vector2(centerX - statsFont.MeasureString("Press Enter to Continue").X / 2f, GraphicsDevice.Viewport.Height - verticalPadding * 1.5f);

            //Find the center of each buff icon
            rateOfFireBuffCenter = new Vector2(rateOfFireBuff.Width / 2, rateOfFireBuff.Height / 2);
            speedBuffCenter = new Vector2(speedBuff.Width / 2, speedBuff.Height / 2);
            pointsBuffCenter = new Vector2(pointsBuff.Width / 2, pointsBuff.Height / 2);
            damageBuffCenter = new Vector2(damageBuff.Width / 2, damageBuff.Height / 2);

            //Set the item frame position
            itemFrame1Pos = new Vector2(centerX - horizontalPadding * 4.5f, centerY - verticalPadding * 2.5f);
            itemFrame2Pos = new Vector2(centerX + horizontalPadding * 3f, centerY - verticalPadding * 2.5f);
            itemFrame3Pos = new Vector2(centerX - horizontalPadding * 4.5f, centerY + verticalPadding * 2f);
            itemFrame4Pos = new Vector2(centerX + horizontalPadding * 3f, centerY + verticalPadding * 2f);

            //set the position of the icon to the center of the item frame
            rateofFirePos = itemFrame1Pos + rateOfFireBuffCenter;
            speedBuffPos = itemFrame2Pos + speedBuffCenter;
            pointsBuffPos = itemFrame3Pos + pointsBuffCenter;
            damageBuffPos = itemFrame4Pos + damageBuffCenter;

            //define the in game icon positions
            iconPositions.Add(new Vector2(GraphicsDevice.Viewport.Width - speedBuffIcon.Width - ICON_PADDING, GraphicsDevice.Viewport.Height - speedBuffIcon.Height - ICON_PADDING));
            iconPositions.Add(new Vector2(GraphicsDevice.Viewport.Width - damageBuffIcon.Width - ICON_PADDING, GraphicsDevice.Viewport.Height - speedBuffIcon.Height - damageBuffIcon.Height - ICON_PADDING * 2));
            iconPositions.Add(new Vector2(GraphicsDevice.Viewport.Width - pointsBuffIcon.Width - ICON_PADDING, GraphicsDevice.Viewport.Height - speedBuffIcon.Height - damageBuffIcon.Height - pointsBuffIcon.Height - ICON_PADDING * 3));
            iconPositions.Add(new Vector2(GraphicsDevice.Viewport.Width - rateOfFireBuffIcon.Width - ICON_PADDING, GraphicsDevice.Viewport.Height - speedBuffIcon.Height - damageBuffIcon.Height - pointsBuffIcon.Height - rateOfFireBuffIcon.Height - ICON_PADDING * 4));

            //set rectangles of each buff
            rateOfFireRect = new Rectangle((int)rateofFirePos.X, (int)rateofFirePos.Y, rateOfFireBuff.Width, rateOfFireBuff.Height);
            speedBuffRect = new Rectangle((int)speedBuffPos.X, (int)speedBuffPos.Y, speedBuff.Width, speedBuff.Height);
            pointsBuffRect = new Rectangle((int)pointsBuffPos.X, (int)pointsBuffPos.Y, pointsBuff.Width, pointsBuff.Height);
            damageBuffRect = new Rectangle((int)damageBuffPos.X, (int)damageBuffPos.Y, damageBuff.Width, damageBuff.Height);

            //set rectangle for the transparent rectangle
            semiTransparentBackgroundRec = new Rectangle(boxX, boxY, boxWidth, boxHeight);

            //set the rectangle for the popup
            popupBackgroundRect = new Rectangle(
                (GraphicsDevice.Viewport.Width - POPUP_BACKGROUND_WIDTH) / 2,
                (GraphicsDevice.Viewport.Height - POPUP_BACKGROUND_HEIGHT) / 2,
                POPUP_BACKGROUND_WIDTH,
                POPUP_BACKGROUND_HEIGHT);

            //find positions and center for all the instructions, stats, headers and popups
            levelScoresPosition = new Vector2(horizontalPadding, verticalPadding * 3);
            statsPosition = new Vector2(GraphicsDevice.Viewport.Width - horizontalPadding * 4, verticalPadding * 3);
            scoreTextPosition = new Vector2(centerX - mainFont.MeasureString("SCORE : ").X / 2f, verticalPadding);
            scoreTextPositionShop = new Vector2(centerX - mainFont.MeasureString("SCORE : ").X / 2f, GraphicsDevice.Viewport.Height - verticalPadding * 3.5f);
            instructionHeaderPosition = new Vector2(centerX - mainFont.MeasureString("INSTRUCTIONS").X / 2f, verticalPadding * INSTRUCTION_HEADER_VERTICAL);
            moveInstructionsPos = new Vector2(centerX - smallText.MeasureString("MOVE: Left and Right arrow keys").X / 2f, verticalPadding * MOVE_INSTRUCTION_HEADER_VERTICAL);
            goalInstructionsPos = new Vector2(centerX - smallText.MeasureString("Kill all the mobs before they go off the screen!").X / 2f, GraphicsDevice.Viewport.Height - verticalPadding * GOAL_INSTRUCTION_HEADER_VERTICAL);
            shootInstructionsPos = new Vector2(centerX - smallText.MeasureString("SHOOT: Spacebar").X / 2f, verticalPadding * SHOOT_INSTRUCTION_HEADER_VERTICAL);
            popupPosition = new Vector2(
                popupBackgroundRect.X + (popupBackgroundRect.Width / 2) - (mainFont.MeasureString(popupText).X / 2),
                popupBackgroundRect.Y + (popupBackgroundRect.Height / 2) - (mainFont.MeasureString(popupText).Y / 2));
            popupTextPosition = new Vector2(
                popupBackgroundRect.X + (popupBackgroundRect.Width / 12f) - (mainFont.MeasureString(popupText).X / 2),
                popupBackgroundRect.Y + (popupBackgroundRect.Height / 4.5f) - (mainFont.MeasureString(popupText).Y / 2));
        }
    }
}
