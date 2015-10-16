using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;

namespace Titanium.Entities
{
    [DelimitedRecord(",")]
    class UnitStats
    {
        public String name;
        public String model;
        public int modelFrameCount;
        public int level;
        public int strength;
        public int agility;
        public int intelligence;
        public int baseAttack;
        public int baseHP;
        public int baseMP;
        public int baseSpeed;

        [FieldHidden]
        public int currentHP;
        [FieldHidden]
        public int currentMP;

        public UnitStats()
        {

        }

        public void normalize()
        {
            currentHP = baseHP;
            currentMP = baseMP;
        }
    }
}
