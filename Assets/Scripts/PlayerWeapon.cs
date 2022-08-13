using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private List<BoxCollider> boxColliders = new();

    private void Start()
    {
        boxColliders.AddRange(GetComponents<BoxCollider>());
    }

    private void FixedUpdate()
    {
        if (Player.I.isAttackAvailable)
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
        if (!Player.I.isAttackAvailable) return;

        Debug.Log(other.tag);
        try
        {
            if (other.CompareTag("Enemy"))
            {

                other.transform.root.GetComponent<Enemy>().SetCuttible();
            }
            else if (other.CompareTag("Cuttible"))
            {
                Debug.Log("??");
                other.transform.root.GetComponent<CubeEnemy>().SetCuttible();
            }
        }
        catch
        {
            // ignored
        }
    }
}
