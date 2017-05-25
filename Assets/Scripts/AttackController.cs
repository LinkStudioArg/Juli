using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour {
	Animator anim;
	public int damage = 100;
	// Use this for initialization

	bool isAttacking = false;
	bool isAxeAttacking = false;
	public float AxeAttackRate;
	public Transform AxePoint;

	public int maxAmmo = 20;
	public int currentAmmo = 2;

	public float attackRange;
	public LayerMask enemyLayerMask;
	public GameObject AxePrefab;

	GameManager gameManager;

	// Update is called once per frame
	void Start()
	{

		maxAmmo = 20;
		currentAmmo = 2;
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		gameManager.UpdateAmmo (currentAmmo);
		anim = GetComponent<Animator> ();

	}

	void Update () {

		if (!isAttacking && Input.GetButton ("Fire1")) {
			
			anim.SetBool ("attack", true);
			//StartCoroutine (Attack());

		} else {
			anim.SetBool ("attack", false);
		}

		if (!isAxeAttacking && Input.GetButton ("Fire2") &&currentAmmo > 0) {

			anim.SetBool ("AxeAttack", true);
			currentAmmo--;
			gameManager.UpdateAmmo (currentAmmo);
			StartCoroutine (AxeAttack());

		} 

	}

	public void Attack(){
		//Debug.Log ("Called");

		isAttacking = true;
		RaycastHit2D hit = Physics2D.Raycast (transform.position, new Vector2(transform.localScale.x, 0),attackRange, enemyLayerMask.value);

		if (hit.collider != null) {
			if (hit.collider.gameObject.tag == "Enemy") {
				hit.collider.gameObject.GetComponent<EnemyBehaviour>().Damage ( damage);
			}
		}
		isAttacking = false;
	
	}

	public void DeactiveAxeAnim()
	{
		anim.SetBool ("AxeAttack", false);
	}

	IEnumerator AxeAttack()
	{
		isAxeAttacking = true;
		yield return new WaitForSeconds (0.1f);
		Instantiate (AxePrefab, new Vector3(AxePoint.position.x, AxePoint.position.y, 2), Quaternion.identity);

		yield return new WaitForSeconds (AxeAttackRate);
		isAxeAttacking = false;
	}

	public void AddAmmo(int amount){
		currentAmmo += amount;
		if (currentAmmo > maxAmmo) {
			currentAmmo = maxAmmo;
		} else if (currentAmmo < 0) {
			currentAmmo = 0;
		}

		gameManager.UpdateAmmo (currentAmmo);
	}
}
