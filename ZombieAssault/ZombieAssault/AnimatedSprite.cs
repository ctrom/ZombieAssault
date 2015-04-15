using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieAssault
{
    abstract class AnimatedSprite : Sprite
    {
        Point frameSize;

        protected Vector2 destination;
        protected Point currentFrame;
        protected Point sheetSize;
        protected int timeSinceLastFrame = 0;
        protected int millisecondsPerFrame;
        protected float rotation;
        protected Vector2 direction;
        float speed;

        public abstract Vector2 Direction
        {
            get;
        }

        public virtual Vector2 Destination
        {
            get { return destination; }
            set { destination = new Vector2((value.X * 24), (value.Y * 24)); }
        }

        public AnimatedSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize,
            float scale, float rotation, float speed, int collisionOffset, Vector2 direction, int millisecondsPerFrame)
            : base(textureImage, position, scale, collisionOffset)
        {
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.rotation = rotation;
            this.speed = speed;
            this.direction = direction;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (position != destination)
            {
                if (Math.Abs(Destination.X - position.X) < Math.Abs(Destination.Y - position.Y))
                {
                    if (position.X < Destination.X)
                        direction.X = Math.Abs(((float)Destination.X - position.X) / (Destination.Y - position.Y) * speed);
                    else if (position.X > Destination.X)
                        direction.X = -Math.Abs(((float)Destination.X - position.X) / (Destination.Y - position.Y) * speed);
                    else
                        direction.X = 0;
                    if (position.Y < Destination.Y)
                        direction.Y = 1 * speed;
                    else if (position.Y > Destination.Y)
                        direction.Y = -1 * speed;
                    else
                        direction.Y = 0;
                }
                else if (Math.Abs(Destination.X - position.X) > Math.Abs(Destination.Y - position.Y))
                {
                    if (position.X < Destination.X)
                        direction.X = 1 * speed;
                    else if (position.X > Destination.X)
                        direction.X = -1 * speed;
                    else
                        direction.X = 0;
                    if (position.Y < Destination.Y)
                        direction.Y = Math.Abs(((float)Destination.Y - position.Y) / (Destination.X - position.X) * speed);
                    else if (position.Y > Destination.Y)
                        direction.Y = -Math.Abs(((float)Destination.Y - position.Y) / (Destination.X - position.X) * speed);
                    else
                        direction.Y = 0;
                }
                else
                {
                    if (position.X < Destination.X)
                        direction.X = 1 / (float)Math.Sqrt(2) * speed;
                    else if (position.X > Destination.X)
                        direction.X = -1 / (float)Math.Sqrt(2) * speed;
                    else
                        direction.X = 0;
                    if (position.Y < Destination.Y)
                        direction.Y = 1 / (float)Math.Sqrt(2) * speed;
                    else if (position.Y > Destination.Y)
                        direction.Y = -1 / (float)Math.Sqrt(2) * speed;
                    else
                        direction.Y = 0;
                }
            }
            else
                direction = Vector2.Zero;

            rotation = (float)(Math.Atan2(Destination.Y - position.Y, Destination.X - position.X)) + (float)Math.PI / 2;

            if (Math.Abs(position.X - Destination.X) < 1 && Math.Abs(position.Y - Destination.Y) < 1)
                position = destination;
            else
                position += direction;

            base.Update(gameTime, clientBounds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(frameSize.X / 2, frameSize.Y / 2);
            spriteBatch.Draw(textureImage,
                position + origin,
                new Rectangle(currentFrame.X * frameSize.X,
                currentFrame.Y * frameSize.Y,
                frameSize.X,
                frameSize.Y),
                Color.White, 
                rotation,
                origin,
                scale,
                SpriteEffects.None, 
                1);
        }

        public Rectangle collisionRectangle
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }
        }
    }
}
