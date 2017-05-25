using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	int maxHealth = 5;
	int currentHealth = 5;
	GameManager gameManager;
	public bool beingAttacked = false;

	void Start()
	{
		currentHealth = 5;
		maxHealth = 5;
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		gameManager.UpdateLife (currentHealth);

	}

	public void AddHealth(int amount){
        
		currentHealth += amount;
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		} else if (currentHealth < 0) {
			currentHealth = 0;
		}
		gameManager.UpdateLife (currentHealth);


	}

	void Update(){
		if (currentHealth <= 0) {
			gameObject.SetActive (false);

		}
	}

	public void ChangeAttackMode(){
		PlayerMovement playerMovement = this.GetComponent<PlayerMovement> ();
		if (beingAttacked) {
			//gameObject.layer = LayerMask.NameToLayer("Player");

			GetComponent<Animator> ().SetBool ("BeingAttacked", false);
			beingAttacked = false;
			playerMovement.canMove = true;
		} else {
			//gameObject.layer = LayerMask.NameToLayer("Dead");
			GetComponent<Animator> ().SetBool ("BeingAttacked", true);
			beingAttacked = true;
			playerMovement.canMove = false;
		}
	}

}
