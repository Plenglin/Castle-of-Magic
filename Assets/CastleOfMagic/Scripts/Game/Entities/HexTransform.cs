using System;
using CastleMagic.Util.Hex;
using UnityEngine;
using UnityEngine.Networking;

namespace CastleMagic.Game.Entites {

    /// <summary>
    /// Represents any object that has a position on the board and cannot be passed through.
    /// </summary>
    [ExecuteInEditMode]
    public class HexTransform : MonoBehaviour {

        private HexCoord position;
        public HexCoord Position {
            get {
                return position;
            }
            set { 
                position = value;
                transform.position = plane.HexToWorldPosition(position);
            }
        }
        private HexPlane plane;

        private void Start() {
            plane = GameObject.FindWithTag("Board").GetComponent<HexPlane>();
        }

    }
}
