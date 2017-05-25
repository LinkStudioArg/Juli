using UnityEngine;
using System.Collections;

public class Axe : MonoBehaviour {

	Vector3 dir;
	public Animator anim;
	public HingeJoint2D hindge;
	bool isMoving = true;
	public int damage = 10;
	public float velocity = 1f;

	void Start(){
		hindge.enabled = false;
		anim.SetBool ("Stuck", false);
		if (GameObject.Find ("Player").transform.lossyScale.x > 0) {
			dir = Vector3.right;

		} else {
			dir = Vector3.left;
			transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
	}
	// Update is called once per frame
	void FixedUpdate () {
		if(isMoving)
			transform.Translate (dir * velocity);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		Animator collAnim = coll.gameObject.GetComponentInChildren<Animator> ();
		isMoving = false;
		Debug.Log (coll.gameObject.name);
		if (coll.collider.gameObject.tag == "Enemy") {
			
			coll.gameObject.GetComponent<EnemyBehaviour> ().Damage (damage);
			if (coll.gameObject.GetComponent<EnemyBehaviour> ().health > 0) {
				Destroy (this.gameObject);
			}

		}

		anim.SetBool ("Stuck", true);
		//anim.Stop ();
		//anim.enabled = false;
		hindge.enabled = true;


		if(coll.rigidbody != null)
		{
			//hindge.connectedAnchor = coll.contacts [0].point;
			hindge.connectedBody = coll.rigidbody;

		}

		this.GetComponent<Collider2D> ().enabled = false;
		Destroy (this.gameObject, 15f);
	}
}
