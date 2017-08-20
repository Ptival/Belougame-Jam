using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belougame_Jam
{
    class Level
    {
        private Texture2D Texture;
        // private Vector2 Position;
        private Rectangle DestinationRect;
        private Rectangle SourceRect;

        public Level(ContentManager content, string file)
        {
            Texture = content.Load<Texture2D>(file);
            DestinationRect = new Rectangle();
            SourceRect = new Rectangle();
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
