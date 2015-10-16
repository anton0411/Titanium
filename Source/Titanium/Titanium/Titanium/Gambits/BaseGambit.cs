using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Titanium.Scenes;

namespace Titanium.Gambits
{
    abstract class BaseGambit
    {
        protected double startTime;
        protected bool finished;

        public BaseGambit(GameTime gameTime)
        {
            startTime = gameTime.TotalGameTime.TotalMilliseconds;
            finished = false;
        }

        public int timeElapsed(GameTime gameTime)
        {
            return (int)(gameTime.TotalGameTime.TotalMilliseconds - startTime);
        }

        public bool isComplete()
        {
            return finished;
        }

        public abstract int update(GameTime gameTime, InputState state);

        public abstract void draw(Scene scene);
    }
}
