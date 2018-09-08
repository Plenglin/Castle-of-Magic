using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

namespace CastleMagic.Util {

    /// <summary>
    /// A representation of a hex board. Internally, coordinates in 2D arrays are stored as axial (x,y)
    /// coordinates. Internal arrays are accessed as <code>array[x][y]</code>.
    /// </summary>
    public class HexBoard {
    
        private readonly BitArray[] openTiles;
        private readonly Dictionary<HexCoord, HexCoord> wormholes = new Dictionary<HexCoord, HexCoord>();
        private readonly int width, height;

        public HexBoard(int width, int height) {
            this.width = width;
            this.height = height;
            openTiles = new BitArray[width];
            for (int i=0; i < width; i++) {
                openTiles[i] = new BitArray(height);
            }
        }
        
        /// <summary>
        /// Perform a BFS on the grid.
        /// </summary>
        /// <param name="start">where to start</param>
        /// <param name="startingEnergy">how much energy to start with</param>
        /// <param name="canPassThrough">are you allowed to pass through this tile?</param>
        /// <param name="canLandOn">are you allowed to finish on this tile? must imply passability</param>
        /// <returns>dudes that you can land on</returns>
        public IEnumerable<Tuple<HexCoord, int>> PerformBFS(HexCoord start, int startingEnergy, Predicate<HexCoord> canPassThrough, Predicate<HexCoord> canLandOn) {
            var toVisit = new Queue<Tuple<HexCoord, int>>();
            var visited = new HashSet<HexCoord>();
            toVisit.Enqueue(Tuple.Create(start, startingEnergy));

            while (toVisit.Count > 0) {
                var pair = toVisit.Dequeue();
                var coord = pair.Item1;
                var energyLeft = pair.Item2;
                visited.Add(coord);
                if (energyLeft < 0) {
                    continue;
                }
                if (canPassThrough(coord)) {
                    if (canLandOn(coord)) {
                        yield return Tuple.Create(coord, energyLeft);
                    }
                    int newEnergy = energyLeft - 1;
                    foreach (var n in coord.GetNeighbors().Where(c => !visited.Contains(c))) {
                        toVisit.Enqueue(Tuple.Create(n, newEnergy));
                    }
                    HexCoord wormholeEndpoint;
                    if (wormholes.TryGetValue(coord, out wormholeEndpoint)) {
                        toVisit.Enqueue(Tuple.Create(wormholeEndpoint, newEnergy));
                    }
                }
            }

        }

        public void CreateWormholePair(HexCoord a, HexCoord b) {
            wormholes[a] = b;
            wormholes[b] = a;
        }

        public void DeleteWormholePair(HexCoord a, HexCoord b) {
            wormholes.Remove(a);
            wormholes.Remove(b);
        }

    }
}
