using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float changeTime;
    [SerializeField] private bool changeDirection;
    
    private float changeDirectionTime;
    
    private void FixedUpdate()
    {
        changeDirectionTime += Time.deltaTime;
        if (changeDirectionTime > changeTime)
        {
            changeDirection = !changeDirection;
            changeDirectionTime = 0;
        }
        
        if (changeDirection) 
        {
            transform.position += Vector3.right * (moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position += Vector3.left * (moveSpeed * Time.deltaTime);
        }
    }
}
