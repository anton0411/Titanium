using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Titanium.Scenes.Panels;

namespace Titanium.Scenes
{
    /// <summary>
    /// The Main Menu
    /// </summary>
    class MainMenuScene : Scene
    {
        ContentManager content;
        SpriteFont font;

        // Possible player actions
        InputAction arena;
        InputAction battle;

        MenuPanel mainMenu;

        public MainMenuScene(): base()
        {
            // Initialize the player actions
            arena = new InputAction(
                new Buttons[] {Buttons.A},
                new Keys[] {Keys.A},
                true
                );

            battle = new InputAction(
                new Buttons[] { Buttons.B },
                new Keys[] { Keys.B },
                true
                );
        }

        public override void draw(GameTime gameTime)
        {
            SceneManager.SpriteBatch.Begin();
            base.draw(gameTime);
            SceneManager.SpriteBatch.End();
        }

        // 
        public override void loadScene()
        {
            if (content == null)
                content = new ContentManager(SceneManager.Game.Services, "Content");

            font = content.Load<SpriteFont>("TestFont");

            // Create the actual Main Menu panel
            mainMenu = new MenuPanel(this, Vector2.Zero, "Main Menu");
            List<MenuItem> options = new List<MenuItem>()
            {
                new MenuItem("Enter the arena!", arena, mainMenu),
                new MenuItem("Enter battle!", battle, mainMenu)
            };

            addPanel(mainMenu);

            mainMenu.center();
        }

        public override void unloadScene() {}

        public override void update(GameTime gameTime, InputState inputState)
        {
            PlayerIndex player;
            if(arena.Evaluate(inputState, null, out player))
            {
                SceneManager.changeScene(SceneState.arena);
            }
            else if (battle.Evaluate(inputState, null, out player))
            {
                SceneManager.changeScene(SceneState.battle);
            }
        }
    }
}
