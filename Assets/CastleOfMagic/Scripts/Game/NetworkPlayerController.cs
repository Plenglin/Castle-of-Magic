using System;
using System.Collections.Generic;
using CastleMagic.Game.Entites;
using CastleMagic.Game.GameInfo.PlayerActions;
using CastleMagic.UI;
using CastleMagic.UI.GameUI;
using CastleMagic.Util.Hex;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace CastleMagic.Game
{
    public class NetworkPlayerController : NetworkBehaviour {

        public EntityController player;
        public List<EntityController> slaves;

        public EntityController selected = null;

        public HexPlane plane;
        public BoardManager boardManager;

        public GameStateManager gm;

        private LinkedList<TurnAction> turnActionsQueued;
        private QueuedActionsDisplay actionsDisplay;

        [SyncVar]
        public bool requestedTurnEnd;

        public void Awake() {
            slaves = new List<EntityController>();
            turnActionsQueued = new LinkedList<TurnAction>();
            requestedTurnEnd = false;
        }

        void Start() {
            var board = GameObject.FindWithTag("Board");
            plane = board.GetComponent<HexPlane>();
            //boardManager = board.GetComponent<BoardManager>();
            boardManager = FindObjectOfType<BoardManager>();
            //gm = GameObject.FindWithTag("GameManager").GetComponent<GameStateManager>();
            //gm = FindObjectOfType<GameStateManager>();
            var prefab = Resources.Load("Prefabs/Entities/EntityPlayer") as GameObject;
            player = Instantiate(prefab).GetComponent<EntityController>();
            Debug.Log(player);

            boardManager.InitializeEntity(player, HexCoord.CreateXY(0, 4));
            slaves.Add(player);

            if (!isLocalPlayer) return;

            GameObject.FindWithTag("SelectionManager").GetComponent<EntitySelectionManager>().player = this;
            actionsDisplay = FindObjectOfType<QueuedActionsDisplay>();
        }

        void Update() {
            
        }

        public void AddTurnAction(TurnAction action) {
            turnActionsQueued.AddLast(action);
            actionsDisplay.AddAction(action);
        }

        public void OnTurnEnd() {
            player.energy = player.maxEnergy;
            requestedTurnEnd = false;
            turnActionsQueued.Clear();
            actionsDisplay.OnNewTurn();
        }

        [Command]
        public void CmdMoveEntity(int target, HexCoord dest) {
            var e = boardManager.GetEntityWithId(target);
            boardManager.MoveEntity(e, dest, true);            
        }

        [Command]
        public void CmdRequestEndTurn() {
            if(gm == null) { // help
                gm = GameObject.FindWithTag("GameManager").GetComponent<GameStateManager>();
            }
            if (!requestedTurnEnd) {
                gm.RequestEndTurn(this);
                requestedTurnEnd = true;
            }
        }

        public override string ToString() {
            return "some player";
        }
    }
}
