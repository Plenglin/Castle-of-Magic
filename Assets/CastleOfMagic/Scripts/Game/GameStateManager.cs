using System.Collections;
using System.Collections.Generic;
using CastleMagic.Game.GameInfo.PlayerActions;
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
        private Dictionary<NetworkPlayerController, LinkedList<TurnAction>> playerActionTable;

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
            playerActionTable = new Dictionary<NetworkPlayerController, LinkedList<TurnAction>>();
        }

        void Update() {
            // lol just go fast
            if (isServer) {
                if (requestedEndTurnPlayers.Count >= numPlayers) {
                    RpcEndTurn();
                }
            }
        }

        public void RequestEndTurn(NetworkPlayerController player) {
            requestedEndTurnPlayers.Enqueue(player);
            playerActionTable[player] = player.turnActionsQueued;
        }

        [ClientRpc]
        void RpcEndTurn() {
            turnNumber++;
            while(requestedEndTurnPlayers.Count > 0) {
                NetworkPlayerController player = requestedEndTurnPlayers.Dequeue();
                Debug.Log($"ENDING TURN OF A PLAYer {player}");
                LinkedList<TurnAction> actions = playerActionTable[player];
                while(!(actions.Count == 0)) {
                    actions.Last.Value.ExecuteAction();
                    actions.RemoveLast();
                }
                player.OnTurnEnd();
            }
        }
    }
}