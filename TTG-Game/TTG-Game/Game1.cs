﻿#region Using Statements
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
    class GameObject
    {
        public Texture2D sprite;
        public Vector2 position;
        public float rotation;
        public Vector2 center;
        public Vector2 velocity;
        public bool alive;

        public Rectangle rectangle
        {
            get
            {
                int left = (int)position.X;
                int width = sprite.Width;
                int top = (int)position.Y;
                int height = sprite.Height;
                return new Rectangle(left, top, width, height);
            }
        }

        public GameObject(Texture2D loadedTexture)
        {
            rotation = 0.0f;
            position = Vector2.Zero;
            sprite = loadedTexture;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            velocity = Vector2.Zero;
            alive = false;
        }
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch GODDAMNIT;

        // Image allocations - Matthew
        private Texture2D sprite;
        private Texture2D StartButton;
        private Texture2D PauseButton;
        private Texture2D ExitButton;
        private Texture2D ResumeButton;
        private Texture2D LoadingScreen;
        private Texture2D FPSOnButton;
        private Texture2D FPSOffButton;
        private Texture2D Background;

        // Sound allocations
        private SoundEffect bang;



        // Sound Instance
        SoundEffect menumusic;
        SoundEffectInstance menumusicInstance;
        SoundEffect gameplaymusic;
        SoundEffectInstance gameplaymusicInstance;
        SoundEffect lifelost;
        SoundEffectInstance lifelostInstance;
        SoundEffect mouseclicksound;
        SoundEffectInstance mouseclicksoundInstance;
        SoundEffect firesound1;
        SoundEffectInstance firesound1Instance;
        SoundEffect firesound2;
        SoundEffectInstance firesound2Instance;

        // POS Allocation - Matthew
        private Vector2 StartButtonPOS;
        private Vector2 ResumeButtonPOS;
        private Vector2 SpritePOS1;
        private Vector2 SpritePOS2;
        private Vector2 ExitButtonPOS;
        private Vector2 LiveSpritePOS;
        private Vector2 LiveSprite2POS;
        private Vector2 LiveSprite3POS;
        private Vector2 Live2SpritePOS;
        private Vector2 Live2Sprite2POS;
        private Vector2 Live2Sprite3POS;


        //Sprite Stuff - Matthew
        private const float SpriteWidth = 50f;
        private const float SpriteHeight = 50f;
        private float speed = 5f;

        //Pixie Sprite
        private Texture2D Pixie1;
        private Texture2D Pixie2;

        //Life stuff
        private int Lives = 0;
        private int Lives2 = 0;
        private bool LeftArena = false;
        private Texture2D LivesSprite;
        private Texture2D LivesSprite2;
        private Texture2D LivesSprite3;
        private Texture2D Lives2Sprite;
        private Texture2D Lives2Sprite2;
        private Texture2D Lives2Sprite3;

        // Window stuff - Matthew

        private Thread backgroundThread;
        private GameStates gameStates;
        private bool isLoading = false;

        //Debug stuff - Matthew
        private SpriteFont font;
        Point frameSize = new Point(31, 31);
        Point currentFrame = new Point(0, 0);
        Point sheetSize = new Point(3, 3);

        Point frameSize2 = new Point(34, 40);
        Point currentFrame2 = new Point(0, 0);
        Point sheetSize2 = new Point(3, 3);

        MouseState mouseState;
        MouseState previousMouseState;


        // Guns stuff - Connor
        GameObject arm;
        private SpriteEffects flip = SpriteEffects.None;

        //Game state
        enum GameStates
        {
            StartMenu,
            Loading,
            Playing,
            Paused,
            Debug
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

            StartButtonPOS = new Vector2((GraphicsDevice.Viewport.Width / 2) - -508, 400);
            ExitButtonPOS = new Vector2((GraphicsDevice.Viewport.Width / 2) - -508, 450);

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

            font = Content.Load<SpriteFont>("Font/SpriteFont1");
            Pixie1 = Content.Load<Texture2D>("Images/sample_1");
            Pixie2 = Content.Load<Texture2D>("Images/sample_2");
            LivesSprite = Content.Load<Texture2D>("Images/life");
            LivesSprite2 = LivesSprite;
            LivesSprite3 = LivesSprite;
            Lives2Sprite = Content.Load<Texture2D>("Images/life2");
            Lives2Sprite2 = Lives2Sprite;
            Lives2Sprite3 = Lives2Sprite;

            // Load content for the guns
            arm = new GameObject(Content.Load<Texture2D>("Images/gun"));


            lifelost = Content.Load<SoundEffect>("Sound/lifelost");
            lifelostInstance = lifelost.CreateInstance();
            lifelostInstance.Volume = 0.5f;
            menumusic = Content.Load<SoundEffect>("Sound/menumusic");
            menumusicInstance = menumusic.CreateInstance();
            menumusicInstance.Volume = 0.5f;

            gameplaymusic = Content.Load<SoundEffect>("Sound/gameplaymusic");
            gameplaymusicInstance = gameplaymusic.CreateInstance();
            gameplaymusicInstance.Volume = 0.5f;

            mouseclicksound = Content.Load<SoundEffect>("Sound/mouseclicksound");
            mouseclicksoundInstance = mouseclicksound.CreateInstance();
            mouseclicksoundInstance.Pitch = 1;

            firesound1 = Content.Load<SoundEffect>("Sound/firesound1");
            firesound1Instance = firesound1.CreateInstance();
            firesound1Instance.Pitch = 1;

            firesound2 = Content.Load<SoundEffect>("Sound/firesound2");
            firesound2Instance = firesound2.CreateInstance();
            firesound2Instance.Volume = 0.8f;




            Background = Content.Load<Texture2D>("Images/background");
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
            KeyboardState keyboardState;
            // Gu stufz
            if (flip == SpriteEffects.FlipHorizontally)
            {
                arm.position = new Vector2(SpritePOS.X, SpritePOS.Y);//mouseState.X, mouseState.Y);
            }
            else
            {
                arm.position = new Vector2(SpritePOS.X, SpritePOS.Y);//mouseState.X, mouseState.Y);
            }

            if (flip == SpriteEffects.FlipHorizontally) //Facing right
            {
                //If we try to aim behind our head then flip the
                //character around so he doesn't break his arm!
                if (arm.rotation < 0)
                {
                    flip = SpriteEffects.None;
                }

                //If we aren't rotating our arm then set it to the
                //default position. Aiming in front of us.
                if (arm.rotation == 0 && Math.Abs(mouseState.Y) < 0.5f)
                {
                    arm.rotation = MathHelper.PiOver2;
                }
            }
            else //Facing left
            {
                //Once again, if we try to aim behind us then
                //flip our character.
                if (arm.rotation > 0)
                {
                    flip = SpriteEffects.FlipHorizontally;
                }

                //If we're not rotating our arm, default it to
                //aim the same direction we're facing.
                //if (arm.rotation == 0 && Math.Abs(gamePadState.ThumbSticks.Right.Length()) < 0.5f)
                //{
                //    arm.rotation = -MathHelper.PiOver2;
                //}
            }


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
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
                //PIXIE NO 1
                if (state.IsKeyDown(Keys.D))
                {
                    //Point currentFrame = new Point(2, 0);
                    currentFrame.Y = 2;
                    currentFrame.X++;
                    arm.rotation = 1.55f;
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = 0;
                    }
                    SpritePOS1.X += speed;
                }

                if (state.IsKeyDown(Keys.A))
                {
                    currentFrame.Y = 1;
                    currentFrame.X++;
                    arm.rotation = 4.7f;
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = 0;
                    }
                    SpritePOS1.X -= speed;
                }

                if (state.IsKeyDown(Keys.W))
                {
                    currentFrame.Y = 3;
                    currentFrame.X++;
                    arm.rotation = 0f;
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = 0;
                    }
                    SpritePOS1.Y -= speed;

                }
                if (state.IsKeyDown(Keys.S))
                {
                    currentFrame.Y = 0;
                    currentFrame.X++;
                    arm.rotation = 3.1f;
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = 0;
                    }
                    SpritePOS1.Y += speed;
                }
<<<<<<< HEAD
                if (SpritePOS1.X > (GraphicsDevice.Viewport.Width - SpriteWidth - 40) || SpritePOS1.X < 40 || SpritePOS1.Y > (GraphicsDevice.Viewport.Height - SpriteHeight - 40) || SpritePOS1.Y < 40)
=======
                if (state.IsKeyDown(Keys.LeftShift))
                {
                    firesound1Instance.Play();
                }
                if (state.IsKeyDown(Keys.RightShift))
                {
                    firesound2Instance.Play();
                }

                // Check if the ball is out of the arena and check the number of lives left and decrease them if necessary - Connor
                if (SpritePOS.X > (GraphicsDevice.Viewport.Width - SpriteWidth - 40) || SpritePOS.X < 40 || SpritePOS.Y > (GraphicsDevice.Viewport.Height - SpriteHeight - 40) || SpritePOS.Y < 40)
>>>>>>> master
                {
                    LeftArena = true;

                    while (LeftArena)
                    {
                        if (Lives == 0)
                        {
                            gameplaymusicInstance.Stop();
                            lifelostInstance.Play();
                            Console.WriteLine("You have no lives left.");
                            Thread.Sleep(1600);
                            gameStates = GameStates.StartMenu;
                            LeftArena = false;

                            

                           

                            break;
                        }
                        else if ((Lives > 0) && (Lives <= 2))
                        {
                            Lives--;
<<<<<<< HEAD
                            lifelost.Play();
=======
                           
                                gameplaymusicInstance.Pause();
                                lifelostInstance.Play();
                                Thread.Sleep(1500);

                                gameplaymusicInstance.Resume();
                                                                                  
>>>>>>> master

                            Console.WriteLine("You lost a life, you have " + Lives + " lives left");
                            LeftArena = false;

                        }

                        if (LeftArena == false)
                        {
                            ResetGame();
                        }
                    }
                }



                //PIXIE NO 2
                if (state.IsKeyDown(Keys.Right))
                {
                    //Point currentFrame = new Point(2, 0);
                    currentFrame2.Y = 2;
                    currentFrame2.X++;
                    if (currentFrame2.X >= sheetSize2.X)
                    {
                        currentFrame2.X = 0;
                    }
                    SpritePOS2.X += speed;
                }

                if (state.IsKeyDown(Keys.Left))
                {
                    currentFrame2.Y = 1;
                    currentFrame2.X++;
                    if (currentFrame2.X >= sheetSize2.X)
                    {
                        currentFrame2.X = 0;
                    }
                    SpritePOS2.X -= speed;
                }

                if (state.IsKeyDown(Keys.Up))
                {
                    currentFrame2.Y = 3;
                    currentFrame2.X++;
                    if (currentFrame2.X >= sheetSize2.X)
                    {
                        currentFrame2.X = 0;
                    }
                    SpritePOS2.Y -= speed;
                }

                if (state.IsKeyDown(Keys.Down))
                {
                    currentFrame2.Y = 0;
                    currentFrame2.X++;
                    if (currentFrame2.X >= sheetSize2.X)
                    {
                        currentFrame2.X = 0;
                    }
                    SpritePOS2.Y += speed;
                }




                // Check if the ball is out of the arena and check the number of lives left and decrease them if necessary - Connor
                if (SpritePOS2.X > (GraphicsDevice.Viewport.Width - SpriteWidth - 40) || SpritePOS2.X < 40 || SpritePOS2.Y > (GraphicsDevice.Viewport.Height - SpriteHeight - 40) || SpritePOS2.Y < 40)
                {
                    LeftArena = true;

                    while (LeftArena)
                    {
                        if (Lives2 == 0)
                        {
                            lifelost.Play();
                            Console.WriteLine("You have no lives left.");
                            gameStates = GameStates.StartMenu;
                            LeftArena = false;
                            break;
                        }
                        else if ((Lives2 > 0) && (Lives2 <= 2))
                        {
                            Lives2--;
                            lifelost.Play();
                            
                            Console.WriteLine("You lost a life, you have " + Lives2 + " lives left");
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
                mouseclicksoundInstance.Play();
            }

            previousMouseState = mouseState;

            if (gameStates == GameStates.Playing && isLoading)
            {
                LoadGame();
                isLoading = false;
            }

            //Pixie Stuff

            //++currentFrame.X;
            //if (currentFrame.X >= sheetSize.X)
            //{
            //    currentFrame.X = 0;
            //    ++currentFrame.Y;
            //    if (currentFrame.Y >= sheetSize.Y)
            //        currentFrame.Y = 0;
            //}


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //Draw the background
<<<<<<< HEAD
            spriteBatch.Draw(Background,new Rectangle(0, 0, Window.ClientBounds.Width,Window.ClientBounds.Height), null,Color.White, 0, Vector2.Zero,SpriteEffects.None, 0);
=======
            spriteBatch.Draw(background, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
>>>>>>> master
            // Draw the menu - Matthew

            if (gameStates == GameStates.StartMenu)
            {
                if (menumusicInstance.State == SoundState.Stopped)
                {
                    gameplaymusicInstance.Stop();
                    menumusicInstance.Play();
                    
                }

                spriteBatch.Draw(StartButton, StartButtonPOS, Color.White);
                spriteBatch.Draw(ExitButton, ExitButtonPOS, Color.White);

            }

            if (gameStates == GameStates.Loading)
            {
                spriteBatch.Draw(LoadingScreen, new Vector2((GraphicsDevice.Viewport.Width / 2) - (LoadingScreen.Width / 2), (GraphicsDevice.Viewport.Height / 2) - (LoadingScreen.Height / 2)), Color.YellowGreen);
            }

            if (gameStates == GameStates.Playing)
            {
<<<<<<< HEAD
                spriteBatch.Draw(Pixie1, SpritePOS1, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.Draw(Pixie2, SpritePOS2, new Rectangle(currentFrame2.X * frameSize2.X, currentFrame2.Y * frameSize2.Y, frameSize2.X, frameSize2.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.Draw(PauseButton, new Vector2(15, 10), Color.White);
=======
                if (menumusicInstance.State == SoundState.Playing)
                {
                    menumusicInstance.Stop();
                    gameplaymusicInstance.Play();
                }
                
                if (gameplaymusicInstance.State == SoundState.Stopped)
                {
                    gameplaymusicInstance.Play();
                }
                // Gun stuffz
                if (LeftArena == false)
                {
                    spriteBatch.Draw(arm.sprite, arm.position, null, Color.White, arm.rotation, arm.center, 1.0f, flip, 0);
                }
                spriteBatch.Draw(Pixie, SpritePOS, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.Draw(PauseButton, new Vector2(0, 0), Color.White);
>>>>>>> master
                if (Lives == 2)
                {
                    spriteBatch.Draw(LivesSprite, LiveSpritePOS, Color.White);
                    spriteBatch.Draw(LivesSprite2, LiveSprite2POS, Color.White);
                    spriteBatch.Draw(LivesSprite3, LiveSprite3POS, Color.White);
                }
                else if (Lives == 1)
                {
                    spriteBatch.Draw(LivesSprite, LiveSpritePOS, Color.White);
                    spriteBatch.Draw(LivesSprite2, LiveSprite2POS, Color.White);
                }
                else
                    spriteBatch.Draw(LivesSprite, LiveSpritePOS, Color.White);

                if (Lives2 == 2)
                {
                    spriteBatch.Draw(Lives2Sprite, Live2SpritePOS, Color.White);
                    spriteBatch.Draw(Lives2Sprite2, Live2Sprite2POS, Color.White);
                    spriteBatch.Draw(Lives2Sprite3, Live2Sprite3POS, Color.White);
                }
                else if (Lives2 == 1)
                {
                    spriteBatch.Draw(Lives2Sprite, Live2SpritePOS, Color.White);
                    spriteBatch.Draw(Lives2Sprite2, Live2Sprite2POS, Color.White);
                }
                else
                    spriteBatch.Draw(Lives2Sprite, Live2SpritePOS, Color.White);

            }
            if (gameStates == GameStates.Paused)
            {
                spriteBatch.Draw(ResumeButton, ResumeButtonPOS, Color.White);
            }

            if (gameStates == GameStates.Debug)
            {
                spriteBatch.Draw(FPSOnButton, new Vector2(30, 30), Color.White);
            }

            LiveSpritePOS = new Vector2((GraphicsDevice.Viewport.Width / 2) - 650, 0);
            LiveSprite2POS = new Vector2((GraphicsDevice.Viewport.Width / 2) - 575, 0);
            LiveSprite3POS = new Vector2((GraphicsDevice.Viewport.Width / 2) - 500, 0);
            Live2SpritePOS = new Vector2((GraphicsDevice.Viewport.Width / 2) - -600, 0);
            Live2Sprite2POS = new Vector2((GraphicsDevice.Viewport.Width / 2) - -525, 0);
            Live2Sprite3POS = new Vector2((GraphicsDevice.Viewport.Width / 2) - -450, 0);
            //spriteBatch.Draw(Pixie, SpritePOS, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
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
            SpritePOS1 = new Vector2((GraphicsDevice.Viewport.Width / 2) - (SpriteWidth / 2), (GraphicsDevice.Viewport.Height / 2) - (SpriteHeight / 2));
            SpritePOS2 = new Vector2((GraphicsDevice.Viewport.Width / 2) - (SpriteWidth / 2), (GraphicsDevice.Viewport.Height / 2) - (SpriteHeight / 2));
            FPSOnButton = Content.Load<Texture2D>("Images/Orb");

            // Testing load screen don't leave this command in on launch - Matthew
            //Thread.Sleep(3000);

            Lives = 2;
            Lives2 = 2;

            gameStates = GameStates.Playing;
            isLoading = false;

        }
        /// <summary>
        /// This variable is for resetting the position of the sprite.
        /// </summary>
        void ResetGame()
        {
            SpritePOS1 = new Vector2((GraphicsDevice.Viewport.Width / 2) - (SpriteWidth / 2), (GraphicsDevice.Viewport.Height / 2) - (SpriteHeight / 2));
            SpritePOS2 = new Vector2((GraphicsDevice.Viewport.Width / 2) - (SpriteWidth / 2), (GraphicsDevice.Viewport.Height / 2) - (SpriteHeight / 2));
        }
        /// <summary>
        /// This variable is for recognising where the mouse is clicking
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// 

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
                    mouseclicksoundInstance.Play();
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
                Rectangle PauseButtonRect = new Rectangle(0, 0, 70, 70);

                if (mouseClickRect.Intersects(PauseButtonRect))
                {
                    mouseclicksoundInstance.Play();
                    gameStates = GameStates.Paused;
                }
            }
            //Resume Button
            if (gameStates == GameStates.Paused)
            {
                Rectangle ResumeButtonRect = new Rectangle((int)ResumeButtonPOS.X, (int)ResumeButtonPOS.Y, 100, 20);
                if (mouseClickRect.Intersects(ResumeButtonRect))
                {
                    mouseclicksoundInstance.Play();
                    gameStates = GameStates.Playing;
                }
            }
        }
    }
}
