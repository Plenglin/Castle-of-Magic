using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace CastleMagic.Game.Entites
{
    /// <summary>
    /// Represents the class an entity represents.
    /// Dictates modifiers to the entity's stats, skills, whatever.
    /// Could represent anything. A crate, a mage, a tree, whatever you desire.
    /// </summary>
    public class EntityClass : NetworkBehaviour
    {
        public readonly float healthMultiplier; // random thing

        // maybe classes should actually contain the actual entity as a field
        public void OnCreation(Entity entity) {
            entity.maxHealth = (int)(entity.maxHealth * healthMultiplier);
        }
    }
}