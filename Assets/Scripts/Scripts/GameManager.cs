using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }
    
    private float rangedFloat;
    [Header("Main Characters Prefabs")]
    [Tooltip("The main character prefabs")]
    public GameObject pickerPrefab;
    public GameObject acerPrefab;
    public GameObject sworderPrefab;

    public static int characterNumber;
    [HideInInspector] public GameObject player;
    PlayerScript playerScript;
    public GameObject livesGroup;
    int livesNumber, initialLiveNumber;

    #region Player abilities varibles
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
    #region

    public static bool mastil, casco;
    public static bool cañon, timon;
    public static bool remos, velas;
    public static bool mapa, brujula;

    #endregion

    [Header("Islands")]
    public Grid gridScript;
    int protoIsland;
    public GameObject[] islands;
    public GameObject[] islandsRain;
    [HideInInspector] public Node startNode, endNode;
    [HideInInspector] public GameObject portalExit;
    bool portalActivated = false;

    #region Number of main resources in island variables

    [HideInInspector] public int woodInMap = 0;
    [HideInInspector] public int rockInMap = 0;
    [HideInInspector] public int fabricInMap = 0;
    [HideInInspector] public int wood2InMap = 0;
    [HideInInspector] public int rock2InMap = 0;
    [HideInInspector] public int fabric2InMap = 0;

    #endregion
    #region Number of resources needed variables

    [HideInInspector] public int woodNeeded;
    [HideInInspector] public int rockNeeded;
    [HideInInspector] public int fabricNeeded;
    [HideInInspector] public int wood2Needed;
    [HideInInspector] public int rock2Needed;
    [HideInInspector] public int fabric2Needed;

    [HideInInspector] public string[] secondaryObjectives;
    #endregion

    [Range(1f, 5f)]
    [SerializeField]
    public int rockByItem;
    [Range(1f, 5f)]
    [SerializeField]
    public int woodByItem;
    [Range(1f, 5f)]
    [SerializeField]
    public int enemiesByItem;

    #region Collected resources in island variables

    [HideInInspector] public int currentCoins = 0;
    [HideInInspector] public int collectedWood = 0;
    [HideInInspector] public int collectedRock = 0;
    [HideInInspector] public int collectedFabrics = 0;
    [HideInInspector] public int collectedWood2 = 0;
    [HideInInspector] public int collectedRock2 = 0;
    [HideInInspector] public int collectedFabrics2 = 0;

    #endregion
    #region Total resources variables

    [HideInInspector] public static int totalCoins = 0;
    [HideInInspector] public static int totalWood = 0;
    [HideInInspector] public static int totalRock = 0;
    [HideInInspector] public static int totalFabrics = 0;
    [HideInInspector] public static int totalWood2 = 0;
    [HideInInspector] public static int totalRock2 = 0;
    [HideInInspector] public static int totalFabrics2 = 0;

    #endregion

    static int lastCharacter = -1;
    static List<int> last5Characters = new List<int>();
    static int lastIsland = -1;

    public float timeByLevel;
    private float remainingTimeInLevel;
    public Text timeText;

    [Header("Start Variables")]
    [Range(1f, 5f)]
    [SerializeField]
    public float waitToStartTime;
    [Range(1f, 5f)]
    [SerializeField]
    public float initialPanelPositionsTime;
    float waitTimer;
    public static bool startGame;
    bool gameOver = false;
    public static bool gameWon;

    public static bool movingCamera;
    public GameObject endCameraPosition;
    public float initialCameraSpeed;
    public static float cameraSpeed;
    Camera mainCamera;
    GameObject cameraAnchor;


    //CANVAS
    [Header("Canvas")]
    public GameObject entreIslasCanvas;
    public GameObject abilitiesCanvas;
    public GameObject mainCanvas;
    public Transform positionRight, positionLeft;
    public float speedPanels;
    float initialSpeedPanels;

    [Header("Main Resource Canvas Panel")]
    public GameObject rightPanel;
    public GameObject recursoPrincipal;
    public GameObject recursoPrincipalTier2;
    #region Main resource panel texts

    public Text recursoPrincipalText;
    public Text recursoPrincipalTextTier2;
    public Text cointsText;
    public Text recursoPrincipalCaughtText;
    public Text recursoPrincipalCaughtTier2;

    #endregion

    [Header("Secundary Resource Canvas Panel")]
    public GameObject rightPanelSecundaries;
    #region Secundary resources panel texts

    public Text woodText;
    public Text woodTextTier2;
    public Text rockText;
    public Text rockTextTier2;
    public Text fabricText;
    public Text fabricTextTier2;

    #endregion

    [Header("Main Weapon Images")]
    public Image sworderImage;
    public Image bomberImage;
    public Image axerImage;
    public Image bomberTier2Image;

    [Header("Secundary Objective Images")]
    public GameObject leftPanel;
    public Text secondaryObjectiveText;
    GameObject[] objectiveImage;
    int secondaryObjectiveID;
    private GameObject[] lights;

    private void Awake()
    {

        Instance = this;
        startGame = false;

        RandomIsland();

        remainingTimeInLevel = timeByLevel;
        initialSpeedPanels = speedPanels;
        mainCamera = Camera.main;
        cameraAnchor = mainCamera.transform.parent.gameObject;

        entreIslasCanvas.SetActive(false);
        abilitiesCanvas.SetActive(false);

        RandomCharacter();
    }


    void Start()
    {
        player = PlayerInstantiation();
        playerScript.enabled = false;

        gridScript.GenerateGrid(characterNumber);

        player.GetComponent<CharacterController>().enabled = false;

        player.transform.position = (startNode.worldPosition + (Vector3.up * 100));
        player.transform.rotation = Quaternion.LookRotation(transform.forward);

        PlayerInitialization();
        LivesInitialization();
        SecundaryObjectivesInitialization();

        player.GetComponent<CharacterController>().enabled = true;

        lights = GameObject.FindGameObjectsWithTag("FireTorch");
        print(lights.Length);
        foreach (GameObject fire in lights)
        {
            print(fire.name);
            fire.SetActive(false);
        }
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.L))
            SoundManager.PlayOneShot(SoundManager.SwordSound, this.transform.position);
        if (startGame)
        {
            if (!gameOver)
            {
                remainingTimeInLevel -= Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.N))
                    EndLevel();

                timeText.text = remainingTimeInLevel.ToString("0");

                if (remainingTimeInLevel <= 0f)
                {
                    livesNumber = 0;
                    player.GetComponent<PlayerScript>().myAnimator.SetBool("Dead", true);
                    EndLevel();
                    remainingTimeInLevel = timeByLevel;
                }
            }
            else if (movingCamera)
                UpdateEndCameraPosition();

            if (Input.GetKeyDown(KeyCode.M))
            {
                totalCoins += 500;
                SaveManager.Instance.Save();
                gameOver = true;
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                totalFabrics += 500;
                totalFabrics2 += 500;
                totalRock += 500;
                totalRock2 += 500;
                totalWood += 500;
                totalWood2 += 500;
                SaveManager.Instance.Save();
                gameOver = true;
            }
        }
        else
            UpdateWaitTimerToStart();

        CameraMovement();
    }


    void CameraMovement()
    {
        if (gameOver)
            cameraAnchor.transform.position = Vector3.MoveTowards(cameraAnchor.transform.position, Vector3.zero, 50f * Time.deltaTime);
        else if (startGame)
            cameraAnchor.transform.position = Vector3.MoveTowards(cameraAnchor.transform.position, player.transform.position, 50f * Time.deltaTime);
    }


    //RANDOMIZE METHODS

    void RandomIsland()
    {
        protoIsland = Random.Range(0, islands.Length);

        while (protoIsland == lastIsland)
            protoIsland = Random.Range(0, islands.Length);

        lastIsland = protoIsland;

        GameObject island = null;
        if (ClimaRandom.Rain)
        {
            island = Instantiate(islandsRain[protoIsland]);
        }
        else
            island = Instantiate(islands[protoIsland]);
        //islands[protoIsland].SetActive(true);
        gridScript.islandParent = island.transform.GetChild(0);
        island.GetComponent<NavMeshSurface>().BuildNavMesh();
        island.GetComponent<NavMeshSurface>().BuildNavMesh();

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
                    characterNumber = Random.Range(0, 3);
            }

            last5Characters.RemoveAt(0);
        }

        else
        {
            characterNumber = Random.Range(0, 3);

            while (characterNumber == lastCharacter)
                characterNumber = Random.Range(0, 3);
        }

        last5Characters.Add(characterNumber);
        lastCharacter = characterNumber;
    }

    int RandomSecondaryObjective()
    {
        return Random.Range(0, secondaryObjectives.Length);
    }



    //INITIALIZATION METHODS

    public GameObject PlayerInstantiation()
    {
        GameObject p = null;

        switch (characterNumber)
        {
            case 0:
                p = Instantiate(acerPrefab);
                playerScript = p.GetComponent<PlayerScript>();
                ApplyAxerAbilities(p);
                break;
            case 1:
                p = Instantiate(pickerPrefab);
                playerScript = p.GetComponent<PlayerScript>();
                ApplyBomberAbilities(p);
                break;
            case 2:
                p = Instantiate(sworderPrefab);
                playerScript = p.GetComponent<PlayerScript>();
                ApplySwordAbilities(p);
                break;
        }

        return p;
    }

    void PlayerInitialization()
    {
        switch (characterNumber)
        {
            case 0:
                livesNumber = 3;

                woodText.gameObject.SetActive(true);
                woodNeeded = woodInMap / 2;
                recursoPrincipalText.text = woodNeeded.ToString();
                recursoPrincipal.transform.GetChild(2).gameObject.SetActive(true);
                fabricText.transform.parent.gameObject.SetActive(true);
                rockText.transform.parent.gameObject.SetActive(true);

                if (treeTier2)
                {
                    wood2Needed = wood2InMap / 2;
                    recursoPrincipalTier2.transform.GetChild(2).gameObject.SetActive(true);
                    recursoPrincipalTier2.SetActive(true);
                    recursoPrincipalTextTier2.text = wood2Needed.ToString();
                }
                if (rockTier2)
                    rockTextTier2.transform.parent.gameObject.SetActive(true);
                if (enemyTier2)
                    fabricTextTier2.transform.parent.gameObject.SetActive(true);

                cointsText.text = currentCoins.ToString();
                axerImage.enabled = true;
                break;
            case 1:
                livesNumber = 2;

                rockText.gameObject.SetActive(true);
                rockNeeded = rockInMap / 2;
                recursoPrincipalText.text = rockNeeded.ToString();
                recursoPrincipal.transform.GetChild(1).gameObject.SetActive(true);
                fabricText.transform.parent.gameObject.SetActive(true);
                woodText.transform.parent.gameObject.SetActive(true);

                if (rockTier2)
                {
                    rock2Needed = rock2InMap / 2;
                    recursoPrincipalTier2.transform.GetChild(1).gameObject.SetActive(true);
                    recursoPrincipalTier2.SetActive(true);
                    recursoPrincipalTextTier2.text = rock2Needed.ToString();
                    bomberTier2Image.enabled = true;
                }
                else
                    bomberImage.enabled = true;
                if (treeTier2)
                    woodTextTier2.transform.parent.gameObject.SetActive(true);
                if (enemyTier2)
                    fabricTextTier2.transform.parent.gameObject.SetActive(true);

                cointsText.text = currentCoins.ToString();
                break;
            case 2:
                livesNumber = 4;

                fabricText.gameObject.SetActive(true);
                fabricNeeded = fabricInMap / 2;
                recursoPrincipalText.text = fabricNeeded.ToString();
                recursoPrincipal.transform.GetChild(3).gameObject.SetActive(true);
                woodText.transform.parent.gameObject.SetActive(true);
                rockText.transform.parent.gameObject.SetActive(true);

                if (enemyTier2)
                {
                    fabric2Needed = fabric2InMap / 2;
                    recursoPrincipalTier2.transform.GetChild(3).gameObject.SetActive(true);
                    recursoPrincipalTier2.SetActive(true);
                    recursoPrincipalTextTier2.text = fabric2Needed.ToString();
                }
                if (rockTier2)
                    rockTextTier2.transform.parent.gameObject.SetActive(true);
                if (treeTier2)
                    woodTextTier2.transform.parent.gameObject.SetActive(true);

                cointsText.text = currentCoins.ToString();
                sworderImage.enabled = true;
                break;
        }
    }

    void LivesInitialization()
    {
        if (titan)
            livesNumber++;
        for (int i = 0; i < livesNumber; i++)
            livesGroup.transform.GetChild(i).gameObject.SetActive(true);
        initialLiveNumber = livesNumber;
    }

    void SecundaryObjectivesInitialization()
    {
        secondaryObjectives = new string[4];
        objectiveImage = new GameObject[4];
        secondaryObjectives[0] = "20s";
        objectiveImage[0] = leftPanel.transform.GetChild(6).gameObject;
        secondaryObjectives[1] = "10s";
        objectiveImage[1] = leftPanel.transform.GetChild(6).gameObject;

        if (characterNumber == 0)
        {
            secondaryObjectives[2] = "" + (70 * woodInMap / 100) + "";
            objectiveImage[2] = leftPanel.transform.GetChild(3).gameObject;
        }
        else if (characterNumber == 1)
        {
            secondaryObjectives[2] = "" + (70 * rockInMap / 100) + "";
            objectiveImage[2] = leftPanel.transform.GetChild(4).gameObject;
        }
        else
        {
            secondaryObjectives[2] = "" + (70 * fabricInMap / 100) + "";
            objectiveImage[2] = leftPanel.transform.GetChild(5).gameObject;
        }
        secondaryObjectives[3] = livesNumber.ToString();
        objectiveImage[3] = leftPanel.transform.GetChild(7).gameObject;

        secondaryObjectiveID = RandomSecondaryObjective();
        objectiveImage[secondaryObjectiveID].SetActive(true);
        secondaryObjectiveText.text = secondaryObjectives[secondaryObjectiveID];
    }



    //UPDATE METHODS

    void UpdateWaitTimerToStart()
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
            rightPanel.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.UpperRight;
        }
    }

    void UpdateEndCameraPosition()
    {
        if (cameraSpeed > initialCameraSpeed / 5)
            cameraSpeed -= Time.deltaTime * initialCameraSpeed / 2f;

        mainCamera.transform.localPosition = Vector3.MoveTowards(mainCamera.transform.localPosition, endCameraPosition.transform.position, cameraSpeed * Time.deltaTime);
        mainCamera.transform.localRotation = Quaternion.RotateTowards(mainCamera.transform.localRotation, endCameraPosition.transform.rotation, cameraSpeed * Time.deltaTime / 3f);

        cameraAnchor.transform.localRotation = Quaternion.RotateTowards(cameraAnchor.transform.localRotation, Quaternion.Euler(new Vector3(0f, 0, 0)), cameraSpeed * Time.deltaTime / 2f);
        if (mainCamera.orthographicSize < 80)
            mainCamera.orthographicSize += cameraSpeed * Time.deltaTime;


        if (GetSqrDistanceXZToPosition(mainCamera.transform.localRotation.eulerAngles, endCameraPosition.transform.rotation.eulerAngles) <= 0.1)
            movingCamera = false;
    }




    //END LEVEL METHODS

    public void LevelComplete()
    {
        EndLevel();
    }


    public bool CheckSecondaryObjective()
    {
        switch (secondaryObjectiveID)
        {
            case 0:
                return remainingTimeInLevel >= 20.0f && livesNumber > 0;
            case 1:
                return remainingTimeInLevel >= 10.0f && livesNumber > 0;
            case 2:
                if (characterNumber == 0) return collectedWood >= 70 * woodInMap / 100 && remainingTimeInLevel > 0 && livesNumber > 0;
                else if (characterNumber == 1) return collectedRock >= 70 * rockInMap / 100 && remainingTimeInLevel > 0 && livesNumber > 0;
                else return collectedFabrics >= 70 * fabricInMap / 100 && remainingTimeInLevel > 0 && livesNumber > 0;
            case 3:
                return livesNumber == initialLiveNumber && remainingTimeInLevel > 0;
            default:
                return false;
        }
    }

    public void EndLevel()
    {
        gameOver = true;

        if (livesNumber <= 0 && !portalActivated) //si muere y no habia cumplido el primer objetivo
        {
            gameWon = false;
            int pers;

            pers = totalFabrics * 10 / 100;
            totalFabrics -= pers;
            MidGameMenuScript.persFabric = pers;
            if (totalFabrics < 0) totalFabrics = 0;

            pers = totalRock * 10 / 100;
            totalRock -= pers;
            MidGameMenuScript.persRock = pers;
            if (totalRock < 0) totalRock = 0;

            pers = totalWood * 10 / 100;
            totalWood -= pers;
            MidGameMenuScript.persWood = pers;
            if (totalWood < 0) totalWood = 0;

            pers = totalFabrics2 * 10 / 100;
            totalFabrics2 -= pers;
            MidGameMenuScript.persFabric2 = pers;
            if (totalFabrics2 < 0) totalFabrics2 = 0;

            pers = totalRock2 * 10 / 100;
            totalRock2 -= pers;
            MidGameMenuScript.persRock2 = pers;
            if (totalRock2 < 0) totalRock2 = 0;

            pers = totalWood2 * 10 / 100;
            totalWood2 -= pers;
            MidGameMenuScript.persWood2 = pers;
            if (totalWood2 < 0) totalWood2 = 0;
        }

        else
        {
            gameWon = true;

            totalFabrics += collectedFabrics;
            totalRock += collectedRock;
            totalWood += collectedWood;
            totalFabrics2 += collectedFabrics2;
            totalRock2 += collectedRock2;
            totalWood2 += collectedWood2;
        }

        if (CheckSecondaryObjective())
            totalCoins += 50;

        totalCoins += currentCoins; //siempre gana las monedas conseguidas en el nivel

        entreIslasCanvas.SetActive(true);

        SaveManager.Instance.Save();

        mainCanvas.SetActive(false);
        movingCamera = true;
        cameraAnchor.GetComponent<CameraRotation>().enabled = false;
        playerScript.enabled = false;
        cameraSpeed = initialCameraSpeed;
    }



    //PICK RESOURCES METHODS

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
            if (woodTier == 1)
                woodText.text = collectedWood.ToString();
            else
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
            if (rockTier == 1)
                rockText.text = collectedRock.ToString();
            else
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
            if (fabricTier == 1)
                fabricText.text = collectedFabrics.ToString();
            else
                fabricTextTier2.text = collectedFabrics2.ToString();
        }
    }



    //ABILITIES METHODS

    void ApplyAxerAbilities(GameObject charac)
    {
        PlayerScript axerAbs = playerScript;

        axerAbs.neededTimeMultiplier = AxerAbilities.resourceSpeedMultiplier;//apply
        axerAbs.axePolivalente = AxerAbilities.Polivalente;//apply
        axerAbs.axeStun = AxerAbilities.axeStunt;//applied
        treeTier2 = AxerAbilities.treeTier2;
        resourceTreeMultiplier = AxerAbilities.resourceMultiplier;//apply
        axerAbs.axeTier2 = AxerAbilities.axerTier2;//applied

        rockTier2 = BomberAbilities.rockTier2;
        enemyTier2 = SwordAbilities.enemyTier2;

        titan = CharacterAbiliities.Titan;//applied
        axerAbs.bootsMovementSpeed = CharacterAbiliities.bootsMovementMultiplier;//applied
        Market = CharacterAbiliities.market;
        islandTier2 = CharacterAbiliities.islandTier2;
        axerAbs.dashActive = CharacterAbiliities.dashActive;//applied
        goldMultiplier = CharacterAbiliities.goldMultiplier;//applied

    }
    void ApplySwordAbilities(GameObject charac)
    {
        PlayerScript swordAbs = playerScript;

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
        PlayerScript bomberAbs = playerScript;

        bomberAbs.neededBombMultiplier = BomberAbilities.resourceSpeedMultiplier;//apply
        bomberAbs.bombPolivalente = BomberAbilities.Polivalente;//apply
        bomberAbs.bomberKnockBack = BomberAbilities.bombKnockBack; //applied
        rockTier2 = BomberAbilities.rockTier2;
        resourceStoneMultiplier = BomberAbilities.resourceMultiplier;//applied
        bomberAbs.bombTier2 = BomberAbilities.explosiveTier2;

        treeTier2 = AxerAbilities.treeTier2;
        enemyTier2 = SwordAbilities.enemyTier2;

        titan = CharacterAbiliities.Titan;//applied
        bomberAbs.bootsMovementSpeed = CharacterAbiliities.bootsMovementMultiplier;//applied
        Market = CharacterAbiliities.market;
        islandTier2 = CharacterAbiliities.islandTier2;
        bomberAbs.dashActive = CharacterAbiliities.dashActive;//applied
        goldMultiplier = CharacterAbiliities.goldMultiplier;//applied
    }



    //BUTTON METHODS

    public void ButtonNextIsland()
    {
        if (islandTier2)
            SceneManager.LoadScene(2);
        else
            SceneManager.LoadScene(1);
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }



    //OTHER METHODS

    public void Damage()
    {
        if (!gameOver)
        {
            livesNumber--;
            livesGroup.transform.GetChild(livesNumber).gameObject.SetActive(false);

            if (livesNumber <= 0)
            {
                SoundManager.PlayOneShot(SoundManager.DeathSound, this.transform.position);
                player.GetComponent<PlayerScript>().myAnimator.SetBool("Dead", true);
                EndLevel();
            }
            else
                SoundManager.PlayOneShot(SoundManager.PlayerHurtSound, this.transform.position);
        }
    }

    public void ActivatePortal()
    {
        SoundManager.PlayOneShot(SoundManager.PortalOpenSound, this.transform.position);
        portalExit.transform.GetChild(0).gameObject.SetActive(false);
        portalExit.transform.GetChild(1).gameObject.SetActive(true);
        portalActivated = true;
    }

    private float GetSqrDistanceXZToPosition(Vector3 initialPosition, Vector3 finalPosition)
    {
        Vector3 vector = finalPosition - initialPosition;
        vector.y = 0;

        return vector.sqrMagnitude;
    }
}