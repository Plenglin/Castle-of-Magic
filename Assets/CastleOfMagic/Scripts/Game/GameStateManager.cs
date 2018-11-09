﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CastleMagic.Game {

    public class GameStateManager : NetworkBehaviour {

        private NetworkLobbyManager networkManager;
        private BoardManager[] boardManager;
        //private PlayerController[] players;
        private int numPlayers;
        private HashSet<NetworkPlayerController> players;

        private Queue<NetworkPlayerController> requestedEndTurnPlayers;

        private int turnNumber;

        void Start() {
            networkManager = GetComponent<NetworkLobbyManager>();
            boardManager = GetComponents<BoardManager>();

            GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
            numPlayers = objects.Length;
            players = new HashSet<NetworkPlayerController>();
            foreach (GameObject go in objects) {
                players.Add(go.GetComponent<NetworkPlayerController>());
            }
            requestedEndTurnPlayers = new Queue<NetworkPlayerController>();
        }

        void Update() {
            // lol just go fast
            if (isServer) {
                if (requestedEndTurnPlayers.Count >= numPlayers) {
                    RpcEndTurn();
                    requestedEndTurnPlayers.Clear();
                }
            }
        }

        public void RequestEndTurn(NetworkPlayerController player) {
            requestedEndTurnPlayers.Enqueue(player);
        }

        [ClientRpc]
        void RpcEndTurn() {
            turnNumber++;
            while(requestedEndTurnPlayers.Count > 0) {
                requestedEndTurnPlayers.Dequeue().OnTurnEnd();
            }
        }
    }

}