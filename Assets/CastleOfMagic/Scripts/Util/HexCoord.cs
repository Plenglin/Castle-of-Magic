using System;
namespace Application
{
    public class HexCoord
    {
        public readonly int x;
        public readonly int y;
        public readonly int z;

        private HexCoord(int x, int y, int z) 
        {
            this.x = x;
            this.y = y;
            this.z = z;
            
        }

        public static HexCoord CreateXY(int x, int y) {
            return new HexCoord(x, y, -x - y);
        }

        public static HexCoord CreateYZ(int y, int z)
        {
            return new HexCoord(-y - z, y, z);
        }

        public static HexCoord CreateXZ(int x, int z)
        {
            return new HexCoord(x, -x - z, z);
        }
    }
}
