using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIEffects : MonoBehaviour
{
    [SerializeField] private GameObject perfect;
    
    public static UIEffects I;

    private void Awake()
    {
        if (I == null)
        {
            I = this;
        } 
        else if (I != this)
        {
            Destroy(gameObject);
        }
    }

    public void Perfect()
    {
        perfect.SetActive(true);
        StartCoroutine(OnPerfect());
    }
    
    private IEnumerator OnPerfect()
    {
        var rect = perfect.GetComponent<RectTransform>();

        float acceleration = 0;
        var isUp = true;
        while (true)
        {
            acceleration += Time.deltaTime + 1f;
            yield return new WaitForSeconds(0.01f);
            if (rect.localScale.x < 1.1) 
            {
                rect.localScale += new Vector3(0.09f, 0.09f, 0.09f);
            }
            
            if (isUp) {
                if (rect.anchoredPosition.y < -100)
                {
                    rect.anchoredPosition += new Vector2(0, 400 * Time.deltaTime * acceleration / Time.timeScale);
                }
                else
                {
                    acceleration = 10f;
                    isUp = false;
                }
            }
            else // if isUp is true
            {
                if (rect.anchoredPosition.y > -150)
                {
                    rect.anchoredPosition -= new Vector2(0, 400 * Time.deltaTime * acceleration / Time.timeScale);
                }
                else
                {
                    break;
                }
            }
        }

        StartCoroutine(SetActiveFalse(perfect, 0.65f));
    }

    private IEnumerator SetActiveFalse(GameObject o, float sec)
    {
        yield return new WaitForSeconds(sec);
        o.SetActive(false);
    }
}
