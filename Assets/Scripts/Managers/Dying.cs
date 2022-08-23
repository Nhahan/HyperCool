using UnityEngine;

public class Dying : MonoBehaviour
{
    [SerializeField] private GameObject dead;

    private void SetDeadActive()
    {
        Time.timeScale = 1;
        dead.SetActive(true);
    }
}
