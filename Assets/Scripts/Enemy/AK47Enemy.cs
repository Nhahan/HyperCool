using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AK47Enemy : Enemy
{
    [SerializeField] private GameObject muzzle;
    [SerializeField] private GameObject bullet;

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
            weapon.transform.localPosition = new Vector3(-0.467f, 0.496f, -0.178f);
            weapon.transform.localRotation = Quaternion.Euler(101.993f, -154.185f, -72.21399f);
        }
        catch
        {
            Destroy(weapon);
        }
    }

    private void Fire()
    {
        Instantiate(bullet, muzzle.transform.position, Quaternion.identity);
    }
}
