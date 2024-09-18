using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Weapons
{
    internal class Weapon
    {
        public int attackRange { get; protected set; }
        public Weapon(int attackRange, ushort textureIndex)
        {
            this.attackRange = attackRange;
        }
    }
}
