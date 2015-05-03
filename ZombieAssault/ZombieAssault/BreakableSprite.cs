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
        Vector2 repairSide;
        float rotation;

        public Vector2 RepairSide
        {
            get { return repairSide; }
        }

        public BreakableSprite(Texture2D textureImage, Vector2 position, float scale, int collisionOffset, Point frameSize, Point currentFrame, Point sheetSize, float rotation)
            : base(textureImage, position, scale, collisionOffset)
        {
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.rotation = rotation;
            if(rotation == 0)
            {
                repairSide = position + new Vector2(1, 0);
            }
            else if(rotation == (float)Math.PI/2)
            {
                repairSide = position + new Vector2(0, 1);
            }
            else if(rotation == (float)(Math.PI * 3)/2)
            {
                repairSide = position - new Vector2(0, 1);
            }
            else
            {
                repairSide = position - new Vector2(1, 0);
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if(health <= 0)
            {
                currentFrame.X = 0;
                currentFrame.Y = 2;
            }
            else if( health > 100)
            {
                currentFrame.X = 0;
                currentFrame.Y = 1;
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
