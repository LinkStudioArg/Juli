using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
	public float health;
	public GameObject bloodParticles;
	public Animator anim;
	Transform Player;
	public int damage = 1;
	bool isDead = false;
	LayerMask DeadLayer;


	// Use this for initialization
	void Start () {
		health = 1;
		Player = GameObject.Find ("Player").transform;
		anim.SetBool ("isDying", false);
		anim.SetBool ("isDead", false);
		anim.SetBool ("isMoving", false);
	}



	// Update is called once per frame
	void Update () {
		//TODO: Movimiento
		Move();

		if (health <= 0 && !isDead) {
			Vector3 particleObjectrot;
			DeadLayer = LayerMask.NameToLayer("Dead") ;
			this.gameObject.layer = DeadLayer.value;
			if (Player.position.x - transform.position.x > 0) {
				particleObjectrot = new Vector3 (0,180, 0);
			}
			else
				particleObjectrot = new Vector3 (0,0,0);

			GameObject obj = (GameObject)Instantiate (bloodParticles, transform.position + new Vector3(0,0,-3), Quaternion.Euler(particleObjectrot));
			obj.transform.parent = this.gameObject.transform;
			isDead = true;
			StartCoroutine (DestroyParticle (obj));
			anim.SetBool ("isDying", true);

			//Destroy (this.gameObject);

		}
	}

	IEnumerator DestroyParticle(GameObject obj )
	{
		
		yield return new WaitForSeconds (15f);
		anim.SetBool ("isDead", true);

		ParticleSystem ps = obj.GetComponentInChildren<ParticleSystem> ();
		ps.Stop ();
		yield return new WaitForSeconds (5f);
		Destroy (obj);
	}

	public void Damage(int amount){
		health -= amount;
	}

	[SerializeField]
	Transform[] waypoints;
	public int currentwaypoint = 0;
	public float speed;

	void Move()
	{
		if (waypoints.Length != 0 && anim.GetBool("isDying") == false) {
			anim.SetBool ("isMoving", true);
			Vector2 playerPos = transform.position;
			Vector2 waypointPos = new Vector2 (waypoints [currentwaypoint].position.x, transform.position.y);
			if (Mathf.Abs (Vector2.Distance (playerPos, waypointPos)) < 0.2f) {
			
				if (currentwaypoint != waypoints.Length - 1) {
					currentwaypoint++;

				} else {
					currentwaypoint = 0;

				}
			} else {
				//Debug.Log (playerPos.x - waypointPos.x);
				if ((playerPos.x - waypointPos.x) < 0) {
					transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
				} else {
					if (transform.localScale.x > 0)
					transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
				}
				transform.position = Vector2.MoveTowards (playerPos, waypointPos, speed * Time.deltaTime);
			}
		} else {
			anim.SetBool ("isMoving", false);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Player") {
			PlayerHealth playerHealth = coll.gameObject.GetComponent<PlayerHealth> ();
			if (!playerHealth.beingAttacked) {
				
				playerHealth.ChangeAttackMode ();
				playerHealth.AddHealth (-damage);
				Rigidbody2D playerRigid = coll.gameObject.GetComponent<Rigidbody2D> ();
				if (transform.position.x - coll.gameObject.transform.position.x > 0)
					playerRigid.velocity = new Vector2(-5, 5);
				else
					playerRigid.velocity = new Vector2(5, 5);;
			}
			else
				playerHealth.ChangeAttackMode ();

		}
	}



}
