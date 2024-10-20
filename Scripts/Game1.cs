global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Microsoft.Xna.Framework.Input;
global using Microsoft.Xna.Framework.Audio;
global using Monogame_Cross_Platform.Scripts.ContentManagers;
global using Monogame_Cross_Platform.Scripts.GameObjects.Tiles;
global using Monogame_Cross_Platform.Scripts.GameObjects;
global using Monogame_Cross_Platform.Scripts.Particles;
global using System;
global using Monogame_Cross_Platform.Scripts.GameObjects.Entities;
global using Monogame_Cross_Platform.Scripts.HUD;
global using System.Collections.Generic;
global using Monogame_Cross_Platform.Scripts.GameObjects.Entities.Player;
global using Monogame_Cross_Platform.Scripts.GameObjects.Entities.Enemy;
global using Monogame_Cross_Platform.Scripts.GameObjects.Weapons;
using Monogame_Cross_Platform.Scripts.Level;

namespace Monogame_Cross_Platform.Scripts
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;
        public static Random rand = new Random();
        
        public static ContentLoader contentLoader;
        DrawThings drawEntities;
        public static RenderTarget2D renderTarget;
        internal static ContentManagers.Camera.Camera camera = new ContentManagers.Camera.Camera();
        public static AudioPlayer audioPlayer;
        public static LevelEditor levelEditor;

        public static GameTime gameTime;

        SpriteFont font; //Temp font
        public static string debugText = "test";

        internal static Player player;
        internal static List<GameObject> currentGameObjects;
        internal static List<Menu> menus = new List<Menu>();
        internal static List<Projectile> activePlayerProjectiles = new List<Projectile>();
        internal static List<Projectile> activeEnemyProjectiles = new List<Projectile>();
        internal static List<Particle> activeParticles = new List<Particle>();
        internal static List<ParticleEmitter> activeParticleEmitters = new List<ParticleEmitter>();

        static bool toExit = false;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            currentGameObjects = new List<GameObject>(); //should be handled elsewhere maybe

            levelEditor = new LevelEditor();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            contentLoader = new ContentLoader(this);

            LevelGenerator.GenerateLevel(1, 7); //TEMP
            audioPlayer = new AudioPlayer(ContentLoader.audioLoaded);
            player = new Player(5000, 250, new Vector2(177, 177), new Hitboxes.Hitbox(0, 0, 31, 31), 0); //Put this in a better spot inside of an initialize level function within update or smth
            

            //Settings.ApplySettingsToFile(); //TEMP
            Settings.InitializeSettings();
            base.Initialize();
        }

        protected override void LoadContent() 
        {
            // TODO: use this.Content to load your game content here
            drawEntities = new DrawThings();
            contentLoader.LoadTextures("AlwaysLoaded");
            contentLoader.LoadTextures("Audio");
            contentLoader.LoadTextures("OtherTextures");

            renderTarget = new RenderTarget2D(GraphicsDevice, Settings.resolutionWidth, Settings.resolutionHeight);
            font = Content.Load<SpriteFont>("Arial"); //Temp font


        }

        protected override void Update(GameTime _gameTime)
        {
            if (toExit)
                Exit();
            gameTime = _gameTime;
            // TODO: Add your update logic here
            audioPlayer.Update();
            camera.Update();
            if (IsActive)
            {
                StaticMouse.Update();

                UpdateThings.UpdateAlwaysUpdateThings(camera);
                GameState.Update(player);
            }
            

            debugText = player.health.ToString(); //TEMP

            
            Settings.UpdateZoom();

            base.Update(gameTime);
        }


        private static bool resetRenderTarget = false;
        protected override void Draw(GameTime gameTime)
        {
            // Drawing everything to render target here
            if (resetRenderTarget)
            {
                Game1.renderTarget = new RenderTarget2D(GraphicsDevice, Game1._graphics.PreferredBackBufferWidth, Game1._graphics.PreferredBackBufferHeight);
                resetRenderTarget = false;
            }

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);
            drawEntities.BeginBuffer(camera);

            //Draws Tiles onto map rendering only the area visible to player
            if (GameState.isInGame)
            {
                for (int x = (int)(player.position.X - _graphics.PreferredBackBufferWidth) / 32; x < (player.position.X + _graphics.PreferredBackBufferWidth) / 32; x++)
                {
                    for (int y = (int)(player.position.Y - _graphics.PreferredBackBufferHeight) / 32; y < (player.position.Y + _graphics.PreferredBackBufferHeight) / 32; y++)
                    {
                        if (x >= 0 && y >= 0 && x <= 512 && y <= 512)
                            drawEntities.AddToDrawBuffer(TileMap.tileMap[x, y], x, y);
                    }
                }
                drawEntities.AddToDrawBuffer(currentGameObjects);
                drawEntities.AddToDrawBuffer(activeParticles);
            }
            drawEntities.DrawBuffer();
            GraphicsDevice.SetRenderTarget(null);

            // Drawing the render target to the screen here
            drawEntities.BeginBuffer();
            drawEntities.spriteBatch.Draw(renderTarget, new Rectangle(0,0, Game1._graphics.PreferredBackBufferWidth, Game1._graphics.PreferredBackBufferHeight), Color.White);
            drawEntities.spriteBatch.DrawString(font, debugText, new Vector2(400, 400), Color.DarkBlue); //draws debug text
            drawEntities.DrawBuffer();

            drawEntities.BeginUiBuffer();
            drawEntities.AddToUiBuffer(menus); //draws menus
            drawEntities.DrawUiBuffer();

            base.Draw(gameTime);
        }
        public static void ExitGame()
        {
            toExit = true;
        }

        public static void ResetRenderTarget()
        {
            resetRenderTarget = true;
        }
    }
}