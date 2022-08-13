using System;
using DynamicMeshCutter;
using Managers;
using UnityEngine;

public class CubeEnemy : MonoBehaviour
{
    private new Light light;

    private void Start()
    {
        light = GetComponentInChildren<Light>();
    }

    private void Update()
    {
        if (GameManager.I.pause) return;

        var y = Mathf.Sin(Time.time) / 3.5f;
        transform.position = new Vector3(transform.position.x, y + 1.5f, transform.position.z);
        transform.transform.eulerAngles += new Vector3(0, 60 * Time.deltaTime, 0);
    }
    
    public void SetCuttible()
    {
        GameManager.I.RemoveEnemyFromList(gameObject);
        Instantiate(ParticleManager.I.hitParticles, transform.position, Quaternion.identity);
        Instantiate(ParticleManager.I.hitParticles, transform.position, Quaternion.identity);
        light.enabled = false;
        Destroy(gameObject, 5f);
    }
}
