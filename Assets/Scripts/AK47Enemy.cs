using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AK47Enemy : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private GameObject bullet;
    
    [SerializeField] private float radius;
    [SerializeField] private float angle;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;

    private Animator animator;
    
    private bool canSeePlayer;

    private Vector3 firstWeaponPosition;
    private Quaternion firstWeaponRotation;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        firstWeaponPosition = weapon.transform.localPosition;
        firstWeaponRotation = weapon.transform.localRotation;
        
        // StartCoroutine(FOVRoutine());
    }

    private void FixedUpdate()
    {
        animator.speed = GameManager.I.gameSpeed;
        
        if (canSeePlayer && !GameManager.I.gameOver)
        {
            if (animator.GetInteger("IsFire") == 1) return;
            var firePosition = Random.Range(0, 3); // TODO 
            animator.SetInteger("IsFire", 1);
            weapon.transform.localPosition = new Vector3(0.01f, 0.153f, 0.151f);
            weapon.transform.localRotation = Quaternion.Euler(109.579f, 16.951f, 25.146f);
            
            WatchPlayer();
        }
        else
        {
            if (animator.GetInteger("IsFire") == 0) return;
            animator.SetInteger("IsFire", 0);
            weapon.transform.localPosition = firstWeaponPosition;
            weapon.transform.localRotation = firstWeaponRotation;
        }
    }

    private void WatchPlayer()
    {
        Debug.Log("Why not?");
        transform.rotation = Quaternion.LookRotation(Player.I.transform.position - transform.position);
    }

    private void Fire()
    {
        Instantiate(bullet, muzzle.transform.position, Quaternion.identity);
    }

    private void LateUpdate()
    {
        canSeePlayer = Vector3.Distance(transform.position, Player.I.transform.position) < radius;
    }
}
