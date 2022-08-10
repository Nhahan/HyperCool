using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;

    private void FixedUpdate()
    {
        if (Player.I.isAttackAvailable)
        {
            boxCollider.enabled = true;
        }
        else
        {
            boxCollider.enabled = false;   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Player.I.isAttackAvailable) return;
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            try
            {
                other.transform.root.GetComponent<Enemy>().SetDestructible();
            }
            catch
            {
                // ignored
            }
        }
    }
}
