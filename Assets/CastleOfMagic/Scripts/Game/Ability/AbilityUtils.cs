using UnityEngine;
using System.Collections;

namespace CastleMagic.Game.Ability {

    public interface IAbilityListener {

        AbilityTarget GetTargetingScheme();
        bool CanCast();

    }

    public abstract class AbilityTarget {
        public IEnumerator OnBeginTargeting() {
            yield return null;
        }
    }

    /// <summary>
    /// Target a specific tile within a range.
    /// </summary>
    [System.Serializable]
    public class TileTarget : AbilityTarget {
        public int minRadius;
        public int maxRadius;
        public int aoeRadius = 0;
    }

    /// <summary>
    /// Target all tiles along a line beginning at the caster and ending a range away at an angle.
    /// </summary>
    [System.Serializable]
    public class LinearTarget : AbilityTarget {
        public int distance;
        public int entityPenetration = 0;
        public int wallPenetration = 0;
    }

    /// <summary>
    /// Similar to LinearTarget, but it does not begin at the caster
    /// </summary>
    [System.Serializable]
    public class VectorTarget : AbilityTarget {
        public int startMaxDistance;
    }

    /// <summary>
    /// Target a specific entity.
    /// </summary>
    [System.Serializable]
    public class EntityTarget : AbilityTarget {
        public int maxDistance;
    }

    /// <summary>
    /// Target self.
    /// </summary>
    [System.Serializable]
    public class SelfTarget : AbilityTarget {

    }

}
