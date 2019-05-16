using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Text totalCoins;
    public GameObject backCircle, backB;
    public Image selectedAbility_Image;
    public Text selectedAbility_Name;
    public Text selectedAbility_TextBig, selectedAbility_TextLittle;
    public GameObject buyText, insufficientText;
    public GameObject buyTriangle, buyY;
    public GameObject buyWithCoinsSquare, buyWithMaterialSquare;
    public Text buyWithCoins_Price, buywithMaterials_Price, buyWithMaterials_MaterialPrice;

    int arrayPositionX, arrayPositionY;
    public GameObject[] arraySword, arrayBomb, arrayAxe, arrayGeneral;
    GameObject[,] imagesMatrix = new GameObject[6, 6];
    Abilities[,] abilitiesMatrix = new Abilities[6, 6];
    Abilities actualAbility;

    public int childImageNumber, childBNImageNumber, childBlockNumber, childSelectedNumber;

    bool canBuy;

    public static List<string> disabledButtonsList = new List<string>();


    void Awake()
    {
        MatrixInitialization();

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
    }

    void UpdateComponents()
    {
        canBuy = false;

        actualAbility = abilitiesMatrix[arrayPositionX, arrayPositionY];
        imagesMatrix[arrayPositionX, arrayPositionY].transform.GetChild(childSelectedNumber).gameObject.SetActive(true);

        selectedAbility_Image.sprite = actualAbility.icono;
        selectedAbility_Name.text = actualAbility.nameAbility;

        if (actualAbility.materialNeeded)
        {
            selectedAbility_TextBig.gameObject.SetActive(false);
            selectedAbility_TextLittle.gameObject.SetActive(true);
            selectedAbility_TextLittle.text = actualAbility.description;

            buyWithMaterialSquare.SetActive(true);
            buyWithCoinsSquare.SetActive(false);
            buywithMaterials_Price.text = actualAbility.price.ToString();
            buyWithMaterials_MaterialPrice.text = actualAbility.materialPrice.ToString();

            switch (actualAbility.materialType)
            {
                case Node.Type.tree:
                    if (actualAbility.materialPrice >= GameManager.totalWood && actualAbility.price >= GameManager.totalCoins)
                        canBuy = true;
                    break;
            }
        }
        else
        {
            selectedAbility_TextBig.gameObject.SetActive(true);
            selectedAbility_TextLittle.gameObject.SetActive(false);
            selectedAbility_TextBig.text = actualAbility.description;

            buyWithMaterialSquare.SetActive(false);
            buyWithCoinsSquare.SetActive(true);
            buyWithCoins_Price.text = actualAbility.price.ToString();

            if (actualAbility.price >= GameManager.totalCoins)
                canBuy = true;
        }

        if (!actualAbility.isBought)
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
        }
    }
}
