using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MidGameMenuScript : MonoBehaviour
{
    public static MidGameMenuScript Instance { set; get; }

    public Color positiveColor;
    public Color negativeColor;

    #region Total resources text

    public Text totalEndCoinsText;
    public Text totalEndWoodText;
    public Text totalEndRockText;
    public Text totalEndFabricText;
    public Text totalEndWoodTier2Text;
    public Text totalEndRockTier2Text;
    public Text totalEndFabricTier2Text;

    #endregion
    #region Result resources text
    public Text resultEndCoinsText;
    public Text resultEndWoodText;
    public Text resultEndRocksText;
    public Text resultEndFabricText;
    public Text resultEndWoodTier2Text;
    public Text resultEndRockTier2Text;
    public Text resultEndFabricsTier2Text;
    #endregion

    public GameObject[] buttonsArray;
    public GameObject buttonMarket, buttonControls;
    int menuArrayNumber;
    public Color buttonSelectedColor;
    GameObject selectedButton;
    Image selectedButtonImage;
    bool hasMoved, movementOn;
    float timerMovement;
    int numberOfButtons;

    bool activeCanvas;
    GameObject cameraAnchor;

    public static float persWood, persWood2;
    public static float persFabric, persFabric2;
    public static float persRock, persRock2;

    public GameObject boatParts;
    public Image[] boatPartsIcons;
    public Transform positionParts;

    bool inOptions;
    public GameObject optionsScreen;


    void Start()
    {
        activeCanvas = false;
        cameraAnchor = Camera.main.transform.parent.gameObject;

        ResultTextsInitialization();
        TotalTextsInitialization();
        BoatPartsInitialization();

        if (GameManager.Instance.Market)
            buttonMarket.SetActive(true);
    }

    private void OnEnable()
    {
        if (activeCanvas)
        {
            TotalTextsInitialization();
            BoatPartsInitialization();
            if (GameManager.Instance.Market)
                buttonMarket.SetActive(true);
        }
    }


    void Update()
    {
        if (!activeCanvas)
        {
            transform.GetChild(0).localPosition = Vector3.MoveTowards(transform.GetChild(0).localPosition, Vector3.zero, GameManager.cameraSpeed * Time.deltaTime * 3.5f);
            boatParts.transform.localPosition = Vector3.MoveTowards(boatParts.transform.localPosition, positionParts.localPosition, GameManager.cameraSpeed * Time.deltaTime * 0.9f);

            if (!GameManager.movingCamera)
            {
                menuArrayNumber = 0;
                selectedButton = buttonsArray[0];
                selectedButtonImage = selectedButton.GetComponent<Image>();
                selectedButtonImage.color = buttonSelectedColor;
                numberOfButtons = buttonsArray.Length - 1;

                activeCanvas = true;
            }
        }
        else if (!inOptions)
            UpdateBetweenIslandMenuButtons();
        else if (InputManager.Instance.GetInputDown("Cancel"))
        {
            GameManager.Instance.soundManager.PlayOneShot(GameManager.Instance.soundManager.ButtonClicked, this.transform.position);
            inOptions = false;
            optionsScreen.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.P))
            TotalTextsInitialization();
    }


    void ResultTextsInitialization()
    {
        if (GameManager.Instance.currentCoins != 0)
        {
            if (GameManager.Instance.CheckSecondaryObjective()) resultEndCoinsText.text = "+" + GameManager.Instance.currentCoins.ToString() + " + 50";
            else resultEndCoinsText.text = "+" + GameManager.Instance.currentCoins.ToString();
            resultEndCoinsText.color = positiveColor;
        }

        GameManager.Instance.currentCoins = 0;

        if (GameManager.gameWon)
        {
            GameManager.Instance.soundManager.PlayOneShot(GameManager.Instance.soundManager.WinIsland, this.transform.position);
            if (GameManager.Instance.collectedFabrics > 0)
            {
                resultEndFabricText.text = "+" + GameManager.Instance.collectedFabrics.ToString();
                resultEndFabricText.color = positiveColor;
            }
            if (GameManager.Instance.collectedRock > 0)
            {
                resultEndRocksText.text = "+" + GameManager.Instance.collectedRock.ToString();
                resultEndRocksText.color = positiveColor;
            }
            if (GameManager.Instance.collectedWood > 0)
            {
                resultEndWoodText.text = "+" + GameManager.Instance.collectedWood.ToString();
                resultEndWoodText.color = positiveColor;
            }
            if (GameManager.Instance.collectedFabrics2 > 0)
            {
                resultEndFabricsTier2Text.text = "+" + GameManager.Instance.collectedFabrics2.ToString();
                resultEndFabricsTier2Text.color = positiveColor;
            }
            if (GameManager.Instance.collectedRock2 > 0)
            {
                resultEndRockTier2Text.text = "+" + GameManager.Instance.collectedRock2.ToString();
                resultEndRockTier2Text.color = positiveColor;
            }
            if (GameManager.Instance.collectedWood2 > 0)
            {
                resultEndWoodTier2Text.text = "+" + GameManager.Instance.collectedWood2.ToString();
                resultEndWoodTier2Text.color = positiveColor;
            }
        }
        else
        {
            GameManager.Instance.soundManager.PlayOneShot(GameManager.Instance.soundManager.LoseIsland, this.transform.position);
            if (persFabric > 0)
            {
                resultEndFabricText.text = "-" + persFabric.ToString();
                resultEndFabricText.color = negativeColor;
            }
            if (persRock > 0)
            {
                resultEndRocksText.text = "-" + persRock.ToString();
                resultEndRocksText.color = negativeColor;
            }
            if (persWood > 0)
            {
                resultEndWoodText.text = "-" + persWood.ToString();
                resultEndWoodText.color = negativeColor;
            }
            if (persFabric2 > 0)
            {
                resultEndFabricsTier2Text.text = "-" + persFabric2.ToString();
                resultEndFabricsTier2Text.color = negativeColor;
            }
            if (persRock2 > 0)
            {
                resultEndRockTier2Text.text = "-" + persRock2.ToString();
                resultEndRockTier2Text.color = negativeColor;
            }
            if (persWood2 > 0)
            {
                resultEndWoodTier2Text.text = "-" + persWood2.ToString();
                resultEndWoodTier2Text.color = negativeColor;
            }
        }
    }

    void TotalTextsInitialization()
    {
        totalEndCoinsText.text = GameManager.totalCoins.ToString();

        totalEndWoodText.text = GameManager.totalWood.ToString();
        totalEndRockText.text = GameManager.totalRock.ToString();
        totalEndFabricText.text = GameManager.totalFabrics.ToString();
        totalEndWoodTier2Text.text = GameManager.totalWood2.ToString();
        totalEndRockTier2Text.text = GameManager.totalRock2.ToString();
        totalEndFabricTier2Text.text = GameManager.totalFabrics2.ToString();


        if (BomberAbilities.rockTier2)
        {
            totalEndRockTier2Text.transform.parent.gameObject.SetActive(true);
            resultEndRockTier2Text.transform.parent.gameObject.SetActive(true);
        }
        if (SwordAbilities.enemyTier2)
        {
            totalEndFabricTier2Text.transform.parent.gameObject.SetActive(true);
            resultEndFabricsTier2Text.transform.parent.gameObject.SetActive(true);
        }
        if (AxerAbilities.treeTier2)
        {
            totalEndWoodTier2Text.transform.parent.gameObject.SetActive(true);
            resultEndWoodTier2Text.transform.parent.gameObject.SetActive(true);
        }
    }

    public void ActivateOptions()
    {
        GameManager.Instance.soundManager.PlayOneShot(GameManager.Instance.soundManager.ButtonClicked, this.transform.position);
        inOptions = true;
        optionsScreen.SetActive(true);
    }

    void BoatPartsInitialization()
    {
        if (GameManager.mastil)
            boatPartsIcons[0].color = Color.white;
        if (GameManager.timon)
            boatPartsIcons[1].color = Color.white;
        if (GameManager.velas)
            boatPartsIcons[2].color = Color.white;
        if (GameManager.mapa)
            boatPartsIcons[3].color = Color.white;
        if (GameManager.casco)
            boatPartsIcons[4].color = Color.white;
        if (GameManager.cañon)
            boatPartsIcons[5].color = Color.white;
        if (GameManager.remos)
            boatPartsIcons[6].color = Color.white;
        if (GameManager.brujula)
            boatPartsIcons[7].color = Color.white;
    }

    void UpdateBetweenIslandMenuButtons()
    {
        cameraAnchor.transform.Rotate(new Vector3(0, 5f * Time.deltaTime, 0));

        float horizontal = InputManager.Instance.GetAxis("Horizontal");
        float vertical = InputManager.Instance.GetAxis("Vertical");

        if (!hasMoved)
        {
            if (vertical < -0.2f)
            {
                menuArrayNumber++;
                selectedButtonImage.color = Color.white;

                if (menuArrayNumber > numberOfButtons)
                    menuArrayNumber = 0;

                if (menuArrayNumber == numberOfButtons && selectedButton == buttonControls)
                    selectedButton = buttonsArray[menuArrayNumber];
                else if (menuArrayNumber == numberOfButtons && GameManager.Instance.Market)
                    selectedButton = buttonMarket;
                else
                    selectedButton = buttonsArray[menuArrayNumber];

                selectedButtonImage = selectedButton.GetComponent<Image>();
                selectedButtonImage.color = buttonSelectedColor;
                hasMoved = true;
            }
            else if (vertical > 0.2f)
            {
                menuArrayNumber--;
                selectedButtonImage.color = Color.white;

                if (menuArrayNumber < 0)
                    menuArrayNumber = numberOfButtons;

                if (menuArrayNumber == numberOfButtons - 1 && selectedButton == buttonsArray[numberOfButtons])
                    selectedButton = buttonControls;
                else if (menuArrayNumber == numberOfButtons && GameManager.Instance.Market)
                    selectedButton = buttonMarket;
                else
                    selectedButton = buttonsArray[menuArrayNumber];

                selectedButtonImage = selectedButton.GetComponent<Image>();
                selectedButtonImage.color = buttonSelectedColor;
                hasMoved = true;
            }
            else if (horizontal > 0.2f)
            {
                if (menuArrayNumber == numberOfButtons && GameManager.Instance.Market && selectedButton != buttonsArray[menuArrayNumber])
                {
                    selectedButtonImage.color = Color.white;
                    selectedButton = buttonsArray[menuArrayNumber];
                    selectedButtonImage = selectedButton.GetComponent<Image>();
                    selectedButtonImage.color = buttonSelectedColor;
                    hasMoved = true;
                }
                else if (menuArrayNumber == numberOfButtons - 1 && selectedButton != buttonControls)
                {
                    selectedButtonImage.color = Color.white;
                    selectedButton = buttonControls;
                    selectedButtonImage = selectedButton.GetComponent<Image>();
                    selectedButtonImage.color = buttonSelectedColor;
                    hasMoved = true;
                }

            }
            else if (horizontal < -0.2f)
            {
                if (menuArrayNumber == numberOfButtons && selectedButton != buttonMarket && GameManager.Instance.Market)
                {
                    selectedButtonImage.color = Color.white;
                    selectedButton = buttonMarket;
                    selectedButtonImage = selectedButton.GetComponent<Image>();
                    selectedButtonImage.color = buttonSelectedColor;
                    hasMoved = true;
                }
                else if (menuArrayNumber == numberOfButtons - 1 && selectedButton != buttonsArray[menuArrayNumber])
                {
                    selectedButtonImage.color = Color.white;
                    selectedButton = buttonsArray[menuArrayNumber];
                    selectedButtonImage = selectedButton.GetComponent<Image>();
                    selectedButtonImage.color = buttonSelectedColor;
                    hasMoved = true;
                }
            }
            else if (InputManager.Instance.GetInputDown("Submit") && gameObject.activeSelf)
            {
                GameManager.Instance.soundManager.PlayOneShot(GameManager.Instance.soundManager.ButtonClicked, this.transform.position);
                selectedButton.GetComponent<Button>().onClick.Invoke();
            }
        }

        if (hasMoved)
        {
            movementOn = true;
            GameManager.Instance.soundManager.PlayOneShot(GameManager.Instance.soundManager.PassButton, this.transform.position);
        }

        if ((vertical <= 0.1f && vertical >= -0.1f && horizontal <= 0.1f && horizontal >= -0.1f && hasMoved) || timerMovement >= 0.3f)
        {
            hasMoved = false;
            timerMovement = 0;
            movementOn = false;
        }

        if (movementOn)
            timerMovement += Time.deltaTime;
    }
}