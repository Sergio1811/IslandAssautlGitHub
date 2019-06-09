using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { set; get; }

    [HideInInspector]
    public bool psController = false;
    [HideInInspector]
    public bool xboxController = false;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CheckConnectedController();
    }

    void CheckConnectedController()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            if (names[x].ToUpper().Contains("XBOX"))
            {
                psController = false;
                xboxController = true;

            }
            else if (names[x].ToUpper() != "")
            {
                psController = true;
                xboxController = false;
            }
        }
    }


    public bool GetInputDown(string input)
    {
        if (psController)
            return Input.GetButtonDown("PS_" + input);

        else if (xboxController)
            return Input.GetButtonDown("XBOX_" + input);

        else
            return Input.GetButtonDown(input);

    }

    public bool GetInput(string input)
    {
        if (psController)
            return Input.GetAxisRaw("PS_" + input) > 0.2;

        else if (xboxController)
            return Input.GetAxisRaw("XBOX_" + input) > 0.2;

        else
            return Input.GetAxisRaw(input) > 0.2;
    }

    public bool GetInputUp(string input)
    {
        if (psController)
            return Input.GetAxisRaw("PS_" + input) < 0.2;

        else if (xboxController)
            return Input.GetAxisRaw("XBOX_" + input) < 0.2;

        else
            return Input.GetAxisRaw(input) < 0.2;
    }

    public float GetAxis(string input)
    {
        if (psController)
            return Input.GetAxisRaw("PS_" + input);

        else if (xboxController)
            return Input.GetAxisRaw("XBOX_" + input);

        else
            return Input.GetAxisRaw(input);
    }
}