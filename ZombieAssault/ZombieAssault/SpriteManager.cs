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
    class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        List<Sprite> spriteList = new List<Sprite>();

        PlayerControlledSprite jack;
        Zombie zombie;

        int timeSinceLastSpawn = 0;
        int millisecondsTilSpawn = 10000;

        public SpriteManager(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            jack = new PlayerControlledSprite(Game.Content.Load<Texture2D>(@"Images/Jack_SpriteSheet"), new Vector2(720, 720), 1f, .375f, (float)Math.PI / 2);

            spriteList.Add(new Zombie(Game.Content.Load<Texture2D>(@"Images/Zombie_SpriteSheet"), new Vector2(240, 240), .5f, .375f, 0));
            spriteList.Add(new Zombie(Game.Content.Load<Texture2D>(@"Images/Zombie_SpriteSheet"), new Vector2(0, 0), .5f, .375f, (float)Math.PI));
            spriteList.Add(new Zombie(Game.Content.Load<Texture2D>(@"Images/Zombie_SpriteSheet"), new Vector2(480, 480), .5f, .375f, (float)Math.PI / 2));
            spriteList.Add(jack);
            spriteList.Add(new StaticSprite(Game.Content.Load<Texture2D>(@"Images/House_Layout(40x40 tiles, 960x960 resolution)"), 
                Vector2.Zero, 1f, 0));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach(Sprite s in spriteList)
            {
                s.Update(gameTime, Game.Window.ClientBounds);
                if(s.GetType() == typeof(Zombie))
                {
                    Zombie temp = (Zombie)s;
                    temp.Destination = new Vector2(jack.Position.X / 24, jack.Position.Y / 24);
                }
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            
            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
