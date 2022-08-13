using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool testMode;
    
    private Rigidbody rb;
    private float defaultFixedDeltaTime;

    private float verticalSpeed;
    private float horizontalSpeed;
    
    private bool action;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (!GameManager.I.gameOver)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                StopCoroutine(ActionRoutine(0.03f));
                StartCoroutine(ActionRoutine(0.25f));
            }
        }

        TimeScale();
    }

    private void TimeScale()
    {
        if (testMode) return;
        
        if (GameManager.I.pause)
        {
            Time.timeScale = 1;
            return;
        }
        
        rb.velocity = new Vector3(variableJoystick.Horizontal, 0, variableJoystick.Vertical) *
                      (movementSpeed * Time.deltaTime);

        var x = Mathf.Abs(rb.velocity.x);
        var z = Mathf.Abs(rb.velocity.z);

        var time = (x != 0 || z != 0) ? 1f : 0.15f;
        var lerpTime = (x != 0 || z != 0) ? 0.05f : 0.5f;

        time = action ? 1 : time;
        lerpTime = action ? 0.05f : lerpTime;

        Time.timeScale = Mathf.Lerp(Time.timeScale, time, lerpTime);
        Time.fixedDeltaTime = Time.timeScale switch
        {
            < 0.3f => 0.004f,
            _ => defaultFixedDeltaTime
        };
    }

    private IEnumerator ActionRoutine(float time)
    {
        action = true;
        yield return new WaitForSeconds(time);
        action = false;
    }
}

