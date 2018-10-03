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

        public void Awake() {
        }

        private void Start() {
            if (!isLocalPlayer) return;

            plane = GameObject.FindWithTag("Board").GetComponent<HexPlane>();
        }

        private void Update() {
            if (!isLocalPlayer) return;
        
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                HexCoord? coordHit;

                int mask = LayerMask.GetMask("Entity");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, float.PositiveInfinity, mask)) {
                    selected = hit.collider.GetComponentInParent<EntityController>();
                    Debug.Log("clicked on " + selected);
                } else if (plane.RaycastToHex(ray, out coordHit)) {
                    Debug.Log("clicked on " + coordHit);
                } else {
                    selected = null;
                }
            }
        }

        [Command]
        public void CmdMoveEntity(int target, HexCoord dest) {
            
        }
    }
}
