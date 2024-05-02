using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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

        MouseState mouseState;
        KeyboardState keyboardState;

        Texture2D titleScreenBackground;

        Texture2D mainBackground;

        Rectangle backgroundRect;

        Texture2D tailsFlyingTexture; //Iamge size is 80x76

        Rectangle tailsRect;
        Texture2D tailsTexture; //Image size is 80x80 pixels
        //Tails and Sonic textures have a height difference of 32 pixels. This is important for keeping them on level ground
        Texture2D sonicTexture; //Image size is 80x112 pixels
        Rectangle sonicRect;
        Texture2D sonicRunningTexture; //Image size is 140x112
        Rectangle sonicRunningRect;
        Vector2 sonicRunningSpeed;

        List<Texture2D> sonicFootTaps = new List<Texture2D>();
        int tappingFrame;

        Texture2D eggmanTexture; //Image size is 100x141 pixels

        Texture2D ringTexture; //Image size is 20x20 pixels
        Rectangle ringRect;

        Texture2D sonicDamagedTexture; //Image size is 80x56
        Rectangle sonicDamagedRect;

        SpriteFont instructions;
        SpriteFont title;

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

            sonicDamagedRect = new Rectangle(40, 40, 150, 70);

            backgroundRect = new Rectangle(0, 0, 1000, 600);

            seconds = 0;

            tappingFrame = 0;

            sonicRunningSpeed = new Vector2(7,0);

            screen = Screen.Title;

            base.Initialize();

            sonicRect = new Rectangle(185, 100, sonicTexture.Width, sonicTexture.Height);
            tailsRect = new Rectangle(100, 132, tailsTexture.Width, tailsTexture.Height);
            ringRect = new Rectangle(205, 120, ringTexture.Width, ringTexture.Height);
            sonicRunningRect = new Rectangle(-1000, 100, sonicRunningTexture.Width, sonicRunningTexture.Height);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            sonicTexture = Content.Load<Texture2D>("sonicSprite");
            tailsTexture = Content.Load<Texture2D>("tailsSprite");
            
            ringTexture = Content.Load<Texture2D>("ringSprite");
            sonicDamagedTexture = Content.Load<Texture2D>("sonicDamagedSprite");
            sonicRunningTexture = Content.Load<Texture2D>("sonicRunningSprite");
            tailsFlyingTexture = Content.Load<Texture2D>("tailsFlyingSprite");
            titleScreenBackground = Content.Load<Texture2D>("sonicAnimationTitle");

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
                sonicRunningRect.Offset(sonicRunningSpeed);
                
                if (sonicRunningRect.Right < 2000)
                {
                    screen = Screen.MainAnimation;
                }
            }

            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (seconds >= 0.5)
            {
                tappingFrame = (tappingFrame + 1) % sonicFootTaps.Count;
                seconds = 0;
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
                _spriteBatch.Draw(titleScreenBackground, backgroundRect, Color.White);
            }
            else if (screen == Screen.RunningScreen)
            {
                _spriteBatch.Draw(sonicRunningTexture, sonicRunningRect, Color.White);
            }

            _spriteBatch.Draw(sonicFootTaps[tappingFrame], new Vector2(250, 100), Color.White);

            _spriteBatch.Draw(tailsTexture, tailsRect, Color.White);
            _spriteBatch.Draw(sonicTexture, sonicRect, Color.White);
            _spriteBatch.Draw(ringTexture, ringRect, Color.White);
            _spriteBatch.Draw(sonicDamagedTexture, sonicDamagedRect, Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}