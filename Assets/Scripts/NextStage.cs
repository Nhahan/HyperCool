using Managers;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Next Stage");
            GameManager.I.clear = true;
        }
    }
}
