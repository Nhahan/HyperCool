using System;
using Managers;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] private float destroyAfter; 
    [SerializeField] private bool startAfterDestroy; 
    
    private void Start()
    {
        Destroy(gameObject, destroyAfter);
    }

    private void OnDestroy()
    {
        if (startAfterDestroy) GameManager.I.pause = false;
    }
}
