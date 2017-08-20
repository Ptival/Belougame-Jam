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
        private RenderTarget2D Texture;
        private Texture2D TileSheet;
        // private Vector2 Position;
        private Rectangle DestinationRect;
        private Rectangle SourceRect;
        public int LevelWidth;
        public int LevelHeight;

        public Level(
            ContentManager content,
            GraphicsDevice graphicsDevice,
            SpriteBatch spriteBatch,
            string tmxFile
            )
        {
            DestinationRect = new Rectangle();
            SourceRect = new Rectangle();

            TmxMap map = new TmxMap(tmxFile);
            LevelWidth = map.Width * map.TileWidth;
            LevelHeight = map.Height * map.TileHeight;

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
            DestinationRect = new Rectangle(
                0, 0,
                GraphicsDevice.Viewport.TitleSafeArea.Width,
                GraphicsDevice.Viewport.TitleSafeArea.Height
                );

            // Texture :  T.W * T.H
            // Screen  :  V.W * V.H

            SourceRect = new Rectangle(
                (int)centeredPlayer.LevelPosition.X, 0,
                (Texture.Height * GraphicsDevice.Viewport.TitleSafeArea.Width) / GraphicsDevice.Viewport.TitleSafeArea.Height,
                Texture.Height
                );
        }

        public void Draw(
            SpriteBatch spriteBatch
            )
        {
            spriteBatch.Draw(Texture, DestinationRect, SourceRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
        }
    }
}
