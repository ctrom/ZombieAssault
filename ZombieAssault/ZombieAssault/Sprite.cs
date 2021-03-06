﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ZombieAssault
{
    //Parent class for all sprites
    abstract class Sprite
    {
        protected Texture2D textureImage;
        protected Vector2 position;
        protected float scale;
        protected int collisionOffset;
        public float health;

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value * new Vector2(SpriteManager.tileSize, SpriteManager.tileSize) + new Vector2(Game1.resOffset - SpriteManager.tileSize - SpriteManager.gridOffset, -2*SpriteManager.tileSize);
            }
        }

        public Sprite(Texture2D textureImage, Vector2 position, float scale, int collisionOffset)
        {
            this.textureImage = textureImage;
            Position = position;
            this.scale = scale;
            this.collisionOffset = collisionOffset;
            health = 100;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (this.health < 1)
                foreach (Sprite s in SpriteManager.spriteList)
                    if (this == s)
                        SpriteManager.spriteList.Remove(s);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage,
                position,
                null,
                Color.White, 0,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0);
        }
    }
}
