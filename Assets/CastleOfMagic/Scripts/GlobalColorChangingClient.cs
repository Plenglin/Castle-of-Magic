﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GlobalColorChangingClient : NetworkBehaviour {

	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) { return; }
		
        if (Input.GetKeyDown (KeyCode.P)) {
			Debug.Log ("wanna change color");
			CmdChangeColor ();
		}
	}

	[Command]
	public void CmdChangeColor() {
		Debug.Log("gonna change color");
        Color c = new Color(Random.value, Random.value, Random.value);

        GameObject o = GameObject.FindWithTag("ColorBoi");
		ColorChangingParent p = o.GetComponent<ColorChangingParent> ();
		p.color = c;
	}
	
}
