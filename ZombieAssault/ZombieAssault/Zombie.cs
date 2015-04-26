﻿using System;
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
    //Instantiable object for basic zombies
    class Zombie : AnimatedSprite
    {
        private PlayerControlledSprite prevTarget;
        private PlayerControlledSprite currTarget;

        public Zombie(Texture2D textureImage, MapNode position, float speed, float scale, float rotation)
            : base(textureImage, position, new Point(64,64), new Point(0,0), new Point(3,1), rotation, speed, scale, 0, new Vector2(0,0), 500)
        {
            destination = position;
        }

        public override Vector2 Direction
        {
            get { return direction; }
        }

        public void Update(GameTime gameTime, Rectangle clientBounds, List<PlayerControlledSprite> targets)
        {
            //algorithm for traversing spritesheet
            currentFrame.Y = 0;
            if (position.Position != destination.Position)
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

            //algorithm for chasing closest target
            prevTarget = currTarget;
            foreach(PlayerControlledSprite s in targets)
            {
                if (prevTarget == null)
                    prevTarget = s;
                if (Math.Sqrt(Math.Pow(position.Position.X - s.Position.Position.X, 2) + Math.Pow(position.Position.Y - s.Position.Position.Y, 2)) < 
                    Math.Sqrt(Math.Pow(position.Position.X - prevTarget.Position.Position.X, 2) + Math.Pow(position.Position.Y - prevTarget.Position.Position.Y, 2)))
                    currTarget = s;
                if (currTarget == null)
                    currTarget = s;
            }
            destination = currTarget.Position;

            base.Update(gameTime, clientBounds);
        }
    }
}
