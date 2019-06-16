using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private CharacterController characterController;
    private Transform cameraAnchor;
    private InputManager inputManager;
    private GameManager gameManager;
    private CameraRotation cameraScript;

    public enum playerType { pick, ace, sword }
    public playerType actualType;

    float xMovement, zMovement;
    private float currentSpeed;
    public float characterSpeed;
    bool canMove = true;

    private float pressedTimer = 0f;
    public float neededPressedTime;
    private bool receiveInputAction = true; //si el jugador ha mantenido pulsado "neededPressedTime", se vuelve false para no seguir recibiendo el input
    public float hitDistance; //distancia a la que se considera que el jugador da al objeto al que apunta
    public float hitDistanceUpgradeMultiplier;
    public Image actionSphere;
    GameObject actionObject;
    bool actionOn = false;
    public float inmortalDurationTime = 2f;
    float inmortalTimer;
    bool inmortal;

    public float dashDistance;
    public float dashCooldown;
    float dashTime = 0;

    public GameObject bomb;
    public GameObject sword;
    public GameObject axe;
    bool bombOn;

    bool lastActionButtonReleased = true;
    bool lastDashButtonReleased = true;

    bool knockBack;
    Vector3 knockDirection;

    [Header("Sword")]
    [Range(min: -1, max: 0)]
    public float angleAttack;
    public float swordCooldown;
    float currentCD;
    bool canAttack;

    [Header("Abilities")]
    #region
    public float neededTimeMultiplier = 1.0f;//applied
    public bool axePolivalente = false;//applied
    public bool axeStun = false;//applied
    public bool axeTier2 = false;//applied

    public float neededBombMultiplier = 1.0f;//applied
    public bool bombPolivalente = false;//applied NOT Working
    public bool bombTier2 = false;
    public bool bomberKnockBack = false;

    public float attackSpeedCooldown = 1.0f;
    public bool swordPolivalente = false;//applied
    public bool swordSeep = false;
    public bool swordTier2 = false;//applied

    public float bootsMovementSpeed = 1.0f;//applied
    public bool dashActive = false;//applied


    #endregion

    float iniPressedTime;
    float iniBombTime;

    [Header("ParticleSystem")]
    public GameObject psWood;
    public GameObject psBombPutDown;
    public GameObject psMagicPoof;
    public GameObject psSand;
    public GameObject psSwordSlash;
    public GameObject psHit;
    public GameObject psClickTextEffect;
    public GameObject psTrial;
    float trialTimer = 1.0f;
    float currentTrialTimer = 0;

    public Animator myAnimator;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        gameManager = GameManager.Instance;
        cameraScript = gameManager.GetComponent<CameraRotation>();

        cameraAnchor = Camera.main.transform.parent.transform;
        actionSphere.transform.rotation = Camera.main.transform.localRotation;
        inmortal = false;
        bombOn = false;

        currentCD = swordCooldown;
        iniPressedTime = neededPressedTime;

        if (actualType == playerType.ace && axeTier2)
            hitDistance *= hitDistanceUpgradeMultiplier;
        if (actualType == playerType.sword && swordTier2)
            hitDistance *= hitDistanceUpgradeMultiplier;
    }

    void Update()
    {
        if (canMove)
            WalkTime();

        //Mirar si ha soltado el trigger antes de volver a hacer la accion
        if (lastActionButtonReleased == false && inputManager.GetInputUp("Action")) lastActionButtonReleased = true;

        if (lastDashButtonReleased == false && inputManager.GetInputUp("Dash"))
        {
            dashTime += Time.deltaTime;

            if (dashTime >= dashCooldown)
            {
                lastDashButtonReleased = true;
                dashTime = 0;
            }
        }

        if (receiveInputAction && !bombOn)
        {
            if (lastActionButtonReleased) StartAction();

            if (actionOn)
                UpdateAction();
        }
        else if (bombOn)
            UpdateBomb();
        else
            EndAction();

        if (lastDashButtonReleased && dashActive) Dash();

        if (inmortal)
            CheckInmortal();

        if (currentCD >= swordCooldown)
            canAttack = true;
        if (!canAttack)
            currentCD += Time.deltaTime;

        if (actualType == playerType.sword && psTrial.activeSelf)
            currentTrialTimer += Time.deltaTime;

        if (currentTrialTimer >= trialTimer)
        {
            currentTrialTimer = 0;
            psTrial.SetActive(false);
        }

    }


    void WalkTime()
    {
        Vector3 movement = Vector3.zero;
        Vector3 fallMovement = Vector3.zero;
        xMovement = inputManager.GetAxis("Horizontal");
        zMovement = inputManager.GetAxis("Vertical");

        if (Mathf.Abs(xMovement) > 0 || Mathf.Abs(zMovement) > 0)
        {
            myAnimator.SetBool("Move", true);
            cameraScript.cameraRotating = false;

            Vector3 forward = cameraAnchor.forward;
            forward.y = 0;
            forward.Normalize();
            Vector3 right = cameraAnchor.right;
            right.y = 0;
            right.Normalize();

            movement = forward * zMovement + right * xMovement;
            transform.localRotation = Quaternion.LookRotation(movement);

            gameManager.cameraFollowSpeed = 5f;
        }

        else
        {
            myAnimator.SetBool("Move", false);
        }

        if (!characterController.isGrounded)
            movement.y = Physics.gravity.y * Time.deltaTime * 10;

        Vector3 controllerMovement = movement.normalized * characterSpeed * bootsMovementSpeed * Time.deltaTime;
        characterController.Move(controllerMovement);
    }


    void Dash()
    {
        if (inputManager.GetInput("Dash"))
        {
            SoundManager.PlayOneShot(SoundManager.DashSound, this.transform.position);
            Instantiate(psMagicPoof, this.transform.position, Quaternion.identity);
            lastDashButtonReleased = false;

            characterController.enabled = false;

            Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y - 1.0f, transform.position.z), transform.forward);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, dashDistance))
            {
                if (hit.collider.gameObject.tag == "IslandCollision" || hit.collider.gameObject.tag == "Island")
                {
                    Vector3 newPos = hit.point + hit.normal * 2;
                    transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
                }

                else
                    transform.position += transform.forward * dashDistance;
            }

            else
                transform.position += transform.forward * dashDistance;

            characterController.enabled = true;

        }
    }


    void StartAction()
    {
        if (inputManager.GetInput("Action") && actualType == playerType.sword && canAttack)
        {
            canAttack = false;
            lastActionButtonReleased = false;
            SwordAttack();
            Instantiate(psSwordSlash, sword.transform.position, Quaternion.identity);
            psTrial.SetActive(true);
        }

        if (inputManager.GetInput("Action") && (actualType != playerType.sword || (actualType == playerType.sword && swordPolivalente)))
        {
            lastActionButtonReleased = false;

            Ray ray = new Ray(transform.position, transform.forward);
            Ray ray2 = new Ray(transform.position + transform.right * 2f, transform.forward);
            Ray ray3 = new Ray(transform.position - transform.right * 2f, transform.forward);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, hitDistance) || Physics.Raycast(ray2, out hit, hitDistance) || Physics.Raycast(ray3, out hit, hitDistance))
            {
                actionObject = hit.collider.gameObject;
                Instantiate(psHit, hit.transform.position, Quaternion.identity);

                switch (actionObject.tag)
                {
                    case "Tree2":
                        transform.LookAt(new Vector3(actionObject.transform.position.x, transform.position.y, actionObject.transform.position.z));
                        if (actualType == playerType.ace)
                        {
                            SoundManager.PlayOneShot(SoundManager.HitTree, this.transform.position);
                            myAnimator.SetBool("Move", false);
                            myAnimator.SetTrigger("Attack");
                            neededPressedTime = iniPressedTime * 2;
                            actionOn = true;
                        }

                        if (actualType == playerType.sword && swordPolivalente)
                        {
                            SoundManager.PlayOneShot(SoundManager.HitTree, this.transform.position);
                            myAnimator.SetBool("Move", false);
                            myAnimator.SetTrigger("Attack");
                            neededPressedTime = iniPressedTime * 2;
                            actionOn = true;
                        }
                        break;
                    case "Tree":
                        transform.LookAt(new Vector3(actionObject.transform.position.x, transform.position.y, actionObject.transform.position.z));
                        if (actualType == playerType.ace)
                        {
                            SoundManager.PlayOneShot(SoundManager.HitTree, this.transform.position);
                            myAnimator.SetBool("Move", false);
                            myAnimator.SetTrigger("Attack");
                            neededPressedTime = iniPressedTime;
                            actionOn = true;
                        }

                        if (actualType == playerType.sword && swordPolivalente)
                        {
                            SoundManager.PlayOneShot(SoundManager.HitTree, this.transform.position);
                            myAnimator.SetBool("Move", false);
                            myAnimator.SetTrigger("Attack");
                            neededPressedTime = iniPressedTime;
                            actionOn = true;
                        }
                        break;

                    case "Enemy2":
                    case "Enemy":
                        if (actualType == playerType.ace && axePolivalente)
                        {
                            SoundManager.PlayOneShot(SoundManager.HitEnemySword, this.transform.position);
                            myAnimator.SetBool("Move", false);
                            myAnimator.SetTrigger("Attack");
                            actionObject.transform.parent.GetComponent<EnemyScript>().GetAttacked(this.gameObject.transform, false);
                        }

                        if (actualType == playerType.ace && axeStun)
                        {
                            myAnimator.SetBool("Move", false);
                            myAnimator.SetTrigger("Attack");
                            actionObject.transform.parent.GetComponent<EnemyScript>().Stun();
                        }
                        break;
                    case "Rock2":
                        if (actualType == playerType.ace && axePolivalente)
                        {
                            SoundManager.PlayOneShot(SoundManager.SwordStoneSound, this.transform.position);
                            myAnimator.SetBool("Move", false);
                            myAnimator.SetTrigger("Attack");
                            neededPressedTime = iniPressedTime * 4;
                            actionOn = true;
                        }
                        if (actualType == playerType.sword && swordPolivalente)
                        {
                            SoundManager.PlayOneShot(SoundManager.SwordStoneSound, this.transform.position);
                            myAnimator.SetBool("Move", false);
                            myAnimator.SetTrigger("Attack");
                            neededPressedTime = iniPressedTime * 2;
                            actionOn = true;
                        }
                        break;

                    case "Rock":
                        if (actualType == playerType.ace && axePolivalente)
                        {
                            SoundManager.PlayOneShot(SoundManager.SwordStoneSound, this.transform.position);
                            myAnimator.SetBool("Move", false);
                            myAnimator.SetTrigger("Attack");
                            neededPressedTime = iniPressedTime * 2;
                            actionOn = true;
                        }
                        if (actualType == playerType.sword && swordPolivalente)
                        {
                            SoundManager.PlayOneShot(SoundManager.SwordStoneSound, this.transform.position);
                            myAnimator.SetBool("Move", false);
                            myAnimator.SetTrigger("Attack");
                            neededPressedTime = iniPressedTime;
                            actionOn = true;
                        }
                        break;
                }
            }

            if (actualType == playerType.pick)
            {
                Instantiate(psBombPutDown, this.transform.position, Quaternion.identity);
                bomb.SetActive(true);
                bomb.transform.SetParent(transform);
                bomb.transform.localPosition = Vector3.zero;
                bomb.transform.SetParent(null);

                bombOn = true;
            }
        }
    }


    void FixedUpdate()
    {
        if (knockBack)
        {
            characterController.Move(knockDirection * 50f * Time.deltaTime);
        }
    }


    void UpdateAction()
    {
        if (inputManager.GetInput("Action"))
        {
            actionSphere.transform.eulerAngles = new Vector3(actionSphere.transform.eulerAngles.x, cameraAnchor.transform.eulerAngles.y, 0); //Rotación esfera de acción
            canMove = false;
            pressedTimer += Time.deltaTime;
            actionSphere.fillAmount = pressedTimer / (neededPressedTime * neededTimeMultiplier);

            if (pressedTimer >= (neededPressedTime * neededTimeMultiplier))
                receiveInputAction = false;
        }
        else
        {
            actionOn = false;
            actionSphere.fillAmount = 0;
            pressedTimer = 0.0f;
            canMove = true;
        }
    }


    void UpdateBomb()
    {
        pressedTimer += Time.deltaTime;
        if (!bombTier2)
        {
            actionSphere.transform.eulerAngles = new Vector3(actionSphere.transform.eulerAngles.x, cameraAnchor.transform.eulerAngles.y, 0); //Rotación esfera de acción
            actionSphere.fillAmount = pressedTimer / (neededPressedTime * neededBombMultiplier);

            if (pressedTimer >= (neededPressedTime * neededBombMultiplier))
            {
                bomb.SendMessage("Explode");
                bombOn = false;
                actionSphere.fillAmount = 0;
                pressedTimer = 0.0f;
            }
        }
        else if (inputManager.GetInput("Action") && pressedTimer >= 0.1f && lastActionButtonReleased)
        {
            Instantiate(psClickTextEffect, this.transform.position + new Vector3(0, 4, 0), Quaternion.identity);
            bomb.SendMessage("Explode");
            bombOn = false;
            actionSphere.fillAmount = 0;
            pressedTimer = 0.0f;
            lastActionButtonReleased = false;
        }
    }


    void EndAction()
    {
        switch (actionObject.tag)
        {
            case "Rock":
                if (actualType == playerType.pick)
                    BreakRock(actionObject.transform.parent.parent.gameObject, 1);
                if (actualType == playerType.ace && axePolivalente)
                    BreakRock(actionObject.transform.parent.parent.gameObject, 1);
                if (actualType == playerType.sword && swordPolivalente)
                    BreakRock(actionObject.transform.parent.parent.gameObject, 1);
                break;

            case "Rock2":
                if (actualType == playerType.pick)
                    BreakRock(actionObject.transform.parent.parent.gameObject, 2);
                if (actualType == playerType.ace && axePolivalente)
                    BreakRock(actionObject.transform.parent.parent.gameObject, 2);
                if (actualType == playerType.sword && swordPolivalente)
                    BreakRock(actionObject.transform.parent.parent.gameObject, 2);
                break;

            case "Tree":
                if (actualType == playerType.ace)
                    CutTree(actionObject.transform.parent.parent.gameObject, 1);
                if (actualType == playerType.sword && swordPolivalente)
                    CutTree(actionObject.transform.parent.parent.gameObject, 1);
                break;
            case "Tree2":
                if (actualType == playerType.ace)
                    CutTree(actionObject.transform.parent.parent.gameObject, 2);
                if (actualType == playerType.sword && swordPolivalente)
                    CutTree(actionObject.transform.parent.parent.gameObject, 2);
                break;
        }

        actionOn = false;
        actionSphere.fillAmount = 0;
        pressedTimer = 0f;
        receiveInputAction = true;
        canMove = true;
        myAnimator.SetBool("Attack", false);
    }


    public void BreakRock(GameObject rock, int rockTier)
    {
        gameManager.PickRock(rockTier);
        Destroy(rock);
    }

    public void CutTree(GameObject tree, int woodTier)
    {
        SoundManager.PlayOneShot(SoundManager.BreakTree, this.transform.position);
        gameManager.PickWood(woodTier);
        Instantiate(psWood, this.transform.position + this.transform.forward.normalized, Quaternion.identity);
        Destroy(tree);
    }

    public void PickFabrics(GameObject chest, int tier)
    {
        gameManager.PickFabrics(tier);
        Destroy(chest);
    }


    void CheckInmortal()
    {
        inmortalTimer += Time.deltaTime;
        if (inmortalTimer >= inmortalDurationTime)
            inmortal = false;
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Dead")
            gameManager.EndLevel();
        else if (hit.gameObject.tag == "Exit")
        {
            gameManager.EndLevel();
            this.gameObject.SetActive(false);
            gameManager.ignorePlayer = true;
        }
        else if ((hit.gameObject.tag == "Chest") && (actualType == playerType.sword || (AxerAbilities.Polivalente && actualType == playerType.ace) || (BomberAbilities.Polivalente && actualType == playerType.pick)))
            PickFabrics(hit.gameObject, 1);
        else if ((hit.gameObject.tag == "Chest2") && (actualType == playerType.sword || (AxerAbilities.Polivalente && actualType == playerType.ace) || (BomberAbilities.Polivalente && actualType == playerType.pick)))
            PickFabrics(hit.gameObject, 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Rock" && actualType == playerType.pick)
        {
            SoundManager.PlayOneShot(SoundManager.PickUpSound, this.transform.position);
            BreakRock(other.transform.parent.gameObject, 1);
        }
        else if (other.gameObject.tag == "Tree" && actualType == playerType.pick)
        {
            SoundManager.PlayOneShot(SoundManager.PickUpSound, this.transform.position);
            gameManager.PickWood(1);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Rock2" && actualType == playerType.pick)
        {
            SoundManager.PlayOneShot(SoundManager.PickUpSound, this.transform.position);
            BreakRock(other.transform.parent.gameObject, 2);
        }
        else if (other.gameObject.tag == "Tree2" && actualType == playerType.pick)
        {
            SoundManager.PlayOneShot(SoundManager.PickUpSound, this.transform.position);
            gameManager.PickWood(2);
            Destroy(other.gameObject);
        }
    }


    public void Damage(Vector3 direction, bool changeInmortal) //changeInmortal para decidir si te da unos segundos de invulnerabilidad, la bomba no te los da
    {
        if (!inmortal)
        {
            inmortal = changeInmortal;
            inmortalTimer = 0;
            if (bomberKnockBack)
            {
                knockDirection = direction;
                StartCoroutine(KnockBack());
            }
            Pushed(direction);
            gameManager.Damage();
        }
    }
    
    void Pushed(Vector3 enemyForward)
    {
        characterController.enabled = false;

        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y - 1.0f, transform.position.z), enemyForward);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, dashDistance))
        {
            if (hit.collider.gameObject.tag == "IslandCollision" || hit.collider.gameObject.tag == "Island")
            {
                Vector3 newPos = hit.point + hit.normal * 2;
                transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
            }

            else
                transform.position += transform.forward * dashDistance / 2;
        }

        else
            transform.position += transform.forward * dashDistance / 2;

        characterController.enabled = true;
    }


    public void SwordAttack()
    {
        bool foundEnemy = false;
        bool repeatedEnemy = false;

        myAnimator.SetBool("Move", false);
        myAnimator.SetBool("Attack", true);
        currentCD = 0;
        Collider[] enemies = Physics.OverlapSphere(this.transform.position, hitDistance);

        RaycastHit[] RaycastAllRay1 = Physics.RaycastAll(transform.position, transform.forward, hitDistance);
        RaycastHit[] RaycastAllRay2 = Physics.RaycastAll(transform.position + transform.right * 1.5f, transform.forward, hitDistance);
        RaycastHit[] RaycastAllRay3 = Physics.RaycastAll(transform.position - transform.right * 1.5f, transform.forward, hitDistance);

        List<GameObject> enemiesList = new List<GameObject>();

        for (int i = 0; i < RaycastAllRay1.Length; i++)
        {
            actionObject = RaycastAllRay1[i].collider.gameObject;

            if (actionObject.tag == "Enemy")
            {
                SoundManager.PlayOneShot(SoundManager.HitEnemySword, this.transform.position);
                EnemyScript enemyScript = actionObject.transform.parent.GetComponent<EnemyScript>();
                enemyScript.GetAttacked(this.gameObject.transform, true);
                foundEnemy = true;
                if (swordTier2)
                    enemyScript.GetAttackedByBomb();
                enemiesList.Add(actionObject);
            }

            if (!swordSeep)
                break;
        }

        if (foundEnemy == false || swordSeep)
        {
            for (int i = 0; i < RaycastAllRay2.Length; i++)
            {
                actionObject = RaycastAllRay2[i].collider.gameObject;

                for (int j = 0; j < enemiesList.Count; j++)
                {
                    if (enemiesList[j] == actionObject)
                    {
                        repeatedEnemy = true;
                        break;
                    }
                }

                if (actionObject.tag == "Enemy" && !repeatedEnemy)
                {
                    EnemyScript enemyScript = actionObject.transform.parent.GetComponent<EnemyScript>();
                    enemyScript.GetAttacked(this.gameObject.transform, true);
                    foundEnemy = true;
                    if (swordTier2)
                        enemyScript.GetAttackedByBomb();
                    enemiesList.Add(actionObject);
                }

                if (!swordSeep)
                    break;

                repeatedEnemy = false;
            }
        }

        if (foundEnemy == false || swordSeep)
        {
            for (int i = 0; i < RaycastAllRay3.Length; i++)
            {
                actionObject = RaycastAllRay3[i].collider.gameObject;

                for (int j = 0; j < enemiesList.Count; j++)
                {
                    if (enemiesList[j] == actionObject)
                    {
                        repeatedEnemy = true;
                        break;
                    }
                }

                if (actionObject.tag == "Enemy" && !repeatedEnemy)
                {
                    EnemyScript enemyScript = actionObject.transform.parent.GetComponent<EnemyScript>();
                    enemyScript.GetAttacked(this.gameObject.transform, true);
                    foundEnemy = true;
                    if (swordTier2)
                        enemyScript.GetAttackedByBomb();
                    enemiesList.Add(actionObject);
                }

                if (!swordSeep)
                    break;

                repeatedEnemy = false;
            }
        }

        if (!foundEnemy)
            SoundManager.PlayOneShot(SoundManager.HitAir, this.transform.position);

    }

    IEnumerator KnockBack()
    {
        knockBack = true;

        yield return new WaitForSeconds(0.2f); //Only knock the enemy back for a short time    

        knockBack = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * hitDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + transform.right * 2f, transform.forward * hitDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position - transform.right * 2f, transform.forward * hitDistance);
    }
}