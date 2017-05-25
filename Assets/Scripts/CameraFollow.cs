using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    

	struct FocusArea
	{
		public Vector2 center;
		public Vector2 velocity;
		public float left, right, top, bottom;
        public FreeParallax parallax;

        public FocusArea (Bounds targetBounds, Vector2 size, FreeParallax n_parallax)
		{
			left = targetBounds.center.x - size.x/2;
			right = targetBounds.center.x + size.x/2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;
            parallax = n_parallax;
			velocity = Vector2.zero;
			center = new Vector2((left + right)/2, (top + bottom) /2);
		}

		public void Update(Bounds targetBounds)
		{
			float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
                parallax.Speed = 15;

            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
                parallax.Speed = -15;
            }
            else
                parallax.Speed = 0;

            left += shiftX;
			right += shiftX;

			float shiftY = 0;
			if (targetBounds.min.y < bottom) {
				shiftY = targetBounds.min.y - bottom;	
			} else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}
			top += shiftY;
			bottom += shiftY;
			center = new Vector2 ((left + right) / 2, (top + bottom) / 2);
			velocity = new Vector2 (shiftX, shiftY);
		}


	}
	public Transform target; //A que objeto debe seguir la camara.

	public Collider2D targetCollider;

	public Vector2 offset;

	public Vector2 focusAreaSize;
	private FocusArea focusArea;
    


    void Start()
	{
        

        focusArea = new FocusArea (targetCollider.bounds, focusAreaSize, GameObject.Find("Parallax").GetComponent<FreeParallax>());

    }
    // Update is called once per frame
    void FixedUpdate () 
	{

		focusArea.Update (targetCollider.bounds);
		Vector2 focusPosition =  focusArea.center + Vector2.right * offset.x + Vector2.up * offset.y;//(focusArea.x + offset.x, focusArea.center.y + offset.y)
		transform.position = Vector3.Lerp(transform.position, (Vector3)focusPosition + Vector3.forward * -10, 0.1f);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color (1, 0, 0, 0.5f);
		Gizmos.DrawCube (focusArea.center, focusAreaSize);
	}

	public void ChangeFocusArea(Vector2 size)
	{
		focusArea.left = targetCollider.bounds.center.x - size.x / 2;
		focusArea.right = targetCollider.bounds.center.x + size.x / 2;
		focusArea.bottom = targetCollider.bounds.min.y;
		focusArea.top = targetCollider.bounds.min.y + size.y;
		focusArea.velocity = Vector2.zero;
		Vector2 newcenter = new Vector2 ((focusArea.left + focusArea.right) / 2, (focusArea.top + focusArea.bottom) / 2);
		focusArea.center = newcenter;
	}
}
