using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Belougame_Jam
{
    class Animation
    {
        Texture2D spriteStrip;
        float scale;
        int elapsedTime;
        int frameTime;
        int frameCount;
        int currentFrame;
        Color color;
        Rectangle sourceRect = new Rectangle();
        Rectangle destinationRect = new Rectangle();
        public int FrameWidth;
        public int FrameHeight;
        public bool Active;
        public bool Looping;
        SpriteEffects Effects;

        public void Initialize(
            Texture2D texture,
            int frameWidth, int frameHeight, int frameCount, int frametime,
            Color color, float scale,
            bool looping
            )
        {
            this.color = color;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            this.frameCount = frameCount;
            frameTime = frametime;
            this.scale = scale;
            Looping = looping;
            spriteStrip = texture;
            elapsedTime = 0;
            currentFrame = 0;
            Active = true;
        }

        public void Update(
            GameTime gameTime,
            Vector2 position,
            Viewport viewport,
            SpriteEffects effects
            )
        {
            if (Active == false) return;
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            Effects = effects;

            // si besoin, mettre à jour le numéro de frame (currentFrame)
            if (elapsedTime > frameTime)
            {
                currentFrame++;
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    if (Looping == false)
                        Active = false;
                }
                elapsedTime = 0;
            }

            // calculer le rectangle de la frame
            /*  (x=0,y=0)---------(x=W,y=0)
             *      |*****************|
             *      |*****************|
             *      |*****************|    hauteur du rectangle H (height)
             *      |*****************|
             *   (x=0,y=H)---------(x=W,y=H)
             *              largeur du rectangle W (width)
             */
            // (x, y, width, height)
            // f    x    y    w    h
            // 0:   0    0    64   64
            // 1:   0   64    64   64
            // 2:   0  128    64   64
            // 3:   0  192    64   64

            // x = 0
            // y = f * hauteur
            // w = largeur
            // h = hauteur
            sourceRect = new Rectangle(0, currentFrame * FrameHeight, FrameWidth, FrameHeight);

            destinationRect = new Rectangle(
                (int)position.X - (int)(viewport.TitleSafeArea.Width * scale) / 2,
                (int)position.Y - (int)(viewport.TitleSafeArea.Height * scale) / 2,
                (int)(viewport.TitleSafeArea.Width * scale),
                (int)(viewport.TitleSafeArea.Height * scale)
            );
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw(
                    spriteStrip, destinationRect, sourceRect,
                    color, 0, Vector2.Zero, Effects, 0
                    );
            }
        }
    }
}
