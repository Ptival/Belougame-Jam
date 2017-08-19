using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Belougame_Jam
{
    class Player
    {
        public Animation PlayerAnimation;
        public Vector2 Position;
        public bool Active;
        public int Health;
        public float Speed;

        public void Initialize(Animation animation, Vector2 position)
        {
            PlayerAnimation = animation;
            Position = position;
            Active = true;
            Health = 100;
            Speed = 1;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            float direction = 0;
            if (keyboardState.IsKeyDown(Keys.D)) { direction += 1; }
            if (keyboardState.IsKeyDown(Keys.A)) { direction -= 1; }
            Position.X += direction * Speed;
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }
    }
}
