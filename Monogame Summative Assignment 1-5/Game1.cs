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

        //Floats, ints, and bools
        float seconds;

        bool sonicRunning;
        int timesSonicsCrossed; //Keeps track of how many times sonic has crossed the screen during the running animation

        bool sonicStopped; //Indicates if Sonic has stopped running

        bool eggmanTurned; //Indicates if Eggman has turned around

        bool tailsIsFlying; //Indicates if Tails is flying

        bool eggmanInMobile; //Indicates if Eggman is in the Eggmobile

        bool eggmanFlyingAway; //Indicates if Eggman is running away

        bool bigTimeDone; //Indicates if the big time voice line has been played

        bool sonicIsThatYouDone; //Indicates if the Sonic is that you voice line has been played

        bool thisIsItEggmanDone; //Indicates if the this is it Eggman voice line has been played

        bool noRetreatDone; //Indicates if the no retreat line has been played
        //

        MouseState mouseState;
        KeyboardState keyboardState;

        //Textures, Rectangles and Vectors
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
        Rectangle eggmanRect;

        Texture2D eggmobileTexture; //Image size is 80x81
        Rectangle eggmobileRect;
        //

        //Fonts
        SpriteFont instructions;
        SpriteFont title;
        //
        
        //Sound Effects
        SoundEffect bigTime;
        SoundEffectInstance bigTimeInstance; //Sonic says the big time line

        SoundEffect sonicIsThatYou;
        SoundEffectInstance sonicIsThatYouInstance; //Eggman says the sonic is that you line

        SoundEffect thisIsItEggman;
        SoundEffectInstance thisIsItEggmanInstance; //Sonic says the this is it Eggman line

        SoundEffect noRetreat;
        SoundEffectInstance noRetreatInstance; //Eggman says no retreat line
        //

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

            eggmanInMobile = false;

            eggmanFlyingAway = false;

            bigTimeDone = false;

            sonicIsThatYouDone = false;

            thisIsItEggmanDone = false;

            noRetreatDone = false;

            timesSonicsCrossed = 0;

            screen = Screen.Title;

            base.Initialize();

            sonicRect = new Rectangle(320, 445 - sonicTexture.Height, sonicTexture.Width, sonicTexture.Height);
            sonicRunningRect = new Rectangle(0, 445 - sonicRunningTexture.Height, sonicRunningTexture.Width, sonicRunningTexture.Height);
            
            tailsRect = new Rectangle(320 - tailsTexture.Width, 445 - tailsTexture.Height, tailsTexture.Width, tailsTexture.Height);
            tailsFlyingRect = new Rectangle(320 - tailsFlyingTexture.Width, 0 - (tailsFlyingTexture.Height * 2), tailsFlyingTexture.Width, tailsFlyingTexture.Height);
            
            eggmanRect = new Rectangle(720, 455 - eggmanTexture.Height, eggmanTexture.Width, eggmanTexture.Height);
            eggmobileRect = new Rectangle(720, 455 - eggmobileTexture.Height, eggmobileTexture.Width, eggmobileTexture.Height);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            //Textures
            sonicTexture = Content.Load<Texture2D>("sonicSprite");
            tailsTexture = Content.Load<Texture2D>("tailsSprite");
            sonicRunningTexture = Content.Load<Texture2D>("sonicRunningSprite");
            tailsFlyingTexture = Content.Load<Texture2D>("tailsFlyingSprite");
            titleScreenBackground = Content.Load<Texture2D>("sonicAnimationTitle");
            mainBackground = Content.Load<Texture2D>("greenHillZoneBackground");
            eggmanTexture = Content.Load<Texture2D>("eggmanSprite");
            eggmobileTexture = Content.Load<Texture2D>("eggMobileTexture");
            //

            //Sound Effects
            bigTime = Content.Load<SoundEffect>("BigTimeSonic");
            bigTimeInstance = bigTime.CreateInstance();

            sonicIsThatYou = Content.Load<SoundEffect>("sonicIsThatYou");
            sonicIsThatYouInstance = sonicIsThatYou.CreateInstance();

            thisIsItEggman = Content.Load<SoundEffect>("thisIsItEggman");
            thisIsItEggmanInstance = thisIsItEggman.CreateInstance();

            noRetreat = Content.Load<SoundEffect>("NoRetreatEggman");
            noRetreatInstance = noRetreat.CreateInstance();
            //

            //Fonts
            instructions = Content.Load<SpriteFont>("instructionText");
            title = Content.Load<SpriteFont>("titleText");
            //

            //Foot taps
            sonicFootTaps.Add(Content.Load<Texture2D>("sonicFootTapOne"));
            sonicFootTaps.Add(Content.Load<Texture2D>("sonicFootTapTwo"));
            //
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

                        if (seconds >= 1 && bigTimeDone == false)
                        {
                            bigTimeInstance.Play();
                            bigTimeDone = true;
                        }

                        if (bigTimeInstance.State == SoundState.Stopped && bigTimeDone)
                        {
                            eggmanTurned = true;
                        }
                    }
                    else if (eggmanTurned == true)
                    {
                        if (sonicIsThatYouDone == false)
                        {
                            sonicIsThatYouInstance.Play();
                            sonicIsThatYouDone = true;
                        }
                        else if (sonicIsThatYouDone && sonicIsThatYouInstance.State == SoundState.Stopped)
                        {
                            if (thisIsItEggmanDone == false)
                            {
                                thisIsItEggmanInstance.Play();
                                thisIsItEggmanDone = true;
                            }
                            else if (thisIsItEggmanDone && thisIsItEggmanInstance.State == SoundState.Stopped)
                            {
                                if (noRetreatDone == false) 
                                {
                                    noRetreatInstance.Play();
                                    noRetreatDone = true;
                                }
                                else if (noRetreatDone && noRetreatInstance.State == SoundState.Stopped)
                                {

                                }
                            }
                        }


                        
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
                }



                if (eggmanTurned == false)
                {
                    _spriteBatch.Draw(eggmanTexture, eggmanRect, Color.White);
                }
                else if (eggmanTurned == true)
                {
                    _spriteBatch.Draw(eggmanTexture, eggmanRect, null, Color.White, 0f, new Vector2(0,0), SpriteEffects.FlipHorizontally, 0f);
                }
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}