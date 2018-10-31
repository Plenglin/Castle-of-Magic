using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using CastleMagic.Game;

namespace CastleMagic.UI.GameUI {
    /// <summary>
    /// Tells the server a player has ended their turn.
    /// </summary>
    public class EndTurnOnClick : MonoBehaviour {
        
        private UnityAction action;

        void Start() {
            action += DoEndTurn;
            GetComponent<Button>().onClick.AddListener(action);
            GameObject manager = GameObject.FindWithTag("NetworkGameManager");
        }

        public void DoEndTurn() {
            
        }
    }
}