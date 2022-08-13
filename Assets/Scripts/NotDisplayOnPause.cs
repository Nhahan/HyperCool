using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class NotDisplayOnPause : MonoBehaviour
{
    private ParticleSystem particle;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
        StartCoroutine(onStart());
    }

    private void FixedUpdate()
    {
        if (GameManager.I.pause)
        {
            particle.Stop();
        }
        else
        {
            particle.Play();
        }
    }

    private IEnumerator onStart()
    {
        yield return new WaitForSeconds(0.1f);
        var particleEmission = particle.emission;
        particleEmission.enabled = true;
    }
}
