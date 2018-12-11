using CastleMagic.Game;
using CastleMagic.Game.Entities;
using CastleMagic.Game.GameInfo.PlayerActions;
using CastleMagic.UI.GameUI;
using CastleMagic.Util.Hex;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CastleMagic.Game.Selection {
    public class EntitySelectionManager : MonoBehaviour {

        public UnityAction OnSelectionChange;

        public NetworkPlayerController player;
        public List<EntityController> slaves;

        private Selectable _selected = null;
        public Selectable selected {
            get {
                return _selected;
            }
            private set {
                var old = _selected;
                _selected = value;
                if (old != value) {
                    OnSelectionChange.Invoke();
                }
            }
        }

        private HexPlane plane;
        private BoardManager boardManager;

        private List<GameObject> highlighters = new List<GameObject>();

        void Start() {
            var board = GameObject.FindWithTag("Board");
            plane = board.GetComponent<HexPlane>();
            boardManager = board.GetComponent<BoardManager>();
            var prefab = Resources.Load("Prefabs/Entities/EntityPlayer") as GameObject;
            Debug.Log(player);
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (selected == null) {
                    selected = HandleBoardSelection(ray);
                    Debug.Log("clicked on entity " + selected);
                } else {
                    var dest = HandleHexSelection(ray);
                    if (!player.ghostPlayer.HexTransform.Position.Equals(dest)) {
                        //player.CmdMoveEntity(selected.GetInstanceID(), (HexCoord) dest);
                        // needs some sort of "ghost" player to represent movement, idk
                        player.AddTurnAction(new TurnActionMove(player, player.ghostPlayer.HexTransform.Position, (HexCoord) dest));
                        ClearSelection();
                    } else {
                        ClearSelection();
                    }
                }

                if (selected != null) {
                    SelectEntity(selected);
                }
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

            int mask = LayerMask.GetMask("Entity");

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, mask)) {
                return hit.collider.GetComponentInParent<Selectable>();
            } else {
                var pos = HandleHexSelection(ray);
                EntityController entity = boardManager.GetEntityAtPosition(pos);
                return entity.GetComponent<Selectable>();
            }
        }

        public void Select(Selectable obj) {
            Debug.Log("Selected " + obj);
            highlighters.ForEach(it => {
                Destroy(it);
            });
            highlighters.Clear();
            var prefab = Resources.Load("Prefabs/UI/HexHighlighter") as GameObject;
            if (obj == player.ghostPlayer || player.slaves.Contains(obj)) {
                foreach (Tuple<HexCoord, int> pair in boardManager.board.PerformBFS(obj.HexTransform.Position, obj.energy)) {
                    //Debug.Log("Adding highlighter to " + pair);
                    var obj = Instantiate(prefab);
                    obj.GetComponent<HighlighterController>().destination = pair.Item1;
                    highlighters.Add(obj);
                }
            }
        }

        public void ClearSelection() {
            selected = null;
            highlighters.ForEach(Destroy);
            highlighters.Clear();
        }

    }
}