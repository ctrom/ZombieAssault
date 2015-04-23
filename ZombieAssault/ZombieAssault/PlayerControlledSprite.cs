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
        //variables for tracking mouse inputs
        private MouseState previousState;
        private MouseState currentState;

        public PlayerControlledSprite(Texture2D textureImage, Vector2 position, float speed, float scale, float rotation)
            : base (textureImage, position, new Point(64, 64), new Point(0,0), new Point(3,2), scale, rotation, speed, 0, new Vector2(0,0), 250)
        {
            destination = position;//initializes destination as starting position
        }

        public override Vector2 Destination
        {
            get
            {
                return base.Destination;
            }
            set
            {
                destination = new Vector2((value.X * SpriteManager.tileSize)-20, (value.Y * SpriteManager.tileSize)-20);
            }
        }

        public override Vector2 Direction
        {
            get { return direction; }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //algorithm for traversing sprite sheet
            currentFrame.Y = 0;//initializes as idle animation
            if (position != destination)
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

            //polls mouse state
            previousState = currentState;
            currentState = Mouse.GetState();
            
            //checks if mouse was right clicked
            if(previousState.RightButton == ButtonState.Released && currentState.RightButton == ButtonState.Pressed)
            {
                Destination = new Vector2((((int)currentState.X)/24), (((int)currentState.Y)/24));//sets destination to mouse position
            }

            base.Update(gameTime, clientBounds);
        }
    }
}
