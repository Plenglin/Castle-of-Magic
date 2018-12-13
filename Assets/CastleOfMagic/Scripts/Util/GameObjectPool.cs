using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

namespace CastleMagic.Util {

    public class GameObjectPool {
        private Stack<GameObject> pool = null;
        private Func<GameObject> factory;
        private int inUse;

        public GameObjectPool(Func<GameObject> factory) {
            this.factory = factory;
        }

        public void Acquire(int n) {
            inUse = n;
            while (pool.Count > inUse) {
                UnityEngine.Object.Destroy(pool.Pop());
            }
            while (pool.Count < inUse) {
                pool.Push(UnityEngine.Object.Instantiate(factory.Invoke()));
            }
        }

        public void Acquire<T>(ICollection<T> items, Action<GameObject, T> f) {
            Acquire(items.Count);
            foreach (var pair in pool.Zip(items, Tuple.Create)) {
                f(pair.Item1, pair.Item2);
            }
        }

        public IReadOnlyCollection<GameObject> GetObjects() {
            return pool;
        }

    }

}