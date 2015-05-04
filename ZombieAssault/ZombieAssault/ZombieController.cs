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
        private static List<Zombie> zombieList;//List of zombies
        private Texture2D zombieTexture;

        //experimental zombie spawn counter
        private int timeSinceLastSpawn;
        private int millisecondsTilSpawn;
        private int wave;

        private SoundEffect zombieDeath;
        private SoundEffect zombieGroan;

        private Map map;

        public static List<Zombie> ZombieList
        {
            get { return zombieList; }
            set { zombieList = value; }
        }

        public int TimeSinceLastSpawn
        {
            get { return timeSinceLastSpawn; }
        }

        public int MillisecondsTilSpawn
        {
            get { return millisecondsTilSpawn; }
            set { millisecondsTilSpawn = value; }
        }

        public int Wave
        {
            get { return wave; }
        }

        public ZombieController(Texture2D zombieTexture, SoundEffect zombieDeath, SoundEffect zombieGroan, int millisecondsTilSpawn = 90000)
        {
            zombieList = new List<Zombie>();
            this.zombieTexture = zombieTexture;
            this.zombieDeath = zombieDeath;
            this.zombieGroan = zombieGroan;
            this.millisecondsTilSpawn = millisecondsTilSpawn;
            map = new Map();
            wave = 0;
        }

        public void Update(GameTime gameTime, Rectangle clientBounds, List<PlayerControlledSprite> targets)
        {
            List<Zombie> newList = new List<Zombie>();
            foreach (Zombie z in zombieList)
            {
                if (z.health > 0)
                    newList.Add(z);
                z.Update(gameTime, clientBounds, targets);
            }
            ZombieList = newList;
            timeSinceLastSpawn += gameTime.ElapsedGameTime.Milliseconds;
            
            if (timeSinceLastSpawn > millisecondsTilSpawn)//checks if required time between spawns has passed
            {
                wave++;
                zombieGroan.Play();
                timeSinceLastSpawn = 0;//resets spawn timer
                Random rand = new Random();
                for(int i = 0; i < (5 * (wave + 1)); i++)//adds zombies to the list at random position off the screen
                {
                    Vector2 position = Vector2.Zero;
                    int x = rand.Next(0, 4);
                    switch(x)
                    {
                        case 0:
                            position = new Vector2(rand.Next(2, 42), 0);
                            break;
                        case 1:
                            position = new Vector2(42, rand.Next(2, 42));
                            break;
                        case 2:
                            position = new Vector2(rand.Next(2, 42), 42);
                            break;
                        case 3:
                            position = new Vector2(0, rand.Next(2, 42));
                            break;
                    }
                    zombieList.Add(new Zombie(zombieTexture, position, .2f * SpriteManager.scaleFactor, .375f * SpriteManager.scaleFactor, 0, map, zombieDeath));
                }
            }
        }
    }
}
