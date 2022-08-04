using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    // [SerializeField] private float gravityScale = 1.0f;
    [SerializeField] private Rigidbody rb;
 
    private float globalGravity = -9.8f;
    private Vector3 angularVelocity;
    
    private ConstantForce gravity;
    private bool isLanded;

    private void Start()
    {
        gravity = gameObject.AddComponent<ConstantForce>();
    }

    private void FixedUpdate()
    {
        if (isLanded) return;
        
        gravity.force = new Vector3(0.0f, globalGravity * GameManager.I.gameSpeed, 0.0f);
        if (GameManager.I.gameSpeed == 0)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Stable")) return;
        
        isLanded = true;
    }
}