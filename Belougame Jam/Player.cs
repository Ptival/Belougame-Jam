using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Belougame_Jam
{
    class Player
    {
        public Animation PlayerAnimation;
        // public Texture2D PlayerTexture;
        public Vector2 Position;
        public bool Active;
        public int Health;

        public void Initialize(Animation animation, Vector2 position)
        {
            PlayerAnimation = animation;
            // PlayerTexture = texture;
            Position = position;
            Active = true;
            Health = 100;
        }

        public void Update(GameTime gameTime)
        {
            // changer la position
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }
    }
}
