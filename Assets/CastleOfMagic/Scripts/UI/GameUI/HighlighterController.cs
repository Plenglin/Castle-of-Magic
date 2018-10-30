using CastleMagic.Game.Entites;
using CastleMagic.Util.Hex;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CastleMagic.UI.GameUI {

    [RequireComponent(typeof(HexTransform))]
    public class HighlighterController : MonoBehaviour {

        public HexCoord destination;

        // Use this for initialization
        void Start() {
            var trs = GetComponent<HexTransform>();
            trs.Position = destination;
        }

        // Update is called once per frame
        void Update() {

        }
    }
}