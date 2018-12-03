using CastleMagic.Game;
using CastleMagic.Game.Entites;
using CastleMagic.Game.GameInfo.PlayerActions;
using CastleMagic.UI.GameUI;
using CastleMagic.Util.Hex;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CastleMagic.UI {
    public class EntitySelectionManager : MonoBehaviour {

        public NetworkPlayerController player;
        public List<EntityController> slaves;

        private EntityController selected = null;

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
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (selected == null) {
                    selected = HandleBoardSelection(ray);
                    Debug.Log("clicked on entity " + selected);
                } else {
                    var dest = HandleHexSelection(ray);
                    if (dest != null) {
                        //player.CmdMoveEntity(selected.GetInstanceID(), (HexCoord) dest);
                        // needs some sort of "ghost" player to represent movement, idk
                        player.AddTurnAction(new ActionMove(player, player.player.HexTransform.Position, (HexCoord) dest));
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

        private EntityController HandleBoardSelection(Ray ray) {
            RaycastHit hit;

            int mask = LayerMask.GetMask("Entity");

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, mask)) {
                return hit.collider.GetComponentInParent<EntityController>();
            } else {
                var pos = HandleHexSelection(ray);
                EntityController entity = boardManager.GetEntityAtPosition(pos);
                if(entity.unselectable) {
                    return null;
                }
                return entity;
            }
        }

        public void SelectEntity(EntityController entity) {
            Debug.Log("Selected " + entity);
            highlighters.ForEach(it => {
                Destroy(it);
            });
            highlighters.Clear();
            var prefab = Resources.Load("Prefabs/UI/HexHighlighter") as GameObject;
            foreach (Tuple<HexCoord, int> pair in boardManager.board.PerformBFS(entity.HexTransform.Position, entity.energy)) {
                Debug.Log("Adding highlighter to " + pair);
                var obj = Instantiate(prefab);
                obj.GetComponent<HighlighterController>().destination = pair.Item1;
                highlighters.Add(obj);
            }
        }

        public void ClearSelection() {
            selected = null;
            highlighters.ForEach(Destroy);
            highlighters.Clear();
        }

    }
}