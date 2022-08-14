using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextStage : MonoBehaviour
{
    [SerializeField] private Image endDim;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            GameManager.I.clear = true;
            StartCoroutine(EndDim());
        }
    }

    private IEnumerator EndDim()
    {
        var i = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            endDim.color += new Color(0, 0, 0, 0.01f);
            i++;
            if (i > 100) break;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
