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

        public int UnitNumber
        {
            get
            {
                return unitNumber;
            }
        }

        public PlayerControlledSprite(Texture2D textureImage, Vector2 position, float speed, float scale, float rotation, int unitNumber)
            : base (textureImage, position, new Point(64, 64), new Point(0,0), new Point(3,2), rotation, speed, scale, 0, new Vector2(0,0), 250)
        {
            destination = Map.getNode(position);//initializes destination as starting position
            this.unitNumber = unitNumber;
        }

        public override Vector2 Direction
        {
            get { return direction; }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //algorithm for traversing sprite sheet
            currentFrame.Y = 0;//initializes as idle animation
            if (position != Destination.Position)
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

            base.Update(gameTime, clientBounds);
        }
    }
}
