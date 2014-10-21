#region Using Statements
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace LinuxTesting
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Image allocations - Matthew
        private Texture2D sprite;
        private Texture2D StartButton;
        private Texture2D PauseButton;
        private Texture2D ExitButton;
        private Texture2D ResumeButton;
        private Texture2D LoadingScreen;
        // Sound allocations
        private SoundEffect bang;


        // POS Allocation - Matthew
        private Vector2 StartButtonPOS;
        private Vector2 ResumeButtonPOS;
        private Vector2 SpritePOS;
        private Vector2 ExitButtonPOS;

        //Sprite Stuff - Matthew
        private const float SpriteWidth = 50f;
        private const float SpriteHeight = 50f;
        private float speed = 12f;
        
        //Life stuff
        private int Lives = 0;
        private bool LeftArena = false;

        // Window stuff - Matthew

        private Thread backgroundThread;
        private GameStates gameStates;
        private bool isLoading = false;

        MouseState mouseState;
        MouseState previousMouseState;

        //Game state
        enum GameStates
        {
            StartMenu,
            Loading,
            Playing,
            Paused
        }

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            //Draw mouse
            IsMouseVisible = true;

            StartButtonPOS = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 200);
            ExitButtonPOS = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 250);

            gameStates = GameStates.StartMenu;

            //LoadGame();
            // Hai Mouse :3
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here

            StartButton = Content.Load<Texture2D>("Images/start");
            ExitButton = Content.Load<Texture2D>("Images/quit");
            LoadingScreen = Content.Load<Texture2D>("Images/loading");

            //bang = Content.Load<SoundEffect>("Sound/bang");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if(Keyboard.GetState().IsKeyDown(Keys.B))
            {
                bang.Play();
            }
            // Load
            if (gameStates == GameStates.Loading && !isLoading)
            {
                // Set background thread
                backgroundThread = new Thread(LoadGame);
                isLoading = true;

                backgroundThread.Start();

            }

            //Grabbing keyboard state 
            KeyboardState state = Keyboard.GetState();

            if (gameStates == GameStates.Playing)
            {

                //Realistic Ball Control..nope. 

                if (state.IsKeyDown(Keys.D))
                {
                    SpritePOS.X += speed;
                }

                if (state.IsKeyDown(Keys.A))
                {
                    SpritePOS.X -= speed;
                }

                if (state.IsKeyDown(Keys.W))
                {
                    SpritePOS.Y -= speed;

                }
                if (state.IsKeyDown(Keys.S))
                {
                    SpritePOS.Y += speed;
                }

                // Check if the ball is out of the arena and check the number of lives left and decrease them if necessary - Connor
                if (SpritePOS.X > (GraphicsDevice.Viewport.Width - SpriteWidth) || SpritePOS.X < 0 || SpritePOS.Y > (GraphicsDevice.Viewport.Height - SpriteHeight) || SpritePOS.Y < 0)
                { 
                    LeftArena = true;

                    while (LeftArena)
                    {
                        if (Lives == 0)
                        {
                            Console.WriteLine("You ahve no lives left.");
                            gameStates = GameStates.StartMenu;
                            LeftArena = false;
                            break;
                        }
                        else if ( (Lives > 0) && (Lives <= 2) )
                        {
                            Lives--;
                            Console.WriteLine("You lost a life, you have " + Lives + " lives left");
                            LeftArena = false;
                        }

                        if (LeftArena == false)
                        {
                            ResetGame();
                        }
                    }
                }

            }
            mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }

            previousMouseState = mouseState;

            if (gameStates == GameStates.Playing && isLoading)
            {
                LoadGame();
                isLoading = false;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.DeepPink);
            spriteBatch.Begin();

            // Draw the menu - Matthew
            if (gameStates == GameStates.StartMenu)
            {
                spriteBatch.Draw(StartButton, StartButtonPOS, Color.White);
                spriteBatch.Draw(ExitButton, ExitButtonPOS, Color.White);
            }

            if (gameStates == GameStates.Loading)
            {
                spriteBatch.Draw(LoadingScreen, new Vector2((GraphicsDevice.Viewport.Width / 2) - (LoadingScreen.Width / 2), (GraphicsDevice.Viewport.Height / 2) - (LoadingScreen.Height / 2)), Color.YellowGreen);
            }

            if (gameStates == GameStates.Playing)
            {
                spriteBatch.Draw(sprite, SpritePOS, Color.White);
                spriteBatch.Draw(PauseButton, new Vector2(0, 0), Color.White);
            }
            if (gameStates == GameStates.Paused)
            {
                spriteBatch.Draw(ResumeButton, ResumeButtonPOS, Color.White);
            }
            // FPS ~ WARNING THIS CODE SHOULD NOT BE DISTRIBUTED ON LAUNCH - Matthew
            //float frame_rate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Console.WriteLine("FPS: " + frame_rate);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
        /// <summary>
        /// This variable creates the game and should only be used when you are loading the core game. Don't use this for anything else.
        /// </summary>
        void LoadGame()
        {
            sprite = Content.Load<Texture2D>("Images/Orb");
            PauseButton = Content.Load<Texture2D>("Images/123");
            ResumeButton = Content.Load<Texture2D>("Images/1234");
            ResumeButtonPOS = new Vector2((GraphicsDevice.Viewport.Width / 2) - (ResumeButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) - (ResumeButton.Height / 2));
            SpritePOS = new Vector2((GraphicsDevice.Viewport.Width / 2) - (SpriteWidth / 2), (GraphicsDevice.Viewport.Height / 2) - (SpriteHeight / 2));

            // Why not yolo
            Thread.Sleep(3000);

            Lives = 2;

            gameStates = GameStates.Playing;
            isLoading = false;

        }
        /// <summary>
        /// This variable is for resetting the position of the sprite.
        /// </summary>
        void ResetGame()
        {
            SpritePOS = new Vector2((GraphicsDevice.Viewport.Width / 2) - (SpriteWidth / 2), (GraphicsDevice.Viewport.Height / 2) - (SpriteHeight / 2));
        }
        /// <summary>
        /// This variable is for recognising where the mouse is clicking
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void MouseClicked(int x, int y)
        {
            //Creates a rectangle around the mouse click - Matthew
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            if (gameStates == GameStates.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)StartButtonPOS.X, (int)StartButtonPOS.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)ExitButtonPOS.X, (int)ExitButtonPOS.Y, 100, 20);
                if (mouseClickRect.Intersects(startButtonRect))
                {
                    gameStates = GameStates.Loading;
                    isLoading = false;
                }
                else if (mouseClickRect.Intersects(exitButtonRect))
                {
                    Exit();
                }
            }
                //Pause button
                if (gameStates == GameStates.Playing)
                {
                    Rectangle PauseButtonRect = new Rectangle(0,0,70,70);

                    if (mouseClickRect.Intersects(PauseButtonRect))
                    {
                        gameStates = GameStates.Paused;
                    }
                }
                //Resume Button
                if (gameStates == GameStates.Paused)
                {
                    Rectangle ResumeButtonRect = new Rectangle((int)ResumeButtonPOS.X, (int)ResumeButtonPOS.Y, 100, 20);
                    if (mouseClickRect.Intersects(ResumeButtonRect))
                    {
                        gameStates = GameStates.Playing;
                    }
                }
            }
        }
    }
