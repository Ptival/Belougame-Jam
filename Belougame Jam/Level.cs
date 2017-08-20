using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using System.IO;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Belougame_Jam
{
    class Level
    {
        private float AspectRatio;
        private RenderTarget2D Texture;
        private Texture2D TileSheet;
        private TmxMap map;
        private int ViewportWidth, ViewportHeight;
        public Vector2 LevelPosition;
        // full level WxH in pixels
        public int LevelWidth { get { return map.Width * map.TileWidth; } }
        public int LevelHeight { get { return map.Height * map.TileHeight; } }
        public List<Rectangle> LevelCollisionBoxes;

        private SoundEffectInstance SongIntroInstance;
        private SoundEffectInstance SongLoopInstance;

        private int LevelViewWidth
        {
            get
            {
                // #ProduitEnCroix
                // Aspect Ratio :                4 : 3
                // View   Ratio :   LevelViewWidth : LevelViewHeight
                return (int)(LevelViewHeight * AspectRatio);
            }
        }
        private int LevelViewHeight { get { return LevelHeight; } }

        private Rectangle DestinationRect
        {
            get
            {
                return new Rectangle(0, 0, ViewportWidth, ViewportHeight);
            }
        }

        private Rectangle SourceRect
        {
            get
            {
                return new Rectangle(
                    (int)LevelPosition.X, (int)LevelPosition.Y,
                    LevelViewWidth,
                    LevelViewHeight
                    );
            }
        }

        public float ZoomFactor
        {
            get
            {
                // ZoomFactor =               2   :   1
                //                ViewPortWidth   :   LevelViewWidth
                return (float)ViewportWidth / (float)LevelViewWidth;
                // same as ViewportHeigth / LevelHeigth when aspect ratio is kept constant
            }
        }

        public Level(
            ContentManager content,
            GraphicsDevice graphicsDevice,
            SpriteBatch spriteBatch,
            string tmxFile,
            float aspectRatio,
            SoundEffect songIntro,
            SoundEffect songLoop)
        {
            AspectRatio = aspectRatio;

            map = new TmxMap(tmxFile);

            TileSheet = content.Load<Texture2D>(Path.GetFileNameWithoutExtension(map.Tilesets[0].Image.Source));
            Texture = new RenderTarget2D(graphicsDevice, LevelWidth, LevelHeight);

            SongIntroInstance = songIntro.CreateInstance();
            SongIntroInstance.Play();
            SongLoopInstance = songLoop.CreateInstance();
            SongLoopInstance.IsLooped = true;

            LevelCollisionBoxes = new List<Rectangle>();

            // painting the level texture
            int nbColumns = map.Tilesets[0].Columns.Value;
            graphicsDevice.SetRenderTarget(Texture);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            foreach (TmxLayer layer in map.Layers)
            {
                foreach (TmxLayerTile tile in layer.Tiles)
                {
                    /* tileSheetColumnNb = 39
                     *    0  1  2  ... 37 38
                     *    39 40 41 ...
                     * Gid 41:
                     *   - x = 32   (41 % 39) * 16
                     *   - y = 16   (41 / 39) * 16
                     */
                    int tileId = tile.Gid - 1; // Tiled seems to reserve 0 for empty
                    Rectangle srcRect = new Rectangle(
                        (tileId % nbColumns) * map.TileWidth,
                        (tileId / nbColumns) * map.TileHeight,
                        map.TileWidth, map.TileHeight
                        );
                    Rectangle dstRect = new Rectangle(
                        tile.X * map.TileWidth, tile.Y * map.TileHeight,
                        map.TileWidth, map.TileHeight
                        );
                    spriteBatch.Draw(TileSheet, dstRect, srcRect, Color.White);

                    if (tile.Gid != 0)
                    {
                        LevelCollisionBoxes.Add(dstRect);
                    }
                }
            }
            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
        }

        public void Update(
            GraphicsDevice GraphicsDevice,
            Player centeredPlayer
            )
        {
            ViewportWidth = GraphicsDevice.Viewport.TitleSafeArea.Width;
            ViewportHeight = GraphicsDevice.Viewport.TitleSafeArea.Height;

            if (SongIntroInstance.State == SoundState.Stopped
                && SongLoopInstance.State == SoundState.Stopped)
            {
                SongLoopInstance.Play();
            }

            LevelPosition = new Vector2(
                MathHelper.Clamp(
                    centeredPlayer.PlayerPosition.X - (int)(3.5 * map.TileWidth),
                    0,
                    LevelWidth - LevelViewWidth
                    )
                , 0
                );
        }

        public void Draw(
            SpriteBatch spriteBatch
            )
        {
            spriteBatch.Draw(
                Texture, DestinationRect, SourceRect,
                Color.White, 0, Vector2.Zero, SpriteEffects.None, 1
                );
        }
    }
}
