using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private new SphereCollider collider;
    
    private Vector3 targetDirection;

    private bool isHit;
    private bool isPerfectHit;
    private float liveTime;
    public GameObject enemy;

    private void Start()
    {
        collider.enabled = false;
        
        var player = Player.I.transform.position;
        targetDirection = new Vector3(player.x, player.y + 1.725f, player.z) - transform.position;
    }

    private void Update()
    {
        liveTime += Time.deltaTime;
        if (liveTime > 10f)
        {
            Destroy(gameObject);
        }

        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                if (isHit) return;
                Destroy(gameObject);
                break;
            case "PlayerWeapon":
                if (isHit) return;
                var d = Vector3.Distance(Player.I.transform.position, transform.position);
                if (d is > 1.35f and < 1.85f)
                {
                    enemy.GetComponent<Enemy>().animator.enabled = false;
                    // StartCoroutine(EnemyAnimatorEnable());
                    isPerfectHit = true;
                }
                else if (d > 2.55f)
                {
                    break;
                }
                else
                {
                    targetDirection = (Player.I.transform.position - transform.position +
                                       new Vector3(Random.Range(-10f, 10f), 1.725f + Random.Range(-2f, 2f),
                                           Random.Range(-1f, 1f))).normalized * 10f;
                }

                isHit = true;
                break;
            case "Enemy":
                if (isHit)
                {
                    collider.enabled = true;
                    Debug.Log("Enemy Hit!");
                    
                    other.transform.root.GetComponent<Enemy>().SetCuttibleByBullet();
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }

                break;
        }
    }

    private void Move()
    {
        if (isPerfectHit)
        {
            targetDirection = (enemy.transform.position - transform.position + new Vector3(0, 1.65f, 0)).normalized * 3f;
        }
        
        transform.position += targetDirection * Time.deltaTime;
    }

    private IEnumerator EnemyAnimatorEnable()
    {
        enemy.GetComponent<Enemy>().animator.enabled = false;
        yield return new WaitForSeconds(3f);
        enemy.GetComponent<Enemy>().animator.enabled = true;
    }
}
