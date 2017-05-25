using UnityEngine;
using System.Collections;

public class Items : MonoBehaviour {

	public enum type {AMMO, HEALTH};
	public type Type;
	public int amount;
	AttackController attackController;
	PlayerHealth playerHealth;

	// Update is called once per frame
	void OnCollisionEnter2D (Collision2D coll) {	
		if (coll.gameObject.tag == "Player") {
			switch (Type) {
			case type.AMMO:
			
				attackController = coll.gameObject.GetComponent<AttackController> ();
				attackController.AddAmmo (amount);

				break;
			case type.HEALTH:
				playerHealth = coll.gameObject.GetComponent<PlayerHealth> ();
				playerHealth.AddHealth (amount);

				break;
			}
			Destroy (this.gameObject);
		}

	}
}
