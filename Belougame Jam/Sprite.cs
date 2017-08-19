using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Belougame_Jam
{
    class Sprite
    {
        private Texture2D texture;
        public Animation animation;
        string File;
        int Width;
        int Height;
        int Frames;
        int FrameTime;
        float Scale;

        public Sprite(ContentManager content, string file, int width, int height, int frames, int frameTime, int scale)
        {
            File = file;
            Frames = frames;
            Height = height;
            Width = width;
            FrameTime = frameTime;
            Scale = scale;

            texture = content.Load<Texture2D>(file);
            animation = new Animation();
            animation.Initialize(
                texture, Vector2.Zero,
                Width, Height, Frames, FrameTime,
                Color.White, scale, true
                );
        }
    }
}
