using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;

public class EnemiesCounter : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;

    private void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        textMeshProUGUI.text = "Enemies Left : " + GameManager.I.enemies.Count.ToString();
    }
}
