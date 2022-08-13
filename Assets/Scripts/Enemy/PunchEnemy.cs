using System.Collections;
using Managers;
using UnityEngine;

public class PunchEnemy : Enemy
{
    [SerializeField] private GameObject muzzle;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float speed;

    private void FixedUpdate()
    {
        if (GameManager.I.pause) return;
        if (GameManager.I.gameOver) return;

        if (!IsAttacking) return;

        try
        {
            StartCoroutine(SetAnimator());
        }
        catch
        {
            Destroy(weapon);
        }

        Move();
    }

    private void Move()
    {
        if (animator.GetBool("IsWalk"))
        {
            transform.position =
                Vector3.MoveTowards(transform.position, Player.I.transform.position, speed * Time.deltaTime);
        }
    }

    protected override void WatchPlayer()
    {
        if (isDead) return;
        
        var targetDirection = animator.GetBool("IsWalk") ? 
            Player.I.transform.position - transform.position + new Vector3(-0.5f, 0, 0) : 
            Player.I.transform.position - transform.position;
        var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1f, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private IEnumerator SetAnimator()
    {
        yield return new WaitForSeconds(1f);
        if (!animator.GetBool("IsFire"))
        {
            yield break;
        }

        animator.SetBool("IsFire", true);


        // var d = Vector3.Distance(Player.I.transform.position, transform.position);
        // switch (d)
        // {
        //     case > 14:
        //         animator.SetBool("IsWalk", true);
        //         break;
        //     case < 10:
        //         animator.SetBool("IsWalk", false);
        //         break;
        // }
    }
}
