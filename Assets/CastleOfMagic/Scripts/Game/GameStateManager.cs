using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CastleMagic.Game {

    public class GameStateManager : NetworkBehaviour {

        private NetworkLobbyManager networkManager;
        private BoardManager[] boardManager;
        //private PlayerController[] players;
        private HashSet<PlayerController> players;

        private int turnNumber;

        void Start() {
            networkManager = GetComponent<NetworkLobbyManager>();
            boardManager = GetComponents<BoardManager>();

            GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
            players = new HashSet<PlayerController>();
            foreach (GameObject go in objects) {
                players.Add(go.GetComponent<PlayerController>());
            }
        }

        void Update() {

        }

        [Command]
        public void CmdRequestEndTurn() {
            
        }
    }

}