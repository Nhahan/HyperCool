using System;
using System.Collections;
using Managers;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player I;
    
    private void Awake()
    {
        if (I == null)
        {
            I = this;
        } 
        else if (I != this)
        {
            Destroy(gameObject);
        }
    }
}
    