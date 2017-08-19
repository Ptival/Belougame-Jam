using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Belougame_Jam
{
    class Player
    {
        public Animation IdleAnimation;
        public Animation RunAnimation;
        public Vector2 Position;
        public bool Active;
        public bool IsRunning;
        public int Health;
        public float Speed;

        public void Initialize(
            Animation idleAnimation,
            Animation runAnimation,
            Vector2 position
            )
        {
            IdleAnimation = idleAnimation;
            RunAnimation = runAnimation;
            Position = position;
            Active = true;
            Health = 100;
            IsRunning = false;
            Speed = 1;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            float direction = 0;

            if (keyboardState.IsKeyDown(Keys.D)) { direction += 1; }
            if (keyboardState.IsKeyDown(Keys.A)) { direction -= 1; }

            IsRunning = direction != 0;

            Position.X += direction * Speed;
            SpriteEffects effects = direction >= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            IdleAnimation.Position = Position;
            IdleAnimation.Update(gameTime, effects);
            RunAnimation.Position = Position;
            RunAnimation.Update(gameTime, effects);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation displayedAnimation = IsRunning ? RunAnimation : IdleAnimation;
            displayedAnimation.Draw(spriteBatch);
        }
    }
}
