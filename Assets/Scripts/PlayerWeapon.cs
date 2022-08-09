using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!Player.I.isAttackAvailable) return;
        
        Debug.Log("Hit! " + other.tag + " / " + other.name);
        if (other.CompareTag("Enemy"))
        {
            other.transform.root.GetComponent<Enemy>().SetDestructible();
        }
    }
}
