using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class HextureTiler : MonoBehaviour {

    public float textureWidth;
    public float textureHeight;

    private Material material;

	// Use this for initialization
	void Start () {
        material = new Material(GetComponent<MeshRenderer>().sharedMaterial);
        GetComponent<MeshRenderer>().sharedMaterial = material;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 scale = 10 * transform.localScale;
        material.SetTextureScale("_MainTex", new Vector2(scale.x / textureWidth, scale.y / textureHeight));
    }
}
