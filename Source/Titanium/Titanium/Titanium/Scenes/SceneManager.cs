using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titanium.Scenes
{
    public enum SceneState
    {
        main,
        arena,
        battle
    }

    /// <summary>
    /// Class that manages the scenes registered to each game state and 
    /// renders them accordingly. Scenes registered 
    /// </summary>
    class SceneManager : DrawableGameComponent
    {
        public static int NUM_SCENESTATES = 3;

        Scene[] scenes = new Scene[NUM_SCENESTATES];

        SceneState currentState;

        InputState input = new InputState();

        SpriteBatch spriteBatch;
        SpriteFont font;

        bool isInitialized;

        /// <summary>
        /// A default SpriteBatch shared by all the scenes. This saves
        /// each screen having to bother creating their own local instance.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }


        /// <summary>
        /// A default font shared by all the scenes. This saves
        /// each screen having to bother loading their own local copy.
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
        }

        /// <summary>
        /// Constructs a new SceneManager.
        /// </summary>
        public SceneManager(Game game): base(game)
        {
            registerScene(new MainMenuScene(), SceneState.main);
            registerScene(new BattleScene(), SceneState.battle);
            registerScene(new ArenaScene(), SceneState.arena);

            currentState = SceneState.main;
            
        }


        /// <summary>
        /// Initializes the screen manager component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
        }

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            ContentManager content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = content.Load<SpriteFont>("TestFont");

            // Tell each of the screens to load their content.
            foreach (Scene scene in scenes)
            {
                if( scene != null )
                    scene.loadScene();
            }
        }

        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Tell each of the screens to unload their content.
            foreach (Scene scene in scenes)
            {
                scene.unloadScene();
            }
        }

        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void registerScene(Scene scene, SceneState state)
        {
            scene.SceneManager = this;

            // If we have a graphics device, tell the scene to load content.
            if (isInitialized)
            {
                scene.loadScene();
            }

            scenes[(int)state] = scene;
        }

        /// <summary>
        /// Allows the current scene to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            input.update();


            if (scenes[(int)currentState] != null)
                scenes[(int)currentState].update(gameTime, input);
            else
                throw new NullReferenceException();
        }

        /// <summary>
        /// Tells the current scene to draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            if (scenes[(int)currentState] != null)
                scenes[(int)currentState].draw(gameTime);
            else
                throw new NullReferenceException();
        }

        /// <summary>
        /// Associates a SceneState with a Scene and optionally switches to that scene.
        /// </summary>
        /// <param name="state">The desired ScreenState to associate with the Scene</param>
        /// <param name="scene">The desired Scene</param>
        /// <param name="change">true if the game should switch to that scene</param>
        public void setScene(SceneState state, Scene scene, bool change)
        {
            scenes[(int)state] = scene;
            if(change)
                changeScene(state);
        }

        /// <summary>
        /// Change the scene.
        /// </summary>
        /// <param name="state">The scene state to transition to</param>
        //TODO: animate the scene transitions
        public void changeScene(SceneState state)
        {
            currentState = state;
        }
    }
}