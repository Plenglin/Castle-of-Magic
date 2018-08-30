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
        }
    }

    void OnChangeHealth (int health)
    {
		if (isLocalPlayer) {
			GameObject.FindGameObjectWithTag ("HealthLabel").GetComponent<Text> ().text = health.ToString ();
		}
    }

	void Update() {
		if (isServer) {
			TakeDamage(1);
		}
	}
}