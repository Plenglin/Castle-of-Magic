using UnityEngine;
using System.Collections;
using System;

namespace CastleMagic.Util.Hex {
    public class HexPlane : MonoBehaviour {

        private Plane plane = new Plane(Vector3.down, 0);
        public float scale = 1.0f;

        public bool drawHexPoints = true;

        public HexCoord? RaycastToHex(Ray ray) {
            var relativeRay = new Ray(transform.InverseTransformPoint(ray.origin), transform.InverseTransformDirection(ray.direction));
            Debug.DrawRay(ray.origin, 10 * ray.direction, Color.white);
            //Debug.DrawRay(relativeRay.origin, 10 * relativeRay.direction, Color.black);
            float dist;
            plane.Raycast(relativeRay, out dist);
            if (dist < 0) {
                return null;
            }
            var hit = ray.GetPoint(dist);
            return PlanePositionToHex(hit.x, hit.z);
        }

        public bool RaycastToHex(Ray ray, out HexCoord? hit) {
            hit = RaycastToHex(ray);
            return hit != null;
        }

        public Vector3 HexToWorldPosition(HexCoord pos) {
            return transform.TransformPoint(HexToPlanePosition(pos));
        }

        public HexCoord PlanePositionToHex(float x, float y) {
            var tx = y * -2 / Constants.SQRT3;
            var ty = x + y / Constants.SQRT3;
            return HexCoord.CreateRoundedXYZ(tx, ty, -tx - ty);
            //return HexCoord.CreateXY((int)tx, (int)ty);
        }

        public Vector3 HexToPlanePosition(HexCoord hex) {
            return new Vector3(hex.x / 2f + hex.y, 0, -hex.x * Constants.SQRT3 / 2);
        }

        private void Update() {
            //Debug.Log(RaycastToHex(Camera.main.ScreenPointToRay(Input.mousePosition)));
        }

        private void OnDrawGizmos() {
            var length = 3 * scale;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, length * HexToPlanePosition(HexCoord.RIGHT));
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, length * HexToPlanePosition(HexCoord.DOWN_R));
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, length * HexToPlanePosition(HexCoord.UP_R));
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, scale);
            if (drawHexPoints) {
                for (int x = 0; x < 30; x++) {
                    for (int y = 0; y < 30; y++) {
                        Gizmos.DrawSphere(HexToPlanePosition(HexCoord.CreateXY(x, y)), 0.1f * scale);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected() {
        }
    }
}