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
            : base (textureImage, position, new Point(64,64), new Point(0,0), new Point(3,2), scale, rotation, speed, 0, new Vector2(0,0), 250)
        {
            destination = position;
        }

        public override Vector2 Direction
        {
            get { return direction; }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            currentFrame.Y = 0;
            if (position != destination)
            {
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > millisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = 1;
                    }
                }
            }
            else
                currentFrame.X = 0;

            previousState = currentState;
            currentState = Mouse.GetState();
            
            if(previousState.RightButton == ButtonState.Released && currentState.RightButton == ButtonState.Pressed)
            {
                destination = new Vector2(currentState.X, currentState.Y);
            }

            base.Update(gameTime, clientBounds);
        }
    }
}
