using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Belougame_Jam
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const string JOHNSON_FILE = "johnson_idle";
        const int JOHNSON_WIDTH = 64;
        const int JOHNSON_HEIGHT = 64;
        const int JOHNSON_FRAMES = 8;
        const int JOHNSON_FRAMETIME = 95;

        const string MICHEL_IDLE_FILE = "michel_idle";
        const int MICHEL_WIDTH = 19;
        const int MICHEL_HEIGHT = 34;
        const int MICHEL_FRAMES = 12;
        const int MICHEL_FRAMETIME = 90;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        // Texture2D playerTexture;
        Texture2D johnsonTexture;
        Texture2D michelTexture;
        Player player;
        Player playerMichel;

        public Game1()
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
            player = new Player();
            playerMichel = new Player();


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Create a new SpriteBatch, which can be used to draw textures.
            Animation playerAnimation = new Animation();
            Animation playerMichelAnimation = new Animation();

            johnsonTexture = Content.Load<Texture2D>(JOHNSON_FILE);
            playerAnimation.Initialize(
                johnsonTexture, Vector2.Zero,
                JOHNSON_WIDTH, JOHNSON_HEIGHT, JOHNSON_FRAMES, JOHNSON_FRAMETIME,
                Color.White, 1, true
                );
            michelTexture = Content.Load<Texture2D>(MICHEL_IDLE_FILE);
            playerMichelAnimation.Initialize(
                michelTexture, Vector2.Zero,
                MICHEL_WIDTH, MICHEL_HEIGHT, MICHEL_FRAMES, MICHEL_FRAMETIME,
                Color.White, 2, true
                );

            Vector2 playerPosition = new Vector2(
                GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2,
                GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(playerAnimation, playerPosition);
            playerMichel.Initialize(playerMichelAnimation, playerPosition);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            base.Update(gameTime);

            player.Update(gameTime);
            playerMichel.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            playerMichel.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
