using System.Collections;
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
    private float attackCooltime;

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
            Time.timeScale = 1;
            rb.velocity = Vector3.zero;
            return;
        }
        
        attackCooltime -= Time.deltaTime / Time.timeScale;
        if (!GameManager.I.gameOver)
        {
            if (attackCooltime > 0) return;
            if (Input.GetMouseButtonDown(0))
            {
                downPos = Input.mousePosition;
                StartCoroutine(Attack());
            }
        }
        
        TimeScale();
    }

    private void TimeScale()
    {
        
        if (GameManager.I.pause || GameManager.I.gameOver)
        {
            Time.timeScale = 1;
            return;
        }

        if (testMode) return;
        
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

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.0125f);
        
        var upPos = Input.mousePosition;
        var d = Vector3.Distance(downPos, upPos);
        if (d > 60f)
        {
            AttackAnimation( downPos.x >= upPos.x);
            StopCoroutine(ActionRoutine(0.03f));
            StartCoroutine(ActionRoutine(0.55f));
        }
    }

    private void AttackAnimation(bool isRight)
    {
        if (GameManager.I.clear) return;
        Pause();
        
        if (GameManager.I.pause) return;

        if (isRight && isAttacking is 0)
        {
            isAttacking = 1;
            StartCoroutine(SetAnimator("IsRightSlash"));
        } 
        else if (!isRight && isAttacking is 0)
        {
            isAttacking = 2;
            StartCoroutine(SetAnimator("IsLeftSlash"));
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
    
    private IEnumerator SetAnimator(string anim)
    {
        if (anim == "IsRightSlash") 
        {
            yield return new WaitForSeconds(0.12f);
            isAttackAvailable = true;

            yield return new WaitForSeconds(0.22f);
            isAttacking = 0;
            isAttackAvailable = false;
            animator.SetBool(anim, false);
            attackCooltime = 0.55f;
        }
        else
        {
            yield return new WaitForSeconds(0.05f);
            isAttackAvailable = true;

            yield return new WaitForSeconds(0.2f);
            isAttacking = 0;
            isAttackAvailable = false;
            animator.SetBool(anim, false);
            attackCooltime = 0.317f;
        }
    }
    
    private void Pause()
    {
        animator.enabled = !GameManager.I.pause;
    }
}

