using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour {

    public float force;
    public float effectTime;

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(GetComponent<Throwable>().dir.normalized * force, ForceMode2D.Impulse);
        if (GetComponent<Throwable>().dir.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }


    private float targetVel;
    private int targetRate;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        if (collision.gameObject.tag == "Boss")
        {
            targetVel = collision.gameObject.GetComponent<BossBehaviour>().velocity;
            targetRate = (int)collision.gameObject.GetComponent<BossBehaviour>().rate;
            collision.gameObject.GetComponent<BossBehaviour>().velocity -= 1.5f;
            collision.gameObject.GetComponent<BossBehaviour>().rate += 1;
            collision.gameObject.GetComponentInChildren<SpriteRenderer>().color -= new Color(0.1f, 0.1f, 0, 0);
            StartCoroutine(CoolDownEnemy(collision.gameObject));
        }
        else
       if (collision.gameObject.tag == "Enemy")
        {
            targetVel = collision.gameObject.GetComponent<EnemyBehaviour>().velocity;
            targetRate = collision.gameObject.GetComponent<EnemyBehaviour>().rate;
            collision.gameObject.GetComponent<EnemyBehaviour>().velocity -= 1.5f;
            collision.gameObject.GetComponent<EnemyBehaviour>().rate += 1;
            collision.gameObject.GetComponentInChildren<SpriteRenderer>().color -= new Color(0.1f,0.1f,0,0);
            StartCoroutine(CoolDownEnemy(collision.gameObject));

        }
        else if (collision.gameObject.tag == "Player")
        {
            targetVel = collision.gameObject.GetComponent<PlayerMovement>().velocity;
            collision.gameObject.GetComponent<PlayerMovement>().velocity -= 1.5f;
            collision.gameObject.GetComponent<SpriteRenderer>().color -= new Color(0.1f, 0.1f, 0, 0);

            StartCoroutine(CoolDownPlayer(collision.gameObject));

        }
        else
            Destroy(this.gameObject);
        return;
    }

    IEnumerator CoolDownEnemy(GameObject go)
    {
        yield return new WaitForSeconds(effectTime);
        if (go.tag == "Boss")
        {
            go.GetComponent<BossBehaviour>().velocity += 1.5f;
            go.GetComponent<BossBehaviour>().rate -= 1;
        }
        else
        {
            go.GetComponent<EnemyBehaviour>().velocity += 1.5f;
            go.GetComponent<EnemyBehaviour>().rate -= 1;
        }
        go.GetComponentInChildren<SpriteRenderer>().color += new Color(0.1f, 0.1f, 0, 0);

        Destroy(this.gameObject);
    }
    IEnumerator CoolDownPlayer(GameObject go)
    {
        yield return new WaitForSeconds(effectTime);
        go.GetComponent<SpriteRenderer>().color += new Color(0.1f, 0.1f, 0, 0);

        go.GetComponent<PlayerMovement>().velocity += 1.5f;
        Destroy(this.gameObject);
    }
}
