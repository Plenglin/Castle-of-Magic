using System;
using CastleMagic.Util.Hex;
using UnityEngine;
using UnityEngine.Networking;

namespace CastleMagic.Game.Entites {

    /// <summary>
    /// Represents any object that has a position on the board and cannot be passed through.
    /// </summary>
    public class HexTransform : NetworkBehaviour {

        public HexCoord position = HexCoord.ZERO;
        private HexPlane plane;

        private void Awake() {
            plane = GameObject.FindWithTag("Board").GetComponent<HexPlane>();
        }

        private void Update() {
            transform.position = plane.HexToWorldPosition(position);
        }

        [ClientRpc]
        public void RpcPositionChanged(HexCoord pos) {
            position = pos;
        }

    }
}
