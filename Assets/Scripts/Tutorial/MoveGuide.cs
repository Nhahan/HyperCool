using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class MoveGuide : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private List<GameObject> setActiveAfter;
    [SerializeField] private GameObject guideText;
    
    private RectTransform guideRect;
    private Vector2 guideStartPos;
    private float guideAcceleration;

    private Vector3 firstPlayerPos;

    private void Start()
    {
        PauseOff();
        guideRect = GetComponent<RectTransform>();
        guideStartPos = guideRect.anchoredPosition;

        firstPlayerPos = Player.I.transform.position;
    }

    private void Update()
    {
        guideAcceleration += Time.deltaTime + 0.1f;
        guideRect.anchoredPosition += new Vector2(0, 100 * Time.deltaTime * guideAcceleration / Time.timeScale);
        if (guideRect.anchoredPosition.y > -500)
        {
            guideRect.anchoredPosition = guideStartPos;
            guideAcceleration = 0;
        }

        if (Vector3.Distance(firstPlayerPos, Player.I.transform.position) > 13.5f)
        {
            GameManager.I.pause = true;
            playerController.SetVelocityToZero();
            Destroy(guideText);
            Destroy(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        setActiveAfter.ForEach(o => o.SetActive(true));
    }

    private void PauseOff()
    {
        GameManager.I.pause = false;
    }
}
