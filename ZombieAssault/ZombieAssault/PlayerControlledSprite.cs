using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieAssault
{
    //Instantiable class for player units
    class PlayerControlledSprite : AnimatedSprite
    {
        
        private int unitNumber;
        private List<Vector2> path;

        public int UnitNumber
        {
            get
            {
                return unitNumber;
            }
        }

        public PlayerControlledSprite(Texture2D textureImage, Vector2 position, float speed, float scale, float rotation, int unitNumber)
            : base (textureImage, position, new Point(64, 64), new Point(0,0), new Point(3,3), rotation, speed, scale, 0, new Vector2(0,0), 200)
        {
            destination = new Vector2(position.X * SpriteManager.tileSize + Game1.resOffset - SpriteManager.gridOffset, position.Y * SpriteManager.tileSize);//initializes destination as starting position
            this.unitNumber = unitNumber;
            path = new List<Vector2>();
        }

        public override Vector2 Direction
        {
            get { return direction; }
        }

        public List<Vector2> Path
        {
            get { return path; }
            set
            {
                path = value;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //algorithm for traversing sprite sheet
            currentFrame.Y = 0;//initializes as idle animation
            if (path.Count != 0)
            {
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > millisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    ++currentFrame.X;//increments to next frame on sheet
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = 1;//resets frame to start of walk animation
                    }
                }
            }
            else
                currentFrame.X = 0;//sets current frame to idle animation
            if (path.Count > 0)
            {
                Destination = path.ElementAt(0);
                if (Math.Abs(destination.X - position.X) < 1 && Math.Abs(destination.Y - position.Y) < 1)
                {
                    path.Remove(path.ElementAt(0));
                }
            }

            base.Update(gameTime, clientBounds);
        }
    }
}
