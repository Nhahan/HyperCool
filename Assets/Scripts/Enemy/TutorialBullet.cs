using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class TutorialBullet : MonoBehaviour
{
    [SerializeField] private new SphereCollider collider;
    [SerializeField] private bool isMelee;
    
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
        if (GameManager.I.pause) return;
        
        Move();

        if (isMelee) return;
        
        liveTime += Time.deltaTime;
        if (liveTime > 10f)
        {
            Destroy(gameObject);
        }

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
                Debug.Log("Tutorial Bullet d=" + d);
                if (d > 1.65f && d < 2.25f)
                {
                    UIEffects.I.Perfect();
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
                                           Random.Range(-1f, 1f))).normalized;
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
        if (isMelee) return;
        
        if (isPerfectHit)
        {
            targetDirection = (enemy.transform.position - transform.position + new Vector3(0, 1.65f, 0)).normalized * 2f;
        }
        
        if (isHit) 
        {     
            transform.position += targetDirection * (Time.deltaTime * 5);
        }
        else
        {
            transform.position += targetDirection * Time.deltaTime;
        }
    }

    private IEnumerator EnemyAnimatorEnable()
    {
        enemy.GetComponent<Enemy>().animator.enabled = false;
        yield return new WaitForSeconds(3f);
        enemy.GetComponent<Enemy>().animator.enabled = true;
    }
}
