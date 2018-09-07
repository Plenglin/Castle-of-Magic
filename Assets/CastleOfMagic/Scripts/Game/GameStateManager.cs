using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CastleMagic.Game {

    public class GameStateManager : NetworkBehaviour {

        private NetworkLobbyManager networkManager;

        // Use this for initialization
        void Start() {
            networkManager = GetComponent<NetworkLobbyManager>();
        }

        // Update is called once per frame
        void Update() {

        }
    }

}