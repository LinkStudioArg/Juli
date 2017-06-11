using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Throwable : MonoBehaviour {
    public int id;
    public Sprite image;
    private Vector2 m_dir;
    public bool throwPosFixed = false;

    public Vector2 dir
    {
        get { return m_dir; }
        set
        {
            if (!throwPosFixed)
            {
                m_dir = value;
            }
            else
            {
                m_dir = new Vector2(value.x, 0);
            }
        }
    }    
}
