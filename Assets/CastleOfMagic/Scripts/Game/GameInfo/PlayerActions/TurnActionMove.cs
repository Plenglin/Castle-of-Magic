using System;
using CastleMagic.Game.Entites;
using CastleMagic.Util.Hex;
using UnityEngine;

namespace CastleMagic.Game.GameInfo.PlayerActions
{
    public class TurnActionMove : TurnAction {
        private HexCoord from;
        private HexCoord to;
        private NetworkPlayerController player;

        public TurnActionMove(NetworkPlayerController player, HexCoord from, HexCoord to) {
            this.from = from;
            this.to = to;
            this.player = player;
        }

        public override bool ExecuteGhostAction() {
            BoardManager bm = player.boardManager;
            if (bm.entitiesByPosition.ContainsKey(to)) {
                return false;
            }
            var shouldMove = false;
            foreach (var pair in bm.board.PerformBFS(from, player.ghostPlayer.energy)) {
                if (pair.Item1.Equals(to)) {
                    player.ghostPlayer.energy = pair.Item2;
                    shouldMove = true;
                    break;
                }
            }
            if (shouldMove) {
                try {
                    player.boardManager.InitializeEntity(player.ghostPlayer, to);
                    player.ghostPlayer.ToggleVisibility(true);
                    return true;
                } catch (ArgumentException) {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// moving entity does the dumb shit again, so just reinitialize entity
        /// </summary>
        public override void UndoGhostAction() {
            player.boardManager.RemoveEntity(player.ghostPlayer);
            player.boardManager.InitializeEntity(player.ghostPlayer, from);
        }

        public override void ExecuteAction() {
            // there might need to be a catch in removeentity if multiple moves are called
            player.boardManager.RemoveEntity(player.ghostPlayer);
            player.ghostPlayer.ToggleVisibility(false);
            player.CmdMoveEntity(player.player.GetInstanceID(), to);
        }

        public override string ActionToString() {
            return $"Move {player} from {from} to {to}";
        }
    }
}
