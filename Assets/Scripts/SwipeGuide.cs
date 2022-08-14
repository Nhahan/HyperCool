using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class SwipeGuide : MonoBehaviour
{
    private RectTransform guideRect;
    private Vector2 guideStartPos;
    private float guideAcceleration;

    private void Start()
    {
        guideRect = transform.GetChild(0).GetComponent<RectTransform>();
        guideStartPos = guideRect.anchoredPosition;
    }

    private void Update()
    {
        guideAcceleration += Time.deltaTime + 0.1f;
        guideRect.anchoredPosition -= new Vector2(100 * Time.deltaTime * guideAcceleration / Time.timeScale, 0);
        if (guideRect.anchoredPosition.x < -guideStartPos.x)
        {
            guideRect.anchoredPosition = guideStartPos;
            guideAcceleration = 0;
        }
        if (GameManager.I.GetEnemiesCount() == 0) Destroy(gameObject);
    }
}
