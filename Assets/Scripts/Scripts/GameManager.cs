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
    public GameObject entreIslasCanvas;
    public GameObject abilitiesCanvas;
    public GameObject mainCanvas;

    int characterNumber;

    int woodInMap = 0;
    int rockInMap = 0;
    int fabricInMap = 0;
    int wood2InMap = 0;
    int rock2InMap = 0;
    int fabric2InMap = 0;

    int woodNeeded;
    int rockNeeded;
    int fabricNeeded;
    int wood2Needed;
    int rock2Needed;
    int fabric2Needed;
    public string[] secondaryObjectives;

    public int rockByItem;
    public int woodByItem;
    public int enemiesByItem;

    [HideInInspector] public int collectedWood = 0;
    [HideInInspector] public int collectedRock = 0;
    [HideInInspector] public int collectedFabrics = 0;
    [HideInInspector] public int collectedWood2 = 0;
    [HideInInspector] public int collectedRock2 = 0;
    [HideInInspector] public int collectedFabrics2 = 0;

    [HideInInspector] public int currentCoins = 0;
    [HideInInspector] public static int totalCoins = 0;
    [HideInInspector] public static int totalWood = 0;
    [HideInInspector] public static int totalRock = 0;
    [HideInInspector] public static int totalFabrics = 0;
    [HideInInspector] public static int totalWood2 = 0;
    [HideInInspector] public static int totalRock2 = 0;
    [HideInInspector] public static int totalFabrics2 = 0;

    public GameObject recursoPrincipal, recursoPrincipalTier2;
    public GameObject principalRockImage, principalWoodImage, principalFabricImage;
    public GameObject principalRockImageTier2, principalWoodImageTier2, principalFabricImageTier2;
    public Text recursoPrincipalText, recursoPrincipalTextTier2, cointsText, recursoPrincipalCaughtText, recursoPrincipalCaughtTier2;

    public GameObject rockSecundary, rockSecundaryTier2, woodSecundary, woodSecundaryTier2, fabricSecundary, fabricSecundaryTier2;
    public Text woodText, woodTextTier2, rockText, rockTextTier2, fabricText, fabricTextTier2;

    public Image sworderImage, bomberImage, axerImage, bomberTier2Image;
    public GameObject woodObjectiveImage, rockObjectiveImage, timeObjectiveImage, fabricObjectiveImage, livesObjectiveImage;
    public Text secondaryObjectiveText;
    GameObject[] objectiveImage;

    public GameObject rightPanel, rightPanelSecundaries, leftPanel;
    public Transform positionRight, positionLeft;
    public float speedPanels;
    float initialSpeedPanels;

    public float timeByLevel;
    private float remainingTimeInLevel;
    public Text timeText;

    [HideInInspector] public GameObject player;
    Movement playerScript;
    GameObject portalExit;

    public Grid gridScript;
    public GameObject tree1x1, tree2x2, tree1x2, enemy, rock1x1, rock2x2, rock1x2, village2x2, village3x3, village4x4, portal, water;
    public GameObject tree1x1_T2, tree2x2_T2, tree1x2_T2, enemy_T2, rock1x1_T2, rock2x2_T2, rock1x2_T2, village2x2_T2, village3x3_T2, village4x4_T2;
    public GameObject decoration1x1, decoration1x2, decoration1x3, decoration1x4, decoration2x2, decoration2x3, decoration2x4;

    Node startNode, endNode;
    int protoIsland;
    public GameObject[] islands;
    Transform islandParent;

    public GameObject livesGroup;
    int livesNumber, initialLiveNumber;

    int secondaryObjectiveID;


    List<MeshRenderer> meshList = new List<MeshRenderer>();

    public float waitToStartTime, initialPanelPositionsTime;
    float waitTimer;
    public bool startGame;
    public float instantiationHeight;

    //Menu entre islas cantidades de recursos
    public Text abilitesCoinsText;
    public Text totalEndCoinsText, totalEndWoodText, totalEndRockText, totalEndFabricText, totalEndWoodTier2Text, totalEndRockTier2Text, totalEndFabricTier2Text;
    public Text resultEndCoinsText, resultEndWoodText, resultEndRocksText, resultEndFabricText, resultEndWoodTier2Text, resultEndRockTier2Text, resultEndFabricsTier2Text;
    public Color positiveColor, negativeColor;

    bool portalActivated = false;

    bool movingCamera;
    public GameObject endCameraPosition;
    public Camera mainCamera;
    public float initialCameraSpeed;
    float cameraSpeed;
    GameObject cameraAnchor;

    public GameObject[] buttonsArray;
    public GameObject buttonMarket;
    int menuArrayNumber;
    public Color buttonSelectedColor;
    GameObject selectedButton;
    bool hasMoved, movementOn;
    float timerMovement;
    int numberOfButtons;

    #region
    [HideInInspector]
    public bool titan = false;//applied
    [HideInInspector]
    public bool islandTier2 = false;
    float goldMultiplier = 1.0f;//applied
    [HideInInspector]
    public bool Market = false;
    float resourceFabricMultiplier = 1.0f;//applied
    [HideInInspector]
    public bool enemyTier2 = false;
    float resourceTreeMultiplier = 1.0f;//aaplied
    [HideInInspector]
    public bool treeTier2 = false;
    float resourceStoneMultiplier = 1.0f;//applied
    [HideInInspector]
    public bool rockTier2 = false;
    #endregion

    static int lastCharacter = -1;
    static List<int> last5Characters = new List<int>();

    static int lastIsland = -1;
    static List<int> lastIslandList = new List<int>();

    bool gameOver = false;

    private void Awake()
    {
        Instance = this;
        startGame = false;

        RandomIsland();
        islands[protoIsland].SetActive(true);
        islandParent = islands[protoIsland].transform.GetChild(0);


        RandomCharacter();

        //characterNumber = 2; // -> 0 para solo leñadores, 1 para solo bombers, 2 para solo sworders
        player = PlayerInstantiation();
        playerScript = player.GetComponent<Movement>();
        playerScript.enabled = false;

        gridScript.GenerateGrid(characterNumber);

        cameraAnchor = mainCamera.transform.parent.gameObject;
    }

    void Start()
    {
        entreIslasCanvas.SetActive(false);
        abilitiesCanvas.SetActive(false);

        islands[protoIsland].GetComponent<NavMeshSurface>().BuildNavMesh();
        remainingTimeInLevel = timeByLevel;

        InstantiateObjectInGrid();

        player.transform.position = (startNode.worldPosition + (Vector3.up * 3));
        player.transform.rotation = Quaternion.LookRotation(transform.forward);

        switch (characterNumber)
        {
            case 0:
                livesNumber = 3;

                woodText.gameObject.SetActive(true);
                woodNeeded = woodInMap / 2;
                recursoPrincipalText.text = woodNeeded.ToString();
                principalWoodImage.SetActive(true);
                fabricSecundary.SetActive(true);
                rockSecundary.SetActive(true);

                if (treeTier2)
                {
                    wood2Needed = wood2InMap / 2;
                    principalWoodImageTier2.SetActive(true);
                    recursoPrincipalTier2.SetActive(true);
                    recursoPrincipalTextTier2.text = wood2Needed.ToString();
                }
                if (rockTier2)
                    rockSecundaryTier2.SetActive(true);
                if (enemyTier2)
                    fabricSecundaryTier2.SetActive(true);

                cointsText.text = currentCoins.ToString();
                axerImage.enabled = true;
                break;
            case 1:
                livesNumber = 2;

                rockText.gameObject.SetActive(true);
                rockNeeded = rockInMap / 2;
                recursoPrincipalText.text = rockNeeded.ToString();
                principalRockImage.SetActive(true);
                fabricSecundary.SetActive(true);
                woodSecundary.SetActive(true);
                if (rockTier2)
                {
                    rock2Needed = rock2InMap / 2;
                    principalRockImageTier2.SetActive(true);
                    recursoPrincipalTier2.SetActive(true);
                    recursoPrincipalTextTier2.text = rock2Needed.ToString();
                    bomberTier2Image.enabled = true;
                }
                else
                    bomberImage.enabled = true;
                if (treeTier2)
                    woodSecundaryTier2.SetActive(true);
                if (enemyTier2)
                    fabricSecundaryTier2.SetActive(true);

                cointsText.text = currentCoins.ToString();
                break;
            case 2:
                livesNumber = 4;

                fabricText.gameObject.SetActive(true);
                fabricNeeded = fabricInMap / 2;
                recursoPrincipalText.text = fabricNeeded.ToString();
                principalFabricImage.SetActive(true);
                rockSecundary.SetActive(true);
                woodSecundary.SetActive(true);
                if (enemyTier2)
                {
                    fabric2Needed = fabric2InMap / 2;
                    principalFabricImageTier2.SetActive(true);
                    recursoPrincipalTier2.SetActive(true);
                    recursoPrincipalTextTier2.text = fabric2Needed.ToString();
                }
                if (rockTier2)
                    rockSecundaryTier2.SetActive(true);
                if (treeTier2)
                    woodSecundaryTier2.SetActive(true);

                cointsText.text = currentCoins.ToString();
                sworderImage.enabled = true;
                break;
        }
        if (titan)
            livesNumber++;
        for (int i = 0; i < livesNumber; i++)
            livesGroup.transform.GetChild(i).gameObject.SetActive(true);
        initialLiveNumber = livesNumber;

        secondaryObjectives = new string[4];
        objectiveImage = new GameObject[4];
        secondaryObjectives[0] = "20s";
        objectiveImage[0] = timeObjectiveImage;
        secondaryObjectives[1] = "10s";
        objectiveImage[1] = timeObjectiveImage;

        if (characterNumber == 0)
        {
            secondaryObjectives[2] = "" + (70 * woodInMap / 100) + "";
            objectiveImage[2] = woodObjectiveImage;
        }
        else if (characterNumber == 1)
        {
            secondaryObjectives[2] = "" + (70 * rockInMap / 100) + "";
            objectiveImage[2] = rockObjectiveImage;
        }
        else
        {
            secondaryObjectives[2] = "" + (70 * fabricInMap / 100) + "";
            objectiveImage[2] = fabricObjectiveImage;
        }
        secondaryObjectives[3] = livesNumber.ToString();
        objectiveImage[3] = livesObjectiveImage;

        secondaryObjectiveID = RandomSecondaryObjective();
        objectiveImage[secondaryObjectiveID].SetActive(true);
        secondaryObjectiveText.text = secondaryObjectives[secondaryObjectiveID];

        initialSpeedPanels = speedPanels;
    }

    void Update()
    {
        if (startGame)
        {
            if (!gameOver) remainingTimeInLevel -= Time.deltaTime;

            else if (movingCamera)
            {
                if (cameraSpeed > initialCameraSpeed / 5)
                    cameraSpeed -= Time.deltaTime * initialCameraSpeed / 2f;

                mainCamera.transform.localPosition = Vector3.MoveTowards(mainCamera.transform.localPosition, endCameraPosition.transform.position, cameraSpeed * Time.deltaTime);
                mainCamera.transform.localRotation = Quaternion.RotateTowards(mainCamera.transform.localRotation, endCameraPosition.transform.rotation, cameraSpeed * Time.deltaTime / 3f);

                cameraAnchor.transform.localRotation = Quaternion.RotateTowards(cameraAnchor.transform.localRotation, Quaternion.Euler(new Vector3(0f, 0, 0)), cameraSpeed * Time.deltaTime / 2f);
                entreIslasCanvas.transform.GetChild(0).localPosition = Vector3.MoveTowards(entreIslasCanvas.transform.GetChild(0).localPosition, Vector3.zero, cameraSpeed * Time.deltaTime * 3.5f);
                if (mainCamera.orthographicSize < 80)
                    mainCamera.orthographicSize += cameraSpeed * Time.deltaTime;


                if (GetSqrDistanceXZToPosition(mainCamera.transform.localRotation.eulerAngles, endCameraPosition.transform.rotation.eulerAngles) <= 0.1)
                {
                    movingCamera = false;
                    menuArrayNumber = 0;
                    selectedButton = buttonsArray[0];
                    selectedButton.GetComponent<Image>().color = buttonSelectedColor;
                    numberOfButtons = buttonsArray.Length - 1;
                }
            }
            else
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

                        if (menuArrayNumber == numberOfButtons && Market)
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

                        if (menuArrayNumber == numberOfButtons && Market)
                            selectedButton = buttonMarket;
                        else
                            selectedButton = buttonsArray[menuArrayNumber];

                        selectedButton.GetComponent<Image>().color = buttonSelectedColor;
                        hasMoved = true;
                    }
                    else if (horizontal > 0.2f && menuArrayNumber == numberOfButtons && selectedButton != buttonsArray[menuArrayNumber] && Market)
                    {
                        selectedButton.GetComponent<Image>().color = Color.white;
                        selectedButton = buttonsArray[menuArrayNumber];
                        selectedButton.GetComponent<Image>().color = buttonSelectedColor;
                        hasMoved = true;
                    }
                    else if (horizontal < -0.2f && menuArrayNumber == numberOfButtons && selectedButton != buttonMarket && Market)
                    {
                        selectedButton.GetComponent<Image>().color = Color.white;
                        selectedButton = buttonMarket;
                        selectedButton.GetComponent<Image>().color = buttonSelectedColor;
                        hasMoved = true;
                    }
                    else if (InputManager.Instance.GetInputDown("Submit"))
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

            if (Input.GetKeyDown(KeyCode.M))
            {
                totalCoins += 500;
                SaveManager.Instance.Save();
                totalEndCoinsText.text = totalCoins.ToString();
                abilitesCoinsText.text = totalCoins.ToString();
                gameOver = true;
            }
        }
        else
        {

            waitTimer += Time.deltaTime;

            if (waitTimer >= waitToStartTime)
            {
                if (speedPanels > initialSpeedPanels / 4)
                    speedPanels -= Time.deltaTime * initialSpeedPanels / 3f;
                rightPanel.transform.position = Vector3.MoveTowards(rightPanel.transform.position, positionRight.transform.position, speedPanels * 1.5f * Time.deltaTime);
                leftPanel.transform.position = Vector3.MoveTowards(leftPanel.transform.position, positionLeft.transform.position, speedPanels * Time.deltaTime);

                if (GetSqrDistanceXZToPosition(rightPanel.transform.position, positionRight.transform.position) <= 0.1 &&
                    GetSqrDistanceXZToPosition(leftPanel.transform.position, positionLeft.transform.position) <= 0.1)
                //if (rightPanel.transform.position == positionRight.transform.position && leftPanel.transform.position == positionLeft.transform.position)
                {
                    waitTimer = 0;
                    startGame = true;

                    rightPanelSecundaries.SetActive(true);
                    
                    playerScript.enabled = true;
                }
            }
            else if (waitTimer >= waitToStartTime - 0.2f && leftPanel.transform.parent != mainCanvas.transform)
            {
                leftPanel.transform.SetParent(mainCanvas.transform);
                rightPanel.GetComponent<VerticalLayoutGroup>().childAlignment = UnityEngine.TextAnchor.UpperRight;
            }

        }
    }

    void RandomIsland()
    {
        protoIsland = Random.Range(0, islands.Length);

        while(protoIsland == lastIsland)
        {
            protoIsland = Random.Range(0, islands.Length);
        }

        lastIsland = protoIsland;
    }

    void RandomCharacter()
    {
        if (last5Characters.Count == 4)
        {
            if (!last5Characters.Contains(0)) characterNumber = 0;
            else if (!last5Characters.Contains(1)) characterNumber = 1;
            else if (!last5Characters.Contains(2)) characterNumber = 2;

            else
            {
                characterNumber = Random.Range(0, 3);

                while (characterNumber == lastCharacter)
                {
                    characterNumber = Random.Range(0, 3);

                }
            }

            last5Characters.RemoveAt(0);
        }

        else
        {
            characterNumber = Random.Range(0, 3);

            while (characterNumber == lastCharacter)
            {
                characterNumber = Random.Range(0, 3);

            }
        }

        last5Characters.Add(characterNumber);
        lastCharacter = characterNumber;
    }

    public GameObject PlayerInstantiation()
    {
        GameObject p = null;

        switch (characterNumber)
        {
            case 0:
                p = Instantiate(acerPrefab);
                ApplyAxerAbilities(p);
                break;
            case 1:
                p = Instantiate(pickerPrefab);
                ApplyBomberAbilities(p);
                break;
            case 2:
                p = Instantiate(sworderPrefab);
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
        gameOver = true;

        protoIsland++;
        if (protoIsland >= islands.Length)
            protoIsland = 0;

        if (livesNumber > 0) //si se pasa el nivel
        {
            totalFabrics += collectedFabrics;
            totalRock += collectedRock;
            totalWood += collectedWood;
            totalFabrics2 += collectedFabrics2;
            totalRock2 += collectedRock2;
            totalWood2 += collectedWood2;

            if (collectedFabrics > 0)
            {
                resultEndFabricText.text = "+" + collectedFabrics.ToString();
                resultEndFabricText.color = positiveColor;
            }
            if (collectedRock > 0)
            {
                resultEndRocksText.text = "+" + collectedRock.ToString();
                resultEndRocksText.color = positiveColor;
            }
            if (collectedWood > 0)
            {
                resultEndWoodText.text = "+" + collectedWood.ToString();
                resultEndWoodText.color = positiveColor;
            }
            if (collectedFabrics2 > 0)
            {
                resultEndFabricsTier2Text.text = "+" + collectedFabrics2.ToString();
                resultEndFabricsTier2Text.color = positiveColor;
            }
            if (collectedRock2 > 0)
            {
                resultEndRockTier2Text.text = "+" + collectedRock2.ToString();
                resultEndRockTier2Text.color = positiveColor;
            }
            if (collectedFabrics > 0)
            {
                resultEndWoodTier2Text.text = "+" + collectedWood2.ToString();
                resultEndWoodTier2Text.color = positiveColor;
            }
        }

        else if (livesNumber <= 0 && !portalActivated) //si muere y no habia cumplido el primer objetivo
        {
            int pers;

            pers = totalFabrics * 10 / 100;
            totalFabrics -= pers;
            if (pers > 0)
            {
                resultEndFabricText.text = "-" + pers.ToString();
                resultEndFabricText.color = negativeColor;
            }
            if (totalFabrics < 0) totalFabrics = 0;

            pers = totalRock * 10 / 100;
            totalRock -= pers;
            if (pers > 0)
            {
                resultEndRocksText.text = "-" + pers.ToString();
                resultEndRocksText.color = negativeColor;
            }
            if (totalRock < 0) totalRock = 0;

            pers = totalWood * 10 / 100;
            totalWood -= pers;
            if (pers > 0)
            {
                resultEndWoodText.text = "-" + pers.ToString();
                resultEndWoodText.color = negativeColor;
            }
            if (totalWood < 0) totalWood = 0;

            pers = totalFabrics2 * 10 / 100;
            totalFabrics2 -= pers;
            if (pers > 0)
            {
                resultEndFabricsTier2Text.text = "-" + pers.ToString();
                resultEndFabricsTier2Text.color = negativeColor;
            }
            if (totalFabrics2 < 0) totalFabrics2 = 0;

            pers = totalRock2 * 10 / 100;
            totalRock2 -= pers;
            if (pers > 0)
            {
                resultEndRockTier2Text.text = "-" + pers.ToString();
                resultEndRockTier2Text.color = negativeColor;
            }
            if (totalRock2 < 0) totalRock2 = 0;

            pers = totalWood2 * 10 / 100;
            totalWood2 -= pers;
            if (pers > 0)
            {
                resultEndWoodTier2Text.text = "-" + pers.ToString();
                resultEndWoodTier2Text.color = negativeColor;
            }
            if (totalWood2 < 0) totalWood2 = 0;
        }

        //si muere y habia activado el portal, no pierde ni gana nada, solo las monedas

        if (currentCoins != 0)
        {
            resultEndCoinsText.text = currentCoins.ToString();
            resultEndCoinsText.color = positiveColor;
        }
        totalCoins += currentCoins; //las monedas las gana siempre

        currentCoins = 0;

        meshList.Clear();
        SaveManager.Instance.Save();


        totalEndCoinsText.text = totalCoins.ToString();
        abilitesCoinsText.text = totalCoins.ToString();

        totalEndWoodText.text = totalWood.ToString();
        totalEndRockText.text = totalRock.ToString();
        totalEndFabricText.text = totalFabrics.ToString();
        totalEndWoodTier2Text.text = totalWood2.ToString();
        totalEndRockTier2Text.text = totalRock2.ToString();
        totalEndFabricTier2Text.text = totalFabrics2.ToString();

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

        mainCanvas.SetActive(false);
        movingCamera = true;
        cameraAnchor.GetComponent<CameraRotation>().enabled = false;
        player.GetComponent<Movement>().enabled = false;
        entreIslasCanvas.SetActive(true);
        cameraSpeed = initialCameraSpeed;

        if (Market)
            buttonMarket.SetActive(true);
    }


    public void PickWood(int woodTier)
    {
        if (woodTier == 1)
            collectedWood += (int)(woodByItem * resourceTreeMultiplier);
        else
            collectedWood2 += (int)(woodByItem * resourceTreeMultiplier);

        currentCoins += (int)(woodByItem * resourceTreeMultiplier * goldMultiplier);
        cointsText.text = currentCoins.ToString();

        if (characterNumber == 0)
        {
            recursoPrincipalCaughtText.text = collectedWood.ToString();
            if (treeTier2)
                recursoPrincipalCaughtTier2.text = collectedWood2.ToString();


            if (collectedWood >= woodNeeded)
            {
                if (treeTier2)
                {
                    if (collectedWood2 >= wood2Needed)
                        ActivatePortal();
                }

                else
                    ActivatePortal();
            }

        }
        else
        {
            woodText.text = collectedWood.ToString();
            if (treeTier2)
                woodTextTier2.text = collectedWood2.ToString();
        }
    }

    public void PickRock(int rockTier)
    {
        if (rockTier == 1)
            collectedRock += (int)(rockByItem * resourceStoneMultiplier);
        else
            collectedRock2 += (int)(rockByItem * resourceStoneMultiplier);

        currentCoins += (int)(rockByItem * resourceStoneMultiplier * goldMultiplier);
        cointsText.text = currentCoins.ToString();

        if (characterNumber == 1)
        {
            recursoPrincipalCaughtText.text = collectedRock.ToString();
            if (rockTier2)
                recursoPrincipalCaughtTier2.text = collectedRock2.ToString();

            if (collectedRock >= rockNeeded)
            {
                if (rockTier2)
                {
                    if (collectedRock2 >= rock2Needed)
                        ActivatePortal();
                }

                else
                    ActivatePortal();
            }
        }
        else
        {
            rockText.text = collectedRock.ToString();
            if (rockTier2)
                rockTextTier2.text = collectedRock2.ToString();
        }
    }

    public void PickFabrics(int fabricTier)
    {
        if (fabricTier == 1)
            collectedFabrics += (int)(enemiesByItem * resourceFabricMultiplier);
        else
            collectedFabrics2 += (int)(enemiesByItem * resourceFabricMultiplier);

        currentCoins += (int)(enemiesByItem * resourceFabricMultiplier * goldMultiplier);
        cointsText.text = currentCoins.ToString();

        if (characterNumber == 2)
        {
            recursoPrincipalCaughtText.text = collectedFabrics.ToString();
            if (enemyTier2)
                recursoPrincipalCaughtTier2.text = collectedFabrics2.ToString();

            if (collectedFabrics >= fabricNeeded)
            {
                if (enemyTier2)
                {
                    if (collectedFabrics2 >= fabric2Needed)
                        ActivatePortal();
                }

                else
                    ActivatePortal();
            }
        }
        else
        {
            fabricText.text = collectedFabrics.ToString();
            if (enemyTier2)
                fabricTextTier2.text = collectedFabrics2.ToString();
        }
    }

    public void Damage()
    {
        if (!gameOver)
        {
            livesNumber--;
            livesGroup.transform.GetChild(livesNumber).gameObject.SetActive(false);

            if (livesNumber <= 0)
                EndProtoLevel();
        }
    }


    public void ActivatePortal()
    {
        portalExit.transform.GetChild(0).gameObject.SetActive(false);
        portalExit.transform.GetChild(1).gameObject.SetActive(true);
        portalActivated = true;
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
                    objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, instantiationHeight + actualNode.gridPositionY * 100, actualNode.worldPosition.z);
                    portalExit = objectInstantiation;
                }

                //TREES T1
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
                    objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, instantiationHeight + actualNode.gridPositionY * 100, actualNode.worldPosition.z);
                    woodInMap += woodByItem;
                    //woodNeeded += 1;
                }

                //TREES T2
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.tree2)
                {
                    switch (actualNode.currentSize)
                    {
                        case Node.Size.s1x1:
                            objectInstantiation = Instantiate(tree1x1_T2, islandParent);
                            ChangeTransitable(actualNode, false, 1, 1);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                        case Node.Size.s1x2:
                            objectInstantiation = Instantiate(tree1x2_T2, islandParent);
                            ChangeTransitable(actualNode, false, 1, 2);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.25f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s2x1:
                            objectInstantiation = Instantiate(tree1x2_T2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 1);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        default:
                            objectInstantiation = Instantiate(tree2x2_T2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 2);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                    }
                    objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, instantiationHeight + actualNode.gridPositionY * 100, actualNode.worldPosition.z);
                    wood2InMap += woodByItem;
                    //woodNeeded += 1;
                }

                //ROCKS T1
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
                            objectInstantiation = Instantiate(rock1x2, islandParent);
                            ChangeTransitable(actualNode, false, 1, 2);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.25f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s2x1:
                            objectInstantiation = Instantiate(rock1x2, islandParent);
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
                    objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, instantiationHeight + actualNode.gridPositionY * 100, actualNode.worldPosition.z);
                    rockInMap += rockByItem;
                    //rockNeeded += 1;


                }

                //ROCKS T2
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.rock2)
                {
                    switch (actualNode.currentSize)
                    {
                        case Node.Size.s1x1:
                            objectInstantiation = Instantiate(rock1x1_T2, islandParent);
                            ChangeTransitable(actualNode, false, 1, 1);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                        case Node.Size.s1x2:
                            objectInstantiation = Instantiate(rock1x2_T2, islandParent);
                            ChangeTransitable(actualNode, false, 1, 2);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.25f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s2x1:
                            objectInstantiation = Instantiate(rock1x2_T2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 1);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        default:
                            objectInstantiation = Instantiate(rock2x2_T2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 2);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                    }
                    objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, instantiationHeight + actualNode.gridPositionY * 100, actualNode.worldPosition.z);
                    rock2InMap += rockByItem;
                    //rockNeeded += 1;
                }

                //VILLAGES T1
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
                    //objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, instantiationHeight + actualNode.gridPositionY * 100, actualNode.worldPosition.z);
                    objectInstantiation.transform.position = new Vector3 (actualNode.worldPosition.x, 7.5f, actualNode.worldPosition.z);
                    objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90 * Random.Range(0, 4), 0);
                    GameObject enemiesGroup = objectInstantiation.transform.GetChild(0).GetChild(2).gameObject;
                    enemiesGroup.SetActive(true);
                    if (characterNumber == 0)
                    {
                        for (int k = 0; k < enemiesGroup.transform.childCount; k++)
                        {
                            enemiesGroup.transform.GetChild(k).GetComponent<EnemyScript>().lives = 3;
                        }
                    }
                    //enemiesNeeded += 1;
                    fabricInMap += enemiesByItem;
                }

                //VILLAGES T2
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.village2)
                {
                    switch (actualNode.currentSize)
                    {
                        case Node.Size.s2x2:
                            objectInstantiation = Instantiate(village2x2_T2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 2);
                            break;
                        case Node.Size.s3x3:
                            objectInstantiation = Instantiate(village3x3_T2, islandParent);
                            ChangeTransitable(actualNode, false, 3, 3);
                            break;
                        default:
                            objectInstantiation = Instantiate(village4x4_T2, islandParent);
                            ChangeTransitable(actualNode, false, 4, 4);
                            break;
                    }
                    //objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, instantiationHeight + actualNode.gridPositionY * 100, actualNode.worldPosition.z);
                    objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, 7.5f, actualNode.worldPosition.z);
                    objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90 * Random.Range(0, 4), 0);
                    GameObject enemiesGroup = objectInstantiation.transform.GetChild(0).GetChild(2).gameObject;
                    enemiesGroup.SetActive(true);
                    if (characterNumber == 0)
                    {
                        for (int k = 0; k < enemiesGroup.transform.childCount; k++)
                        {
                            enemiesGroup.transform.GetChild(k).GetComponent<EnemyScript>().lives = 3;
                        }
                    }
                    //enemiesNeeded += 1;
                    fabric2InMap += enemiesByItem;
                }

                //ENEMIES T1
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.enemy)
                {
                    objectInstantiation = Instantiate(enemy, islandParent);
                    //objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, instantiationHeight + actualNode.gridPositionY * 100, actualNode.worldPosition.z);
                    objectInstantiation.transform.position = actualNode.worldPosition;
                    objectInstantiation.SetActive(true);
                    actualNode.isTransitable = false;
                }

                //ENEMIES T2
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.enemy2)
                {
                    objectInstantiation = Instantiate(enemy_T2, islandParent);
                    //objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, instantiationHeight + actualNode.gridPositionY * 100, actualNode.worldPosition.z);
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
                    objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, instantiationHeight + actualNode.gridPositionY * 100, actualNode.worldPosition.z);
                }
                else if (actualNode.currentType == Node.Type.water)
                {
                    //objectInstantiation = Instantiate(water, islandParent);
                    //objectInstantiation.transform.position = actualNode.worldPosition;
                }

            }
        }
    }

    void AddMeshes(GameObject o)
    {
        MeshRenderer[] meshesToAdd = o.GetComponent<meshesArray>().meshArray;

        for (int i = 0; i < meshesToAdd.Length; i++)
        {
            meshList.Add(meshesToAdd[i]);
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

        rockTier2 = BomberAbilities.rockTier2;

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

        treeTier2 = AxerAbilities.treeTier2;
        rockTier2 = BomberAbilities.rockTier2;

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
        bomberAbs.bomberKnockBack = BomberAbilities.bombKnockBack; //applied
        rockTier2 = BomberAbilities.rockTier2;
        resourceStoneMultiplier = BomberAbilities.resourceMultiplier;//applied
        bomberAbs.bombTier2 = BomberAbilities.explosiveTier2;

        treeTier2 = AxerAbilities.treeTier2;

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
                if (characterNumber == 0) return collectedWood >= 70 * woodInMap / 100;
                else if (characterNumber == 1) return collectedRock >= 70 * rockInMap / 100;
                else return collectedFabrics >= 70 * fabricInMap / 100;
            case 3:
                return livesNumber == initialLiveNumber;
            default:
                return false;
        }
    }

    public void ButtonNextIsland()
    {
        if (islandTier2)
            SceneManager.LoadScene(1);
        else
            SceneManager.LoadScene(0);
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }


    private float GetSqrDistanceXZToPosition(Vector3 initialPosition, Vector3 finalPosition)
    {
        Vector3 vector = finalPosition - initialPosition;
        vector.y = 0;

        return vector.sqrMagnitude;
    }

}
