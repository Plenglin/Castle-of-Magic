using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using CastleMagic.Util.Hex;

namespace CastleMagic.Game.Entites {

    /// <summary>
    /// Represents the class an entity represents.
    /// Dictates modifiers to the entity's stats, skills, whatever.
    /// Could represent anything. A crate, a mage, a tree, whatever you desire.
    /// </summary>
    [RequireComponent(typeof(HexTransform))]
    public class EntityController : NetworkBehaviour {

        public int maxEnergy;
        [SyncVar]
        public int energy;
        public int maxHealth;
        [SyncVar]
        public int health;
        [SyncVar]
        public bool invunerable;

        protected HexTransform entity;

        private void Awake() {
            entity = GetComponent<HexTransform>();
            entity.AttachToController(this);
        }

        [ClientRpc]
        public void RpcPositionChanged(HexCoord pos) {
            entity.OnPositionChanged(pos);
        }

    }
}