﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System.Linq;

namespace Belougame_Jam
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class JungleAdventures : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Player> players;

        public JungleAdventures()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            players = new List<Player>();
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

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Sprite johnsonIdle = new Sprite(Content, "johnson_idle", 64, 64, 8, 100, 1);
            Sprite michelIdle = new Sprite(Content, "michel_idle", 19, 34, 12, 90, 2);

            var playerSprites = new List<Sprite>();
            playerSprites.Add(johnsonIdle);
            playerSprites.Add(michelIdle);

            foreach (var it in playerSprites.Select((v, i) => new { Sprite = v, Index = i })) {
                Vector2 position = new Vector2(
                    GraphicsDevice.Viewport.TitleSafeArea.X
                    + GraphicsDevice.Viewport.TitleSafeArea.Width * (it.Index + 1) / 3,
                    GraphicsDevice.Viewport.TitleSafeArea.Y
                    + GraphicsDevice.Viewport.TitleSafeArea.Height / 2
                );
                Player player = new Player();
                player.Initialize(it.Sprite.animation, position);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);

            players.ForEach(p => p.Update(gameTime));
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
            players.ForEach(p => p.Draw(spriteBatch));
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}