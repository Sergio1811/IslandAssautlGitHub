using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public Color buttonSelectedColor;
    public Image[] buttonsArray;
    public GameObject[] buttonsArrayDef;
    Image actualImage;
    int arrayNumber;
    int numberOfButtons;

    bool hasMoved, movementOn;
    float timerMovement;

    bool inOptions;
    public GameObject optionsScreen;

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
        if (!inOptions)
        {
            UpdateButtons();

            if (InputManager.Instance.GetInputDown("Submit"))
            {
                GameManager.Instance.soundManager.PlayOneShot(GameManager.Instance.soundManager.ButtonClicked, this.transform.position);
                switch (arrayNumber)
                {
                    case 0:
                        PlayerPrefs.DeleteAll();
                        PlayerPrefs.Save();
                        SceneManager.LoadScene(1);
                        break;
                    case 1:
                        SceneManager.LoadScene(1);
                        break;
                    case 2:
                        inOptions = true;
                        optionsScreen.SetActive(true);
                        break;
                    case 3:
                        Application.Quit();
                        break;
                }
            }
        }
        else if (InputManager.Instance.GetInputDown("Cancel"))
        {
            GameManager.Instance.soundManager.PlayOneShot(GameManager.Instance.soundManager.ButtonClicked, this.transform.position);
            inOptions = false;
            optionsScreen.SetActive(false);
        }
    }


    void UpdateButtons()
    {
        float vertical = -InputManager.Instance.GetAxis("Vertical");
        if (vertical == 0)
            vertical = InputManager.Instance.GetAxis("CameraMovementY");

        if (!hasMoved)
        {
            if (vertical > 0.5f)
            {
                buttonsArrayDef[arrayNumber].SetActive(false);
                hasMoved = true;
                arrayNumber++;
                if (arrayNumber >= numberOfButtons)
                    arrayNumber = 0;
            }
            else if (vertical < -0.5f)
            {
                buttonsArrayDef[arrayNumber].SetActive(false);
                hasMoved = true;
                arrayNumber--;
                if (arrayNumber < 0)
                    arrayNumber = numberOfButtons - 1;
            }

            if (hasMoved)
            {
                GameManager.Instance.soundManager.PlayOneShot(GameManager.Instance.soundManager.PassButton, this.transform.position);
                buttonsArrayDef[arrayNumber].SetActive(true);
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
    }
}