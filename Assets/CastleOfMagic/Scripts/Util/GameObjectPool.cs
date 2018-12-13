using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

namespace CastleMagic.Util {

    public class GameObjectPool {
        private Stack<GameObject> pool = new Stack<GameObject>();
        private Func<GameObject> factory;
        private int inUse;

        public GameObjectPool(Func<GameObject> factory) {
            this.factory = factory;
        }

        public Stack<GameObject> Acquire(int n) {
            inUse = n;
            while (pool.Count > inUse) {
                UnityEngine.Object.Destroy(pool.Pop());
            }
            while (pool.Count < inUse) {
                pool.Push(factory.Invoke());
            }
            return pool;
        }

        public void Acquire<T>(ICollection<T> items, Action<GameObject, T> f) {
            foreach (var pair in Acquire(items.Count).Zip(items, Tuple.Create)) {
                f(pair.Item1, pair.Item2);
            }
        }

    }

}