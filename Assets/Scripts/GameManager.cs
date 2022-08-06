using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    public bool gameOver;

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
    }

    private void FixedUpdate()
    {
    }
}
