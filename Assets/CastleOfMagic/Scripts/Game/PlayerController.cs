using System;
using CastleMagic.Game.Entites;
using UnityEngine.Networking;

namespace CastleMagic.Game
{
    public class PlayerController : NetworkBehaviour
    {
        public EntityController entity;

        public void Awake() {
        }
    }
}
