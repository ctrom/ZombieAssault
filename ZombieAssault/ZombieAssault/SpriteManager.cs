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
    //class that controls all sprites in game
    class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        List<Sprite> spriteList = new List<Sprite>();
        ZombieController zombieController;

        PlayerControlledSprite jack;

        private Texture2D mapTexture;
        private Texture2D cursorTexture;
        private Texture2D highlightTexture;
        private Vector2 cursorPosition;

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

            jack = new PlayerControlledSprite(Game.Content.Load<Texture2D>(@"Images/Jack_SpriteSheet"), new Vector2(720, 720), 1f, .375f, (float)Math.PI / 2);//creates player unit
            zombieController = new ZombieController(Game.Content.Load<Texture2D>(@"Images/Zombie_SpriteSheet"));

            ////adds zombies to sprite list
            //spriteList.Add(new Zombie(Game.Content.Load<Texture2D>(@"Images/Zombie_SpriteSheet"), new Vector2(240, 240), .5f, .375f, 0));
            //spriteList.Add(new Zombie(Game.Content.Load<Texture2D>(@"Images/Zombie_SpriteSheet"), new Vector2(0, 0), .5f, .375f, (float)Math.PI));
            //spriteList.Add(new Zombie(Game.Content.Load<Texture2D>(@"Images/Zombie_SpriteSheet"), new Vector2(480, 480), .5f, .375f, (float)Math.PI / 2));

            cursorTexture = Game.Content.Load<Texture2D>(@"Images/Cursor_Sprite");
            mapTexture = Game.Content.Load<Texture2D>(@"Images/House_Layout(40x40 tiles, 960x960 resolution)");
            highlightTexture = Game.Content.Load<Texture2D>(@"Images/Highlight_Sprite");

            spriteList.Add(jack);//adds player unit to sprite list

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach(Sprite s in spriteList)//runs update method of each sprite in the list
            {
                s.Update(gameTime, Game.Window.ClientBounds);
                //if(s.GetType() == typeof(Zombie))//if sprite is a zombie, updates destination to player unit position
                //{
                //    Zombie temp = (Zombie)s;
                    
                //}
            }

            zombieController.Update(gameTime, Game.Window.ClientBounds);

            cursorPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            
            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(cursorTexture,cursorPosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            spriteBatch.Draw(mapTexture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(highlightTexture, new Vector2(((int)cursorPosition.X / 24) * 24, ((int)cursorPosition.Y / 24) * 24), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, .2f);

            foreach (Sprite s in zombieController.ZombieList)
                s.Draw(gameTime, spriteBatch);
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
