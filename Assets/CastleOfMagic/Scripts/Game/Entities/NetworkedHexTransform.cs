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
            if (isServer) {
                Debug.Log("repeating sps");
                InvokeRepeating("SetPositionRepeating", 0.0f, 2.0f);
            }
        }

        private void SetPositionRepeating() {
            RpcSetPosition(trs.Position);
        }

        [ClientRpc]
        private void RpcSetPosition(HexCoord pos) {
            trs.Position = pos;
        }

    }
}