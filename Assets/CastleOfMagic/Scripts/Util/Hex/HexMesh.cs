using UnityEngine;
using System.Collections;

namespace CastleMagic.Util.Hex {

    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class HexMesh : MonoBehaviour {

        private Vector3[] vertices;
        private Mesh mesh;

        public int width = 10;
        public int height = 10;

        private void Awake() {
            vertices = new Vector3[width * height * 8];
            for (int y = 0; y < height; y++) {
                int y4 = 4 * y;
                int[] indexOffsets = new int[] { 2 * width * y4, 2 * width * (y4 + 1), 2 * width * (y4 + 2), 2 * width * (y4 + 3) };
                float[] xOffsets = new float[] { 0, 0.5f, 0.5f, 1 };
                float[] yOffsets = new float[] { 0, HexCoord.HEX_HEIGHT / 4, HexCoord.HEX_HEIGHT * 3 / 4, HexCoord.HEX_HEIGHT};
                for (int i=0; i < 4; i++) {
                    for (int x=0; x < width * 2 - 1; x++) {
                        int index = indexOffsets[i] + x;
                        vertices[index] = new Vector3(
                            y + x + xOffsets[i],
                            1.5f * HexCoord.HEX_HEIGHT * y + yOffsets[i], 
                            0f
                        );
                    }
                }
            }
        }

        // Update is called once per frame
        void Update() {

        }

        private void OnDrawGizmos() {
            if (vertices == null) {
                return;
            }
            foreach (var v in vertices) {
                Gizmos.DrawSphere(v, 0.1f);
            }
        }
    }
}