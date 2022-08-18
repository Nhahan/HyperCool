using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class MoveGuide : MonoBehaviour
{
    [SerializeField] private List<GameObject> setActiveAfter; 
    private void OnDestroy()
    {
        setActiveAfter.ForEach(o => o.SetActive(true));
    }

    private void PauseOff()
    {
        GameManager.I.pause = false;
    }
}
