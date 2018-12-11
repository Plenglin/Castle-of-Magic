using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Collections;

namespace CastleMagic.Util {

    public class GameObjectPool {
        private Stack<GameObject> pool = null;
        private Func<GameObject> factory;
        private int inUse;

        public GameObjectPool(Func<GameObject> factory) {
            this.factory = factory;
        }

        public IEnumerator<GameObject> Acquire(int n) {
            inUse = n;
            while (pool.Count > inUse) {
                UnityEngine.Object.Destroy(pool.Pop());
            }
            while (pool.Count < inUse) {
                UnityEngine.Object.Instantiate(factory.Invoke());
            }
            return pool.GetEnumerator();
        }
        
    }

}