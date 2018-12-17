using CastleMagic.Game;
using CastleMagic.Game.Entities;
using CastleMagic.Game.GameInfo.PlayerActions;
using CastleMagic.UI.GameUI;
using CastleMagic.Util;
using CastleMagic.Util.Hex;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;

namespace CastleMagic.Game.Selection {
    public class SelectionManager : MonoBehaviour {

        public UnityAction OnSelectionChange;
        public UnityAction OnHexSelection;

        public NetworkPlayerController player;

        public HexCoord? lastSelectedCoord = null;
        public Selectable selected {
            get;
            private set;
        }

        private HexPlane plane;
        private BoardManager boardManager;
        private RectTransform infoPane;
        private GameObject highlighterPrefab;

        private GameObjectPool highlighterPool;
        private int mask;

        private void Start() {
            infoPane = GameObject.FindWithTag("InfoPane").GetComponent<RectTransform>();

            var board = GameObject.FindWithTag("Board");
            plane = board.GetComponent<HexPlane>();
            boardManager = board.GetComponent<BoardManager>();

            highlighterPrefab = Resources.Load("Prefabs/UI/HexHighlighter") as GameObject;
            highlighterPool = new GameObjectPool(() => Instantiate(highlighterPrefab));

            mask = LayerMask.GetMask("Selectable");

            // Entity movement highlighter behavior
            OnSelectionChange += () => {
                var entity = selected?.GetComponent<EntityController>();
                Debug.Log($"Creating highlighters for {entity}");
                if (entity != null) {
                    var coords = boardManager.board.PerformBFS(
                            entity.HexTransform.Position,
                            entity.energy,
                            x => !boardManager.IsPositionOccupied(x))
                        .Select(x => x.Item1)
                        .ToList();
                    highlighterPool.Acquire(coords, (obj, coord) => {
                        var ht = obj.GetComponent<HexTransform>();
                        ht.Position = coord;
                        ht.UpdatePhysicalPosition();
                    });
                }
            };

            OnHexSelection += () => {
                var entity = selected?.GetComponent<EntityController>();
                if (entity != null) {
                    var dest = lastSelectedCoord;
                    if (!player.ghostPlayer.HexTransform.Position.Equals(dest)) {
                        //player.CmdMoveEntity(selected.GetInstanceID(), (HexCoord) dest);
                        // needs some sort of "ghost" player to represent movement, idk
                        player.AddTurnAction(new TurnActionMove(player, player.ghostPlayer.HexTransform.Position, (HexCoord)dest));
                    }
                }
                ClearSelection();
            };
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var newSelection = HandleBoardSelection(ray);
                var oldSelection = selected;

                selected = newSelection;

                if (newSelection != oldSelection) {
                    oldSelection?.OnDeselected(infoPane);
                    newSelection?.OnSelected(infoPane);

                    OnSelectionChange.Invoke();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                selected?.OnDeselected(infoPane);
                selected = null;
                OnSelectionChange.Invoke();
                ClearSelection();
            }
        }

        private HexCoord? HandleHexSelection(Ray ray) {
            HexCoord? coordHit;
            if (plane.RaycastToHex(ray, out coordHit)) {
                Debug.Log("clicked on hex " + coordHit);
            } else {
                coordHit = null;
            }
            return coordHit;
        }

        private Selectable HandleBoardSelection(Ray ray) {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, mask)) {
                return hit.collider.GetComponentInParent<Selectable>();
            } else {
                var pos = HandleHexSelection(ray);
                EntityController entity = boardManager.GetEntityAtPosition(pos);
                return entity?.GetComponent<Selectable>();
            }
        }

        public void ClearSelection() {
            selected = null;
            highlighterPool.Acquire(0);
        }

    }
}