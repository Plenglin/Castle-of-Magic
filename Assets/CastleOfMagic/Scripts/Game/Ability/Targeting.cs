using UnityEngine;
using System.Collections;
using CastleMagic.Util.Hex;
using System;
using CastleMagic.Game.Entites;
using System.Collections.Generic;

namespace CastleMagic.Game.Ability {

    public abstract class TargetingMethod : ScriptableObject {

        /// <summary>
        /// Called when the casting begins.
        /// </summary>
        /// <param name="caster">the entity performing the cast</param>
        /// <returns>a data object to persist through the cast in order to store data</returns>
        public abstract TargetingData OnTargetingBegin(EntityController caster);

    }

    [CreateAssetMenu(fileName = "PointTargeting", menuName = "Targeting Method/Point")]
    public class PointTarget : TargetingMethod {
        public int maxRadius;
        public int aoeRadius = 0;

        public override TargetingData OnTargetingBegin(EntityController caster) {
            throw new NotImplementedException();
        }

        public class Data : TargetingData {
            public HashSet<HexCoord> GetSelectedTiles() {
                throw new NotImplementedException();
            }

            public void OnClick(HexCoord pos) {
                throw new NotImplementedException();
            }

            public HashSet<HexShading> OnHover(EntityController caster, Vector2 r) {
                throw new NotImplementedException();
            }

            public bool ShouldCast() {
                throw new NotImplementedException();
            }
        }
    }

    [CreateAssetMenu(fileName = "LinearTargeting", menuName = "Targeting Method/Linear")]
    public class LinearTarget : TargetingMethod {
        public int distance;
        public int entityPenetration = 0;
        public bool penetratesWalls = false;

        public override TargetingData OnTargetingBegin(EntityController caster) {
            return new Data(this, caster);
        }

        public class Data : TargetingData {

            public LinearTarget parent;
            public EntityController caster;
            public HexCoord delta;

            private bool isFinished = false;
            private BoardManager boardManager;

            public Data(LinearTarget parent, EntityController caster) {
                this.parent = parent;
                this.caster = caster;

                boardManager = FindObjectOfType<BoardManager>();
            }

            public HashSet<HexCoord> GetSelectedTiles() {
                var coords = new HashSet<HexCoord>();
                int penetrationLeft = parent.entityPenetration;
                var pos = caster.HexTransform.Position + delta;
                for (int i=0; i < parent.distance && penetrationLeft != 0; i++) {
                    pos += delta;
                    if ((parent.penetratesWalls && boardManager.board.WallExistsAt(pos)) || !boardManager.board.IsValidPosition(pos)) {
                        break;
                    }
                    if (boardManager.GetEntityAtPosition(pos) != null) {
                        penetrationLeft--;
                    }
                }
                return coords;
            }

            public void OnClick(HexCoord pos) {
                isFinished = true;
            }

            public HashSet<HexShading> OnHover(EntityController caster, Vector2 r) {
                return new HashSet<HexShading>();
            }

            public bool ShouldCast() {
                return isFinished;
            }
        }
    }

    [CreateAssetMenu(fileName = "VectorTargeting", menuName = "Targeting Method/Vector")]
    public class VectorTarget : TargetingMethod {
        public int distance;
        public int entityPenetration = 0;
        public bool penetratesWalls = false;

        public override TargetingData OnTargetingBegin(EntityController caster) {
            throw new NotImplementedException();
        }
    }

    public interface TargetingData {
        /// <summary>
        /// Called when the user clicks on a tile.
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="data"></param>
        void OnClick(HexCoord pos);

        /// <summary>
        /// Should we end now and cast the spell? Called after initialization and once every time the user
        /// clicks on a tile.
        /// </summary>
        /// <returns>whether or not we should cast</returns>
        bool ShouldCast();

        /// <summary>
        /// Called when the user hovers over something.
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="r">Hovered-over physical coordinate relative to player physical position</param>
        /// <returns>a set of hex shadings</returns>
        HashSet<HexShading> OnHover(EntityController caster, Vector2 r);

        /// <summary>
        /// Can be called by <see cref="AbilityBehavior"/>s to get all the affected tiles after the cast is finished.
        /// </summary>
        /// <returns>a set of hex tiles affected by this targeting</returns>
        HashSet<HexCoord> GetSelectedTiles();
    }

    public struct HexShading {
        HexCoord position;
        Color color;
    }

    /*[System.Serializable]
    public interface IAbilityTargetingFactory {

    }*/

    /*
    public abstract class AbilityTargetData : MonoBehaviour {
        public abstract void OnBeginTargeting();
    }

    /// <summary>
    /// Target a specific tile within a range.
    /// </summary>
    [System.Serializable]
    public class PointTarget : AbilityTargetData {
        public int minRadius;
        public int maxRadius;
        public int aoeRadius = 0;

        public override void OnBeginTargeting() {
            yield return null;
        }
    }

    /// <summary>
    /// Target all tiles along a line beginning at the caster and ending a range away at an angle.
    /// </summary>
    [System.Serializable]
    public class LinearTarget : AbilityTargetData {
        public int distance;
        public int entityPenetration = 0;
        public int wallPenetration = 0;
    }

    /// <summary>
    /// Similar to LinearTarget, but it does not begin at the caster
    /// </summary>
    [System.Serializable]
    public class VectorTarget : AbilityTargetData {
        public int startMaxDistance;
    }

    /// <summary>
    /// Target a specific entity.
    /// </summary>
    [System.Serializable]
    public class EntityTarget : AbilityTargetData {
        public int maxDistance;
    }

    /// <summary>
    /// Target self.
    /// </summary>
    [System.Serializable]
    public class SelfTarget : AbilityTargetData {

    }*/

}
