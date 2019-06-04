using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketManager : MonoBehaviour
{
    public GameObject backMenu;

    public Text totalCoins;
    public Text totalWood, totalWood2, totalRock, totalRocks2, totalFabric, totalFabrics2;
    public GameObject totalWood2Object, totalRock2Object, totalFabrics2Object;
    public GameObject backCircle, backB;
    public Text selectedProduct_Name;
    public Text selectedProduct_Text;
    public GameObject buyText, insufficientText;
    public GameObject buyTriangle, buyY;
    public Text buyPrice;

    GameObject actualIcono, actualSelectedImage;
    public GameObject iconoMoneda, iconoMadera, iconoMaderaT2, iconoRoca, iconoRocaT2, iconoPieles, iconoPielesT2;

    int arrayPositionX, arrayPositionY;
    public GameObject[] arrayWood, arrayRock, arrayFabrics;
    GameObject[,] imagesMatrix = new GameObject[3, 4];
    Product[,] productsMatrix = new Product[3, 4];
    Product actualProduct;

    public int childSelectedNumber, childTextNumber;

    bool canBuy, hasMoved;

    float timerMovement;
    bool movementOn = false;
    

    private void OnEnable()
    {
        backMenu.SetActive(false);

        MatrixInitialization();
        UpdateTotals();

        arrayPositionX = 0;
        arrayPositionY = 0;

        if (InputManager.Instance.psController)
        {
            backCircle.SetActive(true);
            buyTriangle.SetActive(true);
        }
        else
        {
            backB.SetActive(true);
            buyY.SetActive(true);
        }

        canBuy = false;
        hasMoved = false;

        UpdateComponents();
    }


    private void Update()
    {
        if (InputManager.Instance.GetInputDown("Cancel"))
        {
            backMenu.SetActive(true);
            gameObject.SetActive(false);
        }

        if (canBuy && InputManager.Instance.GetInputDown("Buy"))
            BuyProduct();

        UpdateButtons();
    }

    private void OnDisable()
    {
        if (actualIcono != null)
            actualIcono.SetActive(false);
        if (actualSelectedImage != null)
            actualSelectedImage.SetActive(false);
    }

    
    void MatrixInitialization()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (i == 0)
                    imagesMatrix[i, j] = arrayWood[j];
                else if (i == 1)
                    imagesMatrix[i, j] = arrayRock[j];
                else if (i == 2)
                    imagesMatrix[i, j] = arrayFabrics[j];

                productsMatrix[i, j] = imagesMatrix[i, j].GetComponent<MarketDisplay>().MarketProduct;

                imagesMatrix[i, j].transform.GetChild(childTextNumber).GetComponent<Text>().text = "x" + productsMatrix[i, j].reward.ToString();
            }
        }

        
        if (GameManager.Instance.enemyTier2)
        {
            totalFabrics2Object.SetActive(true);
            imagesMatrix[2, 1].SetActive(true);
            imagesMatrix[2, 3].SetActive(true);
        }
        if (GameManager.Instance.rockTier2)
        {
            totalRock2Object.SetActive(true);
            imagesMatrix[1, 1].SetActive(true);
            imagesMatrix[1, 3].SetActive(true);
        }
        if (GameManager.Instance.treeTier2)
        {
            totalWood2Object.SetActive(true);
            imagesMatrix[0, 1].SetActive(true);
            imagesMatrix[0, 3].SetActive(true);
        }
    }



    void UpdateTotals()
    {
        totalCoins.text = GameManager.totalCoins.ToString();
        totalFabric.text = GameManager.totalFabrics.ToString();
        totalRock.text = GameManager.totalRock.ToString();
        totalWood.text = GameManager.totalWood.ToString();

        if (GameManager.Instance.enemyTier2)
            totalFabrics2.text = GameManager.totalFabrics2.ToString();
        if (GameManager.Instance.rockTier2)
            totalRocks2.text = GameManager.totalRock2.ToString();
        if (GameManager.Instance.treeTier2)
            totalWood2.text = GameManager.totalWood2.ToString();
    }


    void UpdateButtons()
    {
        float horizontal = InputManager.Instance.GetAxis("Horizontal");
        if (horizontal == 0)
            horizontal = InputManager.Instance.GetAxis("CameraMovementX");
        float vertical = -InputManager.Instance.GetAxis("Vertical");
        if (vertical == 0)
            vertical = InputManager.Instance.GetAxis("CameraMovementY");

        if (!hasMoved)
        {
            if (vertical > 0.5f && arrayPositionX < 2)
            {
                hasMoved = true;
                arrayPositionX++;

                if ((arrayPositionY == 3 || arrayPositionY == 1) && ((arrayPositionX == 2 && !GameManager.Instance.enemyTier2) || (arrayPositionX == 1 && !GameManager.Instance.rockTier2)))
                    arrayPositionY--;
            }
            else if (vertical < -0.5f && arrayPositionX > 0)
            {
                hasMoved = true;
                arrayPositionX--;

                if ((arrayPositionY == 3 || arrayPositionY == 1) && ((arrayPositionX == 1 && !GameManager.Instance.rockTier2) || (arrayPositionX == 0 && !GameManager.Instance.treeTier2)))
                    arrayPositionY--;
            }

            if (horizontal > 0.5f && arrayPositionY < 3)
            {
                hasMoved = true;
                arrayPositionY++;

                if (arrayPositionY == 3 && ((arrayPositionX == 2 && !GameManager.Instance.enemyTier2) || (arrayPositionX == 1 && !GameManager.Instance.rockTier2) || (arrayPositionX == 0 && !GameManager.Instance.treeTier2)))
                    arrayPositionY--;
                if (arrayPositionY == 1 && ((arrayPositionX == 2 && !GameManager.Instance.enemyTier2) || (arrayPositionX == 1 && !GameManager.Instance.rockTier2) || (arrayPositionX == 0 && !GameManager.Instance.treeTier2)))
                    arrayPositionY++;
            }
            else if (horizontal < -0.5f && arrayPositionY > 0)
            {
                hasMoved = true;
                arrayPositionY--;

                if (arrayPositionY == 1 && ((arrayPositionX == 2 && !GameManager.Instance.enemyTier2) || (arrayPositionX == 1 && !GameManager.Instance.rockTier2) || (arrayPositionX == 0 && !GameManager.Instance.treeTier2)))
                    arrayPositionY--;
            }

            if (hasMoved)
            {
                if (actualIcono != null)
                    actualIcono.SetActive(false);
                if (actualSelectedImage != null)
                    actualSelectedImage.SetActive(false);
                movementOn = true;
                UpdateComponents();
            }
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

    void UpdateComponents()
    {
        canBuy = false;

        actualProduct = productsMatrix[arrayPositionX, arrayPositionY];
        actualSelectedImage = imagesMatrix[arrayPositionX, arrayPositionY].transform.GetChild(childSelectedNumber).gameObject;
        actualSelectedImage.SetActive(true);

        selectedProduct_Name.text = actualProduct.nameProduct;
        selectedProduct_Text.text = actualProduct.description;
        buyPrice.text = actualProduct.price.ToString();

        switch (actualProduct.idName)
        {
            case "wood":
                actualIcono = iconoMoneda;
                if (actualProduct.price <= GameManager.totalCoins)
                    canBuy = true;
                break;
            case "rock":
                actualIcono = iconoMoneda;
                if (actualProduct.price <= GameManager.totalCoins)
                    canBuy = true;
                break;
            case "fabric":
                actualIcono = iconoMoneda;
                if (actualProduct.price <= GameManager.totalCoins)
                    canBuy = true;
                break;
            case "wood2":
                actualIcono = iconoMoneda;
                if (actualProduct.price <= GameManager.totalCoins)
                    canBuy = true;
                break;
            case "rock2":
                actualIcono = iconoMoneda;
                if (actualProduct.price <= GameManager.totalCoins)
                    canBuy = true;
                break;
            case "fabric2":
                actualIcono = iconoMoneda;
                if (actualProduct.price <= GameManager.totalCoins)
                    canBuy = true;
                break;

            case "moneyForWood":
                actualIcono = iconoMadera;
                if (actualProduct.price <= GameManager.totalWood)
                    canBuy = true;
                break;
            case "moneyForRocks":
                actualIcono = iconoRoca;
                if (actualProduct.price <= GameManager.totalRock)
                    canBuy = true;
                break;
            case "moneyForFabrics":
                actualIcono = iconoPieles;
                if (actualProduct.price <= GameManager.totalFabrics)
                    canBuy = true;
                break;
            case "moneyForWood2":
                actualIcono = iconoMaderaT2;
                if (actualProduct.price <= GameManager.totalWood2)
                    canBuy = true;
                break;
            case "moneyForRocks2":
                actualIcono = iconoRocaT2;
                if (actualProduct.price <= GameManager.totalRock2)
                    canBuy = true;
                break;
            case "moneyForFabrics2":
                actualIcono = iconoPielesT2;
                if (actualProduct.price <= GameManager.totalFabrics2)
                    canBuy = true;
                break;
        }

        actualIcono.SetActive(true);


        if (canBuy)
        {
            buyText.gameObject.SetActive(true);
            insufficientText.gameObject.SetActive(false);
        }
        else
        {
            buyText.gameObject.SetActive(false);
            insufficientText.gameObject.SetActive(true);
        }
    }



    void BuyProduct()
    {
        canBuy = false;

        switch (actualProduct.idName)
        {
            case "wood":
                GameManager.totalWood += (int)actualProduct.reward;
                GameManager.totalCoins -= (int)actualProduct.price;
                break;
            case "rock":
                GameManager.totalRock += (int)actualProduct.reward;
                GameManager.totalCoins -= (int)actualProduct.price;
                break;
            case "fabric":
                GameManager.totalFabrics += (int)actualProduct.reward;
                GameManager.totalCoins -= (int)actualProduct.price;
                break;
            case "wood2":
                GameManager.totalWood2 += (int)actualProduct.reward;
                GameManager.totalCoins -= (int)actualProduct.price;
                break;
            case "rock2":
                GameManager.totalRock2 += (int)actualProduct.reward;
                GameManager.totalCoins -= (int)actualProduct.price;
                break;
            case "fabric2":
                GameManager.totalFabrics2 += (int)actualProduct.reward;
                GameManager.totalCoins -= (int)actualProduct.price;
                break;

            case "moneyForWood":
                GameManager.totalCoins += (int)actualProduct.reward;
                GameManager.totalWood -= (int)actualProduct.price;
                break;
            case "moneyForRocks":
                GameManager.totalCoins += (int)actualProduct.reward;
                GameManager.totalRock -= (int)actualProduct.price;
                break;
            case "moneyForFabrics":
                GameManager.totalCoins += (int)actualProduct.reward;
                GameManager.totalFabrics -= (int)actualProduct.price;
                break;
            case "moneyForWood2":
                GameManager.totalCoins += (int)actualProduct.reward;
                GameManager.totalWood2 -= (int)actualProduct.price;
                break;
            case "moneyForRocks2":
                GameManager.totalCoins += (int)actualProduct.reward;
                GameManager.totalRock2 -= (int)actualProduct.price;
                break;
            case "moneyForFabrics2":
                GameManager.totalCoins += (int)actualProduct.reward;
                GameManager.totalFabrics2 -= (int)actualProduct.price;
                break;
        }
        
        SaveManager.Instance.Save();

        UpdateTotals();
        UpdateComponents();
    }
}