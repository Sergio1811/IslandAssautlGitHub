using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject backMenu;

    public Text totalCoins;
    public GameObject backCircle, backB;
    public Image selectedAbility_Image;
    public Text selectedAbility_Name;
    public Text selectedAbility_TextBig, selectedAbility_TextLittle;
    public GameObject buyText, insufficientText;
    public GameObject buyTriangle, buyY;
    public GameObject buyWithCoinsSquare, buyWithMaterialSquare;
    public Text buyWithCoins_Price, buywithMaterials_Price, buyWithMaterials_MaterialPrice;
    GameObject actualIcono;
    public GameObject iconoMadera, iconoMaderaT2, iconoRoca, iconoRocaT2, iconoPieles, iconoPielesT2;

    int arrayPositionX, arrayPositionY;
    public GameObject[] arraySword, arrayBomb, arrayAxe, arrayGeneral;
    GameObject[,] imagesMatrix = new GameObject[6, 6];
    Abilities[,] abilitiesMatrix = new Abilities[6, 6];
    Abilities actualAbility;

    public int childImageNumber, childBNImageNumber, childBlockNumber, childSelectedNumber;

    bool canBuy, hasMoved;
    public static string boughtString = "bought";
    public static string unlockedString = "unlocked";
    public Sprite lockedAbilitySprite;

    float timerMovement;
    bool movementOn = false;



    void Awake()
    {
        //    Time.timeScale = 1;

        //    MatrixInitialization();
        //    IconsInitialization();

        //    arrayPositionX = 3;
        //    arrayPositionY = 3;

        //    totalCoins.text = GameManager.totalCoins.ToString();
        //    if (InputManager.Instance.psController)
        //    {
        //        backCircle.SetActive(true);
        //        buyTriangle.SetActive(true);
        //    }
        //    else
        //    {
        //        backB.SetActive(true);
        //        buyY.SetActive(true);
        //    }

        //    UpdateComponents();
        //    canBuy = false;
    }


    private void OnEnable()
    {
        Time.timeScale = 1;

        MatrixInitialization();
        IconsInitialization();

        arrayPositionX = 3;
        arrayPositionY = 3;

        totalCoins.text = GameManager.totalCoins.ToString();
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

        UpdateComponents();
        canBuy = false;
    }


    private void Update()
    {
        if (InputManager.Instance.GetInputDown("Cancel"))
        {
            backMenu.SetActive(true);
            gameObject.SetActive(false);
        }

        if (canBuy && InputManager.Instance.GetInputDown("Buy"))
            BuyAbility();


        float horizontal = InputManager.Instance.GetAxis("Horizontal");
        if (horizontal == 0)
            horizontal = InputManager.Instance.GetAxis("CameraMovementX");
        float vertical = -InputManager.Instance.GetAxis("Vertical");
        if (vertical == 0)
            vertical = InputManager.Instance.GetAxis("CameraMovementY");

        if (!hasMoved)
        {
            if (vertical > 0.5f && arrayPositionX < 5)
            {
                imagesMatrix[arrayPositionX, arrayPositionY].transform.GetChild(childSelectedNumber).gameObject.SetActive(false);
                arrayPositionX++;
                hasMoved = true;
            }
            else if (vertical < -0.5f && arrayPositionX > 0)
            {
                imagesMatrix[arrayPositionX, arrayPositionY].transform.GetChild(childSelectedNumber).gameObject.SetActive(false);
                arrayPositionX--;
                hasMoved = true;
            }

            if (horizontal > 0.5f && arrayPositionY < 5)
            {
                imagesMatrix[arrayPositionX, arrayPositionY].transform.GetChild(childSelectedNumber).gameObject.SetActive(false);
                arrayPositionY++;
                hasMoved = true;
            }
            else if (horizontal < -0.5f && arrayPositionY > 0)
            {
                imagesMatrix[arrayPositionX, arrayPositionY].transform.GetChild(childSelectedNumber).gameObject.SetActive(false);
                arrayPositionY--;
                hasMoved = true;
            }

            if (hasMoved)
            {
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




    void MatrixInitialization()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (i < 3 && j < 3)
                    imagesMatrix[i, j] = arrayAxe[j + i * 3];
                else if (i < 3 && j > 2)
                    imagesMatrix[i, j] = arrayBomb[j + i * 3 - 3];
                else if (i > 2 && j < 3)
                    imagesMatrix[i, j] = arraySword[j + (i - 3) * 3];
                else
                    imagesMatrix[i, j] = arrayGeneral[j + (i - 3) * 3 - 3];

                abilitiesMatrix[i, j] = imagesMatrix[i, j].GetComponent<AbilityDisplay>().Ability;
            }
        }

        if (!PlayerPrefs.HasKey(boughtString + abilitiesMatrix[3, 3].saverString))
        {
            PlayerPrefs.SetInt(boughtString + abilitiesMatrix[3, 3].saverString, 1);
            PlayerPrefs.SetInt(boughtString + abilitiesMatrix[2, 2].saverString, 1);
            PlayerPrefs.SetInt(boughtString + abilitiesMatrix[3, 2].saverString, 1);
            PlayerPrefs.SetInt(boughtString + abilitiesMatrix[2, 3].saverString, 1);

            PlayerPrefs.SetInt(unlockedString + abilitiesMatrix[3, 4].saverString, 1);
            PlayerPrefs.SetInt(unlockedString + abilitiesMatrix[4, 3].saverString, 1);
            PlayerPrefs.SetInt(unlockedString + abilitiesMatrix[1, 2].saverString, 1);
            PlayerPrefs.SetInt(unlockedString + abilitiesMatrix[2, 1].saverString, 1);
            PlayerPrefs.SetInt(unlockedString + abilitiesMatrix[3, 1].saverString, 1);
            PlayerPrefs.SetInt(unlockedString + abilitiesMatrix[4, 2].saverString, 1);
            PlayerPrefs.SetInt(unlockedString + abilitiesMatrix[2, 4].saverString, 1);
            PlayerPrefs.SetInt(unlockedString + abilitiesMatrix[1, 3].saverString, 1);
        }
    }

    void IconsInitialization()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                imagesMatrix[i, j].transform.GetChild(childImageNumber).GetComponent<Image>().sprite = abilitiesMatrix[i, j].icono;
                imagesMatrix[i, j].transform.GetChild(childBNImageNumber).GetComponent<Image>().sprite = abilitiesMatrix[i, j].iconoByN;


                if (PlayerPrefs.GetInt(boughtString + abilitiesMatrix[i, j].saverString) == 1)
                    abilitiesMatrix[i, j].isBought = true;
                else
                    abilitiesMatrix[i, j].isBought = false;

                if (PlayerPrefs.GetInt(unlockedString + abilitiesMatrix[i, j].saverString) == 1)
                    abilitiesMatrix[i, j].isUnlocked = true;
                else
                    abilitiesMatrix[i, j].isUnlocked = false;


                if (abilitiesMatrix[i, j].isBought)
                    imagesMatrix[i, j].transform.GetChild(childImageNumber).gameObject.SetActive(true);
                else if (abilitiesMatrix[i, j].isUnlocked)
                    imagesMatrix[i, j].transform.GetChild(childBNImageNumber).gameObject.SetActive(true);
                else
                    imagesMatrix[i, j].transform.GetChild(childBlockNumber).gameObject.SetActive(true);
            }
        }
    }



    void UpdateComponents()
    {
        canBuy = false;

        actualAbility = abilitiesMatrix[arrayPositionX, arrayPositionY];
        imagesMatrix[arrayPositionX, arrayPositionY].transform.GetChild(childSelectedNumber).gameObject.SetActive(true);

        if (actualAbility.isBought)
            selectedAbility_Image.sprite = actualAbility.icono;
        else if (actualAbility.isUnlocked)
            selectedAbility_Image.sprite = actualAbility.iconoByN;
        else
            selectedAbility_Image.sprite = lockedAbilitySprite;
        selectedAbility_Name.text = actualAbility.nameAbility;


        if (actualAbility.materialNeeded)
        {
            selectedAbility_TextBig.gameObject.SetActive(false);
            selectedAbility_TextLittle.gameObject.SetActive(true);
            selectedAbility_TextLittle.text = actualAbility.description;

            if (actualAbility.isUnlocked && !actualAbility.isBought)
            {
                buyWithMaterialSquare.SetActive(true);
                buyWithCoinsSquare.SetActive(false);
                buywithMaterials_Price.text = actualAbility.price.ToString();
                if (actualIcono != null)
                    actualIcono.SetActive(false);

                switch (actualAbility.materialType)
                {
                    case Node.Type.tree:
                        buyWithMaterials_MaterialPrice.text = GameManager.totalWood.ToString() + "/" + actualAbility.materialPrice.ToString();
                        actualIcono = iconoMadera;
                        if (actualAbility.materialPrice <= GameManager.totalWood && actualAbility.price <= GameManager.totalCoins)
                            canBuy = true;
                        break;
                    case Node.Type.tree2:
                        buyWithMaterials_MaterialPrice.text = GameManager.totalWood2.ToString() + "/" + actualAbility.materialPrice.ToString();
                        actualIcono = iconoMaderaT2;
                        if (actualAbility.materialPrice <= GameManager.totalWood2 && actualAbility.price <= GameManager.totalCoins)
                            canBuy = true;
                        break;
                    case Node.Type.rock:
                        buyWithMaterials_MaterialPrice.text = GameManager.totalRock.ToString() + "/" + actualAbility.materialPrice.ToString();
                        actualIcono = iconoRoca;
                        if (actualAbility.materialPrice <= GameManager.totalRock && actualAbility.price <= GameManager.totalCoins)
                            canBuy = true;
                        break;
                    case Node.Type.rock2:
                        buyWithMaterials_MaterialPrice.text = GameManager.totalRock2.ToString() + "/" + actualAbility.materialPrice.ToString();
                        actualIcono = iconoRocaT2;
                        if (actualAbility.materialPrice <= GameManager.totalRock2 && actualAbility.price <= GameManager.totalCoins)
                            canBuy = true;
                        break;
                    case Node.Type.enemy:
                        buyWithMaterials_MaterialPrice.text = GameManager.totalFabrics.ToString() + "/" + actualAbility.materialPrice.ToString();
                        actualIcono = iconoPieles;
                        if (actualAbility.materialPrice <= GameManager.totalFabrics && actualAbility.price <= GameManager.totalCoins)
                            canBuy = true;
                        break;
                    case Node.Type.enemy2:
                        buyWithMaterials_MaterialPrice.text = GameManager.totalFabrics2.ToString() + "/" + actualAbility.materialPrice.ToString();
                        actualIcono = iconoPielesT2;
                        if (actualAbility.materialPrice <= GameManager.totalFabrics2 && actualAbility.price <= GameManager.totalCoins)
                            canBuy = true;
                        break;
                }
                actualIcono.SetActive(true);
            }
        }
        else
        {
            selectedAbility_TextBig.gameObject.SetActive(true);
            selectedAbility_TextLittle.gameObject.SetActive(false);
            selectedAbility_TextBig.text = actualAbility.description;

            if (actualAbility.isUnlocked && !actualAbility.isBought)
            {
                buyWithMaterialSquare.SetActive(false);
                buyWithCoinsSquare.SetActive(true);
                buyWithCoins_Price.text = actualAbility.price.ToString();

                if (actualAbility.price <= GameManager.totalCoins)
                    canBuy = true;
            }
        }

        if (!actualAbility.isBought && actualAbility.isUnlocked)
        {
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
        else
        {
            buyWithMaterialSquare.SetActive(false);
            buyWithCoinsSquare.SetActive(false);
            buyText.gameObject.SetActive(false);
            insufficientText.gameObject.SetActive(false);
        }
    }



    void BuyAbility()
    {
        canBuy = false;

        if (actualAbility.materialNeeded)
        {
            buyWithMaterialSquare.SetActive(false);

            switch (actualAbility.materialType)
            {
                case Node.Type.tree:
                    GameManager.totalWood -= (int)actualAbility.materialPrice;
                    break;
                case Node.Type.tree2:
                    GameManager.totalWood2 -= (int)actualAbility.materialPrice;
                    break;
                case Node.Type.rock:
                    GameManager.totalRock -= (int)actualAbility.materialPrice;
                    break;
                case Node.Type.rock2:
                    GameManager.totalRock2 -= (int)actualAbility.materialPrice;
                    break;
                case Node.Type.enemy:
                    GameManager.totalFabrics -= (int)actualAbility.materialPrice;
                    break;
                case Node.Type.enemy2:
                    GameManager.totalFabrics2 -= (int)actualAbility.materialPrice;
                    break;
            }
        }

        GameManager.totalCoins -= (int)actualAbility.price;
        totalCoins.text = GameManager.totalCoins.ToString();
        SaveManager.Instance.Save();
        GameManager.Instance.totalEndCoinsText.text = GameManager.totalCoins.ToString();
        PlayerPrefs.SetInt(boughtString + actualAbility.saverString, 1);
        imagesMatrix[arrayPositionX, arrayPositionY].transform.GetChild(childBNImageNumber).gameObject.SetActive(false);
        imagesMatrix[arrayPositionX, arrayPositionY].transform.GetChild(childImageNumber).gameObject.SetActive(true);
        selectedAbility_Image.sprite = actualAbility.icono;
        buyWithMaterialSquare.SetActive(false);
        buyWithCoinsSquare.SetActive(false);
        buyText.gameObject.SetActive(false);
        insufficientText.gameObject.SetActive(false);

        CallUpdateMethods(actualAbility.saverString);


        Abilities unlockNeighbour;

        if (arrayPositionX < 3)
        {
            if (arrayPositionX - 1 >= 0)
            {
                unlockNeighbour = abilitiesMatrix[arrayPositionX - 1, arrayPositionY];

                if (!unlockNeighbour.isUnlocked && !unlockNeighbour.isBought)
                {
                    abilitiesMatrix[arrayPositionX - 1, arrayPositionY].isUnlocked = true;
                    imagesMatrix[arrayPositionX - 1, arrayPositionY].transform.GetChild(childBlockNumber).gameObject.SetActive(false);
                    imagesMatrix[arrayPositionX - 1, arrayPositionY].transform.GetChild(childBNImageNumber).gameObject.SetActive(true);
                    PlayerPrefs.SetInt(unlockedString + unlockNeighbour.saverString, 1);
                }
            }
        }
        else
        {
            if (arrayPositionX + 1 < 6)
            {
                unlockNeighbour = abilitiesMatrix[arrayPositionX + 1, arrayPositionY];

                if (!unlockNeighbour.isUnlocked && !unlockNeighbour.isBought)
                {
                    abilitiesMatrix[arrayPositionX + 1, arrayPositionY].isUnlocked = true;
                    imagesMatrix[arrayPositionX + 1, arrayPositionY].transform.GetChild(childBlockNumber).gameObject.SetActive(false);
                    imagesMatrix[arrayPositionX + 1, arrayPositionY].transform.GetChild(childBNImageNumber).gameObject.SetActive(true);
                    PlayerPrefs.SetInt(unlockedString + unlockNeighbour.saverString, 1);
                }
            }
        }

        if (arrayPositionY < 3)
        {
            if (arrayPositionY - 1 >= 0)
            {
                unlockNeighbour = abilitiesMatrix[arrayPositionX, arrayPositionY - 1];

                if (!unlockNeighbour.isUnlocked && !unlockNeighbour.isBought)
                {
                    abilitiesMatrix[arrayPositionX, arrayPositionY - 1].isUnlocked = true;
                    imagesMatrix[arrayPositionX, arrayPositionY - 1].transform.GetChild(childBlockNumber).gameObject.SetActive(false);
                    imagesMatrix[arrayPositionX, arrayPositionY - 1].transform.GetChild(childBNImageNumber).gameObject.SetActive(true);
                    PlayerPrefs.SetInt(unlockedString + unlockNeighbour.saverString, 1);
                }
            }
        }
        else
        {
            if (arrayPositionY + 1 < 6)
            {
                unlockNeighbour = abilitiesMatrix[arrayPositionX, arrayPositionY + 1];

                if (!unlockNeighbour.isUnlocked && !unlockNeighbour.isBought)
                {
                    abilitiesMatrix[arrayPositionX, arrayPositionY + 1].isUnlocked = true;
                    imagesMatrix[arrayPositionX, arrayPositionY + 1].transform.GetChild(childBlockNumber).gameObject.SetActive(false);
                    imagesMatrix[arrayPositionX, arrayPositionY + 1].transform.GetChild(childBNImageNumber).gameObject.SetActive(true);
                    PlayerPrefs.SetInt(unlockedString + unlockNeighbour.saverString, 1);
                }
            }
        }

        PlayerPrefs.Save();
    }



    void CallUpdateMethods(string ability)
    {
        switch (ability)
        {
            case "casco":
                //desbloquear casco
                break;
            case "mastil":
                //desbloquear mastil
                break;

            case "conmocion":
                AxerAbilities.axeStunt = true;
                break;
            case "doblefilo":
                AxerAbilities.axerTier2 = true;
                break;
            case "eficaciamadera":
                AxerAbilities.resourceMultiplier = 1.5f;
                break;
            case "eficienciamadera":
                AxerAbilities.resourceSpeedMultiplier = 0.5f;
                break;
            case "maestrodelhacha":
                AxerAbilities.treeTier2 = true;
                break;
            case "polivalentemadera":
                AxerAbilities.Polivalente = true;
                break;


            case "cañon":
                //desbloquear cañón
                break;
            case "timon":
                //desbloquear timón
                break;

            case "empujon":
                BomberAbilities.bombKnockBack = true;
                break;
            case "c4":
                BomberAbilities.explosiveTier2 = true;
                break;
            case "eficaciarocas":
                BomberAbilities.resourceMultiplier = 1.5f;
                break;
            case "eficienciarocas":
                BomberAbilities.resourceSpeedMultiplier = 0.5f;
                break;
            case "artificiero":
                BomberAbilities.rockTier2 = true;
                break;
            case "polivalenterocas":
                BomberAbilities.Polivalente = true;
                break;


            case "remos":
                //desbloquear remo
                break;
            case "vela":
                //desbloquear vela
                break;

            case "barrido":
                SwordAbilities.swordSweep = true;
                break;
            case "espadon":
                SwordAbilities.swordTier2 = true;
                break;
            case "eficaciaataque":
                SwordAbilities.resourceMultiplier = 1.5f;
                break;
            case "eficienciaataque":
                SwordAbilities.resourceSpeedMultiplier = 0.5f;
                break;
            case "gladiador":
                SwordAbilities.enemyTier2 = true;
                break;
            case "polivalenteataque":
                SwordAbilities.Polivalente = true;
                break;


            case "brujula":
                //desbloquear brujula
                break;
            case "mapa":
                //desbloquear mapa
                break;

            case "dash":
                CharacterAbiliities.dashActive = true;
                break;
            case "comerciante":
                CharacterAbiliities.market = true;
                break;
            case "botas":
                CharacterAbiliities.bootsMovementMultiplier = 1.25f;
                break;
            case "exprimir":
                CharacterAbiliities.goldMultiplier = 2.0f;
                break;
            case "explorador":
                CharacterAbiliities.islandTier2 = true;
                break;
            case "titan":
                CharacterAbiliities.Titan = true;
                break;
        }
    }
}
