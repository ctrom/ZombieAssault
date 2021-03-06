﻿using System;
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

        protected Vector2 destination;
        protected Point currentFrame;
        protected Point sheetSize;//number of animation frames on the sprite sheet, example 3 across and 2 down or Point(3,2)
        protected int timeSinceLastFrame = 0;
        protected int millisecondsPerFrame;
        protected float rotation;//in radians
        protected Vector2 direction;
        float speed;
        protected List<Vector2> path;

        public abstract Vector2 Direction
        {
            get;
        }

        //Destination property, multiplies desired tile indexes by tile size to get coordinates
        //use only for setting desination based on index of tile rather than pixel coordinate
        public virtual Vector2 Destination
        {
            get { return destination; }
            set
            {
                destination = value;
            }
        }

        public List<Vector2> Path
        {
            get { return path; }
            set
            {
                path = value;
            }
        }

        public AnimatedSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, float rotation, float speed, float scale, int collisionOffset, Vector2 direction, int millisecondsPerFrame)
            : base(textureImage, position, scale, collisionOffset)
        {
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.rotation = rotation;
            this.speed = speed;
            this.direction = direction;
            this.millisecondsPerFrame = millisecondsPerFrame;
            path = new List<Vector2>();
        }

        //public virtual List<MapNode> FindPath(MapNode destNode)
        //{
        //    List<MapNode> path = new List<MapNode>();
        //    List<MapNode> nodes = Map.nodeList();
        //    if(Map.getNode(findIndex(position)).Type == 1 && destNode.Type == 2)//Actor is outside and destination is inside
        //    {
        //        var accessPoints = from node in nodes
        //                           where node.Type == 3
        //                           select node;

        //        MapNode closest = null;
        //        foreach(MapNode n in accessPoints)
        //        {
        //            if (closest == null)
        //                closest = n;
        //            else if(distance(closest.Position) > distance(n.Position))
        //            {
        //                closest = n;
        //            }
        //        }
        //        path.Add(closest);
        //    }
        //    path.Add(destNode);
        //    return path;
        //}

        private Vector2 findIndex(Vector2 pos)
        {
            return new Vector2(((int)(((pos.X - 4) - Game1.resOffset) / SpriteManager.tileSize) + 2), ((int)((pos.Y + 1) / SpriteManager.tileSize) + 2));
        }

        private float distance(Vector2 pos)
        {
            return Math.Abs(pos.X - position.X) + Math.Abs(pos.Y - position.Y);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //List<MapNode> path = FindPath(destination);

            
            if(Destination == null)
            {
                Destination = position + new Vector2(Game1.resOffset, Game1.resOffset);
            }
            if (Position != Destination)
            {
                //Main movement code, controls how the sprite translates each update
                //rough implementation, needs to be redone to work with window scaling and to create more uniform speed regardless of walk angle
                if (position != Destination)//checks if sprite is at destination
                {
                    rotation = (float)(Math.Atan2(Destination.Y - position.Y, Destination.X - position.X)) + (float)Math.PI / 2;//calculates angle of rotation so sprite faces destination

                    if (Math.Abs(Destination.X - position.X) < Math.Abs(Destination.Y - position.Y))//executes if magnitude of y difference is greater than that of x
                    {
                        //Checks if x requires translation, either positive, negative or none
                        //multiplied by factor of speed
                        if (position.X < Destination.X)
                            direction.X = /*isCollision(1) * */Math.Abs(((float)Destination.X - position.X) / (Destination.Y - position.Y) * speed);
                        else if (position.X > Destination.X)
                            direction.X = /*isCollision(3) * */-Math.Abs(((float)Destination.X - position.X) / (Destination.Y - position.Y) * speed);
                        else
                            direction.X = 0;
                        //checks if y requires translation, either positive, negative, or none
                        //multiplied by factor of speed
                        if (position.Y < Destination.Y)
                            direction.Y = /*isCollision(0) * */1 * speed;
                        else if (position.Y > Destination.Y)
                            direction.Y = /*isCollision(2) * */-1 * speed;
                        else
                            direction.Y = 0;
                    }
                    else if (Math.Abs(Destination.X - position.X) > Math.Abs(Destination.Y - position.Y))//executes if magnitude of x difference is greater than that of y
                    {
                        //Checks if x requires translation, either positive, negative or none
                        //multiplied by factor of speed
                        if (position.X < Destination.X)
                            direction.X = /*isCollision(1) * */1 * speed;
                        else if (position.X > Destination.X)
                            direction.X = /*isCollision(3) * */-1 * speed;
                        else
                            direction.X = 0;
                        //checks if y requires translation, either positive, negative, or none
                        //multiplied by factor of speed
                        if (position.Y < Destination.Y)
                            direction.Y = /*isCollision(0) * */Math.Abs(((float)Destination.Y - position.Y) / (Destination.X - position.X) * speed);
                        else if (position.Y > Destination.Y)
                            direction.Y = /*isCollision(2) * */-Math.Abs(((float)Destination.Y - position.Y) / (Destination.X - position.X) * speed);
                        else
                            direction.Y = 0;
                    }
                    else//executes if magnitudes are the same
                    {
                        //determines if x translation is positive, negative, or none
                        if (position.X < Destination.X)
                            direction.X = /*isCollision(1) * */1 / (float)Math.Sqrt(2) * speed;
                        else if (position.X > Destination.X)
                            direction.X = /*isCollision(3) * */-1 / (float)Math.Sqrt(2) * speed;
                        else
                            direction.X = 0;
                        //determines if y translation is positive, negative, or none
                        if (position.Y < Destination.Y)
                            direction.Y = /*isCollision(0) * */1 / (float)Math.Sqrt(2) * speed;
                        else if (position.Y > Destination.Y)
                            direction.Y = /*isCollision(2) * */-1 / (float)Math.Sqrt(2) * speed;
                        else
                            direction.Y = 0;
                    }
                }
                else
                    direction = Vector2.Zero;//sets to no translation on update

                Vector2 diff = new Vector2(Math.Abs(position.X - Destination.X), Math.Abs(position.Y - Destination.Y));

                //sets position to destination if within 1 pixel in both x and y to prevent sprite from stuttering
                if (Math.Abs((float)position.X - Destination.X) < 1 && Math.Abs((float)position.Y - Destination.Y) < 1)
                {
                    position = Destination;
                }
                else
                    position += direction;
            }

            base.Update(gameTime, clientBounds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(frameSize.X / 2, frameSize.Y / 2);
            spriteBatch.Draw(textureImage,
                position + new Vector2(2, SpriteManager.tileSize/2),
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
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }
        }

        /*theoretical collision detection - A
         * the way i am thinking this could work is to call it in the 
         * movement code. the movement code can pass in an integer that
         * represents the direction it wants to check and it checks for 
         * collisions in that direction. it then returns either 1 for no 
         * collision or 0 for a collision and then we just multiply the 
         * direction by that 1 or 0 to either have it move as normal or
         * zero its direction. 
         */ 
        protected int isCollision(int whichDirection)
        {
            Rectangle nextPosition;
            Rectangle otherSprite;
            foreach (Sprite s in SpriteManager.spriteList)
            {
                if (this != s)
                {
                    otherSprite = new Rectangle((int)s.Position.X, (int)s.Position.Y, this.frameSize.X,
                                this.frameSize.Y);
                    switch (whichDirection)
                    {
                        case 0: //going up
                            nextPosition = new Rectangle((int)this.position.X, (int)this.position.Y + 1, 
                                this.frameSize.X, this.frameSize.Y);
                            if (nextPosition.Intersects(otherSprite))
                                return 0;
                            else
                                return 1;
                        case 1: //going right
                            nextPosition = new Rectangle((int)this.position.X + 1, (int)this.position.Y,
                                this.frameSize.X, this.frameSize.Y);
                            if (nextPosition.Intersects(otherSprite))
                                return 0;
                            else
                                return 1;
                        case 2: //going down
                            nextPosition = new Rectangle((int)this.position.X, (int)this.position.Y - 1,
                                this.frameSize.X, this.frameSize.Y);
                            if (nextPosition.Intersects(otherSprite))
                                return 0;
                            else
                                return 1;
                        case 3: //going left
                            nextPosition = new Rectangle((int)this.position.X - 1, (int)this.position.Y,
                                this.frameSize.X, this.frameSize.Y);
                            if (nextPosition.Intersects(otherSprite))
                                return 0;
                            else
                                return 1;
                        default: return 0;
                    }                    
                }
                else return 0;
            }
            return 0;
        }
        
    }
}
