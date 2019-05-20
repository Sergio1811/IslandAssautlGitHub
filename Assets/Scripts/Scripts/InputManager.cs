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
                //print("XBOX ONE CONTROLLER CONNECTED");
                psController = false;
                xboxController = true;

            }
            else
            {
                //print("PS4 CONTROLLER CONNECTED");
                psController = true;
                xboxController = false;
            }
        }

    }

    public bool GetInputDown(string input)
    {
        if (psController)
            return Input.GetButtonDown("PS_" + input);

        if (xboxController)
            return Input.GetButtonDown("XBOX_" + input);

        return false;
    }

    public bool GetInput(string input)
    {
        if (psController)
            return Input.GetAxisRaw("PS_" + input) > 0.2;

        if (xboxController)
            return Input.GetAxisRaw("XBOX_" + input) > 0.2;

        return false;
    }

    public bool GetInputUp(string input)
    {
        if (psController)
            return Input.GetAxisRaw("PS_" + input) < 0.2;

        if (xboxController)
            return Input.GetAxisRaw("XBOX_" + input) < 0.2;

        return false;
    }

    public float GetAxis(string input)
    {
        if (psController)
            return Input.GetAxisRaw("PS_" + input);

        if (xboxController)
            return Input.GetAxisRaw("XBOX_" + input);

        return 0;
    }
}
