using UnityEngine;
using System.Collections;
using UnityEditor;
using CastleMagic.Game.Entites;

namespace CastleMagic.Game.Ability {

    [CreateAssetMenu(fileName = "Ability", menuName = "New Ability")]
    public class BaseAbility : ScriptableObject {

        [Tooltip("What the ability is called")]
        public string displayName = "Ability";

        [Tooltip("How much it costs to cast this ability")]
        public int energy = 0;

        [Tooltip("How many charges you can store")]
        public int maxCharges = 1;

        [Tooltip("How many turns before you get another charge")]
        public int chargeRegen = 1;

        public TargetingMethod targetingMethod;
        public AbilityBehavior castBehavior;
    }

}