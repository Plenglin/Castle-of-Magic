using UnityEngine;
using System.Collections;
using CastleMagic.Game.GameInfo.PlayerActions;
using UnityEngine.Events;

namespace CastleMagic.UI.GameUI {

    /// <summary>
    /// This button adds an action to the queue of actions the player will perform on a turn.
    /// Presumabely.
    /// </summary>
    public class TurnActionOnClick : MonoBehaviour {
        
        public TurnAction actionToAdd;
        private UnityAction action;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}