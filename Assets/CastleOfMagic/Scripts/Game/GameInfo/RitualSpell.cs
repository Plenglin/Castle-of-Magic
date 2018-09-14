using System;
using UnityEngine.Networking;

namespace CastleMagic.Game.GameInfo
{
    /// <summary>
    /// Represents spell requirements for a given ritual.
    /// </summary>
    public class RitualSpell : NetworkBehaviour
    {
        public readonly string spellName;
        public int progress;

        public readonly string minStateName;
        public int minProgress;
        public readonly string maxStateName;
        public int maxProgress;
    }
}
