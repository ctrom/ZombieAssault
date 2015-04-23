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
        ZombieController zombieController;//zombie manager component, handles spawns and list of zombies

        PlayerControlledSprite jack;//player unit

        public static readonly float scaleFactor = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height/960f;//factor by which sprites will be scaled to, based on resolution
        public static readonly int tileSize = (int)(scaleFactor * 24);

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
            }

            zombieController.Update(gameTime, Game.Window.ClientBounds, jack.Position);//updates zombie spawner

            cursorPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            
            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(cursorTexture,cursorPosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);//draws cursor
            spriteBatch.Draw(mapTexture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, scaleFactor, SpriteEffects.None, 0);//draws map
            spriteBatch.Draw(highlightTexture, new Vector2(((int)cursorPosition.X / tileSize) * tileSize, ((int)cursorPosition.Y / tileSize) * tileSize), null, Color.White, 0, Vector2.Zero, scaleFactor, SpriteEffects.None, .2f);//draws tile highlight
            foreach (Sprite s in zombieController.ZombieList)//draws zombies in the spawner's list
                s.Draw(gameTime, spriteBatch);
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
