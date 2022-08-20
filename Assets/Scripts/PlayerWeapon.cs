using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private PlayerController controller;
    private readonly List<BoxCollider> boxColliders = new();

    private void Start()
    {
        controller = transform.root.GetComponent<PlayerController>();
        boxColliders.AddRange(GetComponents<BoxCollider>());
    }

    private void FixedUpdate()
    {
        if (controller.isAttackAvailable)
        {
            boxColliders.ForEach(boxCollider => boxCollider.enabled = true);
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
}
