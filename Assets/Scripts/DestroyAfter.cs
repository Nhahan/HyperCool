using System.Collections.Generic;
using Managers;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] private float destroyAfter; 
    [SerializeField] private bool startAfterDestroy; 
    [SerializeField] private bool pauseOff;
    [SerializeField] private List<GameObject> setActiveAfter;
    
    private void Start()
    {
        Destroy(gameObject, destroyAfter);
    }

    private void OnDestroy()
    {
        setActiveAfter.ForEach(o => o.SetActive(true));
        if (startAfterDestroy && pauseOff) GameManager.I.pause = false;
    }
}
