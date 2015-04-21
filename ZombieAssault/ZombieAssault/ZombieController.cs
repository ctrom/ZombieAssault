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

        public ZombieController(int millisecondsTilSpawn = 10000)
        {
            zombieList = new List<Zombie>();
            this.millisecondsTilSpawn = millisecondsTilSpawn;
        }

        public void Update(GameTime gameTime)
        {
            timeSinceLastSpawn += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastSpawn > millisecondsTilSpawn)//checks if required time between spawns has passed
            {
                timeSinceLastSpawn = 0;//resets spawn timer
                
                //spawn code here
            }
        }
    }
}
