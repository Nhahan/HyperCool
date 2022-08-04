using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    
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
        
    }
    
    private void FixedUpdate()
    {
        var gameSpeed = rb.velocity.magnitude / 5f;
        GameManager.I.gameSpeed = gameSpeed > 0.05f ? gameSpeed : 0 ;
    }
}
