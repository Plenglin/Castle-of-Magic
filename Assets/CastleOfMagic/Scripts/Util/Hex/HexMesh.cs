using UnityEngine;
using System.Collections;

namespace CastleMagic.Util.Hex {

    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class HexMesh : MonoBehaviour {

        private Vector3[] vertices;
        private Mesh mesh;

        public int width = 10;
        public int height = 10;

        private int offset;

        private void Awake() {
            GenerateVertices();
        }

        private void GenerateVertices() {
            offset = width + 1;
            vertices = new Vector3[width * height * 4];
            for (int y = 0; y < height / 2 + 1; y++) {
                int y4 = 4 * y;
                int[] indexOffsets = new int[] { offset * y4, offset * (y4 + 1), offset * (y4 + 2), offset * (y4 + 3) };
                float[] xOffsets = new float[] { 0, 0.5f, 0.5f, 1 };
                float[] yOffsets = new float[] { 0, HexCoord.HEX_HEIGHT / 4, HexCoord.HEX_HEIGHT * 3 / 4, HexCoord.HEX_HEIGHT };
                for (int i = 0; i < 4; i++) {
                    for (int x = 0; x <= width; x++) {
                        int index = indexOffsets[i] + x;
                        Debug.Log(index);
                        vertices[index] = new Vector3(
                            y + x + xOffsets[i],
                            1.5f * HexCoord.HEX_HEIGHT * y + yOffsets[i],
                            0f
                        );
                    }
                }
            }
        }

        private void GenerateTriangles() {

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