using System;
using CastleMagic.Util.Hex;
using UnityEngine.Networking;

namespace CastleMagic.Game.Entites
{
    /// <summary>
    /// Represents any object that has a position on the board and cannot be passed through.
    /// </summary>
    public abstract class Entity : NetworkBehaviour
    {
        public HexCoord position;

        public int maxEnergy;
        public int energy;
        public int maxHealth;
        public int health;
        public bool invunerable;

        protected EntityClass entityClass;
    }
}
