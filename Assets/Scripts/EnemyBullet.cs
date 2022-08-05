using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 targetDirection;

    private void Start()
    {
        var player = Player.I.transform.position;
        targetDirection = new Vector3(player.x, player.y + 1.5f, player.z) - transform.position;
    }

    private void FixedUpdate()
    {
        Invoke(nameof(Move), 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        if (GameManager.I.gameSpeed != 0) 
        {
            transform.position += targetDirection * GameManager.I.gameSpeed / 30f;
        }
    }
}
