using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dim : MonoBehaviour
{
    private Image image;
    
    private void Start()
    {
        image = GetComponent<Image>();
        
        StartCoroutine(EndDim());
    }

    private IEnumerator EndDim()
    {
        var i = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            image.color += new Color(0, 0, 0, 0.01f);
            i++;
            if (i > 100) break;
        }
    }
}
