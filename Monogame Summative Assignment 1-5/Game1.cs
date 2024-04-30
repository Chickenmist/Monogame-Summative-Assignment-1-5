using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Summative_Assignment_1_5
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        enum Screen
        {
            Intro

        }

        Screen screen;

        float seconds;

        MouseState mouseState;

        Texture2D tailsTexture; //Image size is 80x80 pixels
        //Tails and Sonic textures have a height difference of 32 pixels. This is important for keeping them on level ground
        Texture2D sonicTexture; //Image size is 80x112 pixels

        Texture2D eggmanTexture; //Image size is 100x141 pixels

        Texture2D ringTexture; //Image size is 20x20 pixels

        Texture2D sonicDamagedTexture; //Image size is 80x56
        Rectangle sonicDamagedRect;

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

            sonicDamagedRect = new Rectangle(40, 40, 280, 100);

            seconds = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            tailsTexture = Content.Load<Texture2D>("tailsSprite");
            sonicTexture = Content.Load<Texture2D>("sonicSprite");
            ringTexture = Content.Load<Texture2D>("ringSprite");
            sonicDamagedTexture = Content.Load<Texture2D>("sonicDamagedSprite");
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            Window.Title = $"{mouseState.X}, {mouseState.Y}";

            seconds = (float)gameTime.TotalGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            _spriteBatch.Draw(tailsTexture, new Vector2(100, 132), Color.White);
            _spriteBatch.Draw(sonicTexture, new Vector2(185, 100), Color.White);
            _spriteBatch.Draw(ringTexture, new Vector2(205, 120), Color.White);
            _spriteBatch.Draw(sonicDamagedTexture, sonicDamagedRect, Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}