using CastleMagic.Game.Entites;
using CastleMagic.Util.Hex;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace CastleMagic.Game {

    public class BoardManager : MonoBehaviour {
        private HexBoard board;
        private Dictionary<HexCoord, EntityController> entitiesByPosition = new Dictionary<HexCoord, EntityController>();
        private Dictionary<int, EntityController> entitiesById = new Dictionary<int, EntityController>();

        public UnityAction<EntityController> OnEntityCreated;
        public UnityAction<EntityController> OnEntityDestroyed;

        public bool MoveEntity(HexCoord from, HexCoord to) {
            EntityController entity = entitiesByPosition[from];
            if (entity == null || entitiesByPosition.ContainsKey(to)) {
                return false;
            }
            entitiesByPosition.Remove(from);
            entitiesByPosition[to] = entity;
            entity.HexTransform.Position = to;
            entity.OnMoved.Invoke(from, to);
            return true;
        }

        public bool MoveEntity(EntityController entity, HexCoord to) {
            return MoveEntity(entity.HexTransform.Position, to);
        }

        public void InitializeEntity(EntityController entity, HexCoord pos) {
            Debug.Log("intiializing " + entity + " at " + pos);
            if (entitiesByPosition.ContainsKey(pos)) {
                throw new ArgumentException($"Can't initialize entity in already occupied position ${pos}!");
            }
            entitiesById[entity.GetInstanceID()] = entity;
            entitiesByPosition[pos] = entity;
            entity.HexTransform.Position = pos;
            if (OnEntityCreated != null) {
                OnEntityCreated.Invoke(entity);
            }
            if (entity.OnMoved != null) {
                entity.OnMoved.Invoke(entity.HexTransform.Position, pos);
            }
        }

        public EntityController GetEntityAtPosition(HexCoord? pos) {
            if (pos == null) {
                return null;
            }
            EntityController entity;
            return entitiesByPosition.TryGetValue((HexCoord)pos, out entity) ? entity : null;
        }

        public EntityController GetEntityWithId(int id) {
            EntityController entity;
            return entitiesById.TryGetValue(id, out entity) ? entity : null;
        }

        public void RemoveEntity(int id) {
            EntityController entity = entitiesById[id];
            entitiesById.Remove(id);
            entitiesByPosition.Remove(entity.HexTransform.Position);
            OnEntityDestroyed.Invoke(entity);
        }

        public void MoveEntity(int target, HexCoord dest) {
            var entity = GetEntityWithId(target);
            if (entity == null) {
                Debug.LogError($"There are no entities with id {target}!");
                return;
            }
            MoveEntity(entity, dest);
        }
    }

}
