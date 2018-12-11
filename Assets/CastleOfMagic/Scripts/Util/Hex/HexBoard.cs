using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace CastleMagic.Util.Hex {

    /// <summary>
    /// A representation of a hex board. Internally, coordinates in 2D arrays are stored as axial (x,y)
    /// coordinates. Internal arrays are accessed as <code>array[x][y]</code>.
    /// </summary>
    [Serializable]
    public class HexBoard {
    
        public readonly BitArray[] openTiles;

        private readonly Dictionary<HexCoord, HexCoord> wormholes = new Dictionary<HexCoord, HexCoord>();
        private readonly int width, height;

        public HexBoard(int width, int height) {
            this.width = width;
            this.height = height;
            openTiles = new BitArray[width];
            for (int i=0; i < width; i++) {
                openTiles[i] = new BitArray(height, true);
            }
        }
        
        /// <summary>
        /// Perform a BFS on the grid.
        /// </summary>
        /// <param name="start">where to start</param>
        /// <param name="startingEnergy">how much energy to start with</param>
        /// <param name="canPassThrough">are you allowed to pass through this tile?</param>
        /// <returns>tuple of (dudes that you can land on, energy left when you land on it)</returns>
        public IEnumerable<Tuple<HexCoord, int>> PerformBFS(HexCoord start, int startingEnergy, Predicate<HexCoord> canPassThrough) {
            yield return Tuple.Create(start, startingEnergy);
            if (startingEnergy <= 0) {
                yield break;
            }

            var toVisit = new Queue<Tuple<HexCoord, int>>();
            var visited = new HashSet<HexCoord>{start};

            start.GetNeighbors().ForEach(c => toVisit.Enqueue((Tuple.Create(c, startingEnergy - 1))));
            while (toVisit.Count > 0) {
                var pair = toVisit.Dequeue();
                var coord = pair.Item1;
                var energyLeft = pair.Item2;
                if (visited.Contains(coord)) {
                    continue;
                }
                visited.Add(coord);
                if (IsValidPosition(coord) && canPassThrough(coord)) {
                    Debug.Log($"doing thing on {coord}");
                    yield return Tuple.Create(coord, energyLeft);
                    int newEnergy = energyLeft - 1;
                    if (energyLeft != 0) {
                        foreach (var n in coord.GetNeighbors()) {
                            toVisit.Enqueue(Tuple.Create(n, newEnergy));
                        }
                        HexCoord wormholeEndpoint;
                        if (wormholes.TryGetValue(coord, out wormholeEndpoint)) {
                            toVisit.Enqueue(Tuple.Create(wormholeEndpoint, newEnergy));
                        }
                    }
                }
            }

        }

        public IEnumerable<Tuple<HexCoord, int>> PerformBFS(HexCoord position, int energy) {
            foreach (Tuple<HexCoord, int> pair in PerformBFS(position, energy, (_) => true)) {
                yield return pair;
            }
        }

        public bool IsCoordInRange(HexCoord coord) {
            return coord.IsValidCoordinate() && coord.x >= 0 && coord.x < width && coord.y >= 0 && coord.y < height;
        }

        public bool IsValidPosition(HexCoord coord) {
            return IsCoordInRange(coord) && WallExistsAt(coord);
        }

        public bool WallExistsAt(HexCoord coord) {
            return openTiles[coord.x][coord.y];
        }

        public void CreateWormholePair(HexCoord a, HexCoord b) {
            if (wormholes.ContainsKey(a)) {
                throw new ArgumentException($"{a} is already a wormhole!");
            }
            if (wormholes.ContainsKey(b)) {
                throw new ArgumentException($"{b} is already a wormhole!");
            }
            wormholes[a] = b;
            wormholes[b] = a;
        }

        public void DeleteWormholePair(HexCoord a, HexCoord b) {
            wormholes.Remove(a);
            wormholes.Remove(b);
        }

    }
}
