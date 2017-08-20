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

namespace Belougame_Jam
{
    class Level
    {
        private float AspectRatio;
        private RenderTarget2D Texture;
        private Texture2D TileSheet;
        private TmxMap map;
        private int ViewportWidth, ViewportHeight;
        private Vector2 LevelPosition;
        // full level WxH in pixels
        public int LevelWidth { get { return map.Width * map.TileWidth; } }
        public int LevelHeight { get { return map.Height * map.TileHeight; } }

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
                    16 * map.TileWidth,
                    LevelHeight
                    );
            }
        }

        public Level(
            ContentManager content,
            GraphicsDevice graphicsDevice,
            SpriteBatch spriteBatch,
            string tmxFile,
            float aspectRatio
            )
        {
            map = new TmxMap(tmxFile);
            AspectRatio = aspectRatio;

            TileSheet = content.Load<Texture2D>(Path.GetFileNameWithoutExtension(map.Tilesets[0].Image.Source));
            Texture = new RenderTarget2D(graphicsDevice, LevelWidth, LevelHeight);

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

            LevelPosition = new Vector2(
                MathHelper.Clamp(
                    centeredPlayer.LevelPosition.X,
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
