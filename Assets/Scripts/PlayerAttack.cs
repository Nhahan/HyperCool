using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject slashEffect;
    [SerializeField] private GameObject slashPoint;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var slash = Instantiate(slashEffect, slashPoint.transform.position, Quaternion.identity);
            Destroy(slash, 0.5f);
        }
    }
}
