using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class HextureTiler : MonoBehaviour {

    public float textureWidth;
    public float textureHeight;

    private Material mat;

	// Use this for initialization
	void Start () {
        mat = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 scale = 10 * transform.localScale;
        mat.SetTextureScale("_MainTex", new Vector2(scale.x / textureWidth, scale.y / textureHeight));
    }
}
