using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject trail;

    private List<GameObject> trails = new(); 

    private Vector3 targetDirection;

    private bool isHit;

    private void Start()
    {
        var player = Player.I.transform.position;
        targetDirection = new Vector3(player.x, player.y + 1.5f, player.z) - transform.position;

    }

    private void FixedUpdate()
    {
        Move();
    }
    
    private void Trail()
    {
        GameObject t;
        if (isHit)
        {
            t = Instantiate(trail, transform.position + targetDirection * 0.01f, Quaternion.Euler(-90, 0, 0));   
        }
        else
        {
            t = Instantiate(trail, transform.position - targetDirection * 0.01f, Quaternion.Euler(-90, 0, 0));
        }
        
        trails.Add(t);
        StartCoroutine(TrailBehavior(t));
    }

    private IEnumerator TrailBehavior(GameObject t)
    {
        yield return new WaitForSeconds(2.5f);
        try
        {
            trails.Remove(t);
            Destroy(t, 2.5f * GameManager.I.gameSpeed);
        }
        catch
        {
            // ignored
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                Debug.Log("Player Hit!");
                RemoveTrails();
                Destroy(gameObject);
                break;
            case "PlayerWeapon":
                isHit = true;
                RemoveTrails();
                break;
            case "Enemy":
                if (isHit)
                {
                    Debug.Log("Enemy Hit!");
                    RemoveTrails();
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void RemoveTrails()
    {
        for (var i = 0; i < trails.Count; i++)
        {
            try
            {
                Destroy(trails[i]);
            }
            catch
            {
                continue;
            }
        }
    }

    private void Move()
    {
        if (isHit)
        {
            transform.position -= targetDirection * GameManager.I.gameSpeed / 40f;   
        }
        else
        {
            transform.position += targetDirection * GameManager.I.gameSpeed / 40f;
        }
    }
}
