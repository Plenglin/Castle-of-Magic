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

        private int mask;

        private void Start() {
            infoPane = GameObject.FindWithTag("InfoPane").GetComponent<RectTransform>();

            var board = GameObject.FindWithTag("Board");
            plane = board.GetComponent<HexPlane>();
            boardManager = board.GetComponent<BoardManager>();

            highlighterPrefab = Resources.Load("Prefabs/UI/HexHighlighter") as GameObject;

            mask = LayerMask.GetMask("Selectable");

            OnHexSelection += () => {
                var entity = selected?.GetComponent<EntityController>();
                Debug.Log("asdf");
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
                
                var pos = HandleHexSelection(ray);
                
                var newSelection = RaycastSelection(ray);
                var oldSelection = selected;

                selected = newSelection;
                Debug.Log($"Clicked on {newSelection}");
                
                if (newSelection != oldSelection) {
                    oldSelection?.OnDeselected(infoPane);
                    newSelection?.OnSelected(infoPane);

                    OnSelectionChange.Invoke();
                }

                if (pos != null) {
                    lastSelectedCoord = pos;
                    OnHexSelection.Invoke();
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
            HexCoord? coordHit = null;
            if (plane.RaycastToHex(ray, out coordHit)) {
                Debug.Log("clicked on hex " + coordHit);
            }
            return coordHit;
        }

        private Selectable RaycastSelection(Ray ray) {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, mask)) {
                return hit.collider.GetComponentInParent<Selectable>();
            }
            return null;
        }

        private EntityController CoordSelection(HexCoord? coord) {
            return boardManager.GetEntityAtPosition(coord);
        }

        public void ClearSelection() {
            selected = null;
        }

    }
}