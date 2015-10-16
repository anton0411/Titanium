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

namespace Titanium.Entities
{
    /**
     * This class provides a base for all in-game entities that must be updated and rendered to the screen.
     */
    public abstract class Entity
    {
        /**
         * The default entity constructor.
         */
        public Entity()
        {

        }
        
        /**
         * The update function called in each frame.
         */
        public abstract void Update(GamePadState gamepadState, KeyboardState keyboardState, MouseState mouseState);

        /**
         * The draw function called at the end of each frame.
         */
        public abstract void Draw(SpriteBatch sb);
    }
}
