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
using Titanium.Entities;
using Titanium.Arena;

namespace Titanium.Scenes
{
    /**
     * This class provides a base for all game scenes.
     * 
     * Each game scene represents a distinct screen in the game.
     */
    class ArenaScene : Scene
    {
        public static ArenaScene instance;

        private Tile[,] baseArena;

        private Tile StartTile;

        // Possible user actions
        InputAction menu,
                    battle,
                    up,
                    down,
                    left,
                    right;

        ContentManager content;

        public ArenaController controller;
        private Character Hero;
        public Camera camera;
        private ArenaTable table;
        private BasicEffect effect;
        
        /**
         * The default scene constructor.
         */
        public ArenaScene() : base()
        {
            instance = this;

            // Create the arena controller
            controller = new ArenaController();

            // Define the user actions
            menu = new InputAction(
                new Buttons[] {Buttons.B},
                new Keys[] {Keys.Back, Keys.Escape},
                true
                );

            battle = new InputAction(
                new Buttons[] {Buttons.X},
                new Keys[] {Keys.Enter, Keys.Space},
                true
                );

            up = new InputAction(
                new Buttons[] { Buttons.LeftThumbstickUp, Buttons.DPadUp },
                new Keys[] { Keys.W, Keys.Up},
                true
                );

            down = new InputAction(
                new Buttons[] { Buttons.LeftThumbstickDown, Buttons.DPadDown },
                new Keys[] { Keys.S, Keys.Down},
                true
                );

            left = new InputAction(
                new Buttons[] { Buttons.LeftThumbstickLeft, Buttons.DPadLeft },
                new Keys[] { Keys.A, Keys.Left },
                true
                );

            right = new InputAction(
                new Buttons[] { Buttons.LeftThumbstickRight, Buttons.DPadRight },
                new Keys[] { Keys.D, Keys.Right },
                true
                );     
           
          
        }

        /**
         * This function is called when a scene is made active.
         */
        public override void loadScene()
        {
            if (content == null)
                content = new ContentManager(SceneManager.Game.Services, "Content");

            // Generate the arena
            ArenaBuilder builder = new ArenaBuilder(6, 6, content, SceneManager.GraphicsDevice.Viewport.AspectRatio, ArenaDifficulty.EASY);
            baseArena = builder.buildArenaBase();
            StartTile = builder.getStartTile();

            //
            if(effect ==null)
                effect = new BasicEffect(SceneManager.Game.GraphicsDevice);//null

            Hero = new Character();
            camera = new Camera(effect, SceneManager.Game.Window.ClientBounds.Width, SceneManager.Game.Window.ClientBounds.Height, SceneManager.GraphicsDevice.Viewport.AspectRatio, Hero.getPosition());
            //load model
            Hero.LoadModel(content, SceneManager.GraphicsDevice.Viewport.AspectRatio);
            
            table = new ArenaTable(getStartTile(), content);

            // Debug arena
            printDebugArena();  
        }

        /**
         * The update function called in each frame.
         */
        public override void update(GameTime gameTime, InputState inputState)
        {
            PlayerIndex player;

            //update Character
            Hero.Update(GamePad.GetState(PlayerIndex.One), Keyboard.GetState(), Mouse.GetState());
            camera.UpdateCamera(Hero.getPosition());
            /*
            if (up.Evaluate(inputState, PlayerIndex.One, out player))
                // Move up
            else if (down.Evaluate(inputState, PlayerIndex.One, out player))
                // Move down
            else if (left.Evaluate(inputState, PlayerIndex.One, out player))
                // Move left
            else if (right.Evaluate(inputState, PlayerIndex.One, out player))
                // Move right
            */

            // Update the tiles
            for (int i = 0; i < baseArena.GetLength(0); i++)
            {
                for (int j = 0; j < baseArena.GetLength(1); j++)
                {
                    baseArena[i, j].Update(GamePad.GetState(PlayerIndex.One), Keyboard.GetState(), Mouse.GetState());
                }
            }

            if ( menu.Evaluate(inputState, null, out player))
            {
                SceneManager.changeScene(SceneState.main);
            }
            else if( battle.Evaluate(inputState, null, out player) )
            {
                SceneManager.changeScene(SceneState.battle);
            }

            controller.update();

            // TEMP: New arena
            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                ArenaController.instance.moveToNextArena();
            }
        }

        /**
         * The draw function called at the end of each frame.
         */
        public override void draw(GameTime gameTime)
        {
            SpriteBatch sb = SceneManager.SpriteBatch;
            SceneManager.GraphicsDevice.Clear(ClearOptions.Target|ClearOptions.DepthBuffer, Color.DarkSlateGray,1.0f,0);
            SceneManager.GraphicsDevice.BlendState = BlendState.Opaque;
            SceneManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            SceneManager.GraphicsDevice.RasterizerState = rs;

            // Draw the table
            table.Draw(sb);

            // Draw the tiles
            for (int i = 0; i < baseArena.GetLength(0); i++)
            {
                for (int j = 0; j < baseArena.GetLength(1); j++)
                {
                    baseArena[i, j].Draw(sb);
                }
            }

            //Draw character
            Hero.Draw(sb);
        }

        /**
         * This function is called when a scene is no longer active.
         */
        public override void unloadScene()
        {
            
        }

        /// <summary>
        /// This function reloads the arena with a new difficulty.
        /// </summary>
        /// <param name="difficulty">The new arena's difficulty</param>
        public void loadNewArena(ArenaDifficulty difficulty)
        {
            // Generate the arena
            ArenaBuilder builder = new ArenaBuilder(6, 6, content, SceneManager.GraphicsDevice.Viewport.AspectRatio, difficulty);
            baseArena = builder.buildArenaBase();
            StartTile = builder.getStartTile();

            //
            if (effect == null)
                effect = new BasicEffect(SceneManager.Game.GraphicsDevice);//null

            Hero = new Character();
            camera = new Camera(effect, SceneManager.Game.Window.ClientBounds.Width, SceneManager.Game.Window.ClientBounds.Height, SceneManager.GraphicsDevice.Viewport.AspectRatio, Hero.getPosition());
            //load model
            Hero.LoadModel(content, SceneManager.GraphicsDevice.Viewport.AspectRatio);

            table = new ArenaTable(getStartTile(), content);

            // Debug arena
            printDebugArena();
        }

        /// <summary>
        /// method for retrieving the Start Tile;
        /// the tile on which the character initially starts on 
        /// when the arena is first loaded.
        /// </summary>
        /// <returns>The Starting Tile.</returns>
        public Tile getStartTile()
        {
            return StartTile;
        }

        private void printDebugArena()
        {
            for (int i = 0; i < baseArena.GetLength(0); i++)
            {
                for (int j = 0; j < baseArena.GetLength(1); j++)
                {
                    Console.Write(baseArena[i, j].getNumConnections() + " ");
                }

                Console.WriteLine();
            }
        }
    }
}
