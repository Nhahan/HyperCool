using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    
    public static Player I;

    [SerializeField] private bool testMode;

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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (testMode) return; 
        
        var velocity = rb.velocity.magnitude;
        
        if (velocity < 0.05f)
        {
            GameManager.I.gameSpeed = 0.01f;
        }
        else
        {
            GameManager.I.gameSpeed = rb.velocity.magnitude / 5;
        }
    }
}
