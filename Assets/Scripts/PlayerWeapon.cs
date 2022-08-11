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
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            try
            {
                other.transform.root.GetComponent<Enemy>().SetCuttible();
            }
            catch
            {
                // ignored
            }
        }
    }
}
