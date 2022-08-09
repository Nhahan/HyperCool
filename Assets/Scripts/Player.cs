using System;
using System.Collections;
using Managers;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject weapon;
    
    public bool isAttacking;
    public bool isAttackAvailable;
    
    public static Player I;
    
    private void Awake()
    {
        if (I == null)
        {
            I = this;
        } 
        else if (I != this)
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;
            
            
            StartCoroutine(SetAnimator("IsRightSlash"));
        }
            
        if (isAttacking) 
        { 
            animator.SetBool("IsRightSlash", true);
        }
        else
        {
            animator.SetBool("IsRightSlash", false);
        }
    }

    private IEnumerator SetAnimator(string anim)
    {
        const float seconds = 0.633f;
        yield return new WaitForSeconds(seconds / 10 * 3.5f);
        weapon.SetActive(true);
        isAttackAvailable = true;

        yield return new WaitForSeconds(seconds / 10 * 5.5f);
        isAttacking = false;
        isAttackAvailable = false;
        animator.SetBool(anim, false);
        weapon.SetActive(false);
    }
}
    