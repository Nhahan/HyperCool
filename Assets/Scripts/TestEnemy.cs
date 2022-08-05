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
    [SerializeField] private GameObject bullet;
    
    private Vector3 firstPosition;

    private float fireCooltime;

    private void Start()
    {
        firstPosition = transform.position;

        StartCoroutine(Attack());
    }
    
    private void FixedUpdate()
    {
        fireCooltime += GameManager.I.gameSpeed;
        if (Mathf.Abs(transform.position.x) - Mathf.Abs(firstPosition.x) > cycleLength)
        {
            changeDirection = !changeDirection;
        }

        Move();
    }

    private IEnumerator Attack()
    {
        while(true) 
        {
            yield return new WaitForSeconds(4f);
            if (fireCooltime > 10f) 
            {
                Instantiate(bullet, transform.position, Quaternion.identity);
                fireCooltime = 0;
            }
        }
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
