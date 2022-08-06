using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickSetterExample : MonoBehaviour
{
    public VariableJoystick variableJoystick;

    public void Start()
    {
        variableJoystick.SetMode(JoystickType.Floating);
        variableJoystick.AxisOptions = AxisOptions.Both;
    }
}