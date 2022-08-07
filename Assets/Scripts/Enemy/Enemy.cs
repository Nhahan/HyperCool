using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected GameObject weapon;
    [SerializeField] protected float radius;
    
    private bool canSeePlayer;
    protected bool IsAttacking;

    protected void WatchPlayer()
    {
        var targetDirection = Player.I.transform.position - transform.position;
        var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1f, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }
    
    protected void LateUpdate()
    {
        if (canSeePlayer) return;
        
        canSeePlayer = Vector3.Distance(transform.position, Player.I.transform.position) < radius;
        if (canSeePlayer) IsAttacking = true;
    }
    
    public void DestroyWeapon()
    {
        Destroy(weapon);
    }
}
