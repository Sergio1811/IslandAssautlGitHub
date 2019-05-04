using System.Collections;
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

    #region
    bool Titan = false;
    bool islandTier2 = false;
    float goldMultiplier = 1.0f;
    bool Market = false;
    bool swordTier2 = false;
    float resourceFabricMultiplier = 1.0f;
    bool enemyTier2 = false;
    bool axeTier2 = false;
    float resourceTreeMultiplier = 1.0f;
    bool treeTier2 = false;
    bool bombTier2 = false;
    float resourceStoneMultiplier = 1.0f;
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
        currentCoinsText.text = "Coins: " + currentCoins.ToString();
        totalCoinsText.text = "Total Coins: " + totalCoins.ToString();
        totalWoodText.text = "Total Wood: " + totalWood.ToString();
        totalRockText.text = "Total Rocks: " + totalRock.ToString();
        totalFabricsText.text = "Total Fabrics: " + totalFabrics.ToString();

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
        livesNumber = livesGroup.transform.childCount;

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
                p = Instantiate(acerPrefab, startNode.worldPosition, Quaternion.LookRotation(transform.forward));
                woodText.gameObject.SetActive(true);
                woodNeeded = woodNeeded - woodNeeded / 2;
                objectiveText.text = "1: Consigue " + woodNeeded * woodByItem + " maderas.";
                ApplyAxerAbilities(p);
                break;
            case 1:
                p = Instantiate(pickerPrefab, startNode.worldPosition, Quaternion.LookRotation(transform.forward));
                rockText.gameObject.SetActive(true);
                rockNeeded = rockNeeded - rockNeeded / 2;
                objectiveText.text = "1: Consigue " + rockNeeded * rockByItem + " rocas.";
                ApplyBomberAbilities(p);
                break;
            case 2:
                p = Instantiate(sworderPrefab, startNode.worldPosition, Quaternion.LookRotation(transform.forward));
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

        print(CheckSecondaryObjective());
        if (CheckSecondaryObjective()) totalCoins += 50;

        SaveManager.Instance.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void PickWood()
    {
        collectedWood += woodByItem;
        currentCoins += woodByItem;
        woodText.text = "Wood: " + collectedWood.ToString();
        currentCoinsText.text = "Coins: " + currentCoins.ToString();
        if (collectedWood >= woodNeeded * woodByItem)
            ActivatePortal();
    }

    public void PickRock()
    {
        collectedRock += rockByItem;
        currentCoins += rockByItem;
        rockText.text = "Rock: " + collectedRock.ToString();
        currentCoinsText.text = "Coins: " + currentCoins.ToString();
        if (collectedRock >= rockNeeded * rockByItem)
            ActivatePortal();
    }

    public void PickFabrics()
    {
        collectedFabrics += enemiesByItem;
        currentCoins += enemiesByItem;
        fabricText.text = "Fabric: " + collectedFabrics.ToString();
        currentCoinsText.text = "Coins: " + currentCoins.ToString();
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
                    enemiesNeeded += 1;
                }
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.enemy)
                {
                    objectInstantiation = Instantiate(enemy, islandParent);
                    objectInstantiation.transform.position = actualNode.worldPosition;
                    actualNode.isTransitable = false;
                    fabricInMap += enemiesByItem;
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

        axerAbs.neededTimeMultiplier = AxerAbilities.resourceSpeedMultiplier;
        axerAbs.axePolivalente = AxerAbilities.Polivalente;
        axerAbs.axeStun = AxerAbilities.axeStunt;
        axerAbs.treeTier2 = AxerAbilities.treeTier2;
        axerAbs.resourceTreeMultiplier = AxerAbilities.resourceMultiplier;
        axerAbs.axeTier2 = AxerAbilities.axerTier2;

        axerAbs.Titan = CharacterAbiliities.Titan;
        axerAbs.bootsMovementSpeed = CharacterAbiliities.bootsMovementMultiplier;
        axerAbs.Market = CharacterAbiliities.market;
        axerAbs.islandTier2 = CharacterAbiliities.islandTier2;
        axerAbs.dashActive = CharacterAbiliities.dashActive;
        axerAbs.goldMultiplier = CharacterAbiliities.goldMultiplier;

    }
    void ApplySwordAbilities(GameObject charac)
    {
        Movement swordAbs = charac.GetComponent<Movement>();

        swordAbs.attackSpeedCooldown = SwordAbilities.resourceSpeedMultiplier;
        swordAbs.swordPolivalente = SwordAbilities.Polivalente;
        swordAbs.swordSeep = SwordAbilities.swordSweep;
        swordAbs.enemyTier2 = SwordAbilities.enemyTier2;
        swordAbs.resourceFabricMultiplier = SwordAbilities.resourceMultiplier;
        swordAbs.swordTier2 = SwordAbilities.swordTier2;

        swordAbs.Titan = CharacterAbiliities.Titan;
        swordAbs.bootsMovementSpeed = CharacterAbiliities.bootsMovementMultiplier;
        swordAbs.Market = CharacterAbiliities.market;
        swordAbs.islandTier2 = CharacterAbiliities.islandTier2;
        swordAbs.dashActive = CharacterAbiliities.dashActive;
        swordAbs.goldMultiplier = CharacterAbiliities.goldMultiplier;
    }
    void ApplyBomberAbilities(GameObject charac)
    {
        Movement bomberAbs = charac.GetComponent<Movement>();

        bomberAbs.neededBombMultiplier = BomberAbilities.resourceSpeedMultiplier;
        bomberAbs.bombPolivalente = BomberAbilities.Polivalente;
        bomberAbs.bombKnockback = BomberAbilities.bombKnockBack;
        bomberAbs.rockTier2 = BomberAbilities.rockTier2;
        bomberAbs.resourceStoneMultiplier = BomberAbilities.resourceMultiplier;
        bomberAbs.bombTier2 = BomberAbilities.explosiveTier2;

        bomberAbs.Titan = CharacterAbiliities.Titan;
        bomberAbs.bootsMovementSpeed = CharacterAbiliities.bootsMovementMultiplier;
        bomberAbs.Market = CharacterAbiliities.market;
        bomberAbs.islandTier2 = CharacterAbiliities.islandTier2;
        bomberAbs.dashActive = CharacterAbiliities.dashActive;
        bomberAbs.goldMultiplier = CharacterAbiliities.goldMultiplier;
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
