using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Titanium.Entities
{
    class Sprite : Entity
    {
        private Rectangle sourceRect, destRect;
        private double elapsed, delay;
        private int frames, posX, posY, frameCount;

        //For testing purpose only
        Texture2D spriteFile;
        String filePath = "";

        public Sprite()
        {
            elapsed = 0;
            delay = 200;
            frames = 0;
            posX = 150;
            posY = 150;
        }


        public void Load(ContentManager content)
        {
            spriteFile = content.Load<Texture2D>(filePath);
            destRect = new Rectangle(posX, posY, spriteFile.Width / frameCount, spriteFile.Height);
        }

        public void setParam(String s, int x, int y, int totalFrames)
        {
            filePath = s;
            posX = x;
            posY = y;
            frameCount = totalFrames;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteFile, destRect, sourceRect, Color.White);
        }

        public override void Update(GamePadState gamepadState, KeyboardState keyboardState, MouseState mouseState)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
            {
                if (frames >= (frameCount-1))
                {
                    frames = 0;
                }
                else
                {
                    frames++;
                }
                elapsed = 0;
            }

            sourceRect = new Rectangle(spriteFile.Width / frameCount * frames, 0, spriteFile.Width / frameCount, spriteFile.Height);
        }
    }
}
