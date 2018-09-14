using System;
using UnityEngine.Networking;

namespace CastleMagic.Game.GameInfo
{
    public class Ritual : NetworkBehaviour
    {
        public readonly string ritualName;

        // replace with map with actual requirement
        public readonly RitualSpell requiredComponents;

        public Ritual()
        {
        }
    }
}
