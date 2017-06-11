using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour {

    public float force;
    public int damage;


	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().AddForce(GetComponent<Throwable>().dir.normalized * force, ForceMode2D.Impulse);
        if (GetComponent<Throwable>().dir.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        StartCoroutine(DestroyKnife());	
	}
    IEnumerator DestroyKnife()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boss")
        {
            collision.gameObject.GetComponent<BossBehaviour>().health -= damage;
        }else if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyBehaviour>().health -= damage;
        }
        else if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<PlayerHealth>().AddHealth(-damage);
        else
            GetComponent<Collider2D>().enabled = false;
        return;
    }
}
