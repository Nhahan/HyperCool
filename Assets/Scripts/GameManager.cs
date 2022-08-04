using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    public float gameSpeed;
    private Vector3 firstGravity;

    private void Awake()
    {
        Application.targetFrameRate = 60;
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
        firstGravity = Physics.gravity;
    }

    private void FixedUpdate()
    {
        Physics.gravity = firstGravity * gameSpeed;
    }
}
