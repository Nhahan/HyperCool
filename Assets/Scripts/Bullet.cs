using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private new SphereCollider collider;
    private Vector3 targetDirection;

    private bool isHit;

    private void Start()
    {
        collider.enabled = false;
        
        var player = Player.I.transform.position;
        targetDirection = new Vector3(player.x, player.y + 1.5f, player.z) - transform.position;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                Debug.Log("Player Hit!");
                Destroy(gameObject);
                break;
            case "PlayerWeapon":
                isHit = true;
                break;
            case "Enemy":
                if (isHit)
                {
                    collider.enabled = true;
                    Debug.Log("Enemy Hit!");
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void Move()
    {
        if (isHit)
        {
            transform.position -= targetDirection * Time.deltaTime;   
        }
        else
        {
            transform.position += targetDirection * Time.deltaTime;
        }
    }
}
