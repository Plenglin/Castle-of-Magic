using System;
using CastleMagic.Util.Hex;

namespace CastleMagic.Game.GameInfo.PlayerActions
{
    public class ActionMove : TurnAction
    {
        private HexCoord from;
        private HexCoord to;
        private NetworkPlayerController player;

        public ActionMove(NetworkPlayerController player, HexCoord from, HexCoord to) {
            this.from = from;
            this.to = to;
            this.player = player;
        }

        public override void ExecuteAction() {
            player.CmdMoveEntity(player.player.GetInstanceID(), to);
        }
    }
}
