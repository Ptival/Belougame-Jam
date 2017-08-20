using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Belougame_Jam
{
    class Background
    {
        private Texture2D Texture;      //The image to use
        private Vector2 Offset;         //Offset to start drawing our image
        public Vector2 Speed;           //Speed of movement of our parallax effect
        public float ZoomFactor;

        private Viewport Viewport;      //Our game viewport

        //Calculate Rectangle dimensions, based on offset/viewport/zoom values
        private Rectangle srcRectangle
        {
            get
            {
                return new Rectangle(0, 0, (int)Texture.Width, (int)Texture.Height);
            }
        }

        public Background(Texture2D texture, Vector2 speed)
        {
            Texture = texture;
            Offset = Vector2.Zero;
            Speed = speed;
        }

        public void Update(Vector2 direction, Viewport viewport, Player player, float zoomFactor)
        {
            Viewport = viewport;
            ZoomFactor = zoomFactor;

            //Calculate the distance to move our image, based on speed
            Vector2 distance = direction * Speed;

            //Update our offset
            Offset = (new Vector2(-player.PlayerPosition.X, 0) * Speed);
            Offset.X = Offset.X % Texture.Width;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Level:   256px
            // Texture: 100px
            // Offset:  50px
            //
            // Texture 0 : (-50,  50, 100)   (0,    50, 50)
            // Texture 1 : (50,  150, 100)   (50,  150, 100)
            // Texture 2 : (150, 250, 100)   (150, 250, 100)
            // Texture 3 : (250, 350, 100)   (250, 256, 6)
            for (
                int textureOffset = (int)(Offset.X - Texture.Width);
                textureOffset <= (Viewport.Width / ZoomFactor);
                textureOffset += Texture.Width
            )
            {
                Rectangle dstRectangle = new Rectangle(
                    (int)(textureOffset * ZoomFactor),
                    0,
                    (int)(Texture.Width * ZoomFactor),
                    (int)(Texture.Height * ZoomFactor)
                    );
                spriteBatch.Draw(Texture, dstRectangle, srcRectangle, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            }
        }
    }
}
