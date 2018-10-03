using CastleMagic.Game.Entites;
using CastleMagic.Util.Hex;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CastleMagic.Game {

    public class BoardManager : MonoBehaviour {
        private HexBoard board;
        private Dictionary<HexCoord, EntityController> entities = new Dictionary<HexCoord, EntityController>();

        public bool MoveEntity(HexCoord from, HexCoord to) {
            EntityController entity = entities[from];
            if (entity == null || entities[to] != null) {
                return false;
            }
            entities.Remove(from);
            entities[to] = entity;
            entity.HexTransform.Position = to;
            return true;
        }

        public bool MoveEntity(EntityController entity, HexCoord to) {
            return MoveEntity(entity.HexTransform.Position, to);
        }

        public void InitializeEntityPosition(EntityController entity, HexCoord pos) {
            if (entities.ContainsKey(pos)) {
                throw new ArgumentException($"Can't initialize entity in already occupied position ${pos}!");
            }

            entities[pos] = entity;
            entity.HexTransform.Position = pos;
        }
    }

}
