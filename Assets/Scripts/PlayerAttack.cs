using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject slashEffect;
    [SerializeField] private GameObject slashPoint;
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float z;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var slash = Instantiate(slashEffect, slashPoint.transform.position, Quaternion.Euler(0, -20 + transform.rotation.y, 35));
            Destroy(slash, 0.5f);
        }
    }
}
