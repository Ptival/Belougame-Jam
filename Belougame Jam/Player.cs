using System;
using SD = System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
        Jumping = 2
    }

    class Player
    {
        public Animation IdleAnimation;
        public Animation MidAirAnimation;
        public Animation RunAnimation;
        public Vector2 PlayerPosition;
        public bool Active;
        public int Health;
        public PlayerDirection Direction;
        public PlayerState State;
        private float SpriteScale;
        private Vector2 Gravity = new Vector2(0.0f, 0.1f);
        private Vector2 Velocity;
        const float velocityBound = 2.0f;
        private Vector2 VelocityMin = new Vector2(-velocityBound, -velocityBound);
        private Vector2 VelocityMax = new Vector2(velocityBound, velocityBound);
        private bool CanJump;

        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle(
                    (int)PlayerPosition.X - (int)(IdleAnimation.FrameWidth / 2),
                    (int)PlayerPosition.Y - (int)IdleAnimation.FrameHeight,
                    IdleAnimation.FrameWidth, IdleAnimation.FrameHeight
                    );
            }
        }

        public void Initialize(
            Animation idleAnimation,
            Animation runAnimation,
            Vector2 position,
            float spriteScale
            )
        {
            IdleAnimation = idleAnimation;
            RunAnimation = runAnimation;
            PlayerPosition = position;
            Active = true;
            CanJump = true;
            Health = 100;
            Direction = PlayerDirection.FacingRight;
            State = PlayerState.Idle;
            SpriteScale = spriteScale;
            Velocity = new Vector2(0, 0);
        }

        public void Update(
            GameTime gameTime,
            KeyboardState keyboardState,
            Level level,
            Viewport viewport
            )
        {
            Vector2 acceleration = new Vector2(0, 0);
            if (keyboardState.IsKeyDown(Keys.D)) { acceleration.X += 1; }
            if (keyboardState.IsKeyDown(Keys.A)) { acceleration.X -= 1; }
            if (keyboardState.IsKeyDown(Keys.Space) && CanJump)
            {
                acceleration.Y = -30.0f;
                CanJump = false;
            }

            Vector2 friction = new Vector2(0, 0);
            if (!CanJump)
            {
                State = PlayerState.Jumping;
            }
            if (acceleration.X == 0)
            {
                State = PlayerState.Idle;
                friction.X = - 0.25f * Velocity.X;
            }
            else if (acceleration.X > 0)
            {
                Direction = PlayerDirection.FacingRight;
                State = PlayerState.Running;
            }
            else
            {
                Direction = PlayerDirection.FacingLeft;
                State = PlayerState.Running;
            }

            Velocity += acceleration + friction + Gravity;
            Velocity.X = MathHelper.Clamp(Velocity.X, VelocityMin.X, VelocityMax.X);
            Velocity.Y = MathHelper.Clamp(Velocity.Y, VelocityMin.Y, VelocityMax.Y);

            SD.RectangleF translatedRectangle =
                new SD.RectangleF(
                    BoundingRectangle.X, BoundingRectangle.Y,
                    BoundingRectangle.Width, BoundingRectangle.Height
                    );
            SD.PointF velocityF = new SD.PointF(Velocity.X, Velocity.Y);
            translatedRectangle.Offset(velocityF);

            List<Rectangle> collidedTiles =
                level.LevelCollisionBoxes.FindAll(b => (
                    new SD.RectangleF(b.X, b.Y, b.Width, b.Height
                ).IntersectsWith(translatedRectangle)));
            foreach (Rectangle collided in collidedTiles)
            {
                // hit box to the left
                if (BoundingRectangle.Left >= collided.Right && translatedRectangle.Left < collided.Right
                    && !(BoundingRectangle.Bottom <= collided.Top || BoundingRectangle.Top >= collided.Bottom)
                    )
                {
                    Velocity.X = 0;
                }
                // hit box to the right
                if (BoundingRectangle.Right <= collided.Left && translatedRectangle.Right > collided.Left
                    && !(BoundingRectangle.Bottom <= collided.Top || BoundingRectangle.Top >= collided.Bottom)
                    )
                {
                    Velocity.X = 0;
                }
                // fell on box
                if (BoundingRectangle.Bottom <= collided.Top && translatedRectangle.Bottom > collided.Top)
                {
                    Velocity.Y = 0;
                    CanJump = true;
                }
                // hit a box above me
                if (BoundingRectangle.Top <= collided.Bottom && translatedRectangle.Top > collided.Bottom)
                {
                    Velocity.Y = 0;
                }
            }

            PlayerPosition += Velocity;
            PlayerPosition.X = MathHelper.Clamp(PlayerPosition.X, 0, level.LevelWidth);
            PlayerPosition.Y = MathHelper.Clamp(PlayerPosition.Y, 0, level.LevelHeight);

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
