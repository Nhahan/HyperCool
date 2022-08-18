using System;
using System.Collections;
using Managers;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public int isAttacking;
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
        if (GameManager.I.clear) return;

        Pause();
        
        if (GameManager.I.pause) return;
        
        if (Input.GetMouseButtonDown(0) && isAttacking is 0)
        {
            isAttacking = 1;
            StartCoroutine(SetAnimator("IsRightSlash", 0.6f));
        } 
        else if (Input.GetMouseButtonDown(1) && isAttacking is 0)
        {
            isAttacking = 2;
            StartCoroutine(SetAnimator("IsLeftSlash", 0.317f));
        }

        switch (isAttacking)
        {
            case 1:
                animator.SetBool("IsRightSlash", true);
                break;
            case 2:
                animator.SetBool("IsLeftSlash", true);
                break;
            default:
                animator.SetBool("IsRightSlash", false);
                animator.SetBool("IsLeftSlash", false);
                break;
        }
    }

    private IEnumerator SetAnimator(string anim, float time)
    {
        yield return new WaitForSeconds(time / 10 * 3f);
        isAttackAvailable = true;

        yield return new WaitForSeconds(time / 10 * 4f);
        isAttacking = 0;
        isAttackAvailable = false;
        animator.SetBool(anim, false);
    }
    
    private void Pause()
    {
        animator.enabled = !GameManager.I.pause;
    }
}
    