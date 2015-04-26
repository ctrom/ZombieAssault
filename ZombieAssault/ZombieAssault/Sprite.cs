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
        protected MapNode position;
        protected float scale;
        protected int collisionOffset;

        public MapNode Position
        {
            get { return position; }
        }

        public Sprite(Texture2D textureImage, MapNode position, float scale, int collisionOffset)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.scale = scale;
            this.collisionOffset = collisionOffset;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage,
                position.Position,
                null,
                Color.White, 0,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0);
        }
    }
}
