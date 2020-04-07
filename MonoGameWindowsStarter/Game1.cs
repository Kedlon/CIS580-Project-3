using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SoundEffect playerHitSFX;

        Player player;

        List<StaticSprite> hazards = new List<StaticSprite>();

        Random random = new Random();

        Stopwatch HitCooldown = new Stopwatch();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //graphics.PreferredBackBufferWidth = 1042;
            //graphics.PreferredBackBufferHeight = 768;
            base.Initialize();
            HitCooldown.Start();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load sfx for player hit
            playerHitSFX = Content.Load<SoundEffect>("Hit_Hurt");

            // TODO: use this.Content to load your game content here
            var playerCar = Content.Load<Texture2D>("PixelCar");
            player = new Player(playerCar);

            //Load backgound
            var backgroundTexture = Content.Load<Texture2D>("Background");
            var backgroundSprite = new StaticSprite(backgroundTexture);
            var backgroundLayer = new ParallaxLayer(this);
            backgroundLayer.Sprites.Add(backgroundSprite);
            backgroundLayer.DrawOrder = 0;
            Components.Add(backgroundLayer);

            //load player
            var playerLayer = new ParallaxLayer(this);
            playerLayer.Sprites.Add(player);
            playerLayer.DrawOrder = 3;
            Components.Add(playerLayer);

            //load midground
            var midgroundTexture = Content.Load<Texture2D>("Midground");
            var midgroundSprite = new StaticSprite(midgroundTexture);
            var midgroundLayer = new ParallaxLayer(this);
            midgroundLayer.Sprites.Add(midgroundSprite);
            midgroundLayer.DrawOrder = 1;
            Components.Add(midgroundLayer);

            //load foregound Textures
            var foregroundTextures = Content.Load<Texture2D>("Foreground");
            var foregroundSprite = new StaticSprite(foregroundTextures);
            var foregroundLayer = new ParallaxLayer(this);
            foregroundLayer.Sprites.Add(foregroundSprite);
            foregroundLayer.DrawOrder = 2;
            Components.Add(foregroundLayer);

            //load and set hazards
            var hazardTexture = Content.Load<Texture2D>("Logs-Big");
            for (int i = 0; i < 5; i++)
            {
                var hazardVector = new Vector2(
                            MathHelper.Lerp(300, 2000, (float)random.NextDouble()), // X between 300 and 2000
                            MathHelper.Lerp(300, 400, (float)random.NextDouble()) // Y between 300 and 400
                            );
                var sprite = new StaticSprite(hazardTexture, hazardVector);
                sprite.bounds = new BoundingRectangle(hazardVector, 64, 64);
                hazards.Add(sprite);
            }
            var hazardLayer = new ParallaxLayer(this);
            foreach (var item in hazards)
            {
                hazardLayer.Sprites.Add(item);
            }
            hazardLayer.DrawOrder = 3;
            Components.Add(hazardLayer);

            //setup for player tracking
            backgroundLayer.ScrollController = new PlayerTrackingScrollController(player, 0.1f);
            midgroundLayer.ScrollController = new PlayerTrackingScrollController(player, 0.4f);
            playerLayer.ScrollController = new PlayerTrackingScrollController(player, 1.0f);
            foregroundLayer.ScrollController = new PlayerTrackingScrollController(player, 1.0f);
            hazardLayer.ScrollController = new PlayerTrackingScrollController(player, 1.0f);
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // TODO: Add your update logic here
            player.Update(gameTime);
            foreach(StaticSprite hazard in hazards)
            {
                if (player.Bounds.CollidesWith(hazard.bounds) && HitCooldown.ElapsedMilliseconds > 2500)
                {
                    playerHitSFX.Play();
                    HitCooldown.Reset();
                    HitCooldown.Start();
                }
            }
            
            base.Update(gameTime);
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            base.Draw(gameTime);
        }
    }
}
