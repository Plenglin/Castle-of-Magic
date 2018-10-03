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
            if (entity == null || entitiesByPosition[to] != null) {
                return false;
            }
            entitiesByPosition.Remove(from);
            entitiesByPosition[to] = entity;
            entity.HexTransform.Position = to;
            return true;
        }

        public bool MoveEntity(EntityController entity, HexCoord to) {
            return MoveEntity(entity.HexTransform.Position, to);
        }

        public void InitializeEntity(EntityController entity, HexCoord pos) {
            if (entitiesByPosition.ContainsKey(pos)) {
                throw new ArgumentException($"Can't initialize entity in already occupied position ${pos}!");
            }
            entitiesById[entity.GetInstanceID()] = entity;
            entitiesByPosition[pos] = entity;
            entity.HexTransform.Position = pos;
            OnEntityCreated.Invoke(entity);
        }

        public void RemoveEntity(int id) {
            EntityController entity = entitiesById[id];
            entitiesById.Remove(id);
            entitiesByPosition.Remove(entity.HexTransform.Position);
            OnEntityDestroyed.Invoke(entity);
        }

    }

}
