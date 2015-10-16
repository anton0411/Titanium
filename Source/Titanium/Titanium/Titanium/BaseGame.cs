using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Titanium.Scenes;
using Titanium.Entities;

namespace Titanium
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BaseGame : Microsoft.Xna.Framework.Game
    {
        public static int SCREEN_WIDTH = 1366;
        public static int SCREEN_HEIGHT = 768;

        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        /**3D world camera*/
        //Camera camera;

        /**BasicEffect object*/
        BasicEffect effect;

        /** An instance to the game object. */
        public static BaseGame instance;

        /** List of Models to be iterated through. */
        List<Entity> modelList;

        // Scene manager instance
        SceneManager sceneManager;


        /**
         * The base constructor for the game.
         */
        public BaseGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            instance = this;

            sceneManager = new SceneManager(this);

            // Set default window properties
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;

            modelList = new List<Entity>();

        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Create Basic Effect
            effect = new BasicEffect(graphics.GraphicsDevice);

            //Creates camera for 3D world
            //camera = new Camera(effect, (float)this.Window.ClientBounds.Width, (float)this.Window.ClientBounds.Height);

            Components.Add(sceneManager);

            base.Initialize();
        }

        /*
        protected void LoadModelsFromPath()
        {
            Model tempModel;
            float tempRatio;
            foreach (MovableModel m in modelList)//General idea is that we can change to specific models which also inherit from Entity for diff load methods
            {
                spriteBatch = new SpriteBatch(GraphicsDevice);
                
                tempModel = Content.Load<Model>(m.getPath());
                tempRatio = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;

                m.setModel(tempModel);
                m.setAspectRatio(tempRatio);
            }
        }*/

        protected override void UnloadContent(){}

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

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
