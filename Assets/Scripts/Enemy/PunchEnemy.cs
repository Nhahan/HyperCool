using System.Collections;
using Managers;
using UnityEngine;

public class PunchEnemy : Enemy
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private float speed;
    
    private float animatorCooltime;

    private void FixedUpdate()
    {
        if (GameManager.I.pause) return;
        if (GameManager.I.gameOver) return;

        if (!IsAttacking) return;
        
        try
        {
            SetAnimator();
        }
        catch
        {
            Destroy(weapon);
        }

        Move();
    }

    private void AutoAttack(float d)
    {
        if (isDead) return;
        if (d < 0.5f)
        {
            GameManager.I.GameOver();
        }
    }

    private void Move()
    {
        var d = Vector3.Distance(Player.I.transform.position, transform.position);
        if (d < 1f)
        {
            animator.SetBool("IsWalk", false);
            AutoAttack(d);
            return;
        }

        if (speed == 0) Nav.isStopped = false;
        if (!canSeePlayer) return;
        if (animator.GetBool("IsWalk"))
        {
            // transform.position =
            //     Vector3.MoveTowards(transform.position, Player.I.transform.position, speed * Time.deltaTime);
            Nav.isStopped = false;
        }
        else
        {
            Nav.isStopped = true;
        }
    }

    protected override void WatchPlayer()
    {
        if (isDead) return;
        
        var targetDirection = animator.GetBool("IsWalk") ? 
            Player.I.transform.position - transform.position + new Vector3(0.75f, 0, 0) : 
            Player.I.transform.position - transform.position;
        var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1f, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void SetAnimator()
    {
        animatorCooltime += Time.deltaTime;
        if (animatorCooltime < 0.2f) return;

        if (Vector3.Distance(Player.I.transform.position, transform.position) < 2.1f)
        {
            bullet.SetActive(true);
            animator.SetBool("IsFire", true);
            animator.SetBool("IsWalk", false);
            speed = 0;
        }
        else
        {
            bullet.SetActive(false);
            animator.SetBool("IsWalk", true);
            animator.SetBool("IsFire", false);
            speed = 1;
        }

        animatorCooltime = 0;
    }
}
