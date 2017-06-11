using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObjectSlot
{
    public bool isEmpty;
    public Throwable throwableObject;
    public int ammo;
    public int id;
    public ThrowableObjectSlot(int id)
    {
        this.id = id;
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