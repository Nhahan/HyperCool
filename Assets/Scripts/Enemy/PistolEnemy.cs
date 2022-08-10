using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

public class PistolEnemy : Enemy
{
    [SerializeField] private GameObject muzzle;
    [SerializeField] private GameObject bullet;

    private Vector3 firstWeaponPosition;
    private Quaternion firstWeaponRotation;

    private void FixedUpdate()
    {
        if (GameManager.I.gameOver) return;

        if (!IsAttacking) return;

        try
        {
            if (animator.GetBool("IsFire")) return;
            var firePosition = Random.Range(0, 3); // TODO 
            animator.SetBool("IsFire", true);
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
            Instantiate(bullet, muzzle.transform.position, Quaternion.identity).GetComponent<Bullet>().enemy = gameObject;
        }
        catch
        {
            // ignored
        }
    }
}
