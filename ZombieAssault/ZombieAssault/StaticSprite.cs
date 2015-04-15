using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieAssault
{
    //Instantiable class for generic static sprites
    class StaticSprite : Sprite
    {
        public StaticSprite(Texture2D textureImage, Vector2 position, float scale, int collisionOffset)
            : base(textureImage, position, scale, collisionOffset)
        {

        }
    }
}
