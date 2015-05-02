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
    //Instantiable object for basic zombies
    class Zombie : AnimatedSprite
    {
        private PlayerControlledSprite prevTarget;
        private PlayerControlledSprite currTarget;
        private Pathfinder pathfinder;
        private Vector2 targetPrevPos;
        private int timeSinceRepath;

        public Zombie(Texture2D textureImage, Vector2 position, float speed, float scale, float rotation, Map map)
            : base(textureImage, position, new Point(64,64), new Point(0,0), new Point(3,1), rotation, speed, scale, 0, new Vector2(0,0), 500)
        {
            List<int> temp = new List<int>();
            temp.Add(1);
            temp.Add(2);
            temp.Add(3);
            pathfinder = new Pathfinder(map, temp);
            timeSinceRepath = 4000;
        }

        public override Vector2 Direction
        {
            get { return direction; }
        }

        private void switchTarget()
        {
            path.Clear();
            timeSinceRepath = 0;
            MapNode startPoint = Map.getNode(new Vector2(((int)(((position.X - 4) - Game1.resOffset) / SpriteManager.tileSize) + 2), ((int)((position.Y) / SpriteManager.tileSize) + 2)));
            //selectedUnit.Destination = Map.getNode(new Vector2(((int)(((currentState.X - 4) - Game1.resOffset) / SpriteManager.tileSize) + 2), ((int)((currentState.Y) / SpriteManager.tileSize) + 2)));//sets destination to mouse position
            Point dest = new Point((((int)(((currTarget.Position.X - 4) - Game1.resOffset) / SpriteManager.tileSize) + 2)), ((int)((currTarget.Position.Y) / SpriteManager.tileSize) + 2));//sets destination to mouse position
            path = pathfinder.FindPath(new Point((int)startPoint.Index.X, (int)startPoint.Index.Y), dest);//new Point((int)selectedUnit.Destination.Index.X, (int)selectedUnit.Destination.Index.Y));
        }

        public void Update(GameTime gameTime, Rectangle clientBounds, List<PlayerControlledSprite> targets)
        {
            //algorithm for chasing closest target
            prevTarget = currTarget;
            foreach (PlayerControlledSprite s in targets)
            {
                if (prevTarget == null)
                {
                    prevTarget = s;
                    targetPrevPos = s.Position;
                }
                if (/*Math.Abs(position.X - s.Position.X) + Math.Abs(position.Y + s.Position.Y) < Math.Abs(position.X - prevTarget.Position.X) + Math.Abs(position.Y - prevTarget.Position.Y))*/Math.Sqrt(Math.Pow(position.X - s.Position.X, 2) + Math.Pow(position.Y - s.Position.Y, 2)) <
                    Math.Sqrt(Math.Pow(position.X - prevTarget.Position.X, 2) + Math.Pow(position.Y - prevTarget.Position.Y, 2)))
                {
                    currTarget = s;
                    switchTarget();
                }
                if (currTarget == null)
                {
                    currTarget = s;
                }
            }
            timeSinceRepath += gameTime.ElapsedGameTime.Milliseconds;
            if(((Math.Abs(targetPrevPos.X - currTarget.Position.X) > 1 || Math.Abs(targetPrevPos.Y - currTarget.Position.Y) > 1) || path.Count == 0) && timeSinceRepath > 500)
            {
                switchTarget();
            }

            if (path.Count > 1)
            {
                Destination = path.ElementAt(0);
                if (Math.Abs(destination.X - position.X) < 1 && Math.Abs(destination.Y - position.Y) < 1)
                {
                    path.Remove(path.ElementAt(0));
                }
            }
            else
                rotation = (float)(Math.Atan2(currTarget.Position.Y - position.Y, currTarget.Position.X - position.X)) + (float)Math.PI / 2;


            //algorithm for traversing spritesheet
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
