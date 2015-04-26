using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieAssault
{
    class MapNode
    {
        private Vector2 position;

        private List<MapNode> neighbors;

        public MapNode(Vector2 index)
        {
            position = new Vector2(index.X * SpriteManager.tileSize + Game1.resOffset, index.Y * SpriteManager.tileSize);
        }
    }
}
