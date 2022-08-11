using DynamicMeshCutter;
using Managers;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject destructible;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    
    [SerializeField] private PlaneBehaviour plane;
    
    [SerializeField] protected GameObject weapon;
    [SerializeField] protected float radius;
    
    public Animator animator;
    protected bool isDead;
    private bool canSeePlayer;
    protected bool IsAttacking;
    private Mesh destructibleMesh;
    private MeshRenderer destructibleMeshRenderer;

    protected void Start()
    {
        plane.transform.eulerAngles += new Vector3(plane.transform.rotation.x + Random.Range(-20f, 20f), Random.Range(-20f, 20f), 0);
        plane.transform.position += new Vector3(0,0, Random.Range(-0.7f, 0.2f));
        animator = gameObject.GetComponent<Animator>();
        destructibleMesh = destructible.GetComponent<MeshFilter>().mesh;
        destructibleMeshRenderer = destructible.GetComponent<MeshRenderer>();
    }

    protected virtual void WatchPlayer()
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
    
    public void SetCuttible()
    {
        if (isDead) return;
        skinnedMeshRenderer.enabled = false;
        skinnedMeshRenderer.BakeMesh(destructibleMesh, true);
        destructibleMeshRenderer.enabled = true;
        plane.Cut();
        
        isDead = true;
        gameObject.layer = LayerMask.NameToLayer("Dead");
        animator.enabled = false;

        Instantiate(ParticleManager.I.hitParticles, transform.position + new Vector3(0, 1.7f, 0), Quaternion.identity);
        Destroy(weapon);


        GameManager.I.RemoveEnemyFromList(gameObject);
        Destroy(gameObject, 5f);
    }
    
    public void SetCuttibleByBullet()
    {
        if (isDead) return;
        plane.GetComponent<PlaneBehaviour>().Separation = 0.15f;
        
        skinnedMeshRenderer.enabled = false;
        skinnedMeshRenderer.BakeMesh(destructibleMesh, true);
        destructibleMeshRenderer.enabled = true;
        
        plane.transform.localPosition += new Vector3(0, 0, 0.35f);
        
        isDead = true;
        gameObject.layer = LayerMask.NameToLayer("Dead");
        animator.enabled = false;

        Instantiate(ParticleManager.I.hitParticles, transform.position + new Vector3(0, 1.71f, 0), Quaternion.identity);
        Instantiate(ParticleManager.I.hitParticles, transform.position + new Vector3(0, 1.74f, 0), Quaternion.identity);
        Destroy(weapon);


        plane.Cut();
        GameManager.I.RemoveEnemyFromList(gameObject);
        Destroy(gameObject, 5f);
    }
}
