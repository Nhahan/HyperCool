using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 targetDirection;

    private bool isHit;

    private void Start()
    {
        Debug.Log("Instantiate Bullet");
        var player = Player.I.transform.position;
        targetDirection = new Vector3(player.x, player.y + 1.5f, player.z) - transform.position;
    }

    private void FixedUpdate()
    {
        Invoke(nameof(Move), 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                Debug.Log("Player Hit!");
                break;
            case "PlayerWeapon":
                isHit = true;
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    private void Move()
    {
        if (GameManager.I.gameSpeed == 0) return;
        
        if (isHit)
        {
            transform.position -= targetDirection * GameManager.I.gameSpeed / 30f;   
        }
        else
        {
            transform.position += targetDirection * GameManager.I.gameSpeed / 30f;
        }
    }
}
