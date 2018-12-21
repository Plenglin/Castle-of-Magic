using CastleMagic.Game.Entities;
using CastleMagic.Util.Hex;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace CastleMagic.Game {

    public class BoardManager : MonoBehaviour {
        public HexBoard board;
        public Dictionary<HexCoord, EntityController> entitiesByPosition = new Dictionary<HexCoord, EntityController>();
        public Dictionary<int, EntityController> entitiesById = new Dictionary<int, EntityController>();

        public UnityAction<EntityController> OnEntityCreated;
        public UnityAction<EntityController> OnEntityDestroyed;

        private void Start() {
            board = new HexBoard("board1.txt");
            InitializeWalls();
        }

        public bool MoveEntity(HexCoord from, HexCoord to, bool consumeEnergy = false) {
            EntityController entity = entitiesByPosition[from];
            if (entity == null || entitiesByPosition.ContainsKey(to)) {
                return false;
            }
            var shouldMove = false;
            foreach (var pair in board.PerformBFS(from, entity.energy)) {
                if (pair.Item1.Equals(to)) {
                    if (consumeEnergy) {
                        entity.energy = pair.Item2;
                    }
                    shouldMove = true;
                    break;
                }
            }
            if (shouldMove) {
                entitiesByPosition.Remove(from);
                entitiesByPosition[to] = entity;
                entity.HexTransform.Position = to;
                entity.OnMoved.Invoke(from, to);
            }
            return true;
        }

        public bool MoveEntity(int target, HexCoord dest, bool consumeEnergy = false)
        {
            var entity = GetEntityWithId(target);
            if (entity == null)
            {
                Debug.LogError($"There are no entities with id {target}!");
                return false;
            }
            return MoveEntity(entity, dest, consumeEnergy);
        }

        public bool MoveEntity(EntityController entity, HexCoord to, bool consumeEnergy = false) {
            return MoveEntity(entity.HexTransform.Position, to, consumeEnergy);
        }

        public void InitializeEntity(EntityController entity, HexCoord pos) {
            Debug.Log("intiializing " + entity + " at " + pos);
            if (IsPositionOccupied(pos)) {
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

        public void RemoveEntity(EntityController entity) {
            RemoveEntity(entity.GetInstanceID());
        }

        public void RemoveEntity(int id) {
            EntityController entity = entitiesById[id];
            entitiesById.Remove(id);
            entitiesByPosition.Remove(entity.HexTransform.Position);
            if (OnEntityDestroyed != null) {
                OnEntityDestroyed.Invoke(entity);
            }
        }

        public bool IsPositionOccupied(HexCoord pos) {
            return entitiesByPosition.ContainsKey(pos);
        }

        public void InitializeWalls() {
            var prefab = Resources.Load("Prefabs/LARGEWALL") as GameObject;
            for(int i = 0; i < board.openTiles.Length; i++) {
                for (int j = 0; j < board.openTiles[i].Length; j++) {
                    if(!board.openTiles[i][j]) {
                        GameObject obj = Instantiate(prefab);
                        obj.GetComponent<HexTransform>().Position = HexCoord.CreateXY(i, j);
                    }
                }
            }
        }
    }

}
