using UnityEngine;
using System.Collections;
using System.ComponentModel;

namespace CastleMagic.Game.Entities {

    public class GhostController : MonoBehaviour {

        [SerializeField]
        [ReadOnly(true)]
        private EntityController parent;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }

}