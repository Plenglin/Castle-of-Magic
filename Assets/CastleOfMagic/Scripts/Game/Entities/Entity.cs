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
        [SyncVar]
        public int energy;
        public int maxHealth;
        [SyncVar]
        public int health;
        [SyncVar]
        public bool invunerable;

        protected EntityClass entityClass;
    }
}
