using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AttackController : MonoBehaviour {
	Animator anim;

	public int damage = 100;

    bool isAttacking = false;

	bool isThrowing = false;

	public float throwAttackRate;

	public Transform throwPoint;

	public int maxAmmo = 100;

	public float attackRange;

	public LayerMask enemyLayerMask;

	GameManager gameManager;

    public List<ThrowableObjectSlot> throwableObjectsList;

    private int SelectedThrowableIndex;

    public int m_selectedThrowableIndex {
        get {
            return SelectedThrowableIndex;
        }
        set
        {
            if (value > 4)
            {
                SelectedThrowableIndex = 0;
            }
            else
            {
                if (value < 0)
                {
                    SelectedThrowableIndex = 4;
                }
                else
                    SelectedThrowableIndex = value;
            }
           
           
        }
    }

    public ThrowableObjectSlot SelectedThrowable;

    public Image throwWeaponImage;

    public Text throwWeaponText;

    void Start()
	{
        throwableObjectsList = new List<ThrowableObjectSlot>();
        for (int i = 0; i < 5; i++)
        {
            throwableObjectsList.Add(new ThrowableObjectSlot(i));
        }
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		anim = GetComponent<Animator> ();

	}

	void Update () {
        Debug.Log(SelectedThrowableIndex);
		if (!isAttacking && Input.GetButton ("Fire1")) {
			
			anim.SetBool ("attack", true);
			//StartCoroutine (Attack());

		} else {
			anim.SetBool ("attack", false);
		}

		if (!isThrowing && Input.GetButtonDown ("Fire2")  && SelectedThrowable!= null) {
            if (!SelectedThrowable.isEmpty && SelectedThrowable.ammo > 0)
            {
                anim.SetBool("AxeAttack", true);
                StartCoroutine(ThrowAttack());
                AddAmmo(-1, SelectedThrowable.throwableObject.id);
            }
        }
        changeWeapon();
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

    private void UpdateThrowUI()
    {
        if (SelectedThrowable != null)
        {
            if (!SelectedThrowable.isEmpty)
            {
                throwWeaponImage.sprite = SelectedThrowable.throwableObject.image;
                throwWeaponImage.enabled = true;
                throwWeaponText.text = "x " + SelectedThrowable.ammo.ToString();
            }
            else
            {
                throwWeaponText.text = "";
                throwWeaponImage.enabled = false;
            }
        }
        else
        {
            throwWeaponText.text = "";
            throwWeaponImage.enabled = false;
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

        m_selectedThrowableIndex = i;
            SelectedThrowable = throwableObjectsList[m_selectedThrowableIndex];
        
    }

    public void DeactiveAxeAnim()
	{
		anim.SetBool ("AxeAttack", false);
	}


    IEnumerator ThrowAttack()
    {
        isThrowing = true;
        Throwable obj = Instantiate<Throwable>(SelectedThrowable.throwableObject, throwPoint.position, Quaternion.identity);
       
            obj.dir = throwPoint.position - transform.position;
        yield return new WaitForSeconds(throwAttackRate);
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

    public void AddWeapon(Throwable weapon, int amount)
    {
        if (hasWeapon(weapon.id))
        {
            AddAmmo(amount, weapon.id);
        }
        else
        {
            ThrowableObjectSlot slot = FindEmptySlot();
            if (slot != null)
            {
                slot.isEmpty = false;
                slot.throwableObject = weapon;
                slot.ammo = amount;
                SelectSlot(slot.id);
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

    private void changeWeapon()
    {
        if (Input.GetButtonDown("ScrollUp"))
        {
            SelectSlot(m_selectedThrowableIndex + 1);
        }
        else if (Input.GetButtonDown("ScrollDown"))
        {
            SelectSlot(m_selectedThrowableIndex - 1);
        }
        UpdateThrowUI();
    }
}
