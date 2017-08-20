﻿using System;
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
        Running = 1,
        SSJing = 2
    }

    class Player
    {
        public Animation IdleAnimation;
        public Animation RunAnimation;
        public Animation SSJAnimation;
        public Vector2 PlayerPosition;
        public bool Active;
        public int Health;
        public float Speed;
        public PlayerDirection Direction;
        public PlayerState State;
        private float SpriteScale;

        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle((int)PlayerPosition.X - (int)(IdleAnimation.FrameWidth / 2),
                                     (int)PlayerPosition.Y - (int)IdleAnimation.FrameHeight,
                                     IdleAnimation.FrameWidth, IdleAnimation.FrameHeight
                                     );
            }
        }

        public void Initialize(
            Animation idleAnimation,
            Animation runAnimation,
            Animation ssjAnimation,
            Vector2 position,
            float spriteScale
            )
        {
            IdleAnimation = idleAnimation;
            RunAnimation = runAnimation;
            SSJAnimation = ssjAnimation;
            PlayerPosition = position;
            Active = true;
            Health = 100;
            Direction = PlayerDirection.FacingRight;
            State = PlayerState.Idle;
            Speed = 3;
            SpriteScale = spriteScale;
        }

        public void Update(
            GameTime gameTime,
            KeyboardState keyboardState,
            Level level,
            Viewport viewport
            )
        {
            float direction = 0;
            if (keyboardState.IsKeyDown(Keys.D)) { direction += 1; }
            if (keyboardState.IsKeyDown(Keys.A)) { direction -= 1; }
            if (keyboardState.IsKeyDown(Keys.T)) { State = PlayerState.SSJing; }


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

            Vector2 movementVector = new Vector2(direction * Speed, 0);
            Rectangle translatedRectangle = new Rectangle(BoundingRectangle.X, BoundingRectangle.Y, BoundingRectangle.Width, BoundingRectangle.Height);
            translatedRectangle.Offset(movementVector);
            if(level.LevelCollisionBoxes.TrueForAll(b => !b.Intersects(translatedRectangle)))
            {
            PlayerPosition.X += direction * Speed;
            PlayerPosition.X = MathHelper.Clamp(PlayerPosition.X, 0, level.LevelWidth);
            }

            Animation currentAnimation = getAnimation();

            Vector2 upperCornerToFeet = new Vector2(
                (currentAnimation.FrameWidth / 2),
                currentAnimation.FrameHeight
                );
            Vector2 playerUpperCornerPosition = PlayerPosition - upperCornerToFeet;
            Vector2 playerViewPositionUnscaled = playerUpperCornerPosition - level.LevelPosition;
            Vector2 playerViewPositionScaled = level.ZoomFactor * playerViewPositionUnscaled;

            getAnimation().Update(
                gameTime,
                playerViewPositionScaled,
                viewport,
                directionEffects(Direction),
                level.ZoomFactor
                );
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
                case PlayerState.SSJing: return SSJAnimation;
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
