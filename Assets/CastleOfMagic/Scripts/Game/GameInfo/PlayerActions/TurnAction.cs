using System;
using UnityEngine.Networking;

namespace CastleMagic.Game.GameInfo.PlayerActions {
    public abstract class TurnAction : NetworkBehaviour {


        public abstract void ExecuteAction();
    }
}
