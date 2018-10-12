using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using CastleMagic.Util.Hex;
using UnityEngine.Events;

namespace CastleMagic.Game.Entites {

    /// <summary>
    /// Represents the class an entity represents.
    /// Dictates modifiers to the entity's stats, skills, whatever.
    /// Could represent anything. A crate, a mage, a tree, whatever you desire.
    /// </summary>
    [RequireComponent(typeof(HexTransform))]
    public class EntityController : NetworkBehaviour {

        public UnityAction<HexCoord, HexCoord> OnMoved;

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

        private void Awake() {
            HexTransform = GetComponent<HexTransform>();
            BoardManager = GameObject.FindWithTag("Board").GetComponent<BoardManager>();
        }

        private void Start() {
            OnMoved += (from, to) => {
                HexTransform.Position = to;
            };
        }

        public override string ToString() {
            return $"EntityController({displayName})";
        }

    }
}