using UnityEngine;
using System.Collections;
using CastleMagic.Game.Entities;
using UnityEngine.Events;
using CastleMagic.Util;
using System.Linq;

namespace CastleMagic.Game.Selection {

    [RequireComponent(typeof(SelectionManager))]
    public class EntitySelectionManager : MonoBehaviour {

        public UnityAction OnSelectionChange;
        public EntityController selected {
            get;
            private set;
        }

        private BoardManager boardManager;
        private SelectionManager selection;
        private GameObjectPool highlighterPool;
        private GameObject highlighterPrefab;

        // Use this for initialization
        private void Start() {
            var board = GameObject.FindWithTag("Board");
            boardManager = board.GetComponent<BoardManager>();
            highlighterPrefab = Resources.Load("Prefabs/UI/HexHighlighter") as GameObject;
            highlighterPool = new GameObjectPool(() => Instantiate(highlighterPrefab));
            selection = GetComponent<SelectionManager>();
            selection.OnSelectionChange += () => {
                var newSel = selection.selected.GetComponent<EntityController>();
                var oldSel = selected;
                if (newSel != oldSel) {
                    selected = newSel;
                    OnSelectionChange.Invoke();
                }
            };

            // Entity movement highlighter behavior
            OnSelectionChange += () => {
                Debug.Log("selected entite");
                var coords = boardManager.board.PerformBFS(
                    selected.HexTransform.Position,
                    selected.energy,
                    x => !boardManager.IsPositionOccupied(x))
                        .Select(x => x.Item1)
                        .ToList();
                    highlighterPool.Acquire(coords, (obj, coord) => {
                        var ht = obj.GetComponent<HexTransform>();
                        ht.Position = coord;
                        ht.UpdatePhysicalPosition();
                });
            };
        }

        // Update is called once per frame
        void Update() {

        }
    }

}