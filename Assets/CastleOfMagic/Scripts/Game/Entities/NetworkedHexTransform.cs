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
        }

        // Update is called once per frame
        void Update() {
            if (isServer) {
                RpcSetPosition(trs.Position);
            }
        }

        [ClientRpc]
        public void RpcSetPosition(HexCoord pos) {
            trs.Position = pos;
        }

    }
}