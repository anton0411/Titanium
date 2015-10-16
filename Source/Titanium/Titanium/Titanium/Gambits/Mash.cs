using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Titanium.Scenes;

namespace Titanium.Gambits
{
    class Mash: BaseGambit
    {
        private int count = 0;

        private InputAction action;

        private int timeLimit = 3000;
        private int timeLeft;

        private PlayerIndex controllingPlayer = PlayerIndex.One;

        public Mash(GameTime gameTime, InputAction action) : base(gameTime)
        {
            this.action = action;
            timeLeft = timeLimit;
        }

        public Mash(GameTime gameTime, InputAction action, int timeLimit) : base(gameTime)
        {
            this.action = action;
            this.timeLimit = timeLimit;
            this.timeLeft = timeLimit;
        }

        public override int update(GameTime gameTime, InputState state)
        {
            PlayerIndex player;
            timeLeft = timeLimit - timeElapsed(gameTime);

            if (timeLeft <= 0)
            {
                finished = true;
            }
            else if (action.Evaluate(state, null, out player))
            {
                ++count;
            }
            return count;
        }

        public override void draw(Scene scene)
        {
            SpriteBatch sb = scene.SceneManager.SpriteBatch;
            SpriteFont font = scene.SceneManager.Font;

            string msg = "Times Pressed: " + count + "\nTime Left: " + TimeSpan.FromMilliseconds(timeLeft);

            sb.DrawString(font, msg, Vector2.Zero, Color.Red);
        }

        
        
    }
}
