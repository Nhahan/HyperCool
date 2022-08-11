using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

public class AK47Enemy : Enemy
{
    [SerializeField] private GameObject muzzle;
    [SerializeField] private GameObject bullet;

    private Vector3 firstWeaponPosition;
    private Quaternion firstWeaponRotation;

    private void FixedUpdate()
    {
        if (GameManager.I.gameOver || GameManager.I.pause) return;

        if (!IsAttacking) return;

        try
        {
            if (animator.GetInteger("IsFire") == 1) return;
            var firePosition = Random.Range(0, 3); // TODO 
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
            Instantiate(bullet, muzzle.transform.position, Quaternion.identity).GetComponent<Bullet>().enemy = gameObject;
        }
        catch
        {
            // ignored
        }
    }
}
