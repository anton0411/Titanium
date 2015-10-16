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
using Titanium.Scenes.Panels;

namespace Titanium.Scenes
{
    /**
     * This class provides a base for all game scenes.
     * 
     * Each game scene represents a distinct screen in the game.
     */
    abstract class Scene
    {

        List<Panel> panels = new List<Panel>();

        /// <summary>
        /// Gets or sets this scene's SceneManager.
        /// </summary>
        public SceneManager SceneManager
        {
            get { return sceneManager; }
            set { sceneManager = value; }
        }
        SceneManager sceneManager;

        /**
         * This function is called when a scene is made active.
         */
        public virtual void loadScene() { }

        /**
         * The update function called in each frame.
         */
        public virtual void update(GameTime gameTime, InputState inputState)
        {
            foreach(Panel panel in panels)
            {
                panel.update(gameTime, inputState);
            }
        }

        /**
         * The draw function called at the end of each frame.
         */
        public virtual void draw(GameTime gameTime)
        {
            foreach (Panel panel in panels)
            {
                panel.draw(gameTime);
            }
        }

        /**
         * This function is called when a scene is no longer active.
         */
        public abstract void unloadScene();

        public virtual void addPanel(Panel p)
        {
            panels.Add(p);
        }
    }
}
