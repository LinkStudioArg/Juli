using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerMovement : MonoBehaviour {
	
	public float velocity;
	public float jumpStrenght;
	public bool isGrounded = false;
	Animator anim;
	Rigidbody2D rigid;

	public ParticleSystem dirtTrail;
	public bool canMove = true;

	public Transform groundCheck;
	public float groundedRadius = .05f;
	public bool isJumping;
    public LayerMask layer;

	// Use this for initialization
	void Start () {
		canMove = true;
		anim = GetComponent<Animator> ();
		rigid = GetComponent<Rigidbody2D> ();
	}


    // Update is called once per frame
    void FixedUpdate()
    {

        anim.SetFloat("Vertical Velocity", rigid.velocity.y);

        //MOVEMENT CONTROL
        if (canMove)
        {
            transform.Translate(Vector2.right * Input.GetAxis("Horizontal") * velocity * Time.deltaTime);

            if (Input.GetAxis("Horizontal") < 0)
            {
                //	parallax.Speed = 15;
                if (transform.localScale.x > 0)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                //	parallax.Speed = -15;
                if (transform.localScale.x < 0)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            if (Input.GetAxis("Horizontal") != 0)
            {
                anim.SetBool("moving", true);
                if (isGrounded)
                {
                    if (dirtTrail.isStopped)
                        dirtTrail.Play();
                }
            }
            else
            {
                anim.SetBool("moving", false);


                dirtTrail.Stop();
                //	parallax.Speed = 0;
            }

        }
        //JUMP CONTROL

        if (CrossPlatformInputManager.GetButton("Jump"))
        {
            if (isGrounded)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpStrenght);
                isJumping = true;

            }
        }



        if (isGrounded)
        {
            
            isJumping = false;
            anim.SetBool("jump", false);
        }
        else
        {

            dirtTrail.Stop();
            anim.SetBool("jump", true);
        }

        //isGrounded = false;
        //jump = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, layer.value);

        if (colliders.Length > 0)
        {
            isGrounded = true;
        }
        else
            isGrounded = false;
    

	}
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.red;

        UnityEditor.Handles.DrawWireDisc(groundCheck.position, Vector3.back, groundedRadius);
    }
#endif
}
