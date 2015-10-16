using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titanium.Entities
{
    class SpriteChar
    {

        public UnitStats unitStats;

        public SpriteChar()
        { }


        private void attack(SpriteChar s)
        {
            //minigame.play(); Returns a float value equal to the % modifier
            //0.75 for a flubbed game, 1.1 for a perfect, etc;
            //playAnimation()
            float gameResult = 1.0f;
            this.unitStats.modifiers.Add(gameResult);
            float damageDone = this.unitStats.attack;
            foreach (float f in this.unitStats.modifiers)
            {
                damageDone = damageDone * f;
            }

            s.unitStats.curHP = s.unitStats.curHP - damageDone;
        }
    }
}
