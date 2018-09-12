using UnityEngine;
using System.Collections;

namespace CastleMagic.Util.Hex {
    public class HexPlane : MonoBehaviour {

        private Plane plane = new Plane(Vector3.up, Vector3.zero);
        public float scale = 1.0f;

        public HexCoord RaycastToHex(Ray ray) {
            var relativeRay = new Ray(transform.TransformPoint(ray.origin), transform.TransformDirection(ray.direction));
            Debug.DrawRay(ray.origin, 10 * ray.direction, Color.white);
            float dist;
            plane.Raycast(relativeRay, out dist);
            if (dist < 0) {
                return null;
            }
            var hit = ray.GetPoint(dist);
            return PlanePositionToHex(hit.x, hit.y);
        }

        public HexCoord PlanePositionToHex(float x, float y) {
            var tx = -y * 2 / Constants.SQRT3;
            var ty = x + y / Constants.SQRT3;
            return HexCoord.CreateRoundedXYZ(tx, ty, -tx - ty);
        }

        public Vector3 HexToPlanePosition(HexCoord hex) {
            return new Vector3(hex.x / 2f + hex.y, 0, -hex.x * Constants.SQRT3 / 2);
        }

        private void Update() {
            Debug.Log(RaycastToHex(Camera.current.ScreenPointToRay(Input.mousePosition)));
        }

        private void OnDrawGizmosSelected() {
            var length = 3 * scale;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, length * HexToPlanePosition(HexCoord.RIGHT));
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, length * HexToPlanePosition(HexCoord.DOWN_R));
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, length * HexToPlanePosition(HexCoord.UP_R));
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(transform.position, scale);
        }

    }
}