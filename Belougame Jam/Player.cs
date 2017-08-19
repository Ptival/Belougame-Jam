using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Belougame_Jam
{
    public enum PlayerDirection
    {
        FacingLeft = 0,
        FacingRight = 1
    }

    public enum PlayerState
    {
        Idle = 0,
        Running = 1
    }   

    class Player
    {
        public Animation IdleAnimation;
        public Animation RunAnimation;
        public Vector2 Position;
        public bool Active;
        public int Health;
        public float Speed;
        public PlayerDirection Direction;
        public PlayerState State;

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
            Direction = PlayerDirection.FacingRight;
            State = PlayerState.Idle;
            Speed = 1;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            float direction = 0;
            if (keyboardState.IsKeyDown(Keys.D)) { direction += 1; }
            if (keyboardState.IsKeyDown(Keys.A)) { direction -= 1; }

            if (direction == 0)
            {
                State = PlayerState.Idle;
            }
            else if (direction > 0)
            {
                Direction = PlayerDirection.FacingRight;
                State = PlayerState.Running;
            } else
            {
                Direction = PlayerDirection.FacingLeft;
                State = PlayerState.Running;
            }

            Position.X += direction * Speed;

            SpriteEffects effects = directionEffects(Direction);            
            IdleAnimation.Position = Position;
            IdleAnimation.Update(gameTime, effects);
            RunAnimation.Position = Position;
            RunAnimation.Update(gameTime, effects);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            getAnimation().Draw(spriteBatch);
        }

        private Animation getAnimation()
        {
            switch (State)
            {
                case PlayerState.Idle: return IdleAnimation;
                case PlayerState.Running: return RunAnimation;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private SpriteEffects directionEffects(PlayerDirection d)
        {
            switch (Direction)
            {
                case PlayerDirection.FacingLeft: return SpriteEffects.FlipHorizontally;
                case PlayerDirection.FacingRight: return SpriteEffects.None;
                default: throw new ArgumentOutOfRangeException();
            };
        }
    }
}
