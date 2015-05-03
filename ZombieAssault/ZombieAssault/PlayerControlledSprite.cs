using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace ZombieAssault
{
    //Instantiable class for player units
    class PlayerControlledSprite : AnimatedSprite
    {
        
        private int unitNumber;

        private Sprite prevTarget;
        private Sprite currTarget;

        private int timeSinceAction;

        public int UnitNumber
        {
            get
            {
                return unitNumber;
            }
        }

        public Sprite CurrTarget
        {
            get { return currTarget; }
            set { currTarget = value; }
        }

        public PlayerControlledSprite(Texture2D textureImage, Vector2 position, float speed, float scale, float rotation, int unitNumber)
            : base (textureImage, position, new Point(64, 64), new Point(0,0), new Point(3,3), rotation, speed, scale, 0, new Vector2(0,0), 200)
        {
            destination = new Vector2(position.X * SpriteManager.tileSize + Game1.resOffset - SpriteManager.gridOffset, position.Y * SpriteManager.tileSize);//initializes destination as starting position
            this.unitNumber = unitNumber;
        }

        public override Vector2 Direction
        {
            get { return direction; }
        }

        

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //algorithm for traversing sprite sheet
            if (currTarget != null && currTarget is Zombie)
                currentFrame.Y = 2;
            else
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

            //if (currTarget == null && ZombieController.ZombieList.Count != 0)
            //    currTarget = ZombieController.ZombieList.ElementAt(0);
            
            //Algorithm for finding closest zombie to unit
            prevTarget = currTarget;
            
            foreach (Zombie s in ZombieController.ZombieList)
            {
                if (prevTarget == null)
                {
                    prevTarget = s;
                }

                if (currTarget == null)
                    currTarget = s;
                
                if (Math.Sqrt(Math.Pow(position.X - s.Position.X, 2) + Math.Pow(position.Y - s.Position.Y, 2)) <
                    Math.Sqrt(Math.Pow(position.X - prevTarget.Position.X, 2) + Math.Pow(position.Y - prevTarget.Position.Y, 2)) || prevTarget.health < 1)
                {
                    currTarget = s;
                }
            }
            if (path.Count == 0 && ZombieController.ZombieList.Count != 0)
                rotation = (float)(Math.Atan2(currTarget.Position.Y - position.Y, currTarget.Position.X - position.X)) + (float)Math.PI / 2;

            timeSinceAction += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceAction > 1000 && currTarget is Zombie)
                playerAttack();
            else if(timeSinceAction > 1000 && currTarget is BreakableSprite)
                playerRepair();

            base.Update(gameTime, clientBounds);
        }

        private void playerAttack()
        {
            if(currTarget != null && (Math.Abs(currTarget.Position.X - this.Position.X) + Math.Abs(currTarget.Position.Y - this.Position.Y) < SpriteManager.tileSize + 4))
            {
                timeSinceAction = 0;
                currTarget.health = currTarget.health - 100;
            }
        }

        private void playerRepair()
        {
            if (currTarget != null && (Math.Abs(currTarget.Position.X - this.Position.X) + Math.Abs(currTarget.Position.Y - this.Position.Y) < SpriteManager.tileSize + 4))
            {
                timeSinceAction = 0;
                currTarget.health = currTarget.health + 10;
            }
        }
    }
}
