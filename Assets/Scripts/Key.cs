using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Called");
        if (collision.gameObject.tag == "Player")
        {
            PlayerCollectable playerCollectable = collision.gameObject.GetComponent<PlayerCollectable>();
            if (playerCollectable != null)
            {
               
                    playerCollectable.keys++;
                    Destroy(this.gameObject);
                
            }
        }
        
    }
}
