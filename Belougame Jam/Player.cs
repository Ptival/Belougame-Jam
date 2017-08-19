﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Belouga
{
    class Player
    {
        public Texture2D PlayerTexture;
        public Vector2 Position;
        public bool Active;
        public int Health;

        public void Initialize(Texture2D texture, Vector2 position)
        {
            PlayerTexture = texture;
            Position = position;
            Active = true;
            Health = 100;
        }

        public void Update()
        {
            // TODO
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
