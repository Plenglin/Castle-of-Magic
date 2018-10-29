using System;
using System.Collections.Generic;
using CastleMagic.Game.Entites;
using CastleMagic.UI;
using CastleMagic.Util.Hex;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace CastleMagic.Game
{
    public class PlayerController : NetworkBehaviour {

        public EntityController player;
        public List<EntityController> slaves;

        public EntityController selected = null;

        private HexPlane plane;
        private BoardManager boardManager;

        [SyncVar]
        public bool requestedTurnEnd;

        public void Awake() {
            slaves = new List<EntityController>();
        }

        private void Start() {
            var board = GameObject.FindWithTag("Board");
            plane = board.GetComponent<HexPlane>();
            boardManager = board.GetComponent<BoardManager>();
            var prefab = Resources.Load("Prefabs/Entities/EntityPlayer") as GameObject;
            player = Instantiate(prefab).GetComponent<EntityController>();
            Debug.Log(player);

            boardManager.InitializeEntity(player, HexCoord.CreateXY(0, 4));
            slaves.Add(player);

            if (!isLocalPlayer) return;

            GameObject.FindWithTag("SelectionManager").GetComponent<EntitySelectionManager>().player = this;
        }

        [Command]
        public void CmdMoveEntity(int target, HexCoord dest) {
            var e = boardManager.GetEntityWithId(target);
            boardManager.MoveEntity(e, dest, true);            
        }
    }
}
