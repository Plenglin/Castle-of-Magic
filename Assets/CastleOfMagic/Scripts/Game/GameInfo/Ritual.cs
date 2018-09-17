using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace CastleMagic.Game.GameInfo
{
    public class Ritual : NetworkBehaviour
    {
        public readonly string ritualName;

        // Which ritual is needed and the required level threshold to activate ritual.
        public readonly Dictionary<RitualSpell, int> requiredComponents;

        public Ritual()
        {
        }
    }
}
