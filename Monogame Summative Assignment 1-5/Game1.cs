using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Monogame_Summative_Assignment_1_5
{ 
    enum Screen
    {
        Title,
        RunningScreen,
        MainAnimation,
        EndScreen
    }
    
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Screen screen;

        float seconds;

        bool sonicRunning;
        int timesSonicsCrossed; //Keeps track of how many times sonic has crossed the screen during the running animation

        bool sonicStopped; //indicates if Sonic has stopped running

        bool eggmanTurned; //indicates if Eggman has turned around

        bool tailsIsFlying; //indicates if Tails is flying

        MouseState mouseState;
        KeyboardState keyboardState;

        Texture2D titleScreenBackground;

        Texture2D mainBackground;

        Rectangle backgroundRect;

        Texture2D tailsFlyingTexture; //Image size is 80x76
        Rectangle tailsFlyingRect;
        Vector2 tailsFlyingSpeed;

        Rectangle tailsRect;
        Texture2D tailsTexture; //Image size is 80x80 pixels
        //Tails and Sonic textures have a height difference of 32 pixels
        Texture2D sonicTexture; //Image size is 80x112 pixels
        Rectangle sonicRect;
        Texture2D sonicRunningTexture; //Image size is 140x112
        Rectangle sonicRunningRect;
        Vector2 sonicRunningSpeed;

        List<Texture2D> sonicFootTaps = new List<Texture2D>();
        int tappingFrame;

        Texture2D eggmanTexture; //Image size is 100x141 pixels

        SpriteFont instructions;
        SpriteFont title;

        SoundEffect bigTime;
        SoundEffectInstance bigTimeInstance; //Sonic says the big time line

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            backgroundRect = new Rectangle(0, 0, 1000, 600);

            seconds = 0;

            tappingFrame = 0;

            sonicRunningSpeed = new Vector2(10,0);

            tailsFlyingSpeed = new Vector2(0, 3);

            sonicRunning = false;

            sonicStopped = false;

            tailsIsFlying = true;

            eggmanTurned = false;

            timesSonicsCrossed = 0;

            screen = Screen.Title;

            base.Initialize();

            sonicRect = new Rectangle(320, 445 - sonicTexture.Height, sonicTexture.Width, sonicTexture.Height);
            sonicRunningRect = new Rectangle(0, 445 - sonicRunningTexture.Height, sonicRunningTexture.Width, sonicRunningTexture.Height);
            
            tailsRect = new Rectangle(320 - tailsTexture.Width, 445 - tailsTexture.Height, tailsTexture.Width, tailsTexture.Height);
            tailsFlyingRect = new Rectangle(320 - tailsFlyingTexture.Width, 0 - (tailsFlyingTexture.Height * 2), tailsFlyingTexture.Width, tailsFlyingTexture.Height);
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            sonicTexture = Content.Load<Texture2D>("sonicSprite");
            tailsTexture = Content.Load<Texture2D>("tailsSprite");
            sonicRunningTexture = Content.Load<Texture2D>("sonicRunningSprite");
            tailsFlyingTexture = Content.Load<Texture2D>("tailsFlyingSprite");
            titleScreenBackground = Content.Load<Texture2D>("sonicAnimationTitle");
            mainBackground = Content.Load<Texture2D>("greenHillZoneBackground");
            eggmanTexture = Content.Load<Texture2D>("eggmanSprite");

            bigTime = Content.Load<SoundEffect>("BigTimeSonic");
            bigTimeInstance = bigTime.CreateInstance();

            instructions = Content.Load<SpriteFont>("instructionText");
            title = Content.Load<SpriteFont>("titleText");

            sonicFootTaps.Add(Content.Load<Texture2D>("sonicFootTapOne"));
            sonicFootTaps.Add(Content.Load<Texture2D>("sonicFootTapTwo"));
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            Window.Title = $"{mouseState.X}, {mouseState.Y}";
           
            if (screen == Screen.Title)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    screen = Screen.RunningScreen;
                }
            }

            if (screen == Screen.RunningScreen)
            {
                seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (seconds >= 0.4)
                {
                    tappingFrame = (tappingFrame + 1) % sonicFootTaps.Count;
                    seconds = 0;
                }
                
                if (keyboardState.IsKeyDown(Keys.Right) && sonicRunning == false)
                {
                    sonicRunning = true;
                    seconds = 0;
                }

                if (sonicRunning == true)
                {
                    sonicRunningRect.X += (int)sonicRunningSpeed.X;

                    if (sonicRunningRect.Left > 1000  && timesSonicsCrossed < 2)
                    {
                        sonicRunningRect.X = 0 - sonicRunningRect.Width;

                        timesSonicsCrossed++;
                    }
                    else if (sonicRunningRect.Left > 1000 && timesSonicsCrossed == 2)
                    {
                        sonicRunningRect.X = 0 - sonicRunningRect.Width;
                        screen = Screen.MainAnimation;
                    }
                }
            }

            if (screen == Screen.MainAnimation)
            {
                if (sonicStopped == false)
                {
                    sonicRunningRect.X += (int)sonicRunningSpeed.X;

                    if (sonicRunningRect.X >= 230)
                    {
                        sonicStopped = true;
                    }
                }
                else if (sonicStopped ==  true)
                {
                    if (tailsIsFlying == true)
                    {
                        tailsFlyingRect.Y += (int)tailsFlyingSpeed.Y;
                        
                        if (tailsFlyingRect.Bottom >= 445)
                        {
                            tailsIsFlying = false;
                        }
                    }

                    if (eggmanTurned == false)
                    {
                        seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

                        if (seconds == 1)
                        {
                            bigTimeInstance.Play();
                            eggmanTurned = true;
                        }
                    }
                    else if (eggmanTurned == true && bigTimeInstance.State == SoundState.Stopped)
                    {

                    }

                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if (screen == Screen.Title)
            {
                _spriteBatch.Draw(titleScreenBackground, backgroundRect, null, Color.White);
                _spriteBatch.DrawString(title, "A REALLY BAD SONIC ANIMATION", new Vector2(240, 359), Color.Black);
                _spriteBatch.DrawString(instructions, "Press enter to proceed or wait for music to end", new Vector2(5, 560), Color.Yellow);

            }
            else if (screen == Screen.RunningScreen)
            {
                if (timesSonicsCrossed % 2 == 0)
                {
                    _spriteBatch.Draw(mainBackground, backgroundRect, null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0f);
                }
                else
                {
                    _spriteBatch.Draw(mainBackground, backgroundRect, Color.White);
                }

                if (sonicRunning == false)
                {
                    _spriteBatch.Draw(sonicFootTaps[tappingFrame], new Vector2(0, 445 - sonicRunningTexture.Height), Color.White);

                    _spriteBatch.DrawString(instructions, "Press -> to make Sonic run", new Vector2(5, 560), Color.White);
                }
                else
                {
                    _spriteBatch.Draw(sonicRunningTexture, sonicRunningRect, Color.White);
                }
            }
            else if (screen == Screen.MainAnimation)
            {
                _spriteBatch.Draw(mainBackground, backgroundRect, Color.White);

                if (sonicStopped == false)
                {
                    _spriteBatch.Draw(sonicRunningTexture, sonicRunningRect, Color.White);
                }
                else if (sonicStopped == true)
                {
                    _spriteBatch.Draw(sonicTexture, sonicRect, Color.White);

                    if (tailsIsFlying == true)
                    {
                        _spriteBatch.Draw(tailsFlyingTexture, tailsFlyingRect, Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(tailsTexture, tailsRect, Color.White);
                    }

                    if (eggmanTurned == false)
                    {
                        
                    }
                }
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}