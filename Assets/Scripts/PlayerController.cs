using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool testMode;
    
    private Rigidbody rb;
    private float defaultFixedDeltaTime;

    private float verticalSpeed;
    private float horizontalSpeed;
    
    public int isAttacking;
    public bool isAttackAvailable;

    private Vector3 downPos;
    private Vector3 upPos;
    private float dragTime;
    
    private bool action;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (GameManager.I.clear)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        
        if (!GameManager.I.gameOver)
        {
            Attack();
        }

        TimeScale();
    }

    private void TimeScale()
    {
        if (testMode) return;
        
        if (GameManager.I.pause)
        {
            Time.timeScale = 1;
            return;
        }
        
        rb.velocity = new Vector3(variableJoystick.Horizontal, 0, variableJoystick.Vertical) *
                      (movementSpeed * Time.deltaTime);

        var x = Mathf.Abs(rb.velocity.x);
        var z = Mathf.Abs(rb.velocity.z);

        var time = (x != 0 || z != 0) ? 1f : 0.15f;
        var lerpTime = (x != 0 || z != 0) ? 0.05f : 0.5f;

        time = action ? 1 : time;
        lerpTime = action ? 0.05f : lerpTime;

        Time.timeScale = Mathf.Lerp(Time.timeScale, time, lerpTime);
        Time.fixedDeltaTime = Time.timeScale switch
        {
            < 0.3f => 0.003f,
            _ => defaultFixedDeltaTime
        };
    }

    private IEnumerator ActionRoutine(float time)
    {
        action = true;
        yield return new WaitForSeconds(time);
        action = false;
    }

    public void SetVelocityToZero()
    {
        rb.velocity = Vector3.zero;
    }

    private void Attack()
    {
        float d = 0;
        dragTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            dragTime = 0;
            downPos = Input.mousePosition;
        }

        
        upPos = Input.mousePosition;
        d = Vector3.Distance(downPos, upPos);
        downPos = Vector3.zero;

        if (d > 300f && dragTime < 0.2f)
        {
            AttackAnimation(upPos.x >= downPos.x);
            Debug.Log("d=" + d + " / dragTime=" + dragTime);
            StopCoroutine(ActionRoutine(0.03f));
            StartCoroutine(ActionRoutine(0.55f));
            d = 0;
        }
    }

    private void AttackAnimation(bool isRight)
    {
        Debug.Log("왜요!");
        if (GameManager.I.clear) return;

        Debug.Log("왜요");
        Pause();
        
        if (GameManager.I.pause) return;
        
        Debug.Log("isAttackAvailable:" + isAttackAvailable);
        Debug.Log("isAttacking:" + isAttacking);
        if (isRight && isAttacking is 0)
        {
            Debug.Log("?");
            isAttacking = 1;
            StartCoroutine(SetAnimator("IsRightSlash", 0.6f));
        } 
        else if (!isRight && isAttacking is 0)
        {
            Debug.Log("!");
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

