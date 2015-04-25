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

        public void Update(GameTime gameTime, Rectangle clientBounds, Vector2 target)
        {
            timeSinceLastSpawn += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastSpawn > millisecondsTilSpawn)//checks if required time between spawns has passed
            {
                timeSinceLastSpawn = 0;//resets spawn timer
                Random rand = new Random();
                for(int i = 0; i < 10; i++)//adds 10 zombies to the list at position (0,0)
                {
                    int x = rand.Next(0, 4);
                    Vector2 position = Vector2.Zero;
                    switch(x)
                    {
                        case 0:
                            position = new Vector2(rand.Next(0, 42)*SpriteManager.tileSize, -2*SpriteManager.tileSize);
                            break;
                        case 1:
                            position = new Vector2(42*SpriteManager.tileSize, rand.Next(0, 42)*SpriteManager.tileSize);
                            break;
                        case 2:
                            position = new Vector2(rand.Next(0, 42)*SpriteManager.tileSize, 42*SpriteManager.tileSize);
                            break;
                        case 3:
                            position = new Vector2(-2 * SpriteManager.tileSize, rand.Next(0, 42) * SpriteManager.tileSize);
                            break;
                    }
                    Console.Write(position+"\n");
                    zombieList.Add(new Zombie(zombieTexture, position, .5f, .375f, 0));
                }
            }

            foreach (Zombie z in zombieList)
                z.Update(gameTime, clientBounds, target);
        }
    }
}
