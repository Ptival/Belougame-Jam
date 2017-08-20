using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Belougame_Jam
{
    public class Background
    {
        private Texture2D Texture;      //The image to use
        private Vector2 Offset;         //Offset to start drawing our image
        public Vector2 Speed;           //Speed of movement of our parallax effect
        public float Scale;              //Zoom level of our image

        private Viewport Viewport;      //Our game viewport

        //Calculate Rectangle dimensions, based on offset/viewport/zoom values
        private Rectangle Rectangle
        {
            get {
                return new Rectangle(
                    (int)(Offset.X), (int)(Offset.Y),
                    (int)((Texture.Height * Viewport.TitleSafeArea.Width) / Viewport.TitleSafeArea.Height),
                    (int)(Viewport.Height / Scale));
            }
        }

        public Background(Texture2D texture, Vector2 speed, float scale)
        {
            Texture = texture;
            Offset = Vector2.Zero;
            Speed = speed;
            Scale = scale;
        }

        public void Update(GameTime gametime, Vector2 direction, Viewport viewport)
        {
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

            //Store the viewport
            Viewport = viewport;

            //Calculate the distance to move our image, based on speed
            Vector2 distance = direction * Speed * elapsed;

            //Update our offset
            Offset += distance;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                new Vector2(Viewport.X, Viewport.Y),
                Rectangle, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 1
                );
        }
    }
}
