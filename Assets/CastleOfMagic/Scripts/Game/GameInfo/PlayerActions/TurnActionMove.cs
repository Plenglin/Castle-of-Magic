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
            foreach (var pair in bm.board.PerformBFS(from, player.ghostPlayer.energy, (x) => !bm.IsPositionOccupied(x))) {
                if (pair.Item1.Equals(to)) {
                    player.ghostPlayer.energy = pair.Item2;
                    shouldMove = true;
                    break;
                }
            }
            if (shouldMove) {
                if (bm.IsPositionOccupied(to)) {
                    return false;
                }
                player.ghostPlayer.HexTransform.Position = to;
                player.ghostPlayer.ToggleVisibility(true);
                return true;
            }
            return false;
        }

        public override void UndoGhostAction() {
            player.ghostPlayer.HexTransform.Position = from;
            if(player.ghostPlayer.HexTransform == player.player.HexTransform) {
                player.ghostPlayer.ToggleVisibility(false);
            }
        }

        public override void ExecuteAction() {
            player.ghostPlayer.ToggleVisibility(false);
            player.CmdMoveEntity(player.player.GetInstanceID(), to);
        }

        public override string ActionToString() {
            return $"Move {player} from {from} to {to}";
        }
    }
}
