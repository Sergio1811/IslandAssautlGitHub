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

    int woodNeeded;
    int rockNeeded;
    int enemiesNeeded;
    public Text objectiveText;

    public int rockByItem;
    public int woodByItem;
    public int enemiesByItem;

    [HideInInspector] public int collectedWood = 0;
    [HideInInspector] public int collectedRock = 0;
    [HideInInspector] public int collectedFabrics = 0;

    [HideInInspector] public int currentCoins = 0;
    [HideInInspector] public static int totalCoins = 0;

    public Text woodText;
    public Text rockText;
    public Text fabricText;
    public Text currentCoinsText;
    public Text totalCoinsText;

    public float timeByLevel;
    private float remainingTimeInLevel;
    public Text timeText;

    [HideInInspector] public GameObject player;
    GameObject portalExit;

    public Grid gridScript;
    public GameObject tree, enemy, rock, decoration, village, portal, water;
    Node startNode, endNode;
    public GameObject[] islands;
    Transform islandParent;

    static int protoIsland;

    public GameObject livesGroup;
    int livesNumber;

    private void Awake()
    {
        Instance = this;
        islands[protoIsland].SetActive(true);
        islandParent = islands[protoIsland].transform.GetChild(0);
        gridScript.GenerateGrid();
    }
    
    void Start()
    {
        currentCoinsText.text = "Coins: " + currentCoins.ToString();
        totalCoinsText.text = "Total Coins: " + totalCoins.ToString();
        islands[protoIsland].GetComponent<NavMeshSurface>().BuildNavMesh();
        remainingTimeInLevel = timeByLevel;
        InstantiateObjectInGrid();
        //player = GetRandomPlayer();
        player = ProtoPlayer();

        livesNumber = livesGroup.transform.childCount;
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
    }


    public GameObject ProtoPlayer()
    {
        GameObject p = null;

        switch (protoIsland)
        {
            case 0:
            case 4:
                p = Instantiate(acerPrefab, startNode.worldPosition, Quaternion.LookRotation(transform.forward));
                woodText.gameObject.SetActive(true);
                woodNeeded = woodNeeded - woodNeeded / 2;
                objectiveText.text = "1: Consigue " + woodNeeded*woodByItem + " maderas.";
                break;
            case 1:
            case 5:
                p = Instantiate(pickerPrefab, startNode.worldPosition, Quaternion.LookRotation(transform.forward));
                rockText.gameObject.SetActive(true);
                rockNeeded = rockNeeded - rockNeeded / 2;
                objectiveText.text = "1: Consigue " + rockNeeded*rockByItem + " rocas.";
                break;
            default:
                p = Instantiate(sworderPrefab, startNode.worldPosition, Quaternion.LookRotation(transform.forward));
                fabricText.gameObject.SetActive(true);
                enemiesNeeded = enemiesNeeded - enemiesNeeded / 2;
                objectiveText.text = "1: Consigue " + enemiesNeeded*enemiesByItem + " pieles.";
                break;
        }
        
        return p;
    }
    
    public GameObject GetRandomPlayer()
    {
        int rdm = Random.Range(1, 4);
        GameObject p = null;

        switch (rdm)
        {
            case 1:
                p = Instantiate(pickerPrefab, startNode.worldPosition, Quaternion.LookRotation(transform.forward));
                woodText.gameObject.SetActive(true);
                break;
            case 2:
                p = Instantiate(acerPrefab, startNode.worldPosition, Quaternion.LookRotation(transform.forward));
                rockText.gameObject.SetActive(true);
                break;
            case 3:
                p = Instantiate(sworderPrefab, startNode.worldPosition, Quaternion.LookRotation(transform.forward));
                fabricText.gameObject.SetActive(true);
                break;
        }
        return p;
    }


    public void EndLevel()
    {
        Destroy(player);
        player = GetRandomPlayer();
    }

    public void EndProtoLevel()
    {
        protoIsland++;
        if (protoIsland >= islands.Length)
            protoIsland = 0;
        SceneManager.LoadScene(0);

        if (livesNumber > 0)
            totalCoins += currentCoins;

        currentCoins = 0;
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

    public void LevelComplete ()
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
                    objectInstantiation.transform.position = actualNode.worldPosition;
                    actualNode.isTransitable = false;
                    portalExit = objectInstantiation;
                }
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.tree)
                {
                    objectInstantiation = Instantiate(tree, islandParent);
                    objectInstantiation.transform.position = actualNode.worldPosition;
                    objectInstantiation.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                    actualNode.isTransitable = false;
                    gridScript.grid[i - 1, j].isTransitable = false;
                    gridScript.grid[i, j - 1].isTransitable = false;
                    gridScript.grid[i - 1, j - 1].isTransitable = false;
                    woodNeeded += 1;
                }
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.rock)
                {
                    objectInstantiation = Instantiate(rock, islandParent);
                    objectInstantiation.transform.position = actualNode.worldPosition;
                    actualNode.isTransitable = false;
                    gridScript.grid[i - 1, j].isTransitable = false;
                    gridScript.grid[i, j - 1].isTransitable = false;
                    gridScript.grid[i - 1, j - 1].isTransitable = false;
                    rockNeeded += 1;
                }
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.village)
                {
                    objectInstantiation = Instantiate(village, islandParent);
                    objectInstantiation.transform.position = actualNode.worldPosition;
                    actualNode.isTransitable = false;
                    gridScript.grid[i - 1, j].isTransitable = false;
                    gridScript.grid[i - 2, j].isTransitable = false;
                    gridScript.grid[i - 3, j].isTransitable = false;
                    gridScript.grid[i, j - 1].isTransitable = false;
                    gridScript.grid[i, j - 2].isTransitable = false;
                    gridScript.grid[i, j - 3].isTransitable = false;
                    gridScript.grid[i - 1, j - 1].isTransitable = false;
                    gridScript.grid[i - 2, j - 1].isTransitable = false;
                    gridScript.grid[i - 3, j - 1].isTransitable = false;
                    gridScript.grid[i - 1, j - 2].isTransitable = false;
                    gridScript.grid[i - 1, j - 3].isTransitable = false;
                    gridScript.grid[i - 2, j - 2].isTransitable = false;
                    gridScript.grid[i - 3, j - 3].isTransitable = false;
                    gridScript.grid[i - 2, j - 3].isTransitable = false;
                    gridScript.grid[i - 3, j - 2].isTransitable = false;
                    enemiesNeeded += 1;
                }
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.enemy)
                {
                    objectInstantiation = Instantiate(enemy, islandParent);
                    objectInstantiation.transform.position = actualNode.worldPosition;
                    actualNode.isTransitable = false;
                }
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.decoration)
                {
                    objectInstantiation = Instantiate(decoration, islandParent);
                    objectInstantiation.transform.position = actualNode.worldPosition;
                    actualNode.isTransitable = false;
                }
                else if (actualNode.currentType == Node.Type.water)
                {
                    objectInstantiation = Instantiate(water, islandParent);
                    objectInstantiation.transform.position = actualNode.worldPosition;
                }

            }
        }
    }
}
