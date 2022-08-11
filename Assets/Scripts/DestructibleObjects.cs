using System;
using System.Collections;
using System.Collections.Generic;
using DinoFracture;
using UnityEngine;
using Random = UnityEngine.Random;

public class DestructibleObjects : MonoBehaviour
{
    private GameObject destructible;
    private MeshRenderer meshRenderer;

    private bool isCollided;

    private void Start()
    {
        destructible = transform.GetChild(0).gameObject;
        meshRenderer = GetComponent<MeshRenderer>();

        destructible.GetComponent<RuntimeFracturedGeometry>().NumFracturePieces = Random.Range(3, 8);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Stable") || isCollided) return;

        isCollided = true;
        meshRenderer.enabled = false;
        destructible.SetActive(true);
    }
}
