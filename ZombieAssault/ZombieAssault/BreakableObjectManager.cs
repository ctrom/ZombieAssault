using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieAssault
{
    class BreakableObjectManager
    {
        private static List<BreakableSprite> breakableList;

        public static List<BreakableSprite> BreakableList
        {
            get { return breakableList; }
        }

        public BreakableObjectManager(Texture2D windowTexture)
        {
            breakableList = new List<BreakableSprite>();
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(11, 20), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), 0));
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(11, 21), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), 0));
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(11, 24), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), 0));
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(11, 27), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), 0));
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(13, 29), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), (float)(3 * Math.PI / 2)));
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(17, 29), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), (float)(3 * Math.PI / 2)));
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(24, 31), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), (float)(3 * Math.PI / 2)));
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(27, 31), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), (float)(3 * Math.PI / 2)));
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(29, 29), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), (float)Math.PI));
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(29, 28), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), (float)Math.PI));
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(29, 24), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), (float)Math.PI));
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(29, 23), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), (float)Math.PI));
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(32, 19), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), (float)Math.PI));
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(32, 17), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), (float)Math.PI));
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(30, 15), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), (float)Math.PI / 2));
            breakableList.Add(new BreakableSprite(windowTexture, new Vector2(26, 15), 1f * SpriteManager.scaleFactor, 0, new Point(24, 24), new Point(0, 0), new Point(1, 3), (float)Math.PI / 2));
        }


        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            foreach (BreakableSprite s in breakableList)
                s.Update(gameTime, clientBounds);
        }
    }
}
