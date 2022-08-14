using Managers;
using UnityEngine;

public class AK47TutorialEnemy : Enemy
{
    [SerializeField] private GameObject muzzle;
    [SerializeField] private GameObject bullet;

    private int bulletCount;

    private void FixedUpdate()
    {
        if (GameManager.I.gameOver || GameManager.I.pause) return;

        if (!IsAttacking) return;

        Attack();
    }

    private void Attack()
    {
        try
        {
            animator.SetInteger("IsFire", 1);
            weapon.transform.localPosition = new Vector3(-0.176f, 0.17f, -0.086f);
            weapon.transform.localRotation = Quaternion.Euler(93.704f, -92.403f, -10.775f);
        }
        catch
        {
            Destroy(weapon);
        }
    }

    private void Fire()
    {
        try
        {
            Instantiate(bullet, muzzle.transform.position, Quaternion.identity).GetComponent<TutorialBullet>().enemy = gameObject;
            Destroy(Instantiate(ParticleManager.I.fireParticles, muzzle.transform.position, Quaternion.identity), 0.09f);
        }
        catch
        {
            // ignored
        }
    }
}
