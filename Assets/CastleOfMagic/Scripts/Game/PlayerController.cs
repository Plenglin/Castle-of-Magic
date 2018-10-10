using System;
using System.Collections.Generic;
using CastleMagic.Game.Entites;
using CastleMagic.Util.Hex;
using UnityEngine;
using UnityEngine.Networking;

namespace CastleMagic.Game
{
    public class PlayerController : NetworkBehaviour {

        public EntityController player;
        public List<EntityController> slaves;

        public EntityController selected = null;

        private HexPlane plane;
        private BoardManager boardManager;

        public void Awake() {
            slaves = new List<EntityController>();
        }

        private void Start() {
            var board = GameObject.FindWithTag("Board");
            plane = board.GetComponent<HexPlane>();
            boardManager = board.GetComponent<BoardManager>();
            var prefab = Resources.Load<GameObject>("CastleOfMagic/Prefabs/Entities/Player.prefab");
            player = Instantiate(prefab).GetComponent<EntityController>();
            slaves.Add(player);
            boardManager.InitializeEntity(player, HexCoord.CreateXY(0, 0));

            if (!isLocalPlayer) return;
        }

        private void Update() {
            if (!isLocalPlayer) return;
        
            if (Input.GetMouseButtonDown(0)) {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (selected == null) {
                    selected = HandleBoardSelection(ray);
                    Debug.Log("clicked on entity " + selected);
                } else {
                    var dest = HandleHexSelection(ray);
                    if (dest != null) {
                        CmdMoveEntity(selected.GetInstanceID(), (HexCoord) dest);
                        selected = null;
                    }
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
                return boardManager.GetEntityAtPosition(pos);
            }
        }

        [Command]
        public void CmdMoveEntity(int target, HexCoord dest) {
            Debug.Log("We are in the beam: " + target + " to " + dest);
            boardManager.MoveEntity(target, dest);
        }
    }
}
