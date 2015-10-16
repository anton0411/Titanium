using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titanium.Scenes.Panels
{
    class MenuPanel: Panel
    {
        string title;

        Color textColor = Color.DarkBlue;

        // The font for the title
        protected SpriteFont font;

        // The font of the items
        public SpriteFont itemFont;

        // Some padding so things aren't all squished
        private int SPACING = 5;

        public MenuPanel(Scene scene, Vector2 pos, string menuTitle): base(scene, pos)
        {
            this.title = menuTitle;
            font = scene.SceneManager.Font;
            itemFont = font;
        }


        /// <summary>
        /// Updates the location of each of the items in the menu.
        /// </summary>
        public void updateMenuItemLocations()
        {
            // The items should be below the title, if there is one
            float currentHeight = string.IsNullOrEmpty(title) ? Position.Y : Position.Y + font.LineSpacing;

            // Update the location of each menu item
            foreach (MenuItem item in subPanels)
            {
                item.Offset = new Vector2(item.Origin.X + Position.X, currentHeight + SPACING);
                currentHeight += item.GetHeight();
            }

        }

        /// <summary>
        /// The height of the menu.
        /// </summary>
        /// <returns>The total height of this menu</returns>
        public float totalHeight()
        {
            float h = string.IsNullOrEmpty(title) ? Position.Y : font.LineSpacing + Position.Y;
            foreach (MenuItem item in subPanels)
                h += item.GetHeight() + SPACING;
            return h;
        }

        /// <summary>
        /// The width of the menu.
        /// </summary>
        /// <returns>The total width of the menu.</returns>
        public float totalWidth()
        {
            float w = 0f;
            float tmp;
            foreach (MenuItem item in subPanels)
            {
                tmp = item.GetWidth();
                if (tmp > w)
                    w = tmp;
            }
            return w;
        }
        
        public override void draw(GameTime gameTime)
        {
            sb.DrawString(font, title, Position, textColor);
            base.draw(gameTime);
        }

        public override void update(GameTime gameTime, InputState inputState)
        {
            base.update(gameTime, inputState);
        }

        /// <summary>
        /// Center this menu in the screen.
        /// </summary>
        public void center()
        {
            int width = Scene.SceneManager.GraphicsDevice.Viewport.Width;
            int height = Scene.SceneManager.GraphicsDevice.Viewport.Height;

            float x = width / 2 - totalWidth() / 2;
            float y = height / 2 - totalHeight() / 2;

            this.Origin = new Vector2(x, y);
            updateMenuItemLocations();
        }
    }
}
