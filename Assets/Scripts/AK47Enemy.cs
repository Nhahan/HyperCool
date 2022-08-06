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
        
        StartCoroutine(FOVRoutine());
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
        Debug.Log("Why not?" + Quaternion.LookRotation(Player.I.transform.position - transform.position));
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
    
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
}
