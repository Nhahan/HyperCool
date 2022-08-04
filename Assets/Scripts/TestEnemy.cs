using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float cycleLength;
    [SerializeField] private bool changeDirection;
    [SerializeField] private bool isRight;
    
    private Vector3 firstPosition;

    private void Start()
    {
        firstPosition = transform.position;
    }
    
    private void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x) - Mathf.Abs(firstPosition.x) > cycleLength)
        {
            changeDirection = !changeDirection;
        }

        Move();
    }

    private void Move()
    {
        if (isRight)
        {
            if (changeDirection)
            {
                transform.position += Vector3.right * (moveSpeed * GameManager.I.gameSpeed * Time.deltaTime);
            }
            else
            {
                transform.position += Vector3.left * (moveSpeed * GameManager.I.gameSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (changeDirection)
            {
                transform.position += Vector3.left * (moveSpeed * GameManager.I.gameSpeed * Time.deltaTime);
            }
            else
            {
                transform.position += Vector3.right * (moveSpeed * GameManager.I.gameSpeed * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
