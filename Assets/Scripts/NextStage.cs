using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextStage : MonoBehaviour
{
    private Image endDim;
    
    private void Start()
    {
        endDim = GetComponent<Image>();
        
        StartCoroutine(EndDim());
    }

    private IEnumerator EndDim()
    {
        GameManager.I.pause = true;
        var i = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            try
            {
                endDim.color += new Color(0, 0, 0, 0.0075f);
            }
            catch
            {
                // ignored
            }

            i++;
            if (i > 254) break;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
