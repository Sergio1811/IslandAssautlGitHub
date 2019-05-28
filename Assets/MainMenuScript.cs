using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public Color buttonSelectedColor;
    public Image[] buttonsArray;
    Image actualImage;
    int arrayNumber;
    int numberOfButtons;

    bool hasMoved, movementOn;
    float timerMovement;

    void Start()
    {
        arrayNumber = 0;
        numberOfButtons = buttonsArray.Length;
        actualImage = buttonsArray[0];
        actualImage.color = buttonSelectedColor;
        hasMoved = false;
    }
    
    void Update()
    {
        float vertical = -InputManager.Instance.GetAxis("Vertical");
        if (vertical == 0)
            vertical = InputManager.Instance.GetAxis("CameraMovementY");

        if (!hasMoved)
        {
            if (vertical > 0.5f)
            {
                hasMoved = true;
                arrayNumber++;
                if (arrayNumber >= numberOfButtons)
                    arrayNumber = 0;
            }
            else if (vertical < -0.5f)
            {
                hasMoved = true;
                arrayNumber--;
                if (arrayNumber < 0)
                    arrayNumber = numberOfButtons - 1;
            }
            
            if (hasMoved)
            {
                actualImage.color = Color.white;
                actualImage = buttonsArray[arrayNumber];
                actualImage.color = buttonSelectedColor;
                movementOn = true;
            }
        }

        if ((vertical <= 0.1f && vertical >= -0.1f && hasMoved) || timerMovement >= 0.3f)
        {
            hasMoved = false;
            timerMovement = 0;
            movementOn = false;
        }

        if (movementOn)
            timerMovement += Time.deltaTime;


        if (InputManager.Instance.GetInputDown("Submit"))
        {
            switch(arrayNumber)
            {
                case 0:
                    SceneManager.LoadScene(1);
                    break;
                case 1:
                    //options
                    break;
                case 2:
                    Application.Quit();
                    break;
            }
        }
    }
}
