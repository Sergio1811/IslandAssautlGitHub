using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    private CharacterController characterController;
    private Transform cameraAnchor;

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

    public GameObject bomb;
    public GameObject sword;
    public GameObject axe;
    bool bombOn;

    bool lastActionButtonReleased = true;
    bool lastStunButtonReleased = true;

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
    void Start()
    {
        characterController = GetComponent<CharacterController>();
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
        //if (actualType == playerType.pick && bombTier2)
        //change bomb to C4
    }

    void Update()
    {
        if (canMove)
            WalkTime();

        //Mirar si ha soltado el trigger antes de volver a hacer la accion
        if (lastActionButtonReleased == false && InputManager.Instance.GetInputUp("Action")) lastActionButtonReleased = true;

        if (lastStunButtonReleased == false && InputManager.Instance.GetInputUp("Stun")) lastStunButtonReleased = true;

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

        if (lastStunButtonReleased && axeStun) Stun();

        if (inmortal)
            CheckInmortal();

        if (currentCD >= swordCooldown)
            canAttack = true;
        if (!canAttack)
            currentCD += Time.deltaTime;

    }


    void WalkTime()
    {
        if (InputManager.Instance.GetInputDown("Dash") && dashActive)
            Dash();

        else
        {
            Vector3 movement = Vector3.zero;
            xMovement = InputManager.Instance.GetAxis("Horizontal");
            zMovement = InputManager.Instance.GetAxis("Vertical");

            if (Mathf.Abs(xMovement) > 0 || Mathf.Abs(zMovement) > 0)
            {
                GameManager.Instance.GetComponent<CameraRotation>().cameraRotating = false;

                Vector3 forward = cameraAnchor.forward;
                forward.y = 0;
                forward.Normalize();
                Vector3 right = cameraAnchor.right;
                right.y = 0;
                right.Normalize();

                movement = forward * zMovement + right * xMovement;
                transform.localRotation = Quaternion.LookRotation(movement);
            }

            if (!characterController.isGrounded)
                movement.y += Physics.gravity.y;

            characterController.Move(movement.normalized * characterSpeed * bootsMovementSpeed * Time.deltaTime);
        }
    }

    void Dash()
    {
        characterController.enabled = false;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, dashDistance))
        {
            if (hit.collider.gameObject.name == "WaterDecorationMesh")
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

    void StartAction()
    {
        if (InputManager.Instance.GetInput("Action") && actualType == playerType.sword && canAttack)
        {
            canAttack = false;
            lastActionButtonReleased = false;
            SwordAttack();
        }
        else if (InputManager.Instance.GetInput("Action"))
        {
            lastActionButtonReleased = false;

            Ray ray = new Ray(transform.position, transform.forward);
            Ray ray2 = new Ray(transform.position + transform.right * 1.5f, transform.forward);
            Ray ray3 = new Ray(transform.position - transform.right * 1.5f, transform.forward);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, hitDistance) || Physics.Raycast(ray2, out hit, hitDistance) || Physics.Raycast(ray3, out hit, hitDistance))
            {
                actionObject = hit.collider.gameObject;
                switch (actionObject.tag)
                {
                    case "Tree":
                        if (actualType == playerType.ace)
                        {
                            axe.GetComponent<Animation>().Play();
                            neededPressedTime = iniPressedTime;
                            actionOn = true;
                        }

                        if (actualType == playerType.sword && swordPolivalente)
                        {
                            neededPressedTime = 2;
                            actionOn = true;
                        }
                        break;
                    case "Enemy":
                        /* if (actualType == playerType.sword)
                         {
                             sword.GetComponent<Animation>().Play();
                             actionObject.transform.parent.GetComponent<EnemyScript>().GetAttacked(this.gameObject.transform);
                         }*/
                        if (actualType == playerType.ace && axePolivalente)
                            actionObject.transform.parent.GetComponent<EnemyScript>().GetAttacked(this.gameObject.transform);
                        break;

                    /*case "Chest":
                        if (actualType == playerType.sword || (AxerAbilities.Polivalente && actualType == playerType.ace) || (BomberAbilities.Polivalente && actualType == playerType.pick))
                            actionOn = true;

                        break;*/
                    case "Rock":
                        if (actualType == playerType.ace && axePolivalente)
                        {
                            neededPressedTime *= 2;
                            actionOn = true;
                        }
                        if (actualType == playerType.sword && swordPolivalente)
                        {
                            neededPressedTime = 2;
                            actionOn = true;
                        }
                        break;
                        //case "Exit":
                        //    GameManager.Instance.LevelComplete();
                        //    break;
                }
            }


            if (actualType == playerType.pick)
            {
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
        if (InputManager.Instance.GetInput("Action"))
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
        else if (InputManager.Instance.GetInput("Action") && pressedTimer >= 0.1f && lastActionButtonReleased)
        {
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
                    BreakRock(actionObject.transform.parent.parent.gameObject);
                if (actualType == playerType.ace && axePolivalente)
                    BreakRock(actionObject.transform.parent.parent.gameObject);
                if (actualType == playerType.sword && swordPolivalente)
                    BreakRock(actionObject.transform.parent.parent.gameObject);
                break;
            case "Tree":
                if (actualType == playerType.ace)
                    CutTree(actionObject.transform.parent.parent.gameObject);
                //if (actualType == playerType.pick && bombPolivalente)
                    //CutTree(actionObject.transform.parent.parent.gameObject);
                if (actualType == playerType.sword && swordPolivalente)
                    CutTree(actionObject.transform.parent.parent.gameObject);
                break;
            case "Chest":
                if (actualType == playerType.sword || (AxerAbilities.Polivalente && actualType == playerType.ace) || (BomberAbilities.Polivalente && actualType == playerType.pick))
                    PickFabrics(actionObject);
                break;
        }

        actionOn = false;
        actionSphere.fillAmount = 0;
        pressedTimer = 0f;
        receiveInputAction = true;
        canMove = true;
    }

    public void BreakRock(GameObject rock)
    {
        GameManager.Instance.PickRock();
        Destroy(rock);
    }

    public void CutTree(GameObject tree)
    {
        GameManager.Instance.PickWood();
        Destroy(tree);
    }

    void Stun()
    {
        if (InputManager.Instance.GetInput("Stun"))
        {
            lastStunButtonReleased = false;

            Ray ray = new Ray(transform.position, transform.forward);
            Ray ray2 = new Ray(transform.position + transform.right * 1.5f, transform.forward);
            Ray ray3 = new Ray(transform.position - transform.right * 1.5f, transform.forward);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, hitDistance) || Physics.Raycast(ray2, out hit, hitDistance) || Physics.Raycast(ray3, out hit, hitDistance))
            {
                actionObject = hit.collider.gameObject;
                if (actionObject.tag == "Enemy")
                {
                    if (actualType == playerType.ace && axeStun)
                        actionObject.transform.parent.GetComponent<EnemyScript>().Stun();
                }
            }
        }
    }

    public void PickFabrics(GameObject chest)
    {
        GameManager.Instance.PickFabrics();
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
            GameManager.Instance.EndProtoLevel();
        else if (hit.gameObject.tag == "Exit")
            GameManager.Instance.LevelComplete();
        else if ((hit.gameObject.tag == "Chest") && ((AxerAbilities.Polivalente && actualType == playerType.ace) || (BomberAbilities.Polivalente && actualType == playerType.pick)))
            PickFabrics(hit.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Rock" && actualType == playerType.pick)
            BreakRock(other.transform.parent.gameObject);
        else if (other.gameObject.tag == "Tree" && actualType == playerType.pick)
            CutTree(other.transform.parent.gameObject);
    }

    public void Damage(Vector3 direction)
    {
        if (!inmortal)
        {
            inmortal = true;
            inmortalTimer = 0;
            knockDirection = direction;
            StartCoroutine(KnockBack());
            GameManager.Instance.Damage();
        }
    }

    public void SwordAttack()
    {
        bool foundEnemy = false;

        sword.GetComponent<Animation>().Play();
        currentCD = 0;
        Collider[] enemies = Physics.OverlapSphere(this.transform.position, hitDistance);

        RaycastHit[] RaycastAllRay1 = Physics.RaycastAll(transform.position, transform.forward, hitDistance);
        RaycastHit[] RaycastAllRay2 = Physics.RaycastAll(transform.position + transform.right * 1.5f, transform.forward, hitDistance);
        RaycastHit[] RaycastAllRay3 = Physics.RaycastAll(transform.position - transform.right * 1.5f, transform.forward, hitDistance);

        for (int i = 0; i < RaycastAllRay1.Length; i++)
        {
            actionObject = RaycastAllRay1[i].collider.gameObject;

            if (actionObject.tag == "Enemy")
            {
                actionObject.transform.parent.GetComponent<EnemyScript>().GetAttacked(this.gameObject.transform);
                foundEnemy = true;
            }

            if (!swordSeep)
                break;
        }

        if (foundEnemy == false || swordSeep)
        {
            for (int i = 0; i < RaycastAllRay2.Length; i++)
            {
                actionObject = RaycastAllRay2[i].collider.gameObject;

                if (actionObject.tag == "Enemy")
                {
                    actionObject.transform.parent.GetComponent<EnemyScript>().GetAttacked(this.gameObject.transform);
                    foundEnemy = true;
                }

                if (!swordSeep)
                    break;
            }
        }

        if (foundEnemy == false || swordSeep)
        {
            for (int i = 0; i < RaycastAllRay3.Length; i++)
            {
                actionObject = RaycastAllRay3[i].collider.gameObject;

                if (actionObject.tag == "Enemy")
                {
                    actionObject.transform.parent.GetComponent<EnemyScript>().GetAttacked(this.gameObject.transform);
                    foundEnemy = true;
                }

                if (!swordSeep)
                    break;
            }
        }



        /*
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].tag == "Enemy")
            {
                if (Vector3.Dot(this.gameObject.transform.GetChild(0).forward, enemies[i].transform.forward) >= angleAttack)
                {
                    enemies[i].transform.parent.GetComponent<EnemyScript>().GetAttacked(this.gameObject.transform);
                   
                    if (!swordSeep)
                        break;
                }
            }
        }*/

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
        Gizmos.DrawRay(transform.position + transform.right * 1.5f, transform.forward * hitDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position - transform.right * 1.5f, transform.forward * hitDistance);

    }
}
