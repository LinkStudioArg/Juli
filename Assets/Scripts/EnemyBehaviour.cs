using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
	public float health;
	public GameObject bloodParticles;
	public Animator anim;
	Transform player;
	public int damage = 1;
	bool isDead = false;
	LayerMask DeadLayer;
    public enum TypeMove { STANDSTILL, PERSUE, PATROL, PATROLPERSUE }
    public TypeMove movementType;
    public int lookDirection; // -1 left, 1 right
    public float velocity;
    public bool shouldMove = false;
    public Transform target;
    public int rate;
    public Throwable weapon;
    private bool isThrowing = false;
    public Transform throwPoint;

    IEnumerator ThrowAttack()
    {
        isThrowing = true;
        Throwable obj = Instantiate<Throwable>(weapon, throwPoint.position, Quaternion.identity);
        obj.dir = throwPoint.position - transform.position;
        yield return new WaitForSeconds(rate);
        isThrowing = false;
    }


    void Start () {
        player = GameObject.Find ("Player").transform;
		anim.SetBool ("isDying", false);
		anim.SetBool ("isDead", false);
		anim.SetBool ("isMoving", false);
	}



	// Update is called once per frame
	void Update () {
		//TODO: Movimiento
        if(!isDead)
		    Move();

		if (health <= 0 && !isDead) {
            Debug.Log("asdasd");
			Vector3 particleObjectrot;
			DeadLayer = LayerMask.NameToLayer("Dead") ;
			this.gameObject.layer = DeadLayer.value;
			if (player.position.x - transform.position.x > 0) {
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


    void StandStill()
    {
        shouldMove = false;
        if (PlayerOnSight() && !isThrowing)
        {
            StartCoroutine( ThrowAttack());
        }

    }



    void Persue()
    {
        if (PlayerOnSight())
        {
            target = player;
            shouldMove = true;
        }
        if (target)
        {
            if (target.position.x - transform.position.x > 0)
                lookDirection = 1;
            else
                lookDirection = -1;
        }

    }
    void Patrol()
    {
        shouldMove = true;
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + lookDirection * Vector3.right , new Vector2(lookDirection, -5), 2);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + lookDirection * Vector3.right , new Vector2(lookDirection, 0), 0.2f);
        
        if (hit1.collider == null)
        {
            lookDirection = -lookDirection;
            return;
        }
        if (hit2.collider != null)
        {
            if (hit2.collider.tag == "Ground") {
                lookDirection = -lookDirection;
            }
        }

    }

    void PatrolPersue()
    {
        Patrol();
        Persue();
    }

    public int enemyLineSight;
    bool PlayerOnSight()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(throwPoint.position, (new Vector3(transform.localScale.x, 0, 0)).normalized, enemyLineSight);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                return true;
            }
            
        }

        return false;
    }

	void Move()
	{
            transform.localScale = new Vector3(lookDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        switch (movementType)
        {
            case TypeMove.STANDSTILL:
                StandStill();
                break;
            case TypeMove.PERSUE:
                Persue();
                break;
            case TypeMove.PATROL:
                Patrol();
                break;
            case TypeMove.PATROLPERSUE:
                PatrolPersue();
                break;
        }
        if (shouldMove)
        {
            anim.SetBool("isMoving", true);
            transform.Translate(Vector2.right * lookDirection * velocity * Time.deltaTime);
        }
        else
        {
            anim.SetBool("isMoving", false); 

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
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + lookDirection * Vector3.right , transform.position + lookDirection * Vector3.right  + (new Vector3(transform.localScale.x, 0, 0)).normalized * enemyLineSight);
    }
#endif



}
