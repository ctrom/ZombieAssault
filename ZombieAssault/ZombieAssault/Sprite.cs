using System;
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
        protected float scale;//factor to scale sprite image to coincide with game map resolution, example .375 to scale 64x64 pixel image to 24x24 tile size
        protected int collisionOffset;

        public Vector2 Position
        {
            get { return position; }
        }

        public Sprite(Texture2D textureImage, Vector2 position, float scale, int collisionOffset)
        {
            this.textureImage = textureImage;
            this.position = position*SpriteManager.scaleFactor;
            this.scale = scale*SpriteManager.scaleFactor;
            this.collisionOffset = collisionOffset;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            
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
