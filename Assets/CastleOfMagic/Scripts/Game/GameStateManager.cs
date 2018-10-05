using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CastleMagic.Game {

    public class GameStateManager : NetworkBehaviour {

        private NetworkLobbyManager networkManager;
        private BoardManager[] boardManager;

        // Use this for initialization
        void Start() {
            networkManager = GetComponent<NetworkLobbyManager>();
            boardManager = GetComponents<BoardManager>();
        }

        // Update is called once per frame
        void Update() {

        }
    }

}