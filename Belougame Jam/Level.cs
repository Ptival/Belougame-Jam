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
        private Vector2 Position;

        public Level(ContentManager content, string file)
        {
            Texture = content.Load<Texture2D>(file);
        }

        public void Update()
        {
        }

        public void Draw(
            GraphicsDevice GraphicsDevice,
            SpriteBatch spriteBatch
            )
        {
            Rectangle destinationRect = new Rectangle(
                0, 0,
                GraphicsDevice.Viewport.TitleSafeArea.Width,
                GraphicsDevice.Viewport.TitleSafeArea.Height
                );

            // Texture :  T.W * T.H
            // Screen  :  V.W * V.H

            Rectangle sourceRect = new Rectangle(
                0, 0,
                (Texture.Height * GraphicsDevice.Viewport.TitleSafeArea.Width) / GraphicsDevice.Viewport.TitleSafeArea.Height,
                Texture.Height
                );

            spriteBatch.Draw(Texture, destinationRect, sourceRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
        }
    }
}
