using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private FirstPersonMovement controller;
    public static Player I;

    [SerializeField] private bool testMode;

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
        controller = GetComponent<FirstPersonMovement>();
    }

    // private void FixedUpdate()
    // {
    //     if (testMode) return; 
    //     
    //     var gameSpeed = rb.velocity.magnitude / 5f;
    //
    //     Time.timeScale = gameSpeed == 0f ? 0.1f : gameSpeed;
    //     controller.speed *= Time.timeScale * (1 / Time.timeScale);
    // }
}
    