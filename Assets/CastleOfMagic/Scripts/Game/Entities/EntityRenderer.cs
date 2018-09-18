using System;
using CastleMagic.Util.Hex;
using UnityEngine;
using UnityEngine.Networking;

namespace CastleMagic.Game.Entites {

    /// <summary>
    /// Represents any object that has a position on the board and cannot be passed through.
    /// </summary>
    public abstract class EntityRenderer : NetworkBehaviour {

        private HexPlane plane;
        protected EntityController controller;

        private void Awake() {
            plane = GameObject.FindWithTag("Board").GetComponent<HexPlane>();
        }

        public void AttachToController(EntityController controller) {
            this.controller = controller;
        }

        public void OnPositionChanged(HexCoord newPos) {
            transform.position = plane.HexToWorldPosition(newPos);
        }

    }
}
