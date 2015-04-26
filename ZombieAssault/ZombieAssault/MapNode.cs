using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieAssault
{
    public class MapNode
    {
        private Vector2 position;
        private Vector2 index;
        private int type;

        public Vector2 Index
        {
            get
            {
                return index;
            }
        }

        public int Type
        {
            get { return type; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public MapNode(Vector2 index, int type)
        {
            this.index = index;
            position = new Vector2(index.X * SpriteManager.tileSize + Game1.resOffset + SpriteManager.gridOffset - 3, index.Y * SpriteManager.tileSize);
            this.type = type;
        }
    }
}
