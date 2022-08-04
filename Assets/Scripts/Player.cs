using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerControls controls;
    private CharacterController controller;
    private Rigidbody rb;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    
    [SerializeField] private float playerSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float slowDownSpeed;
    // [SerializeField] private float jumpHeight = 1.0f;
    // private float gravityValue = -9.81f;

    private Vector2 move;
    private Vector2 rotate;

    private float xRotation;

    public static Player Instance;
    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        } 
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        
        controls = new PlayerControls();

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        
        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;
    }
    
    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            SlowDown();
        }
    }

    private void FixedUpdate()
    {
        // groundedPlayer = controller.isGrounded;
        // if (groundedPlayer && playerVelocity.y < 0)
        // {
        //     playerVelocity.y = 0f;
        // }
        //
        // var movementInput = controls.Gameplay.Move.ReadValue<Vector2>();
        // // var move = new Vector3(movementInput.x * x, 0f, movementInput.y * y);
        // // controller.Move(move * (Time.deltaTime * playerSpeed));
        //
        // controller.transform.Rotate(new Vector3(movementInput.x, 0, 0) * (playerSpeed * Time.deltaTime));
        // controller.Move(new Vector3(0, 0, movementInput.y) * (playerSpeed * Time.deltaTime));
        //
        // if (movementInput != Vector2.zero)
        // {
        //     gameObject.transform.forward = movementInput;
        // }
        //
        // // Changes the height position of the player..
        // // if (Input.GetButtonDown("Jump") && groundedPlayer)
        // // {
        // //     playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        // // }
        //
        // playerVelocity.y += gravityValue * Time.deltaTime;
        // controller.Move(playerVelocity * Time.deltaTime);
        // if (Input.GetKey("w"))
        // {
        //     var x = move.normalized.x;
        //     var y = move.normalized.y;
        //     rb.velocity = new Vector3(x * playerSpeed, 0, y * playerSpeed);
        // }

        // rb.velocity = new Vector3(move.x * playerSpeed, 0, move.y * playerSpeed);

        // var xRotation = rotate.y * Time.deltaTime;
        // xRotation = Mathf.Clamp(xRotation, -80, 80);
        // Debug.Log("xRotation:" + xRotation);
        //
        // transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        // transform.Rotate(Vector3.up * (xRotation * rotateSpeed *Time.deltaTime), Space.World);
        transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
    }

    private void SlowDown()
    {
        if (rb.velocity == Vector3.zero) return;

        move *= slowDownSpeed;
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
