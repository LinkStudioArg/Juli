using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour {

    public float range;
    public int damage;
    public float timeToExplode;
    public LayerMask layer;
    public GameObject explosionPrefab;
    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(GetComponent<Throwable>().dir.normalized * 3, ForceMode2D.Impulse);
        StartCoroutine(Explode());

    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(timeToExplode);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0, layer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag == "Boss")
            {
                hit.collider.gameObject.GetComponent<BossBehaviour>().health -= damage;
            }
            else
            if (hit.collider.gameObject.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<EnemyBehaviour>().health -= damage;
            }
            else
                hit.collider.gameObject.GetComponent<PlayerHealth>().AddHealth(-damage);
        }
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GameObject obj = Instantiate<GameObject>(explosionPrefab,this.transform.position, Quaternion.identity);
        StartCoroutine(DestroyExplosion(obj));
    }

    IEnumerator DestroyExplosion(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(obj);
        Destroy(this.gameObject);
    }
}
