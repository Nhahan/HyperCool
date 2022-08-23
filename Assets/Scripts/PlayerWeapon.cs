using System.Collections;
using System.Collections.Generic;
using TrailsFX;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private PlayerController controller;
    private TrailEffect trailEffect;
    private readonly List<BoxCollider> boxColliders = new();

    private void Start()
    {
        controller = transform.root.GetComponent<PlayerController>();
        trailEffect = GetComponent<TrailEffect>();
        boxColliders.AddRange(GetComponents<BoxCollider>());
    }

    private void FixedUpdate()
    {
        if (controller.isAttackAvailable)
        {
            boxColliders.ForEach(boxCollider => boxCollider.enabled = true);
            StartCoroutine(SetTrail());
        }
        else
        {
            boxColliders.ForEach(boxCollider => boxCollider.enabled = false);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!controller.isAttackAvailable) return;

        try
        {
            if (other.CompareTag("Enemy"))
            {

                other.transform.root.GetComponent<Enemy>().SetCuttible();
            }
            else if (other.CompareTag("Cuttible"))
            {
                other.transform.root.GetComponent<CubeEnemy>().SetCuttible();
            }
        }
        catch
        {
            // ignored
        }
    }

    private IEnumerator SetTrail()
    {
        yield return new WaitForSeconds(0.05f);
        trailEffect.maxStepsPerFrame = 72;
        
        yield return new WaitForSeconds(0.1f);
        
        trailEffect.maxStepsPerFrame = 0;
    }
}
