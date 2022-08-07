using System;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Vector3 firstRot;

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            transform.Rotate(50, -25, 0);
        }
    }
}
