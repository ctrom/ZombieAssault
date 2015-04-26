using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieAssault
{
    //Parent class for all mobile sprites
    abstract class AnimatedSprite : Sprite
    {
        Point frameSize;//dimensions of individual animation frames on sprite sheet

        protected MapNode destination;
        protected Point currentFrame;
        protected Point sheetSize;//number of animation frames on the sprite sheet, example 3 across and 2 down or Point(3,2)
        protected int timeSinceLastFrame = 0;
        protected int millisecondsPerFrame;
        protected float rotation;//in radians
        protected Vector2 direction;
        float speed;

        public abstract Vector2 Direction
        {
            get;
        }

        //Destination property, multiplies desired tile indexes by tile size to get coordinates
        //use only for setting desination based on index of tile rather than pixel coordinate
        public virtual MapNode Destination
        {
            get { return destination; }
            set 
            {
                if (value.Type != 0)
                {
                    destination = value;
                }
            }
        }

        public AnimatedSprite(Texture2D textureImage, MapNode position, Point frameSize, Point currentFrame, Point sheetSize, float rotation, float speed, float scale, int collisionOffset, Vector2 direction, int millisecondsPerFrame)
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
            //Main movement code, controls how the sprite translates each update
            //rough implementation, needs to be redone to work with window scaling and to create more uniform speed regardless of walk angle
            if (position.Position != Destination.Position)//checks if sprite is at destination
            {
                rotation = (float)(Math.Atan2(Destination.Position.Y - position.Position.Y, Destination.Position.X - position.Position.X)) + (float)Math.PI / 2;//calculates angle of rotation so sprite faces destination

                if (Math.Abs(Destination.Position.X - position.Position.X) < Math.Abs(Destination.Position.Y - position.Position.Y))//executes if magnitude of y difference is greater than that of x
                {
                    //Checks if x requires translation, either positive, negative or none
                    //multiplied by factor of speed
                    if (position.Position.X < Destination.Position.X)
                        direction.X = Math.Abs(((float)Destination.Position.X - position.Position.X) / (Destination.Position.Y - position.Position.Y) * speed);
                    else if (position.Position.X > Destination.Position.X)
                        direction.X = -Math.Abs(((float)Destination.Position.X - position.Position.X) / (Destination.Position.Y - position.Position.Y) * speed);
                    else
                        direction.X = 0;
                    //checks if y requires translation, either positive, negative, or none
                    //multiplied by factor of speed
                    if (position.Position.Y < Destination.Position.Y)
                        direction.Y = 1 * speed;
                    else if (position.Position.Y > Destination.Position.Y)
                        direction.Y = -1 * speed;
                    else
                        direction.Y = 0;
                }
                else if (Math.Abs(Destination.Position.X - position.Position.X) > Math.Abs(Destination.Position.Y - position.Position.Y))//executes if magnitude of x difference is greater than that of y
                {
                    //Checks if x requires translation, either positive, negative or none
                    //multiplied by factor of speed
                    if (position.Position.X < Destination.Position.X)
                        direction.X = 1 * speed;
                    else if (position.Position.X > Destination.Position.X)
                        direction.X = -1 * speed;
                    else
                        direction.X = 0;
                    //checks if y requires translation, either positive, negative, or none
                    //multiplied by factor of speed
                    if (position.Position.Y < Destination.Position.Y)
                        direction.Y = Math.Abs(((float)Destination.Position.Y - position.Position.Y) / (Destination.Position.X - position.Position.X) * speed);
                    else if (position.Position.Y > Destination.Position.Y)
                        direction.Y = -Math.Abs(((float)Destination.Position.Y - position.Position.Y) / (Destination.Position.X - position.Position.X) * speed);
                    else
                        direction.Y = 0;
                }
                else//executes if magnitudes are the same
                {
                    //determines if x translation is positive, negative, or none
                    if (position.Position.X < Destination.Position.X)
                        direction.X = 1 / (float)Math.Sqrt(2) * speed;
                    else if (position.Position.X > Destination.Position.X)
                        direction.X = -1 / (float)Math.Sqrt(2) * speed;
                    else
                        direction.X = 0;
                    //determines if y translation is positive, negative, or none
                    if (position.Position.Y < Destination.Position.Y)
                        direction.Y = 1 / (float)Math.Sqrt(2) * speed;
                    else if (position.Position.Y > Destination.Position.Y)
                        direction.Y = -1 / (float)Math.Sqrt(2) * speed;
                    else
                        direction.Y = 0;
                }
            }
            else
                direction = Vector2.Zero;//sets to no translation on update

            

            //sets position to destination if within 1 pixel in both x and y to prevent sprite from stuttering
            if (Math.Abs(position.Position.X - Destination.Position.X) < 1 && Math.Abs(position.Position.Y - Destination.Position.Y) < 1)
            {
                position = destination;
            }
            else
                position.Position += direction;

            base.Update(gameTime, clientBounds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(frameSize.X / 2, frameSize.Y / 2);
            spriteBatch.Draw(textureImage,
                position.Position + new Vector2(2, SpriteManager.tileSize/2),
                new Rectangle(currentFrame.X * frameSize.X,
                currentFrame.Y * frameSize.Y,
                frameSize.X,
                frameSize.Y),
                Color.White, 
                rotation,
                origin,
                scale,
                SpriteEffects.None, 
                .5f);
        }

        //holdover from lab code, not sure how it works or what it does
        public Rectangle collisionRectangle
        {
            get
            {
                return new Rectangle(
                    (int)position.Position.X + collisionOffset,
                    (int)position.Position.Y + collisionOffset,
                    frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }
        }
    }
}
