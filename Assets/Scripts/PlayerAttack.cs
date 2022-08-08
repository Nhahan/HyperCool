using System.Collections;
using System.Collections.Generic;
using DynamicMeshCutter;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject slashEffect;
    [SerializeField] private GameObject slashPoint;
    [SerializeField] private PlaneBehaviour plane;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var slash = Instantiate(slashEffect, slashPoint.transform.position, Quaternion.Euler(0, -20 + transform.rotation.y, 35));
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies)
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) < 4)
                {
                    enemy.GetComponent<AK47Enemy>().SetDestructible();
                }
            }
            Destroy(slash, 0.5f);
        }
    }
}
