using UnityEngine;
using System.Collections;

namespace CastleMagic.Util.Hex {
    public class HexPlane : MonoBehaviour {

        private Plane plane = new Plane(Vector3.up, Vector3.zero);

        public HexCoord RaycastToHex(Ray ray) {
            var relativeRay = new Ray(transform.worldToLocalMatrix * ray.origin, transform.worldToLocalMatrix * ray.direction);
            Debug.DrawRay(relativeRay.origin, 100 * relativeRay.direction, Color.red);
            float dist;
            plane.Raycast(relativeRay, out dist);
            if (dist < 0) {
                return null;
            }
            var hit = relativeRay.GetPoint(dist);
            return PlanePositionToHex(hit.x, hit.z);
        }

        public HexCoord PlanePositionToHex(float x, float y) {
            var tx = x - y / Constants.SQRT3;
            var ty = 2 * y / Constants.SQRT3;
            return HexCoord.CreateRoundedXYZ(tx, ty, -tx - ty);
        }

        public Vector3 HexToPlanePosition(HexCoord hex) {
            return new Vector3(hex.x + hex.y / 2, 0, hex.y * Constants.SQRT3 / 2);
        }

        private void Update() {
            Debug.Log(RaycastToHex(Camera.current.ScreenPointToRay(Input.mousePosition)));
        }

        private void OnDrawGizmosSelected() {
            Gizmos.DrawWireCube(transform.position, new Vector3(10, 0, 10));
        }

    }
}