using UnityEngine;

public class Dying : MonoBehaviour
{
    [SerializeField]private GameObject dead;

    private void SetDeadActive()
    {
        dead.SetActive(true);
    }
}
