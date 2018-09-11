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

        protected int maxEnergy;
        protected int energy;
        protected int maxHealth;
        protected int health;
        protected bool invunerable;
    }
}
