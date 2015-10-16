using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titanium.Entities
{
    class UnitStats
    {
        public float curHP, maxHP, curMP, maxMP, attack;
        public List<float> modifiers;

        public UnitStats()
        {
            modifiers = new List<float>();
        }

    }
}
