﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }

    public GameObject pickerPrefab;
    public GameObject acerPrefab;
    public GameObject sworderPrefab;
    int characterNumber;

    int woodInMap = 0;
    int rockInMap = 0;
    int fabricInMap = 0;

    int woodNeeded;
    int rockNeeded;
    int enemiesNeeded;
    public Text objectiveText;
    public Text secondaryObjectiveText;
    public string[] secondaryObjectives;

    public int rockByItem;
    public int woodByItem;
    public int enemiesByItem;

    [HideInInspector] public int collectedWood = 0;
    [HideInInspector] public int collectedRock = 0;
    [HideInInspector] public int collectedFabrics = 0;

    [HideInInspector] public int currentCoins = 0;
    [HideInInspector] public static int totalCoins = 0;
    [HideInInspector] public static int totalWood = 0;
    [HideInInspector] public static int totalRock = 0;
    [HideInInspector] public static int totalFabrics = 0;

    public Text woodText;
    public Text rockText;
    public Text fabricText;
    public Text currentCoinsText;
    public Text totalCoinsText;
    public Text totalWoodText;
    public Text totalRockText;
    public Text totalFabricsText;

    public float timeByLevel;
    private float remainingTimeInLevel;
    public Text timeText;

    [HideInInspector] public GameObject player;
    GameObject portalExit;

    public Grid gridScript;
    public GameObject tree1x1, tree2x2, tree1x2, enemy, rock1x1, rock2x2, rocks1x2, village2x2, village3x3, village4x4, portal, water;
    public GameObject decoration1x1, decoration1x2, decoration1x3, decoration1x4, decoration2x2, decoration2x3, decoration2x4;

    Node startNode, endNode;
    int protoIsland;
    public GameObject[] islands;
    Transform islandParent;

    public GameObject livesGroup;
    int livesNumber;

    int secondaryObjectiveID;
   
    public bool titan = false;//applied
    #region
    bool islandTier2 = false;
    float goldMultiplier = 1.0f;//applied
    bool Market = false;
    float resourceFabricMultiplier = 1.0f;//applied
    bool enemyTier2 = false;
    float resourceTreeMultiplier = 1.0f;//aaplied
    bool treeTier2 = false;
    float resourceStoneMultiplier = 1.0f;//applied
    bool rockTier2 = false;
    #endregion

    private void Awake()
    {
        Instance = this;

        protoIsland = Random.Range(0, islands.Length);
        islands[protoIsland].SetActive(true);
        islandParent = islands[protoIsland].transform.GetChild(0);

        characterNumber = Random.Range(0, 3);

        gridScript.GenerateGrid(characterNumber);

    }

    void Start()
    {
        currentCoinsText.text = "Monedas: " + currentCoins.ToString();
        totalCoinsText.text = "Monedas totales: " + totalCoins.ToString();
        totalWoodText.text = "Madera total: " + totalWood.ToString();
        totalRockText.text = "Rocas totales: " + totalRock.ToString();
        totalFabricsText.text = "Pieles totales: " + totalFabrics.ToString();

        islands[protoIsland].GetComponent<NavMeshSurface>().BuildNavMesh();
        remainingTimeInLevel = timeByLevel;
        InstantiateObjectInGrid();
        player = PlayerInstantiation();

        secondaryObjectives = new string[4];
        secondaryObjectives[0] = "Abandona la isla con 20 segundos restantes";
        secondaryObjectives[1] = "Abandona la isla con 10 segundos restantes";
        if (player.GetComponent<Movement>().actualType == Movement.playerType.ace) secondaryObjectives[2] = "Abandona la isla con un mínimo de " + (70 * woodInMap / 100) + " de tu recurso";
        else if (player.GetComponent<Movement>().actualType == Movement.playerType.pick) secondaryObjectives[2] = "Abandona la isla con un mínimo de " + (70 * rockInMap / 100) + " de tu recurso";
        else secondaryObjectives[2] = "Abandona la isla con un mínimo de " + (70 * fabricInMap / 100) + " de tu recurso";
        secondaryObjectives[3] = "Abandona la isla sin recibir daño";

        secondaryObjectiveID = RandomSecondaryObjective();

        switch (characterNumber)
        {
            case 0:
                livesNumber = 3;
                break;
            case 1:
                livesNumber = 2;
                break;
            case 2:
                livesNumber = 4;
                break;
        }
        if (titan)
            livesNumber++;
        for (int i = 0; i < livesNumber; i++)
            livesGroup.transform.GetChild(i).gameObject.SetActive(true);

        secondaryObjectiveText.text = secondaryObjectives[secondaryObjectiveID];
    }

    void Update()
    {
        remainingTimeInLevel -= Time.deltaTime;

        timeText.text = remainingTimeInLevel.ToString("0");

        if (remainingTimeInLevel <= 0f)
        {
            //EndLevel();
            EndProtoLevel();
            remainingTimeInLevel = timeByLevel;
        }

        if (Input.GetKeyDown(KeyCode.N))
            EndProtoLevel();

        if (Input.GetKeyDown(KeyCode.R))
            SaveManager.Instance.ResetSaving();
    }


    public GameObject PlayerInstantiation()
    {
        GameObject p = null;

        switch (characterNumber)
        {
            case 0:
                p = Instantiate(acerPrefab, startNode.worldPosition + (Vector3.up * 2), Quaternion.LookRotation(transform.forward));
                woodText.gameObject.SetActive(true);
                woodNeeded = woodNeeded - woodNeeded / 2;
                objectiveText.text = "1: Consigue " + woodNeeded * woodByItem + " maderas.";
                ApplyAxerAbilities(p);
                break;
            case 1:
                p = Instantiate(pickerPrefab, startNode.worldPosition + (Vector3.up * 2), Quaternion.LookRotation(transform.forward));
                rockText.gameObject.SetActive(true);
                rockNeeded = rockNeeded - rockNeeded / 2;
                objectiveText.text = "1: Consigue " + rockNeeded * rockByItem + " rocas.";
                ApplyBomberAbilities(p);
                break;
            case 2:
                p = Instantiate(sworderPrefab, startNode.worldPosition + (Vector3.up*2), Quaternion.LookRotation(transform.forward));
                fabricText.gameObject.SetActive(true);
                enemiesNeeded = enemiesNeeded - enemiesNeeded / 2;
                objectiveText.text = "1: Consigue " + enemiesNeeded * enemiesByItem + " pieles.";
                ApplySwordAbilities(p);
                break;
        }

        return p;
    }



    public void EndLevel()
    {
        Destroy(player);
        player = PlayerInstantiation();
    }

    public void EndProtoLevel()
    {
        protoIsland++;
        if (protoIsland >= islands.Length)
            protoIsland = 0;

        if (livesNumber > 0)
        {
            totalCoins += currentCoins;
            totalFabrics += collectedFabrics;
            totalRock += collectedRock;
            totalWood += collectedWood;
        }

        currentCoins = 0;


        SaveManager.Instance.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void PickWood()
    {
        collectedWood += (int)(woodByItem*resourceTreeMultiplier);
        currentCoins += (int)(woodByItem * goldMultiplier);
        woodText.text = "Madera: " + collectedWood.ToString();
        currentCoinsText.text = "Monedas: " + currentCoins.ToString();
        if (collectedWood >= woodNeeded * woodByItem)
            ActivatePortal();
    }

    public void PickRock()
    {
        collectedRock += (int)(rockByItem*resourceStoneMultiplier);
        currentCoins += (int)(rockByItem*goldMultiplier);
        rockText.text = "Rocas: " + collectedRock.ToString();
        currentCoinsText.text = "Monedas: " + currentCoins.ToString();
        if (collectedRock >= rockNeeded * rockByItem)
            ActivatePortal();
    }

    public void PickFabrics()
    {
        collectedFabrics += (int)(enemiesByItem*resourceFabricMultiplier);
        currentCoins += (int)(enemiesByItem * goldMultiplier);
        fabricText.text = "Pieles: " + collectedFabrics.ToString();
        currentCoinsText.text = "Monedas: " + currentCoins.ToString();
        if (collectedFabrics >= enemiesNeeded * enemiesByItem)
            ActivatePortal();
    }

    public void Damage()
    {
        livesNumber--;
        livesGroup.transform.GetChild(livesNumber).gameObject.SetActive(false);

        if (livesNumber <= 0)
            EndProtoLevel();
    }


    public void ActivatePortal()
    {
        portalExit.transform.GetChild(0).gameObject.SetActive(false);
        portalExit.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void LevelComplete()
    {
        if (CheckSecondaryObjective()) totalCoins += 50;
        EndProtoLevel();
    }


    void InstantiateObjectInGrid()
    {
        GameObject objectInstantiation;

        for (int i = gridScript.grid.GetLength(0) - 1; i >= 0; i--)
        {
            for (int j = gridScript.grid.GetLength(1) - 1; j >= 0; j--)
            {
                Node actualNode = gridScript.grid[i, j];

                if (actualNode.isTransitable && actualNode.currentType == Node.Type.entry)
                {
                    startNode = actualNode;
                    actualNode.isTransitable = false;
                }
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.exit)
                {
                    endNode = actualNode;
                    objectInstantiation = Instantiate(portal, islandParent);
                    ChangeTransitable(actualNode, false, 1, 1);
                    objectInstantiation.transform.position = actualNode.worldPosition;
                    portalExit = objectInstantiation;
                }
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.tree)
                {
                    switch (actualNode.currentSize)
                    {
                        case Node.Size.s1x1:
                            objectInstantiation = Instantiate(tree1x1, islandParent);
                            ChangeTransitable(actualNode, false, 1, 1);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                        case Node.Size.s1x2:
                            objectInstantiation = Instantiate(tree1x2, islandParent);
                            ChangeTransitable(actualNode, false, 1, 2);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.25f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s2x1:
                            objectInstantiation = Instantiate(tree1x2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 1);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        default:
                            objectInstantiation = Instantiate(tree2x2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 2);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                    }
                    objectInstantiation.transform.position = actualNode.worldPosition;
                    woodInMap += woodByItem;
                    woodNeeded += 1;
                }
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.rock)
                {
                    switch (actualNode.currentSize)
                    {
                        case Node.Size.s1x1:
                            objectInstantiation = Instantiate(rock1x1, islandParent);
                            ChangeTransitable(actualNode, false, 1, 1);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                        case Node.Size.s1x2:
                            objectInstantiation = Instantiate(rocks1x2, islandParent);
                            ChangeTransitable(actualNode, false, 1, 2);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.25f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s2x1:
                            objectInstantiation = Instantiate(rocks1x2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 1);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        default:
                            objectInstantiation = Instantiate(rock2x2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 2);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                    }
                    objectInstantiation.transform.position = actualNode.worldPosition;
                    rockInMap += rockByItem;
                    rockNeeded += 1;
                }
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.village)
                {
                    switch (actualNode.currentSize)
                    {
                        case Node.Size.s2x2:
                            objectInstantiation = Instantiate(village2x2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 2);
                            break;
                        case Node.Size.s3x3:
                            objectInstantiation = Instantiate(village3x3, islandParent);
                            ChangeTransitable(actualNode, false, 3, 3);
                            break;
                        default:
                            objectInstantiation = Instantiate(village4x4, islandParent);
                            ChangeTransitable(actualNode, false, 4, 4);
                            break;
                    }
                    objectInstantiation.transform.position = actualNode.worldPosition;
                    objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90 * Random.Range(0, 4), 0);
                    objectInstantiation.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                    enemiesNeeded += 1;
                    fabricInMap += enemiesByItem;
                }
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.enemy)
                {
                    objectInstantiation = Instantiate(enemy, islandParent);
                    objectInstantiation.transform.position = actualNode.worldPosition;
                    objectInstantiation.SetActive(true);
                    actualNode.isTransitable = false;
                }
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.decoration)
                {
                    switch (actualNode.currentSize)
                    {
                        case Node.Size.s1x2:
                            objectInstantiation = Instantiate(decoration1x2, islandParent);
                            ChangeTransitable(actualNode, false, 1, 2);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.25f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s2x1:
                            objectInstantiation = Instantiate(decoration1x2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 1);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        case Node.Size.s1x3:
                            objectInstantiation = Instantiate(decoration1x3, islandParent);
                            ChangeTransitable(actualNode, false, 1, 3);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.5f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s3x1:
                            objectInstantiation = Instantiate(decoration1x3, islandParent);
                            ChangeTransitable(actualNode, false, 3, 1);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        case Node.Size.s1x4:
                            objectInstantiation = Instantiate(decoration1x4, islandParent);
                            ChangeTransitable(actualNode, false, 1, 4);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.75f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s4x1:
                            objectInstantiation = Instantiate(decoration1x4, islandParent);
                            ChangeTransitable(actualNode, false, 4, 1);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        case Node.Size.s2x2:
                            objectInstantiation = Instantiate(decoration2x2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 2);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90 * Random.Range(0, 4), 0);
                            break;
                        case Node.Size.s2x3:
                            objectInstantiation = Instantiate(decoration2x3, islandParent);
                            ChangeTransitable(actualNode, false, 2, 3);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(-0.25f, 0, -0.5f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s3x2:
                            objectInstantiation = Instantiate(decoration2x3, islandParent);
                            ChangeTransitable(actualNode, false, 3, 2);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        case Node.Size.s2x4:
                            objectInstantiation = Instantiate(decoration2x4, islandParent);
                            ChangeTransitable(actualNode, false, 2, 4);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(-0.25f, 0, -0.75f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s4x2:
                            objectInstantiation = Instantiate(decoration2x4, islandParent);
                            ChangeTransitable(actualNode, false, 4, 2);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        default:
                            objectInstantiation = Instantiate(decoration1x1, islandParent);
                            ChangeTransitable(actualNode, false, 1, 1);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90 * Random.Range(0, 4), 0);
                            break;
                    }
                    objectInstantiation.transform.position = actualNode.worldPosition;
                }
                else if (actualNode.currentType == Node.Type.water)
                {
                    objectInstantiation = Instantiate(water, islandParent);
                    objectInstantiation.transform.position = actualNode.worldPosition;
                }

            }
        }
    }


    void ChangeTransitable(Node nodeToChange, bool _isTransitable, int sizeX, int sizeY)
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                gridScript.grid[nodeToChange.gridPositionX - i, nodeToChange.gridPositionY - j].isTransitable = _isTransitable;
            }
        }
    }

    void ApplyAxerAbilities(GameObject charac)
    {
        Movement axerAbs = charac.GetComponent<Movement>();

        axerAbs.neededTimeMultiplier = AxerAbilities.resourceSpeedMultiplier;//apply
        axerAbs.axePolivalente = AxerAbilities.Polivalente;//apply
        axerAbs.axeStun = AxerAbilities.axeStunt;//applied
        treeTier2 = AxerAbilities.treeTier2;
        resourceTreeMultiplier = AxerAbilities.resourceMultiplier;//apply
        axerAbs.axeTier2 = AxerAbilities.axerTier2;//applied
       

        titan = CharacterAbiliities.Titan;//applied
        axerAbs.bootsMovementSpeed = CharacterAbiliities.bootsMovementMultiplier;//applied
        Market = CharacterAbiliities.market;
        islandTier2 = CharacterAbiliities.islandTier2;
        axerAbs.dashActive = CharacterAbiliities.dashActive;//applied
        goldMultiplier = CharacterAbiliities.goldMultiplier;//applied

    }
    void ApplySwordAbilities(GameObject charac)
    {
        Movement swordAbs = charac.GetComponent<Movement>();

        swordAbs.attackSpeedCooldown = SwordAbilities.resourceSpeedMultiplier;
        swordAbs.swordPolivalente = SwordAbilities.Polivalente;//apply
        swordAbs.swordSeep = SwordAbilities.swordSweep;
        enemyTier2 = SwordAbilities.enemyTier2;
        resourceFabricMultiplier = SwordAbilities.resourceMultiplier;//applied
        swordAbs.swordTier2 = SwordAbilities.swordTier2;//applied
        

        titan = CharacterAbiliities.Titan;//applied
        swordAbs.bootsMovementSpeed = CharacterAbiliities.bootsMovementMultiplier;//applied
        Market = CharacterAbiliities.market;
        islandTier2 = CharacterAbiliities.islandTier2;
        swordAbs.dashActive = CharacterAbiliities.dashActive;//applied
        goldMultiplier = CharacterAbiliities.goldMultiplier;//applied
    }
    void ApplyBomberAbilities(GameObject charac)
    {
        Movement bomberAbs = charac.GetComponent<Movement>();

        bomberAbs.neededBombMultiplier = BomberAbilities.resourceSpeedMultiplier;//apply
        bomberAbs.bombPolivalente = BomberAbilities.Polivalente;//apply
        bomberAbs.bomb.GetComponent<Bomb>().knockback = BomberAbilities.bombKnockBack; //applied
        rockTier2 = BomberAbilities.rockTier2;
        resourceStoneMultiplier = BomberAbilities.resourceMultiplier;//applied
        bomberAbs.bombTier2 = BomberAbilities.explosiveTier2;
        

        titan = CharacterAbiliities.Titan;//applied
        bomberAbs.bootsMovementSpeed = CharacterAbiliities.bootsMovementMultiplier;//applied
        Market = CharacterAbiliities.market;
        islandTier2 = CharacterAbiliities.islandTier2;
        bomberAbs.dashActive = CharacterAbiliities.dashActive;//applied
        goldMultiplier = CharacterAbiliities.goldMultiplier;//applied
    }

    int RandomSecondaryObjective()
    {
        return Random.Range(0, secondaryObjectives.Length);
    }

    bool CheckSecondaryObjective()
    {
        switch (secondaryObjectiveID)
        {
            case 0:
                return remainingTimeInLevel >= 20.0f;
            case 1:
                return remainingTimeInLevel >= 10.0f;
            case 2:
                if (player.GetComponent<Movement>().actualType == Movement.playerType.ace) return collectedWood >= 70 * woodInMap / 100;
                else if (player.GetComponent<Movement>().actualType == Movement.playerType.pick) return collectedRock >= 70 * rockInMap / 100;
                else return collectedFabrics >= 70 * fabricInMap / 100;
            case 3:
                return livesNumber == 3; //habrá que modificarlo si cambiamos el numero maximo de vidas
            default:
                return false;
        }
    }

}
