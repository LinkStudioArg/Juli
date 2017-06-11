using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Door : MonoBehaviour {

    public bool state = false;
    public Sprite openedSprite;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerCollectable playerCollectable = collision.gameObject.GetComponent<PlayerCollectable>();
            if (playerCollectable != null)
            {
                if (playerCollectable.keys > 0)
                {
                    playerCollectable.keys--;
                    state = true;
                    this.GetComponentInChildren<SpriteRenderer>().sprite = openedSprite;
                    this.GetComponent<Collider2D>().enabled = false;
                }
            }
        }
    }
}
