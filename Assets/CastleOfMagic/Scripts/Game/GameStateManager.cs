using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CastleMagic.Game {

    public class GameStateManager : NetworkBehaviour {

        private NetworkLobbyManager networkManager;
        private BoardManager[] boardManager;
        private PlayerController[] players;

        private int turnNumber;

        // Use this for initialization
        void Start() {
            networkManager = GetComponent<NetworkLobbyManager>();
            boardManager = GetComponents<BoardManager>();

            var playerControllers = GameObject.FindGameObjectsWithTag("Player");
            players = new PlayerController[playerControllers.Length];
            for (int i = 0; i < playerControllers.Length; i++) {
                players[i] = playerControllers[i].GetComponent<PlayerController>();
            }
        }

        // Update is called once per frame
        void Update() {

        }
    }

}