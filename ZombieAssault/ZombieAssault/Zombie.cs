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
    class Zombie : AnimatedSprite
    {
        
        
        private MouseState prevMouseState = Mouse.GetState();

        public Zombie(Texture2D textureImage, Vector2 position, float speed, float scale, float rotation)
            : base(textureImage, position, new Point(64,64), new Point(0,0), new Point(3,1), scale, rotation, speed, 0, new Vector2(0,0), 500)
        {

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

            base.Update(gameTime, clientBounds);
        }
    }
}
