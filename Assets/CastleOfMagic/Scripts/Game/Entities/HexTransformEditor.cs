using UnityEngine;
using UnityEditor;
using CastleMagic.Util.Hex;

namespace CastleMagic.Game.Entites {

    [CustomEditor(typeof(HexTransform))]
    public class HexTransformEditor : Editor {

        public override void OnInspectorGUI() {
            HexTransform trs = (HexTransform)target;
            //EditorGUILayout.BeginHorizontal();
            int x = EditorGUILayout.IntField("x", trs.Position.x);
            int y = EditorGUILayout.IntField("y", trs.Position.y);
            //EditorGUILayout.EndHorizontal();
            trs.Position = HexCoord.CreateXY(x, y);
            EditorUtility.SetDirty(target);
        }
    }
}