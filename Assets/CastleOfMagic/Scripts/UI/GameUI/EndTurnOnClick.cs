using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CastleMagic.UI.GameUI {
    public class EndTurnOnClick : MonoBehaviour {
        private UnityAction action;

        // Use this for initialization
        void Start() {
            action += DoEndTurn;
            GetComponent<Button>().onClick.AddListener(action);
        }

        public void DoEndTurn() {
            // something something end turn
        }
    }
}