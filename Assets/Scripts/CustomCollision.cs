using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCollision : MonoBehaviour
{
    [SerializeField] private float customKnockbackTime;
    [SerializeField] private float customMass;
    
    private Vector3 knockbackDirection;
    private float knockbackPlaytime;

    private Rigidbody rb;

    private bool isLanded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!(knockbackPlaytime > 0)) return;
        PlayKnockback();
    }
    
    private void StartKnockback(Vector3 normal)
    {
        knockbackDirection = normal;
        knockbackPlaytime = customKnockbackTime;
    }
        
    private void PlayKnockback()
    {
        var decreaseAmount = GameManager.I.gameSpeed / 3;
        
        transform.position += knockbackDirection * decreaseAmount / customMass;
        knockbackPlaytime -= decreaseAmount;

        if (isLanded) return;
        knockbackDirection -= new Vector3(0, 0.1f * decreaseAmount, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Stable"))
        {
            knockbackPlaytime = 0f;
            isLanded = true;
            return;
        }
        
        var normal = (transform.position - collision.gameObject.transform.position).normalized;
        StartKnockback(normal);
    }
}
