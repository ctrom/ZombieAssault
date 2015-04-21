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
    /**
     * Controls zombie spawning and manages list of zombies on the map
     */
    class ZombieController
    {
        private List<Zombie> zombieList;//List of zombies
        private Texture2D zombieTexture;

        //experimental zombie spawn counter
        private int timeSinceLastSpawn;
        private int millisecondsTilSpawn;

        public List<Zombie> ZombieList
        {
            get { return zombieList; }
        }

        public int MillisecondsTilSpawn
        {
            get { return millisecondsTilSpawn; }
            set { millisecondsTilSpawn = value; }
        }

        public ZombieController(Texture2D zombieTexture, int millisecondsTilSpawn = 10000)
        {
            zombieList = new List<Zombie>();
            this.zombieTexture = zombieTexture;
            this.millisecondsTilSpawn = millisecondsTilSpawn;
        }

        public void Update(GameTime gameTime)
        {
            timeSinceLastSpawn += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastSpawn > millisecondsTilSpawn)//checks if required time between spawns has passed
            {
                timeSinceLastSpawn = 0;//resets spawn timer

                for(int i = 0; i < 10; i++)
                {
                    zombieList.Add(new Zombie(zombieTexture, Vector2.Zero, .5f, .375f, 0));
                }
            }
        }
    }
}
