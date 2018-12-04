using UnityEngine;
using System.Collections;
using CastleMagic.Game.Entites;

namespace CastleMagic.Game.Ability {

    public abstract class AbilityBehavior : ScriptableObject {
        public abstract void OnSpellCast(EntityController caster, TargetingData targeting);
    }

    [CreateAssetMenu(fileName = "ApplyDamage", menuName = "Spell Behavior/Apply Damage")]
    public class ApplyDamage : AbilityBehavior {

        public int damage;

        public override void OnSpellCast(EntityController caster, TargetingData targeting) {
            var boardManager = FindObjectOfType<BoardManager>();
            foreach (var coord in targeting.GetSelectedTiles()) {
                var target = boardManager.GetEntityAtPosition(coord);
                if (target != null) {
                    target.TakeDamage(damage);
                }
            }
        }
    }

}