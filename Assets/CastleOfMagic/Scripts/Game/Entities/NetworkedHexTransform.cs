using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using CastleMagic.Util.Hex;

namespace CastleMagic.Game.Entites {

    [RequireComponent(typeof(HexTransform))]
    public class NetworkedHexTransform : NetworkBehaviour {

        private HexTransform trs;
        
        // Use this for initialization
        void Start() {
            trs = GetComponent<HexTransform>();
            trs.OnPositionChanged += (delta) => {
                if (isServer) {
                    RpcSetPosition(delta.end);
                }
            };
        }

        [ClientRpc]
        public void RpcSetPosition(HexCoord pos) {
            trs.Position = pos;
        }

    }
}