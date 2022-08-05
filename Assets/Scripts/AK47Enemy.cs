using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    
    [SerializeField] public float radius;
    [SerializeField] public float angle;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;

    private Animator animator;
    
    public bool canSeePlayer;

    private Vector3 firstWeaponPosition;
    private Quaternion firstWeaponRotation;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        firstWeaponPosition = weapon.transform.localPosition;
        firstWeaponRotation = weapon.transform.localRotation;
        
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            FieldOfViewCheck();
        }
    }

    private void FixedUpdate()
    {
        if (canSeePlayer)
        {
            var firePosition = Random.Range(0, 3); // TODO 
            animator.SetInteger("IsFire", 1);
            weapon.transform.localPosition = new Vector3(0.01f, 0.153f, 0.151f);
            weapon.transform.localRotation = Quaternion.Euler(109.579f, 16.951f, 25.146f);
            transform.rotation = Quaternion.LookRotation(Player.I.transform.position - transform.position);
        }
        else
        {
            animator.SetInteger("IsFire", 0);
            weapon.transform.localPosition = firstWeaponPosition;
            weapon.transform.localRotation = firstWeaponRotation;
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            var target = rangeChecks[0].transform;
            var directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                var distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}