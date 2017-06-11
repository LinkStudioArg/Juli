using UnityEngine;
using System.Collections;

public class Items : MonoBehaviour {

	public enum type {WEAPON,AMMO, HEALTH};
	public type Type;
	public int amount;
    public Throwable weapon;
	AttackController attackController;
	PlayerHealth playerHealth;

	// Update is called once per frame
	void OnCollisionEnter2D (Collision2D coll) {	
		if (coll.gameObject.tag == "Player") {
			switch (Type) {
			    case type.AMMO:
			
				    attackController = coll.gameObject.GetComponent<AttackController> ();
                        if (coll.gameObject.GetComponent<AttackController>().hasWeapon(weapon.id))
                        {
                            attackController.AddAmmo(amount, weapon.id);
                            Destroy(this.gameObject);
                        }

				    break;
			    case type.HEALTH:
				    playerHealth = coll.gameObject.GetComponent<PlayerHealth> ();
				    playerHealth.AddHealth (amount);
                        Destroy(this.gameObject);
                        break;
                case type.WEAPON:
                    coll.gameObject.GetComponent<AttackController>().AddWeapon(weapon, amount);
                    Destroy(this.gameObject);

                    break;
			}
			
		}

	}
}
