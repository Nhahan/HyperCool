using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour
{
    [SerializeField] private GameObject restart;

    private void SetRestartActive()
    {
        restart.SetActive(true);
    }
}
