using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ColorChangingParent : NetworkBehaviour {
	[SyncVar]
	public Color color;

	public void OnColorChanged(Color nc) {
		GetComponent<Image>().color = nc;
	}
}
