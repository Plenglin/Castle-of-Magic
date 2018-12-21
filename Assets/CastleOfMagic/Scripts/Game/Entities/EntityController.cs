using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using CastleMagic.Util.Hex;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using CastleMagic.Game.Ability;
using Unity.Collections;

namespace CastleMagic.Game.Entities {

    /// <summary>
    /// Represents the class an entity represents.
    /// Dictates modifiers to the entity's stats, skills, whatever.
    /// Could represent anything. A crate, a mage, a tree, whatever you desire.
    /// </summary>
    [RequireComponent(typeof(HexTransform))]
    public class EntityController : NetworkBehaviour {

        public UnityAction<HexCoord, HexCoord> OnMoved;

        [SerializeField]
        [ReadOnly]
        private GhostController ghost = null;

        public string displayName;

        public int maxEnergy;

        public void TakeDamage(int damage) {
            health -= damage;
        }

        [SyncVar]
        public int energy;
        public int maxHealth;
        [SyncVar]
        public int health;
        [SyncVar]
        public bool invunerable;
        public bool unselectable;

        public GameObject ghostTemplate;

        private bool isPlayerControlled = false;

        public HexTransform HexTransform {
            get;
            private set;
        }

        public BoardManager BoardManager {
            get;
            private set;
        }

        public List<BaseAbility> abilities = new List<BaseAbility>();

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

        public HashSet<HexCoord> GetAccessibleTiles() {
            var set = new HashSet<HexCoord>();
            foreach (var pair in BoardManager.board.PerformBFS(HexTransform.Position, energy)) {
                set.Add(pair.Item1);
            }
            return set;
        }

        public void ToggleVisibility(bool visible) {
            Renderer r = GetComponentInChildren<Renderer>() as Renderer;
            r.enabled = visible;
        }

        public void ToggleVisibility() {
            Renderer r = GetComponentInChildren<Renderer>() as Renderer;
            r.enabled = !r.enabled;
        }
    }
}