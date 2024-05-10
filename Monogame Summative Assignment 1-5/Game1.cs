using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Monogame_Summative_Assignment_1_5
{ 

    //Wilson

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

        float songDuration; //these are to keep track if the title song has ended
        float songTime;

        bool sonicRunning;
        int timesSonicsCrossed; //Keeps track of how many times sonic has crossed the screen during the running animation

        bool sonicStopped; //Indicates if Sonic has stopped running

        bool eggmanTurned; //Indicates if Eggman has turned around

        bool tailsIsFlying; //Indicates if Tails is flying

        bool eggmanInMobile; //Indicates if Eggman is in the Eggmobile

        bool eggmanBobing; //Indicates if Eggman is at the height where he starts bobing

        bool eggmanFlyingAway; //Indicates if Eggman is running away

        bool bigTimeDone; //Indicates if the big time voice line has been played

        bool sonicIsThatYouDone; //Indicates if the Sonic is that you voice line has been played

        bool thisIsItEggmanDone; //Indicates if the this is it Eggman voice line has been played

        bool noRetreatDone; //Indicates if the no retreat line has been played

        bool thatWasCoolDone; //Indicates if that was cool line has been played
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
        Vector2 eggmobileSpeed;

        Texture2D capsuleTexture; //Image size is 100x125
        Rectangle capsuleRect;
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

        SoundEffect thatWasCool;
        SoundEffectInstance thatWasCoolInstance; //Sonic says that was cool
        //

        //Songs
        Song titleSong;
        Song runSong;
        Song eggmanSong;
        Song winSong;
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

            eggmobileSpeed = new Vector2(0, -5);

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

            thatWasCoolDone = false;

            timesSonicsCrossed = 0;

            songTime = 0;

            screen = Screen.Title;
            
            eggmobileRect = new Rectangle(720, 354, 100, 101);
            
            base.Initialize();

            songDuration = titleSong.Duration.Milliseconds;

            sonicRect = new Rectangle(320, 445 - sonicTexture.Height, sonicTexture.Width, sonicTexture.Height);
            sonicRunningRect = new Rectangle(0, 445 - sonicRunningTexture.Height, sonicRunningTexture.Width, sonicRunningTexture.Height);
            
            tailsRect = new Rectangle(320 - tailsTexture.Width, 445 - tailsTexture.Height, tailsTexture.Width, tailsTexture.Height);
            tailsFlyingRect = new Rectangle(320 - tailsFlyingTexture.Width, 0 - (tailsFlyingTexture.Height * 2), tailsFlyingTexture.Width, tailsFlyingTexture.Height);
            
            eggmanRect = new Rectangle(720, 455 - eggmanTexture.Height, eggmanTexture.Width, eggmanTexture.Height);

            capsuleRect = new Rectangle(890, 455 - capsuleTexture.Height, capsuleTexture.Width, capsuleTexture.Height);
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
            capsuleTexture = Content.Load<Texture2D>("capsuleSprite");
            //

            //Sound Effects and Songs
            bigTime = Content.Load<SoundEffect>("BigTimeSonic");
            bigTimeInstance = bigTime.CreateInstance();

            sonicIsThatYou = Content.Load<SoundEffect>("sonicIsThatYou");
            sonicIsThatYouInstance = sonicIsThatYou.CreateInstance();

            thisIsItEggman = Content.Load<SoundEffect>("thisIsItEggman");
            thisIsItEggmanInstance = thisIsItEggman.CreateInstance();

            noRetreat = Content.Load<SoundEffect>("NoRetreatEggman");
            noRetreatInstance = noRetreat.CreateInstance();

            thatWasCool = Content.Load<SoundEffect>("thatWasCool");
            thatWasCoolInstance = thatWasCool.CreateInstance();

            titleSong = Content.Load<Song>("TitleScreenMusic");
            runSong = Content.Load<Song>("GreenHillZone");
            eggmanSong = Content.Load<Song>("DangerontheDanceFloor");
            winSong = Content.Load<Song>("ActClear");
            MediaPlayer.Play(titleSong);
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

            //Window.Title = $"{mouseState.X}, {mouseState.Y}";
           
            if (screen == Screen.Title)
            {
                songTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (keyboardState.IsKeyDown(Keys.Enter) || songTime >= songDuration)
                {
                    screen = Screen.RunningScreen;

                    MediaPlayer.Stop();

                    MediaPlayer.Play(runSong);
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

                if (sonicRunning)
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

                        MediaPlayer.Stop();

                        MediaPlayer.Play(eggmanSong);
                    }
                }
            }

            if (screen == Screen.MainAnimation)
            {
                seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (!sonicStopped)
                {
                    sonicRunningRect.X += (int)sonicRunningSpeed.X;

                    if (sonicRunningRect.X >= 230)
                    {
                        sonicStopped = true;
                    }
                }
                else if (sonicStopped)
                {
                    if (tailsIsFlying)
                    {
                        tailsFlyingRect.Y += (int)tailsFlyingSpeed.Y;
                        
                        if (tailsFlyingRect.Bottom >= 445)
                        {
                            tailsIsFlying = false;
                        }
                    }

                    if (eggmanTurned == false && noRetreatDone == false)
                    {
                        if (seconds >= 1 && bigTimeDone == false)
                        {
                            bigTimeInstance.Play();
                            
                            bigTimeDone = true;
                        }

                        if (bigTimeInstance.State == SoundState.Stopped && bigTimeDone)
                        {
                            eggmanTurned = true;

                            seconds = 0;
                        }
                    }
                    else if (eggmanTurned)
                    {
                        if (!eggmanInMobile)
                        {
                            if (!sonicIsThatYouDone)
                            {
                                sonicIsThatYouInstance.Play();
                                sonicIsThatYouDone = true;
                            }
                            else if (sonicIsThatYouDone && sonicIsThatYouInstance.State == SoundState.Stopped)
                            {
                                if (!thisIsItEggmanDone)
                                {
                                    thisIsItEggmanInstance.Play();
                                    thisIsItEggmanDone = true;
                                }
                                else if (thisIsItEggmanDone && thisIsItEggmanInstance.State == SoundState.Stopped)
                                {
                                    if (!noRetreatDone)
                                    {
                                        noRetreatInstance.Play();
                                        noRetreatDone = true;
                                    }
                                    else if (noRetreatDone && noRetreatInstance.State == SoundState.Stopped)
                                    {
                                        eggmanInMobile = true;
                                        seconds = 0;
                                    }
                                }
                            }
                        }
                        else if (eggmanInMobile)
                        {
                            Window.Title = seconds.ToString();
                            eggmobileRect.Offset(eggmobileSpeed);
                            if (eggmobileRect.Top <= 80)
                            {
                                eggmanBobing = true;

                                if (eggmanBobing)
                                {
                                    if (seconds >= 3)
                                    {
                                        eggmanFlyingAway = true;
                                    }

                                    if (!eggmanFlyingAway)
                                    {
                                        eggmobileSpeed = new Vector2(0, 1);
                                    }
                                    else if (eggmanFlyingAway)
                                    {
                                        eggmobileSpeed = new Vector2(2, 1);
                                    }
                                }
                            }
                            else if (eggmanBobing && eggmobileRect.Bottom >= 200)
                            {
                                eggmobileSpeed.Y *= -1;
                            }
                        }
                    }
                }

                if (eggmobileRect.X >= _graphics.PreferredBackBufferWidth + (eggmobileRect.Width * 2))
                {
                    screen = Screen.EndScreen;

                    MediaPlayer.Play(winSong);
                }
            }
            
            if (screen == Screen.EndScreen)
            {
                if (!thatWasCoolDone)
                {
                    thatWasCoolInstance.Play();
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

                if (!sonicRunning)
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

                if (!sonicStopped)
                {
                    _spriteBatch.Draw(sonicRunningTexture, sonicRunningRect, Color.White);
                }
                else if (sonicStopped)
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

                if (!eggmanInMobile)
                {
                    if (!eggmanTurned)
                    {
                        _spriteBatch.Draw(eggmanTexture, eggmanRect, Color.White);
                    }
                    else if (eggmanTurned && eggmanInMobile == false)
                    {
                        _spriteBatch.Draw(eggmanTexture, eggmanRect, null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0f);
                    }
                }
                else if (eggmanInMobile)
                {
                    if (!eggmanFlyingAway)
                    {
                        _spriteBatch.Draw(eggmobileTexture, eggmobileRect, Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(eggmobileTexture, eggmobileRect, null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0f);
                    }
                }

            }
            else if (screen == Screen.EndScreen)
            {
                _spriteBatch.Draw(titleScreenBackground, backgroundRect, Color.White);
                _spriteBatch.DrawString(title, "The End!", new Vector2(240, 359), Color.Black);
                _spriteBatch.DrawString(instructions, "Press enter to close", new Vector2(5, 560), Color.Yellow);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}