using System;
using UnityEngine.Networking;

namespace CastleMagic.Game.GameInfo.PlayerActions {
    public abstract class TurnAction {

        public abstract void ExecuteAction();

        public virtual String ActionToString() {
            return $"Action {this}";
        }
    }
}
