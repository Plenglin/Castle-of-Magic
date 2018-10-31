using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CastleMagic.Game {

    public class GameStateManager : NetworkBehaviour {

        private NetworkLobbyManager networkManager;
        private BoardManager[] boardManager;

        private int turnNumber;

        void Start() {
            networkManager = GetComponent<NetworkLobbyManager>();
            boardManager = GetComponents<BoardManager>();
        }

        void Update() {

        }

        [Command]
        public void CmdRequestEndTurn() {
            
        }
    }

}