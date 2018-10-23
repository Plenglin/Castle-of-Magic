using System;
using UnityEngine.Networking;

namespace CastleMagic.Game.GameInfo.PlayerAcrtions {
    public abstract class TurnAction : NetworkBehaviour {


        public abstract void ExecuteAction();
    }
}
