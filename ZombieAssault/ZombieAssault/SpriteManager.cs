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

        public static List<Sprite> spriteList = new List<Sprite>();
        ZombieController zombieController;//zombie manager component, handles spawns and list of zombies
        PlayerManager playerManager;
        BreakableObjectManager breakableObjectManager;

        public static readonly float scaleFactor = Game1.resHeight / 960f;//factor by which sprites will be scaled to, based on resolution
        public static readonly float tileSize = scaleFactor * 24;
        public static readonly float gridOffset = ((float)Game1.resHeight / Game1.resWidth) * tileSize + 1;

        private Texture2D mapTexture;
        private Texture2D titleTexture;
        private Texture2D gameOverTexture;
        private Texture2D cursorTexture;
        private Texture2D highlightTexture;
        private Texture2D unitHudTexture;
        private Texture2D gameInfoHudTexture;
        private Texture2D healthbarGreenTexture;
        private Texture2D healthbarRedTexture;
        private Texture2D startButtonTexture;
        private Texture2D exitButtonTexture;
        private Texture2D controlsButtonTexture;

        private SoundEffect hitZombie;
        private SoundEffect zombieGroan;
        private SoundEffect humanDeath;
        private SoundEffect zombieDeath;

        private SpriteFont font;

        private Vector2 cursorPosition;
        private Vector2 screenCenter;

        private int gameState;

        private Rectangle startButtonRectangle;
        private Rectangle exitButtonRectangle;
        private Rectangle controlsButtonRectangle;
        private Rectangle jackHealthBar;
        private Rectangle ericHealthBar;
        private Rectangle sarahHealthBar;
        private Rectangle meganHealthBar;

        private MouseState mouseState;
        private MouseState previousMouseState;

        public int GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }

        public SpriteManager(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            jackHealthBar = new Rectangle((int)(Game1.resOffset - 175 * scaleFactor), (int)(210 * scaleFactor), (int)(scaleFactor * 152), (int)scaleFactor * 40);
            ericHealthBar = new Rectangle((int)(Game1.resOffset - 175 * scaleFactor), (int)(403 * scaleFactor), (int)(scaleFactor * 152), (int)scaleFactor * 40);
            sarahHealthBar = new Rectangle((int)(Game1.resOffset - 175 * scaleFactor), (int)(595 * scaleFactor), (int)(scaleFactor * 152), (int)scaleFactor * 40);
            meganHealthBar = new Rectangle((int)(Game1.resOffset - 175 * scaleFactor), (int)(787 * scaleFactor), (int)(scaleFactor * 152), (int)scaleFactor * 40);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            zombieGroan = Game.Content.Load<SoundEffect>(@"Audio/ZombieGroans");
            hitZombie = Game.Content.Load<SoundEffect>(@"Audio/HittingZombie");
            humanDeath = Game.Content.Load<SoundEffect>(@"Audio/HumanDeath");
            zombieDeath = Game.Content.Load<SoundEffect>(@"AUdio/ZombieDeath");

            playerManager = new PlayerManager(Game.Content.Load<Texture2D>(@"Images/Jack_SpriteSheet"), Game.Content.Load<Texture2D>(@"Images/Eric_SpriteSheet"), Game.Content.Load<Texture2D>(@"Images/Sarah_SpriteSheet"), Game.Content.Load<Texture2D>(@"Images/Megan_SpriteSheet"), humanDeath, hitZombie);
            zombieController = new ZombieController(Game.Content.Load<Texture2D>(@"Images/Zombie_SpriteSheet"), zombieDeath, zombieGroan);
            breakableObjectManager = new BreakableObjectManager(Game.Content.Load<Texture2D>(@"Images/Window_Spritesheet"));
            cursorTexture = Game.Content.Load<Texture2D>(@"Images/Cursor_Sprite");
            mapTexture = Game.Content.Load<Texture2D>(@"Images/House_Layout(40x40 tiles, 960x960 resolution)");
            titleTexture = Game.Content.Load<Texture2D>(@"Images/Title_Screen");
            gameOverTexture = Game.Content.Load<Texture2D>(@"Images/GameOverScreen");
            highlightTexture = Game.Content.Load<Texture2D>(@"Images/Highlight_Sprite");
            unitHudTexture = Game.Content.Load<Texture2D>(@"Images/HUD/Hud_UnitInfo");
            gameInfoHudTexture = Game.Content.Load<Texture2D>(@"Images/HUD/Hud_GameInfo");
            healthbarGreenTexture = Game.Content.Load<Texture2D>(@"Images/HUD/HealthBar/HealthBar");
            healthbarRedTexture = Game.Content.Load<Texture2D>(@"Images/HUD/HealthBar/HealthBarUnder");
            startButtonTexture = Game.Content.Load<Texture2D>(@"Images/Buttons/StartButton");
            exitButtonTexture = Game.Content.Load<Texture2D>(@"Images/Buttons/ExitButton");
            controlsButtonTexture = Game.Content.Load<Texture2D>(@"Images/Buttons/ControlsButton");

            font = Game.Content.Load<SpriteFont>(@"Font/courier");

            screenCenter = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            startButtonRectangle = new Rectangle((int)(screenCenter.X - 100 * scaleFactor), (int)(screenCenter.Y - 50 * scaleFactor), 200 * (int)scaleFactor, 60 * (int)scaleFactor);
            controlsButtonRectangle = new Rectangle((int)(screenCenter.X - 100 * scaleFactor), (int)(screenCenter.Y + 50 * scaleFactor), 200 * (int)scaleFactor, 60 * (int)scaleFactor);
            exitButtonRectangle = new Rectangle((int)(screenCenter.X - 100 * scaleFactor), (int)(screenCenter.Y + 150 * scaleFactor), 200 * (int)scaleFactor, 60 * (int)scaleFactor);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //wait for mouseclick
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                 MouseClicked(mouseState.X, mouseState.Y);
            }

            foreach (Sprite s in spriteList)//runs update method of each sprite in the list
            {
                s.Update(gameTime, Game.Window.ClientBounds);
            }
            if (GameState == 1)
            {
                if (playerManager.UnitList.Count == 0)
                    gameState = 3;
                playerManager.Update(gameTime, Game.Window.ClientBounds);//updates player units
                zombieController.Update(gameTime, Game.Window.ClientBounds, playerManager.UnitList);//updates zombie spawner
                breakableObjectManager.Update(gameTime, Game.Window.ClientBounds);//updates breakable objects
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    GameState = 2;
            }
            if (gameState == 2 && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                GameState = 1;
            }

            cursorPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            spriteBatch.Draw(cursorTexture, cursorPosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);//draws cursor

            if (GameState == 0)
            {
                spriteBatch.Draw(titleTexture, new Vector2(Game1.resOffset, 0), null, Color.White, 0, Vector2.Zero, scaleFactor, SpriteEffects.None, 0);
                spriteBatch.Draw(startButtonTexture, startButtonRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .5f);
                spriteBatch.Draw(controlsButtonTexture, controlsButtonRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .5f);
                spriteBatch.Draw(exitButtonTexture, exitButtonRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .5f);
            }
            if (GameState == 1 || GameState == 2 || gameState == 3)
            {
                jackHealthBar.Width = (int)((playerManager.Jack.health / 100) * 152 * scaleFactor);
                ericHealthBar.Width = (int)((playerManager.Eric.health / 100) * 152 * scaleFactor);
                sarahHealthBar.Width = (int)((playerManager.Sarah.health / 100) * 152 * scaleFactor);
                meganHealthBar.Width = (int)((playerManager.Megan.health / 100) * 152 * scaleFactor);

                foreach (Sprite s in spriteList)
                    s.Draw(gameTime, spriteBatch);
                spriteBatch.Draw(mapTexture, new Vector2(Game1.resOffset, 0), null, Color.White, 0, Vector2.Zero, scaleFactor, SpriteEffects.None, 0);//draws map
                spriteBatch.Draw(highlightTexture, new Vector2((int)((cursorPosition.X - gridOffset) / tileSize) * tileSize + gridOffset, (int)(cursorPosition.Y / tileSize) * tileSize), null, Color.White, 0, Vector2.Zero, scaleFactor, SpriteEffects.None, .2f);//draws tile highlight
                spriteBatch.Draw(unitHudTexture, new Vector2((int)(Game1.resOffset - 315 * scaleFactor), 0), null, Color.White, 0, Vector2.Zero, scaleFactor, SpriteEffects.None, .8f);
                spriteBatch.Draw(gameInfoHudTexture, new Vector2((int)(Game1.resWidth - Game1.resOffset), 0), null, Color.White, 0, Vector2.Zero, scaleFactor, SpriteEffects.None, .8f);

                spriteBatch.Draw(healthbarGreenTexture, jackHealthBar, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                spriteBatch.Draw(healthbarRedTexture, new Rectangle(jackHealthBar.X, jackHealthBar.Y, (int)(152 * scaleFactor), 40 * (int)scaleFactor), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .9f);
                spriteBatch.Draw(healthbarGreenTexture, ericHealthBar, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                spriteBatch.Draw(healthbarRedTexture, new Rectangle(ericHealthBar.X, ericHealthBar.Y, (int)(152 * scaleFactor), 40 * (int)scaleFactor), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .9f);
                spriteBatch.Draw(healthbarGreenTexture, sarahHealthBar, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                spriteBatch.Draw(healthbarRedTexture, new Rectangle(sarahHealthBar.X, sarahHealthBar.Y, (int)(152 * scaleFactor), 40 * (int)scaleFactor), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .9f);
                spriteBatch.Draw(healthbarGreenTexture, meganHealthBar, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                spriteBatch.Draw(healthbarRedTexture, new Rectangle(meganHealthBar.X, meganHealthBar.Y, (int)(152 * scaleFactor), 40 * (int)scaleFactor), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .9f);

                spriteBatch.DrawString(font, ((zombieController.MillisecondsTilSpawn - zombieController.TimeSinceLastSpawn)/1000).ToString(), new Vector2((int)(Game1.resWidth - Game1.resOffset + (140 * scaleFactor)), (int)(215 * scaleFactor)), Color.Black, 0, Vector2.Zero, scaleFactor * 2, SpriteEffects.None, 1);
                spriteBatch.DrawString(font, zombieController.Wave.ToString(), new Vector2((int)(Game1.resWidth - Game1.resOffset + (140 * scaleFactor)), (int)(415 * scaleFactor)), Color.Black, 0, Vector2.Zero, scaleFactor * 2, SpriteEffects.None, 1);

                foreach (Sprite s in ZombieController.ZombieList)//draws zombies in the spawner's list
                    s.Draw(gameTime, spriteBatch);
                foreach (Sprite s in playerManager.UnitList)
                    s.Draw(gameTime, spriteBatch);
                foreach (Sprite s in BreakableObjectManager.BreakableList)
                    s.Draw(gameTime, spriteBatch);

                if(zombieController.Wave % 5 == 0)
                {
                    playerManager.healUnits();
                }
            }
            if(gameState == 2 || gameState == 3)
            {
                spriteBatch.Draw(gameOverTexture, new Vector2(Game1.resOffset, 0), null, Color.White, 0, Vector2.Zero, scaleFactor, SpriteEffects.None, 1);
                spriteBatch.Draw(startButtonTexture, startButtonRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .9f);
                spriteBatch.Draw(controlsButtonTexture, controlsButtonRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .9f);
                spriteBatch.Draw(exitButtonTexture, exitButtonRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .9f);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void MouseClicked(int x, int y)
        {
            //creates a rectangle of 1x1 around the place where the mouse was clicked
            Rectangle mouseClickRect = new Rectangle(x, y, 1, 1);

            if (gameState == 0)
            {
                if (mouseClickRect.Intersects(startButtonRectangle)) //player clicked start button
                {
                    gameState = 1;
                }
                else if (mouseClickRect.Intersects(exitButtonRectangle)) //player clicked exit button
                {
                    Game.Exit();
                }
            }
            if(gameState == 2)
            {
                if (mouseClickRect.Intersects(startButtonRectangle)) //player clicked start button
                {
                    gameState = 1;
                }
                else if (mouseClickRect.Intersects(exitButtonRectangle)) //player clicked exit button
                {
                    Game.Exit();
                }
            }
            if(gameState == 3)
            {
                if (mouseClickRect.Intersects(startButtonRectangle)) //player clicked start button
                {
                    playerManager = new PlayerManager(Game.Content.Load<Texture2D>(@"Images/Jack_SpriteSheet"), Game.Content.Load<Texture2D>(@"Images/Eric_SpriteSheet"), Game.Content.Load<Texture2D>(@"Images/Sarah_SpriteSheet"), Game.Content.Load<Texture2D>(@"Images/Megan_SpriteSheet"), humanDeath, hitZombie); 
                    zombieController = new ZombieController(Game.Content.Load<Texture2D>(@"Images/Zombie_SpriteSheet"), zombieDeath, zombieGroan);
                    breakableObjectManager = new BreakableObjectManager(Game.Content.Load<Texture2D>(@"Images/Window_Spritesheet"));
                    gameState = 1;
                }
                else if (mouseClickRect.Intersects(exitButtonRectangle)) //player clicked exit button
                {
                    Game.Exit();
                }
            }
        }
    }
}
