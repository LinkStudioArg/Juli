using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AttackController : MonoBehaviour {
	Animator anim;
	public int damage = 100;
	// Use this for initialization

	bool isAttacking = false;
	bool isThrowing = false;
	public float throwAttackRate;
	public Transform throwPoint;

	public int maxAmmo = 20;

	public float attackRange;
	public LayerMask enemyLayerMask;

	GameManager gameManager;

    public List<ThrowableObjectSlot> throwableObjectsList;

    public int SelectedThrowableIndex;

    public ThrowableObjectSlot SelectedThrowable;
    // Update is called once per frame
    void Start()
	{
        throwableObjectsList = new List<ThrowableObjectSlot>();
        for (int i = 0; i < 5; i++)
        {
            throwableObjectsList.Add(new ThrowableObjectSlot());
        }
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		anim = GetComponent<Animator> ();

	}

	void Update () {

		if (!isAttacking && Input.GetButton ("Fire1")) {
			
			anim.SetBool ("attack", true);
			//StartCoroutine (Attack());

		} else {
			anim.SetBool ("attack", false);
		}

		if (!isThrowing && Input.GetButtonDown ("Fire2")  && !SelectedThrowable.isEmpty && SelectedThrowable.ammo > 0) {
			anim.SetBool ("AxeAttack", true);
            StartCoroutine(ThrowAttack());
            AddAmmo(-1, SelectedThrowable.throwableObject.id);
        }

        //make selection

    }

	public void Attack(){
		//Debug.Log ("Called");

		isAttacking = true;
		RaycastHit2D hit = Physics2D.Raycast (transform.position, new Vector2(transform.localScale.x, 0),attackRange, enemyLayerMask.value);

		if (hit.collider != null) {
			if (hit.collider.gameObject.tag == "Enemy") {
				hit.collider.gameObject.GetComponent<EnemyBehaviour>().Damage ( damage);
			}
		}
		isAttacking = false;
	
	}

    public Image throwWeaponImage;
    public Text throwWeaponText;
    private void UpdateThrowUI()
    {
        if (!SelectedThrowable.isEmpty)
        {
            throwWeaponImage.sprite = SelectedThrowable.throwableObject.image;
            throwWeaponText.text = SelectedThrowable.ammo.ToString();
        }
        else
        {
            throwWeaponText.text = "";
            throwWeaponImage = null;
        }
    }

    private bool isThrowableListEmpty()
    {
        foreach (ThrowableObjectSlot slot in throwableObjectsList)
        {
            if (!slot.isEmpty)
            {
                return false;
            }
        }
        return true;
    }

    private void SelectSlot(int i)
    {
        if (i > 4 || i < 0)
            return;
        else { 

            Debug.Log("lalala");
            SelectedThrowableIndex = i;
            SelectedThrowable = throwableObjectsList[SelectedThrowableIndex];
        }
    }

    public void DeactiveAxeAnim()
	{
		anim.SetBool ("AxeAttack", false);
	}

	IEnumerator ThrowAttack()
	{
        isThrowing = true;
//		yield return new WaitForSeconds (0.1f);
		Instantiate (SelectedThrowable.throwableObject, new Vector3(throwPoint.position.x, throwPoint.position.y, 2), Quaternion.identity);
        yield return new WaitForSeconds (throwAttackRate);
        isThrowing = false;
	}

    public bool hasWeapon(int id)
    {
        Debug.Log(throwableObjectsList[0].isEmpty);

        foreach (ThrowableObjectSlot slot in throwableObjectsList)
        {

            if (!slot.isEmpty)
            {
                if (slot.throwableObject.id == id)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private ThrowableObjectSlot getSlotWithWeapon(int id)
    {
        foreach (ThrowableObjectSlot slot in throwableObjectsList)
        {
            if (!slot.isEmpty)
            {
                if (slot.throwableObject.id == id)
                {
                    return slot;
                }
            }
        }
        return null;
    }

    public void AddAmmo(int amount, int id){

        ThrowableObjectSlot slot = getSlotWithWeapon(id);
        if (slot != null)
        {
            //slot.ammo += amount;
            if ((slot.ammo + amount) > maxAmmo)
                slot.ammo = maxAmmo;
            else if ((slot.ammo + amount) == 0)
            {
                slot.Empty();
            }
            else
                slot.ammo += amount;
            UpdateThrowUI();

        }
    }

    public void AddWeapon(Throwable weapon)
    {
        if (hasWeapon(weapon.id))
        {
            AddAmmo(5, weapon.id);
        }
        else
        {
            ThrowableObjectSlot slot = FindEmptySlot();
            if (slot != null)
            {
                slot.isEmpty = false;
                slot.throwableObject = weapon;
                slot.ammo = 5;
                SelectedThrowable = slot;
                UpdateThrowUI();
            }
        }
    }

    private ThrowableObjectSlot FindEmptySlot()
    {
        foreach (ThrowableObjectSlot slot in throwableObjectsList)
        {
            if (slot.isEmpty)
            {
                slot.Empty();
                return slot;
            }
        }
        return null;
    }
}
[System.Serializable]
public class ThrowableObjectSlot
{
    public bool isEmpty;
    public Throwable throwableObject;
    public int ammo;

    public ThrowableObjectSlot()
    {
        Debug.Log("Created");
        isEmpty = true;
        throwableObject = null;
        ammo = 0;
    }

    public void Empty()
    {

        isEmpty = true;
        throwableObject = null;
        ammo = 0;
    }
}