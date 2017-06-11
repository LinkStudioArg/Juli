using UnityEngine;
using System.Collections;

public class Axe : MonoBehaviour {

	public Animator anim;
	public HingeJoint2D hindge;
	bool isMoving = true;
	public int damage = 10;
	public float velocity = 1f;
    public enum Target { Player, Enemy};
    public Target targetType;
    Throwable throwableComponent;

	void Start(){
        if (targetType == Target.Enemy)
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerWeapons");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyWeapons");

        }
        throwableComponent = GetComponent<Throwable>();
		hindge.enabled = false;
		anim.SetBool ("Stuck", false);
		if (throwableComponent.dir.x < 0) {
		
			transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
	}
	// Update is called once per frame
	void FixedUpdate () {
		if(isMoving)
			transform.Translate (throwableComponent.dir * velocity);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		Animator collAnim = coll.gameObject.GetComponentInChildren<Animator> ();
		isMoving = false;
		Debug.Log (coll.gameObject.name);
        if (coll.gameObject.tag == "Boss")
        {
            coll.gameObject.GetComponent<BossBehaviour>().health -= damage;
        }
        else
        if (coll.collider.gameObject.tag ==targetType.ToString()) {
            if (targetType == Target.Enemy)
            {
                coll.gameObject.GetComponent<EnemyBehaviour>().Damage(damage);
                if (coll.gameObject.GetComponent<EnemyBehaviour>().health > 0)
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                coll.gameObject.GetComponent<PlayerHealth>().AddHealth(-1);
                Destroy(this.gameObject);
                return;
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
