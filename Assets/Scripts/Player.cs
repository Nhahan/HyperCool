using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    public static Player I;
    
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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
}
    