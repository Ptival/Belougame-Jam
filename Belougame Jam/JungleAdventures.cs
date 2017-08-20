using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TiledSharp;

namespace Belougame_Jam
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class JungleAdventures : Game
    {
        public const float ASPECT_RATIO = 4.0f / 3.0f;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Level level;
        List<Player> players;
        List<Background> Backgrounds;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        // GamePadState currentGamePadState;
        // GamePadState previousGamePadState;
        // MouseState currentMouseState;
        // MouseState previousMouseState;

        public JungleAdventures()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            players = new List<Player>();
            Backgrounds = new List<Background>();

            this.Window.AllowUserResizing = true;

            graphics.PreferredBackBufferHeight = 192 * 2;
            graphics.PreferredBackBufferWidth = (int)(graphics.PreferredBackBufferHeight * ASPECT_RATIO);
            graphics.ApplyChanges();

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        public Vector2 speedForLayer(int layer)
        {
            float speedBase = 1 / 1000f;
            float speedFactor = 1 / 10f;
            return new Vector2(speedBase + speedFactor * layer, 0);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Backgrounds.Add(new Background(Content.Load<Texture2D>("plx-1"), speedForLayer(0)));
            Backgrounds.Add(new Background(Content.Load<Texture2D>("plx-2"), speedForLayer(1)));
            Backgrounds.Add(new Background(Content.Load<Texture2D>("plx-3"), speedForLayer(2)));
            Backgrounds.Add(new Background(Content.Load<Texture2D>("plx-4"), speedForLayer(3)));
            Backgrounds.Add(new Background(Content.Load<Texture2D>("plx-5"), speedForLayer(4)));


            SoundEffect songIntro = Content.Load<SoundEffect>("jungle_intro");
            SoundEffect songLoop = Content.Load<SoundEffect>("jungle_loop");

            level = new Level(
                            Content,
                            GraphicsDevice,
                            spriteBatch,
                            "Content/Levels/Level 0/level_0.tmx",
                            ASPECT_RATIO,
                            songIntro,
                            songLoop
                        );

            float johnsonScale = 0.5f;
            float michelScale = johnsonScale * 2.0f;
            Sprite johnsonIdle = new Sprite(Content, "johnson_idle", 64, 64, 8, 100, johnsonScale);
            Sprite johnsonRun = new Sprite(Content, "johnson_run", 64, 64, 6, 100, johnsonScale);
            Sprite michelIdle = new Sprite(Content, "michel_idle", 21, 35, 12, 90, michelScale);
            Sprite michelRun = new Sprite(Content, "michel_run", 23, 34, 8, 90, michelScale);
            Sprite michelSSJ = new Sprite(Content, "michel_ssj_transformation", 22, 37, 2, 90, michelScale);


            var playerSprites = new List<Tuple<float, Sprite, Sprite, Sprite>>();
            //playerSprites.Add(new Tuple<float, Sprite, Sprite>(johnsonScale, johnsonIdle, johnsonRun));
            playerSprites.Add(new Tuple<float, Sprite, Sprite, Sprite>(michelScale, michelIdle, michelRun, michelSSJ));

            foreach (var it in playerSprites.Select((v, i) => new { Sprite = v, Index = i }))
            {
                Vector2 position = new Vector2(50 + it.Index * 32, 160);
                Player player = new Player();
                player.Initialize(
                    it.Sprite.Item2.animation,
                    it.Sprite.Item3.animation,
                    it.Sprite.Item4.animation,
                    position,
                    it.Sprite.Item1
                    );
                players.Add(player);
            }

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
            // preverse aspect ratio
            graphics.PreferredBackBufferHeight = (int)(GraphicsDevice.Viewport.Width * (1 / ASPECT_RATIO));
            graphics.ApplyChanges();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            base.Update(gameTime);

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            Vector2 direction = Vector2.Zero;
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                direction += new Vector2(-1, 0);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.D)) {
                direction += new Vector2(1, 0);
            }

            players.ForEach(p => p.Update(gameTime, currentKeyboardState, level, GraphicsDevice.Viewport));
            Backgrounds.ForEach(bg => bg.Update(direction, GraphicsDevice.Viewport, players[0], level.ZoomFactor));
            level.Update(GraphicsDevice, players.First());
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
            foreach (Background bg in Backgrounds) { bg.Draw(spriteBatch); }
            level.Draw(spriteBatch);
            players.ForEach(p => p.Draw(spriteBatch));
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
