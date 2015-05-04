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
    /*
     * Class that manages player controlled units.
     */
    class PlayerManager
    {
        private List<PlayerControlledSprite> unitList;
        private PlayerControlledSprite selectedUnit;

        private PlayerControlledSprite jack;
        private PlayerControlledSprite eric;
        private PlayerControlledSprite sarah;
        private PlayerControlledSprite megan;

        Map map = new Map();
        private Pathfinder pathfinder;

        //variables for tracking mouse inputs
        private MouseState previousState;
        private MouseState currentState;

        public List<PlayerControlledSprite> UnitList
        {
            get
            {
                return unitList;
            }
        }

        public PlayerControlledSprite Jack
        {
            get { return jack; }
        }
        public PlayerControlledSprite Eric
        {
            get { return eric; }
        }

        public PlayerControlledSprite Sarah
        {
            get { return sarah; }
        }

        public PlayerControlledSprite Megan
        {
            get { return megan; }
        }
        
        public PlayerManager(Texture2D jackTexture, Texture2D ericTexture, Texture2D sarahTexture, Texture2D meganTexture, SoundEffect humanDeath, SoundEffect hitZombie)
        {
            unitList = new List<PlayerControlledSprite>();

            List<int> passableTiles = new List<int>();
            passableTiles.Add(2);
            passableTiles.Add(3);
            pathfinder = new Pathfinder(map, passableTiles);

            jack = new PlayerControlledSprite(jackTexture, new Vector2(19, 18), 1f * SpriteManager.scaleFactor, .375f * SpriteManager.scaleFactor, 0, 1, humanDeath, hitZombie);
            eric = new PlayerControlledSprite(ericTexture, new Vector2(20, 18), 1f * SpriteManager.scaleFactor, .375f * SpriteManager.scaleFactor, 0, 2, humanDeath, hitZombie);
            sarah = new PlayerControlledSprite(sarahTexture, new Vector2(19, 19), 1f * SpriteManager.scaleFactor, .375f * SpriteManager.scaleFactor, 0, 3, humanDeath, hitZombie);
            megan = new PlayerControlledSprite(meganTexture, new Vector2(20, 19), 1f * SpriteManager.scaleFactor, .375f * SpriteManager.scaleFactor, 0, 4, humanDeath, hitZombie);

            unitList.Add(jack);
            unitList.Add(eric);
            unitList.Add(sarah);
            unitList.Add(megan);
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            List<PlayerControlledSprite> newList = new List<PlayerControlledSprite>();
            foreach (PlayerControlledSprite s in unitList)
            {
                if (s.health > 0)
                {
                    newList.Add(s);
                }
            }
            unitList = newList;
            KeyboardState keyboard = Keyboard.GetState();

            if(keyboard.IsKeyDown(Keys.D1))
            {
                foreach(PlayerControlledSprite s in unitList)
                {
                    if (s.UnitNumber == 1)
                        selectedUnit = s;
                }
            }
            else if (keyboard.IsKeyDown(Keys.D2))
            {
                foreach (PlayerControlledSprite s in unitList)
                {
                    if (s.UnitNumber == 2)
                        selectedUnit = s;
                }
            }
            else if (keyboard.IsKeyDown(Keys.D3))
            {
                foreach (PlayerControlledSprite s in unitList)
                {
                    if (s.UnitNumber == 3)
                        selectedUnit = s;
                }
            }
            else if (keyboard.IsKeyDown(Keys.D4))
            {
                foreach (PlayerControlledSprite s in unitList)
                {
                    if (s.UnitNumber == 4)
                        selectedUnit = s;
                }
            }

            if (selectedUnit != null)
            {
                //polls mouse state
                previousState = currentState;
                currentState = Mouse.GetState();

                //checks if mouse was right clicked
                if (previousState.RightButton == ButtonState.Released && currentState.RightButton == ButtonState.Pressed)
                {
                    Point startPoint = new Point(((int)(((selectedUnit.Position.X - 4) - Game1.resOffset) / SpriteManager.tileSize) + 2), ((int)((selectedUnit.Position.Y) / SpriteManager.tileSize) + 2));
                    Point dest = new Point((((int)(((currentState.X - 4) - Game1.resOffset) / SpriteManager.tileSize) + 2)), ((int)((currentState.Y) / SpriteManager.tileSize) + 2));//sets destination to mouse position
                    MapNode destNode = Map.getNode(new Vector2((int)((((currentState.X - 4) - Game1.resOffset) / SpriteManager.tileSize) + 2), ((int)((currentState.Y) / SpriteManager.tileSize) + 2)));
                    //Console.WriteLine(destNode + ":" + Map.Matrix[destNode.X, destNode.Y]);
                    if(destNode.Type == 1)
                    {
                        foreach (BreakableSprite b in BreakableObjectManager.BreakableList)
                        {
                            if (Math.Abs(destNode.Position.X - b.Position.X) < 3 && Math.Abs(destNode.Position.Y - b.Position.Y) < 3)
                            {
                                dest = new Point((int)b.RepairSide.X, (int)b.RepairSide.Y);
                                selectedUnit.CurrTarget = b;
                                break;
                            }
                        }
                    }
                    List<Vector2> path = pathfinder.FindPath(startPoint, dest);//new Point((int)selectedUnit.Destination.Index.X, (int)selectedUnit.Destination.Index.Y));
                    selectedUnit.Path = path;
                }
            }

            foreach (PlayerControlledSprite s in unitList)
                s.Update(gameTime, clientBounds);
        }

        //attempt at line of sight
        
        public bool isInLineOfSight(Vector2 targetPosition)
        {
            Vector3 startingPosition = new Vector3(selectedUnit.Position, 0);
            Vector3 direction = new Vector3(selectedUnit.Position - targetPosition, 0);
            float distance = Math.Abs(targetPosition.X - selectedUnit.Position.X) + Math.Abs(targetPosition.Y - selectedUnit.Position.Y);

            direction.Normalize();

            Ray lineOfSight = new Ray(startingPosition, direction);

            for(int i = 0; i < 44; i++)
            {
                for(int j = 0; j < 44; j++)
                {
                    MapNode temp = Map.getNode(new Vector2(i,j));
                    float tempDist = Math.Abs(temp.Position.X - selectedUnit.Position.X) + Math.Abs(temp.Position.Y - selectedUnit.Position.Y);
                    if (Map.Matrix[i, j] != 2 && lineOfSight.Intersects(new BoundingBox(new Vector3(temp.Position.X, temp.Position.Y, 0), new Vector3(temp.Position.X + SpriteManager.tileSize, temp.Position.Y + SpriteManager.tileSize, 0))) < distance)
                        return false;
                }
            }
            return true;
        }

        public void healUnits()
        {
            foreach(PlayerControlledSprite s in unitList)
            {
                s.health = 100;
            }
        }
    }
}
