using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class AK47Enemy : Enemy
{
    [SerializeField] private GameObject destructible;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    
    [SerializeField] private GameObject muzzle;
    [SerializeField] private GameObject bullet;

    private bool isDead;

    private Animator animator;
    private Vector3 firstWeaponPosition;
    private Quaternion firstWeaponRotation;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        // firstWeaponPosition = weapon.transform.localPosition;
        // firstWeaponRotation = weapon.transform.localRotation;
    }

    private void FixedUpdate()
    {
        if (GameManager.I.gameOver) return;
        
        WatchPlayer();

        if (!IsAttacking) return;

        try
        {
            if (animator.GetInteger("IsFire") == 1) return;
            var firePosition = Random.Range(0, 3); // TODO 
            animator.SetInteger("IsFire", 1);
            weapon.transform.localPosition = new Vector3(-0.176f, 0.17f, -0.086f);
            weapon.transform.localRotation = Quaternion.Euler(93.704f, -92.403f, -10.775f);
        }
        catch
        {
            Destroy(weapon);
        }
    }

    public void SetDestructible()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Dead")) return;
        gameObject.layer = LayerMask.NameToLayer("Dead");

        Instantiate(ParticleManager.I.hitParticles, transform.position, Quaternion.identity);
        Destroy(weapon);
        skinnedMeshRenderer.enabled = false;
        destructible.SetActive(true);
        skinnedMeshRenderer.BakeMesh(destructible.GetComponent<MeshFilter>().mesh, true);
    }

    private void Fire()
    {
        Instantiate(bullet, muzzle.transform.position, Quaternion.identity);
    }
}
