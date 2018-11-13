using UnityEngine;
using System.Collections;

namespace CastleMagic.Game.Ability {

    [CreateAssetMenu(fileName = "Ability", menuName = "Castle of Magic/New Ability", order = 1)]
    public class BaseAbility : ScriptableObject {
        public string displayName = "Ability";
        public int energy = 0;
        public int regeneration = 1;
        public int charges = 1;
    }

}