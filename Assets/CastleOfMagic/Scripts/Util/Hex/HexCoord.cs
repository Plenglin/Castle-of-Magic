using System;
using System.Collections.Generic;

namespace CastleMagic.Util.Hex {

    public class HexCoord {

        public static readonly HexCoord ZERO = new HexCoord(0, 0, 0);
        public static readonly HexCoord RIGHT = new HexCoord(0, 1, -1);
        public static readonly HexCoord LEFT = new HexCoord(0, -1, 1);
        public static readonly HexCoord UP_L = new HexCoord(-1, 0, 1);
        public static readonly HexCoord DOWN_R = new HexCoord(1, 0, -1);
        public static readonly HexCoord UP_R = new HexCoord(-1, 1, 0);
        public static readonly HexCoord DOWN_L = new HexCoord(1, -1, 0);

        public static readonly float HEX_WIDTH = 1f;
        public static readonly float HEX_HEIGHT = 1.154700538f;

        public readonly int x;
        public readonly int y;
        public readonly int z;

        public static readonly HexCoord[] DIRECTIONS = {RIGHT, DOWN_R, DOWN_L, LEFT, UP_L, UP_R};

        public int CubeDistance {
            get {
                return (Math.Abs(x) + Math.Abs(y) + Math.Abs(z)) / 2;
            }
        }

        private HexCoord(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public bool IsValidCoordinate() {
            return x + y + z == 0;
        }

        public static HexCoord operator +(HexCoord a, HexCoord b) {
            return new HexCoord(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static HexCoord operator -(HexCoord a, HexCoord b) {
            return new HexCoord(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static HexCoord operator *(int a, HexCoord b) {
            return new HexCoord(b.x * a, b.y * a, b.z * a);
        }

        public static HexCoord operator *(HexCoord b, int a) {
            return new HexCoord(b.x * a, b.y * a, b.z * a);
        }

        public int DistanceFrom(HexCoord other) {
            return (Math.Abs(x - other.x) + Math.Abs(y - other.y) + Math.Abs(z - other.z)) / 2;
        }

        public IEnumerable<HexCoord> GetSurrounding(int radius) {
            for (int dx=-radius; dx <= radius; dx++) {
                for (int dy=Math.Max(-radius, -dx - radius); dy <= Math.Min(radius, -dx + radius); dy++) {
                    yield return CreateXY(x + dx, y + dy);
                }
            }
        }

        public List<HexCoord> GetNeighbors() {
            var coords = new List<HexCoord>();
            foreach (var d in DIRECTIONS) {
                coords.Add(this + d);
            }
            return coords;
        }

        public override String ToString() {
            return $"<Hex {x},{y},{z}>";
        }

        public static HexCoord CreateXY(int x, int y) {
            return new HexCoord(x, y, -x - y);
        }

        public static HexCoord CreateYZ(int y, int z) {
            return new HexCoord(-y - z, y, z);
        }

        public static HexCoord CreateXZ(int x, int z) {
            return new HexCoord(x, -x - z, z);
        }

        public static HexCoord CreateRoundedXYZ(float x, float y, float z) {
            var rx = (int)x;
            var ry = (int)y;
            var rz = (int)z;

            var dx = Math.Abs(rx - x);
            var dy = Math.Abs(ry - y);
            var dz = Math.Abs(rz - z);

            if (dx > dy && dx > dz) {
                rx = -ry - rz;
            } else if (dy > dz) {
                ry = -rx - rz;
            } else {
                rz = -rx - ry;
            }
            return new HexCoord(rx, ry, rz);
        }

    }
}