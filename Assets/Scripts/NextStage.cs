using System.Collections;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
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
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            endDim.color += new Color(0, 0, 0, 0.01f);
            if (endDim.color.a == 255) break;
        }
        Debug.Log("End");
    }
}
