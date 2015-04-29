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

        public PlayerManager(Texture2D jackTexture)
        {
            unitList = new List<PlayerControlledSprite>();

            pathfinder = new Pathfinder(map);

            unitList.Add(new PlayerControlledSprite(jackTexture, new Vector2(20, 20), 1f, .375f, 0, 1));
            unitList.Add(new PlayerControlledSprite(jackTexture, new Vector2(10, 10), 1f, .375f, 0, 2));
            unitList.Add(new PlayerControlledSprite(jackTexture, new Vector2(30, 30), 1f, .375f, 0, 3));
            unitList.Add(new PlayerControlledSprite(jackTexture, new Vector2(15, 15), 1f, .375f, 0, 4));
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
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
                    MapNode startPoint = Map.getNode(new Vector2(((int)(((selectedUnit.Position.X - 4) - Game1.resOffset) / SpriteManager.tileSize) + 2), ((int)((selectedUnit.Position.Y) / SpriteManager.tileSize) + 2)));
                    //selectedUnit.Destination = Map.getNode(new Vector2(((int)(((currentState.X - 4) - Game1.resOffset) / SpriteManager.tileSize) + 2), ((int)((currentState.Y) / SpriteManager.tileSize) + 2)));//sets destination to mouse position
                    Point dest = new Point((((int)(((currentState.X - 4) - Game1.resOffset) / SpriteManager.tileSize) + 2)), ((int)((currentState.Y) / SpriteManager.tileSize) + 2));//sets destination to mouse position
                    List<Vector2> path = pathfinder.FindPath(new Point((int)startPoint.Index.X, (int)startPoint.Index.Y), dest);//new Point((int)selectedUnit.Destination.Index.X, (int)selectedUnit.Destination.Index.Y));
                    selectedUnit.Path = path;
                    foreach (Vector2 v in path)
                        Console.WriteLine(v);
                    Console.WriteLine(Game1.resOffset + ":" + SpriteManager.tileSize + ":" + new Vector2((((int)(((currentState.X - 4) - Game1.resOffset) / SpriteManager.tileSize) + 2)), ((int)((currentState.Y) / SpriteManager.tileSize) + 2)));
                    Console.WriteLine(dest);
                }
            }

            foreach (Sprite s in unitList)
                s.Update(gameTime, clientBounds);
        }

        //attempt at line of sight
        /*
        public bool isInLineOfSight(Vector2 targetPosition)
        {
            Vector3 startingPosition = new Vector3(selectedUnit.Position, 0);
            Vector3 direction = new Vector3(selectedUnit.Position - targetPosition, 0);
            double distance = Math.Sqrt(Math.Abs(targetPosition.X - selectedUnit.Position.X) + Math.Abs(targetPosition.Y - selectedUnit.Position.Y));

            direction.Normalize();

            Ray lineOfSight = new Ray(startingPosition, direction);

            foreach node in the map
            {
                if node is impassable and lineOfSight.Intersects(node)
                    return false
            }
            return true;
        }
        */
    }
}
