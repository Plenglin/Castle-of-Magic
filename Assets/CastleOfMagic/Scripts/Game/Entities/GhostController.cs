using UnityEngine;
using System.Collections;
using System.ComponentModel;
using CastleMagic.Game.Selection;

namespace CastleMagic.Game.Entities {

    [RequireComponent(typeof(HexTransform), typeof(Selectable))]
    public class GhostController : MonoBehaviour {

        [SerializeField]
        [ReadOnly(true)]
        public EntityController parent;

        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }

}