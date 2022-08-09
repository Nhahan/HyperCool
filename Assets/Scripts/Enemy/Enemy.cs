using System;
using Managers;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject destructible;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    
    [SerializeField] protected GameObject weapon;
    [SerializeField] protected float radius;
    
    protected Animator Animator;
    private bool isDead;
    private bool canSeePlayer;
    protected bool IsAttacking;
    private Mesh destructibleMesh;

    protected void Start()
    {
        Animator = gameObject.GetComponent<Animator>();
        destructibleMesh = destructible.GetComponent<MeshFilter>().mesh;
    }

    private void WatchPlayer()
    {
        if (isDead) return;
        
        var targetDirection = Player.I.transform.position - transform.position;
        var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1f, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }
    
    protected void LateUpdate()
    {
        WatchPlayer();
        
        if (canSeePlayer) return;

        canSeePlayer = Vector3.Distance(transform.position, Player.I.transform.position) < radius;
        if (canSeePlayer) IsAttacking = true;
    }
    
    public void SetDestructible()
    {
        if (isDead) return;
        isDead = true;
        gameObject.layer = LayerMask.NameToLayer("Dead");
        Animator.enabled = false;

        Instantiate(ParticleManager.I.hitParticles, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        Destroy(weapon);
        skinnedMeshRenderer.enabled = false;
        destructible.SetActive(true);
        skinnedMeshRenderer.BakeMesh(destructibleMesh, true);
        
        Destroy(gameObject, 4f);
    }
    
    public void DestroyWeapon()
    {
        Destroy(weapon);
    }
}
