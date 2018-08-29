using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour {

    public const int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    public RectTransform healthBar;

    public void TakeDamage(int amount)
    {
		Debug.Assert(isServer);
        
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
        }
    }

    void OnChangeHealth (int health)
    {
		if (GetComponent<NetworkIdentity> ().netId == this.netId) {
			GameObject.FindGameObjectWithTag ("HealthLabel").GetComponent<Text> ().text = health.ToString ();
		}
    }

	void Update() {
		if (isServer) {
			Debug.Log ("is serv");
			TakeDamage(1);
		}
		Debug.Log ("is cli");
	}
}