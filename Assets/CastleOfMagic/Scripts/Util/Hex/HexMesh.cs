using UnityEngine;
using System.Collections;

namespace CastleMagic.Util.Hex {

    [RequireComponent(typeof(MeshCollider))]
    public class HexMesh : MonoBehaviour {

        private Vector3[] vertices;
        private int[] triangles;
        private Mesh mesh;

        public int width = 10;
        public int height = 10;

        private int vertRowOffset;

        private void Awake() {
            GenerateVertices();
            GenerateTriangles();

            mesh = new Mesh {
                name = "Procedural Hex Grid",
                vertices = vertices,
                triangles = triangles
            };
            GetComponent<MeshCollider>().sharedMesh = mesh;
            GetComponent<MeshFilter>().mesh = mesh;
        }

        private void GenerateVertices() {
            vertRowOffset = width + 1;
            vertices = new Vector3[width * height * 4];
            for (int y = 0; y < height / 2 + 1; y++) {
                int y4 = 4 * y;
                int[] indexOffsets = new int[] { vertRowOffset * y4, vertRowOffset * (y4 + 1), vertRowOffset * (y4 + 2), vertRowOffset * (y4 + 3) };
                float[] xOffsets = new float[] { 0, 0.5f, 0.5f, 1 };
                float[] yOffsets = new float[] { 0, HexCoord.HEX_HEIGHT / 4, HexCoord.HEX_HEIGHT * 3 / 4, HexCoord.HEX_HEIGHT };
                for (int i = 0; i < 4; i++) {
                    for (int x = 0; x <= width; x++) {
                        int index = indexOffsets[i] + x;
                        vertices[index] = new Vector3(
                            y + x + xOffsets[i],
                            0f,
                            1.5f * HexCoord.HEX_HEIGHT * y + yOffsets[i]
                        );
                    }
                }
            }
        }

        private void GenerateTriangles() {
            triangles = new int[12 * width * height];
            for (int y=0; y < height; y++) {
                int rowOffset = y * width;
                for (int x=0; x < width; x++) {
                    int triIndexOffset = 12 * (rowOffset + x);
                    int yOffset0 = (2 * y) * vertRowOffset;
                    int yOffset1 = (2 * y + 1) * vertRowOffset;
                    int yOffset2 = (2 * y + 2) * vertRowOffset;
                    int yOffset3 = (2 * y + 3) * vertRowOffset;

                    triangles[triIndexOffset + 0] = yOffset1 + x + 1;
                    triangles[triIndexOffset + 1] = yOffset0 + x + 1;
                    triangles[triIndexOffset + 2] = yOffset1 + x;

                    triangles[triIndexOffset + 3] = yOffset2 + x;
                    triangles[triIndexOffset + 4] = yOffset1 + x + 1;
                    triangles[triIndexOffset + 5] = yOffset1 + x;

                    triangles[triIndexOffset + 6] = yOffset1 + x + 1;
                    triangles[triIndexOffset + 7] = yOffset2 + x;
                    triangles[triIndexOffset + 8] = yOffset2 + x + 1;

                    triangles[triIndexOffset + 9] = yOffset2 + x;
                    triangles[triIndexOffset + 10] = yOffset3 + x;
                    triangles[triIndexOffset + 11] = yOffset2 + x + 1;

                }
            }
        }

        public HexCoord TriangleToHex(int triangleIndex) {
            var hexIndex = triangleIndex >> 2;
            var y = hexIndex / width;
            var x = hexIndex % width;
            return HexCoord.CreateXY(x, y);
        }

    }
}