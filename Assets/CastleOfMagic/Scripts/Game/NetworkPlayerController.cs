using System;
using System.Collections.Generic;
using CastleMagic.Game.Entities;
using CastleMagic.Game.GameInfo.PlayerActions;
using CastleMagic.Game.Selection;
using CastleMagic.UI;
using CastleMagic.UI.GameUI;
using CastleMagic.Util.Hex;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace CastleMagic.Game {
    public class NetworkPlayerController : NetworkBehaviour {

        public EntityController player;
        public EntityController ghostPlayer;
        public List<EntityController> slaves;

        public EntityController selected = null;

        public HexPlane plane;
        public BoardManager boardManager;

        private Lazy<GameStateManager> lazyGM = new Lazy<GameStateManager>(() => {
            return GameObject.FindWithTag("GameManager").GetComponent<GameStateManager>();
        });

        public LinkedList<TurnAction> turnActionsQueued;
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
            var ghostPrefab = Resources.Load("Prefabs/Entities/GhostEntityPlayer") as GameObject;
            player = Instantiate(prefab).GetComponent<EntityController>();
            Debug.Log(player);

            boardManager.InitializeEntity(player, HexCoord.CreateXY(0, 4));
            slaves.Add(player);

            if (!isLocalPlayer) return;

            FindObjectOfType<SelectionManager>().player = this;
            actionsDisplay = FindObjectOfType<QueuedActionsDisplay>();

            ghostPlayer = Instantiate(ghostPrefab).GetComponent<EntityController>();
            ghostPlayer.ToggleVisibility(false);
            ghostPlayer.energy = player.energy;
            ghostPlayer.HexTransform.Position = player.HexTransform.Position;
            Debug.Log(ghostPlayer);
        }

        void Update() {
            
        }

        public void AddTurnAction(TurnAction action) {
            if (action.ExecuteGhostAction()) {
                turnActionsQueued.AddLast(action);
                actionsDisplay.AddAction(action);
            } else {
                action.UndoGhostAction();
            }
        }

        public void OnTurnEnd() {
            player.energy = player.maxEnergy;
            ghostPlayer.energy = player.energy;
            ghostPlayer.HexTransform.Position = player.HexTransform.Position;
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
            if (!requestedTurnEnd) {
                lazyGM.Value.RequestEndTurn(this);
                requestedTurnEnd = true;
            }
        }

        public bool IsControlling(EntityController entity) {
            return slaves.Contains(entity);
        }

        public override string ToString() {
            return "Player " + this.GetHashCode();
        }
    }
}
