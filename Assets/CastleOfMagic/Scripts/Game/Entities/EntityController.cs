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

        public string displayName;

        public int maxEnergy;
        [SyncVar]
        public int energy;
        public int maxHealth;
        [SyncVar]
        public int health;
        [SyncVar]
        public bool invunerable;

        public HexTransform HexTransform {
            get;
            private set;
        }

        public BoardManager BoardManager {
            get;
            private set;
        }

        private void Start() {
            HexTransform = GetComponent<HexTransform>();
            BoardManager = GameObject.FindWithTag("Board").GetComponent<BoardManager>();
            BoardManager.InitializeEntity(this, HexTransform.Position);
        }

    }
}