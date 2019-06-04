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
    public GameObject buttonMarket;
    int menuArrayNumber;
    public Color buttonSelectedColor;
    GameObject selectedButton;
    bool hasMoved, movementOn;
    float timerMovement;
    int numberOfButtons;

    bool activeCanvas;
    GameObject cameraAnchor;

    public static float persWood, persWood2;
    public static float persFabric, persFabric2;
    public static float persRock, persRock2;


    void Start()
    {
        activeCanvas = false;
        cameraAnchor = Camera.main.transform.parent.gameObject;

        ResultTextsInitialization();
        TotalTextsInitialization();

        if (GameManager.Instance.Market)
            buttonMarket.SetActive(true);
    }


    void Update()
    {
        if (!activeCanvas)
        {
            transform.GetChild(0).localPosition = Vector3.MoveTowards(transform.GetChild(0).localPosition, Vector3.zero, GameManager.cameraSpeed * Time.deltaTime * 3.5f);

            if (!GameManager.movingCamera)
            {
                menuArrayNumber = 0;
                selectedButton = buttonsArray[0];
                selectedButton.GetComponent<Image>().color = buttonSelectedColor;
                numberOfButtons = buttonsArray.Length - 1;

                activeCanvas = true;
            }
        }
        else
            UpdateBetweenIslandMenuButtons();

        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.totalCoins += 500;
            SaveManager.Instance.Save();
            totalEndCoinsText.text = GameManager.totalCoins.ToString();
        }
    }




    void ResultTextsInitialization()
    {
        if (GameManager.Instance.currentCoins != 0)
        {
            if (GameManager.Instance.CheckSecondaryObjective()) resultEndCoinsText.text = GameManager.Instance.currentCoins.ToString() + " + 50";
            else resultEndCoinsText.text = GameManager.Instance.currentCoins.ToString();
            resultEndCoinsText.color = positiveColor;
        }

        if (GameManager.gameWon)
        {
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
                selectedButton.GetComponent<Image>().color = Color.white;

                if (menuArrayNumber > numberOfButtons)
                    menuArrayNumber = 0;

                if (menuArrayNumber == numberOfButtons && GameManager.Instance.Market)
                    selectedButton = buttonMarket;
                else
                    selectedButton = buttonsArray[menuArrayNumber];

                selectedButton.GetComponent<Image>().color = buttonSelectedColor;
                hasMoved = true;
            }
            else if (vertical > 0.2f)
            {
                menuArrayNumber--;
                selectedButton.GetComponent<Image>().color = Color.white;

                if (menuArrayNumber < 0)
                    menuArrayNumber = numberOfButtons;

                if (menuArrayNumber == numberOfButtons && GameManager.Instance.Market)
                    selectedButton = buttonMarket;
                else
                    selectedButton = buttonsArray[menuArrayNumber];

                selectedButton.GetComponent<Image>().color = buttonSelectedColor;
                hasMoved = true;
            }
            else if (horizontal > 0.2f && menuArrayNumber == numberOfButtons && selectedButton != buttonsArray[menuArrayNumber] && GameManager.Instance.Market)
            {
                selectedButton.GetComponent<Image>().color = Color.white;
                selectedButton = buttonsArray[menuArrayNumber];
                selectedButton.GetComponent<Image>().color = buttonSelectedColor;
                hasMoved = true;
            }
            else if (horizontal < -0.2f && menuArrayNumber == numberOfButtons && selectedButton != buttonMarket && GameManager.Instance.Market)
            {
                selectedButton.GetComponent<Image>().color = Color.white;
                selectedButton = buttonMarket;
                selectedButton.GetComponent<Image>().color = buttonSelectedColor;
                hasMoved = true;
            }
            else if (InputManager.Instance.GetInputDown("Submit") && gameObject.activeSelf)
                selectedButton.GetComponent<Button>().onClick.Invoke();
        }

        if (hasMoved)
            movementOn = true;

        if ((vertical <= 0.1f && vertical >= -0.1f && horizontal <= 0.1f && horizontal >= -0.1f && hasMoved) || timerMovement >= 0.3f)
        {
            hasMoved = false;
            timerMovement = 0;
            movementOn = false;
        }

        if (movementOn)
            timerMovement += Time.deltaTime;
    }


    public void UpdateTexts(string type)
    {
        switch (type)
        {
            case "wood":
                totalEndWoodText.text = GameManager.totalWood.ToString();
                break;
            case "rock":
                totalEndRockText.text = GameManager.totalRock.ToString();
                break;
            case "fabric":
                totalEndFabricText.text = GameManager.totalFabrics.ToString();
                break;
            case "wood2":
                totalEndWoodTier2Text.text = GameManager.totalWood2.ToString();
                break;
            case "rock2":
                totalEndRockTier2Text.text = GameManager.totalRock2.ToString();
                break;
            case "fabric2":
                totalEndFabricTier2Text.text = GameManager.totalFabrics2.ToString();
                break;

            case "moneyForWood":
                totalEndWoodText.text = GameManager.totalWood.ToString();
                break;
            case "moneyForRocks":
                totalEndRockText.text = GameManager.totalRock.ToString();
                break;
            case "moneyForFabrics":
                totalEndFabricText.text = GameManager.totalFabrics.ToString();
                break;
            case "moneyForWood2":
                totalEndWoodTier2Text.text = GameManager.totalWood2.ToString();
                break;
            case "moneyForRocks2":
                totalEndRockTier2Text.text = GameManager.totalRock2.ToString();
                break;
            case "moneyForFabrics2":
                totalEndFabricTier2Text.text = GameManager.totalFabrics2.ToString();
                break;
        }

        totalEndCoinsText.text = GameManager.totalCoins.ToString();
    }
}
