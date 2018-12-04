using System;
using CastleMagic.Util.Hex;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace CastleMagic.Game.Entites {

    /// <summary>
    /// Represents any object that has a position on the board and cannot be passed through.
    /// </summary>
    [ExecuteInEditMode]
    public class HexTransform : MonoBehaviour {

        public UnityAction<PositionDelta> OnPositionChanged;

        private HexCoord position = HexCoord.ZERO;
        public HexCoord Position {
            get {
                return position;
            }
            set {
                HexCoord old = position;
                position = value;
                UpdatePhysicalPosition();
                try {
                    OnPositionChanged.Invoke(new PositionDelta(old, position));
                } catch (NullReferenceException) { }
            }
        }
        private Lazy<HexPlane> lazyPlane = new Lazy<HexPlane>(() =>
        {
            return GameObject.FindWithTag("Board").GetComponent<HexPlane>();
        });

        private void Start() {
        }

        public void UpdatePhysicalPosition() {
            transform.position = lazyPlane.Value.HexToWorldPosition(position);
        }

    }

}
