using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using CastleMagic.Game;
using UnityEngine.Networking;

namespace CastleMagic.UI.GameUI {
    /// <summary>
    /// Tells the server a player has ended their turn.
    /// </summary>
    public class EndTurnOnClick : NetworkBehaviour {
        
        private UnityAction action;
        private GameStateManager gm;
        private NetworkPlayerController localPlayer;

        void Start() {
            action += DoEndTurn;
            GetComponent<Button>().onClick.AddListener(action);
            //GameObject obj = GameObject.FindWithTag("GameManager");
            gm = FindObjectOfType<GameStateManager>();
            //gm = obj.GetComponent<GameStateManager>();

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject p in players) {
                if(p.GetComponent<NetworkIdentity>().isLocalPlayer) {
                    localPlayer = p.GetComponent<NetworkPlayerController>();
                    break;
                }
            }
        }

        public void DoEndTurn() {
            Debug.Log("tried to end turn thing");
            Debug.Log(localPlayer);
            if (!localPlayer.requestedTurnEnd) {
                localPlayer.CmdRequestEndTurn();
            }
        }
    }
}