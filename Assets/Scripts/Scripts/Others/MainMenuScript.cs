﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public Color buttonSelectedColor;
    public GameObject[] buttonsArrayDef;
    int arrayNumber;
    int numberOfButtons;

    bool hasMoved, movementOn;
    float timerMovement;

    bool inOptions;
    public GameObject optionsScreen;

    bool loadScene;
    public GameObject loadingScreen;
    public Transform loadingIcon;
    public GameObject cameraMenu;
    public GameObject continueText;
    bool almostCompleted;


    void Start()
    {
        arrayNumber = 0;
        numberOfButtons = buttonsArrayDef.Length;
        hasMoved = false;
        loadScene = false;
        almostCompleted = false;
    }

    void Update()
    {
        if (!inOptions && !loadScene)
        {
            UpdateButtons();

            if (InputManager.Instance.GetInputDown("Submit"))
            {
                SoundManager.PlayOneShot(SoundManager.ButtonClicked, cameraMenu.transform.position);
                switch (arrayNumber)
                {
                    case 0:
                        ResetAll();
                        loadScene = true;
                        loadingScreen.SetActive(true);
                        StartCoroutine(LoadNewScene(1));
                        break;
                    case 1:
                        loadScene = true;
                        loadingScreen.SetActive(true);
                        StartCoroutine(LoadNewScene(1));
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
        else if (InputManager.Instance.GetInputDown("Cancel") && inOptions)
        {
            SoundManager.PlayOneShot(SoundManager.ButtonClicked, cameraMenu.transform.position);
            inOptions = false;
            optionsScreen.SetActive(false);
        }
        else if (loadScene)
            loadingIcon.localEulerAngles -= new Vector3(0, 0, 200f) * InputManager.deltaTime;
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
                SoundManager.PlayOneShot(SoundManager.PassButton, cameraMenu.transform.position);
                buttonsArrayDef[arrayNumber].SetActive(true);
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
            timerMovement += InputManager.deltaTime;
    }


    void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        AxerAbilities.initialized = false;
        BomberAbilities.initialized = false;
        SwordAbilities.initialized = false;
        CharacterAbiliities.initialized = false;

        GameManager.mastil = false;
        GameManager.timon = false;
        GameManager.brujula = false;
        GameManager.casco = false;
        GameManager.mapa = false;
        GameManager.velas = false;
        GameManager.cañon = false;
        GameManager.remos = false;
    }


    IEnumerator LoadNewScene(int sceneNumber)
    {
        AsyncOperation async = null;

        async = SceneManager.LoadSceneAsync(sceneNumber);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            if (async.progress >= 0.9f)
            {
                if (!almostCompleted)
                {
                    continueText.SetActive(true);
                    loadingIcon.parent.gameObject.SetActive(false);
                    almostCompleted = true;
                }
                else if (InputManager.Instance.GetInputDown("Submit") || InputManager.Instance.GetInputDown("Cancel"))
                    async.allowSceneActivation = true;
            }
            
            yield return null;
        }
    }
}