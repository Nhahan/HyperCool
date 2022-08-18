using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class SwipeGuide : MonoBehaviour
{
    [SerializeField] private List<GameObject> setActiveAfter;
    [SerializeField] private GameObject guideText;
    [SerializeField] private GameObject dim;
    
    private RectTransform guideRect;
    private Vector2 guideStartPos;
    private float guideAcceleration;

    private int guideTime;

    private void Start()
    {
        guideRect = GetComponent<RectTransform>();
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
            guideTime++;
        }
        
        if (guideTime > 1)
        {
            DimOff();
            PauseOff();
        }


        if (GameManager.I.GetEnemiesCount() == 0)
        {
            Destroy(guideText);
            Destroy(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        setActiveAfter.ForEach(o => o.SetActive(true));
    }
    
    private void DimOff()
    {
        Destroy(dim);
    }

    private void PauseOff()
    {
        GameManager.I.pause = false;
    }
}
