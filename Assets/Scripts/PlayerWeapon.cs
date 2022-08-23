using System.Collections;
using System.Collections.Generic;
using TrailsFX;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private PlayerController controller;
    [SerializeField] private List<TrailRenderer> trails;
    private readonly List<BoxCollider> boxColliders = new();

    private void Start()
    {
        var root = transform.root;
        controller = root.GetComponent<PlayerController>();
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
        trails.ForEach(trail => trail.time = 0.35f);
        
        yield return new WaitForSeconds(0.3f);
        trails.ForEach(trail => trail.time = 0);
    }
}
