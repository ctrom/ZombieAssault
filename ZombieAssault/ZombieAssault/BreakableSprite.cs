using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieAssault
{
    class BreakableSprite : Sprite
    {
        Point frameSize;
        Point currentFrame;
        Point sheetSize;
        float rotation;

        public BreakableSprite(Texture2D textureImage, Vector2 position, float scale, int collisionOffset, Point frameSize, Point currentFrame, Point sheetSize, float rotation)
            : base(textureImage, position, scale, collisionOffset)
        {
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.rotation = rotation;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if(health <= 0)
            {
                currentFrame.X = 0;
                currentFrame.Y = 2;
            }
            base.Update(gameTime, clientBounds);
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(frameSize.X / 2, frameSize.Y / 2);
            spriteBatch.Draw(textureImage,
                position + new Vector2(2, SpriteManager.tileSize / 2),
                new Rectangle(currentFrame.X * frameSize.X,
                currentFrame.Y * frameSize.Y,
                frameSize.X,
                frameSize.Y),
                Color.White,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                .25f);
        }
    }
}
