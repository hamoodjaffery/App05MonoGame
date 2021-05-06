using System;
using MacApp05Game.Controllers;
using MacApp05Game.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MacApp05Game
{
    /// <summary>
    /// This is a basic 2D monogame, where the player
    /// moves around the screen, collecting diamonds while trying
    /// to avoid the dog (enemy) to harm it by firing bullets
    /// at the dog or by running away from the dog!
    /// </summary>
    public class App05Game : Game
    {
        #region Constants

        public const int HD_Height = 720;
        public const int HD_Width = 1280;

        #endregion

        #region Attribute

        private readonly GraphicsDeviceManager graphicsManager;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;

        private SpriteFont arialFont;
        private SpriteFont verdanaFont;

        private Texture2D backgroundImage;
        private SoundEffect flameEffect;

        private int score;
        private int health;
        private Random randomGenerator;
        
        private readonly DiamondsController diamondsController;
        private BulletController bulletController;

        private AnimatedPlayer playerSprite;
        private AnimatedSprite enemySprite;

        private Button restartButton;
        private Button quitButton;
        #endregion

        public App05Game()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            diamondsController = new DiamondsController();
        }

        /// <summary>
        /// Setup the game window size to 720P 1280 x 720 pixels
        /// With no camera or scrolling feature.
        /// </summary>
        protected override void Initialize()
        {
            graphicsManager.PreferredBackBufferWidth = HD_Width;
            graphicsManager.PreferredBackBufferHeight = HD_Height;

            graphicsManager.ApplyChanges();

            graphicsDevice = graphicsManager.GraphicsDevice;

            randomGenerator = new Random();
            score = 0;
            health = 100;

            base.Initialize();
        }

        /// <summary>
        /// This method is used to upload all the required game contents,
        /// images, sounds, fonts, and music effects.
        /// This method will display all the contents when the game starts!
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundImage = Content.Load<Texture2D>(
                "images/green_background720p");

            // Load Music and SoundEffects

            SoundController.LoadContent(Content);
            //SoundController.PlaySong("Adventure");

            // Load Fonts

            arialFont = Content.Load<SpriteFont>("arial");
            verdanaFont = Content.Load<SpriteFont>("verdana");

            // animated sprites that will be used to set up the player
            // and the enemy
            // also set up the diamonds

            SetupAnimatedPlayer();
            SetupEnemy();

            diamondsController.CreateDiamonds(Content);
        }

        /// <summary>
        /// This is the Animated Player Sprite with four
        /// directions, up, down, left and right
        /// </summary>
        private void SetupAnimatedPlayer()
        {
            Texture2D sheet4x3 = Content.Load<Texture2D>("images/rsc-sprite-sheet1");

            AnimationController contoller = new AnimationController(graphicsDevice, sheet4x3, 4, 3);

            string[] keys = new string[] { "Down", "Left", "Right", "Up" };
            contoller.CreateAnimationGroup(keys);

            playerSprite = new AnimatedPlayer()
            {
                CanWalk = true,
                Scale = 2.0f,

                Position = new Vector2(200, 200),
                Speed = 210,
                Direction = new Vector2(1, 0),

                Rotation = MathHelper.ToRadians(0),
                RotationSpeed = 0f
            };

            contoller.AppendAnimationsTo(playerSprite);
            
            Texture2D bulletImage = Content.Load<Texture2D>("images/bullet");
            bulletController = new BulletController(bulletImage);
            playerSprite.bulletController = bulletController;

        }

        /// <summary>
        /// This is an Animated Enemy Sprite with
        /// directions, up, down, left and right.  Has no intelligence!
        /// </summary>
        private void SetupEnemy()
        {
            Texture2D sheet4x3 = Content.Load<Texture2D>("images/rsc-sprite-sheet3");

            AnimationController manager = new AnimationController(graphicsDevice, sheet4x3, 4, 3);

            string[] keys = new string[] { "Down", "Left", "Right", "Up" };

            manager.CreateAnimationGroup(keys);

            enemySprite = new AnimatedSprite()
            {
                Scale = 2.0f,

                Position = new Vector2(1000, 200),
                Direction = new Vector2(-1, 0),
                Speed = 0.5f,

                Rotation = MathHelper.ToRadians(0),
            };
            manager.AppendAnimationsTo(enemySprite);
            enemySprite.PlayAnimation("Left");
        }


        /// <summary>
        /// Called 60 frames/per second and updates the positions
        /// of all the drawable objects
        /// </summary>
        /// <param name="gameTime">
        /// Can work out the elapsed time since last call if
        /// you want to compensate for different frame rates
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            // Update Chase Game

            Vector2 distance = playerSprite.Position - enemySprite.Position;
            if(distance.X >= 200 || distance.Y >= 200)
            {
                // if true enemy is outside of field of view
                // so you could move randomly isntead
                // because he has stepped to far away
                // You can pretend that enemy can't see the player
            }

            enemySprite.Direction = 
                new Vector2(playerSprite.Position.X - enemySprite.Position.X, 
                            playerSprite.Position.Y - enemySprite.Position.Y);
            
            playerSprite.Update(gameTime);
            enemySprite.Update(gameTime);


            if (playerSprite.HasCollided(enemySprite))
            {
                playerSprite.IsActive = false;
                playerSprite.IsAlive = false;
                playerSprite.IsVisible = false;

                enemySprite.IsActive = false;
                enemySprite.IsVisible = false;
                enemySprite.IsAlive = false;
            }

            diamondsController.Update(gameTime);
            diamondsController.HasCollided(playerSprite);
            bulletController.UpdateBullets(gameTime);

            bulletController.HasCollided(enemySprite);

            if (!enemySprite.IsAlive)
            {
                ReactivateEnemy();
            }    
            

            base.Update(gameTime);
        }

        private void ReactivateEnemy()
        {
            //int x = randomGenerator.Next(800) + 100;
            //int y = randomGenerator.Next(500) + 100;

            enemySprite.Position = new Vector2(1000,200);
            enemySprite.Direction = new Vector2(-1, 0);
            enemySprite.Speed = 0.5f;

            enemySprite.IsAlive = true;
            enemySprite.IsVisible = true;
            enemySprite.IsActive = true;
        }

        // <summary>
        /// Called 60 frames/per second and Draw all the 
        /// sprites and other drawable images here
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LawnGreen);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundImage, Vector2.Zero, Color.White);

            //restartButton.Draw(spriteBatch);
            //quitButton.Draw(spriteBatch);

            if (!playerSprite.IsAlive)
            {
                spriteBatch.DrawString(verdanaFont, "LOSER!", new Vector2(200, 200), Color.Yellow);
                spriteBatch.End();
            }

            else if (playerSprite.IsAlive && playerSprite.score >= 200)
            {
                spriteBatch.DrawString(verdanaFont, "WINNER", new Vector2(200, 200), Color.Yellow);
                spriteBatch.End();
            }
            else
            {

                playerSprite.Draw(spriteBatch);
                bulletController.DrawBullets(spriteBatch);
                diamondsController.Draw(spriteBatch);
                enemySprite.Draw(spriteBatch);

                DrawGameStatus(spriteBatch);
                DrawGameFooter(spriteBatch);

                spriteBatch.End();
                base.Draw(gameTime);

            }

        }

        /// <summary>
        /// Display the name fo the game and the current score
        /// and health of the player at the top of the screen
        /// </summary>
        public void DrawGameStatus(SpriteBatch spriteBatch)
        {
            Vector2 topLeft = new Vector2(4, 4);
            string status = $"Score = {playerSprite.score}";

            spriteBatch.DrawString(arialFont, status, topLeft, Color.White);

            string game = "Diamond Chase";
            Vector2 gameSize = arialFont.MeasureString(game);
            Vector2 topCentre = new Vector2((HD_Width / 2 - gameSize.X / 2), 4);
            spriteBatch.DrawString(arialFont, game, topCentre, Color.White);

            string healthText = $"Health = {playerSprite.health:0}%";
            Vector2 healthSize = arialFont.MeasureString(healthText);
            Vector2 topRight = new Vector2(HD_Width - (healthSize.X + 4), 4);
            spriteBatch.DrawString(arialFont, healthText, topRight, Color.White);

        }

        /// <summary>
        /// Display the Module, the authors and the application name
        /// at the bottom of the screen
        /// </summary>
        public void DrawGameFooter(SpriteBatch spriteBatch)
        {
            int margin = 20;

            string names = "Hamood Jaffery";
            string app = "App05: MonogGame";
            string module = "BNU CO453-2021";

            Vector2 namesSize = verdanaFont.MeasureString(names);
            Vector2 appSize = verdanaFont.MeasureString(app);

            Vector2 bottomCentre = new Vector2((HD_Width - namesSize.X)/ 2, HD_Height - margin);
            Vector2 bottomLeft = new Vector2(margin, HD_Height - margin);
            Vector2 bottomRight = new Vector2(HD_Width - appSize.X - margin, HD_Height - margin);

            spriteBatch.DrawString(verdanaFont, names, bottomCentre, Color.Yellow);
            spriteBatch.DrawString(verdanaFont, module, bottomLeft, Color.Yellow);
            spriteBatch.DrawString(verdanaFont, app, bottomRight, Color.Yellow);

        }
    }
}
