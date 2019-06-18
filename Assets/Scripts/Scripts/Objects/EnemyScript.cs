//using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    GameManager gameManager;

    public GameObject player;
    PlayerScript playerScript;
    public Node patrolPoint;
    List<Node> availableNodes;
    private NavMeshAgent agent;

    public bool patroler = false;

    public enum state { stay, patrol, chase, attack, dead }
    public state currentState;

    public float chaseDistance;
    public float attackDistance;
    public float attackCoolDown;
    private float attackTimer = 0;

    Vector3 initialPos;
    float initialDistance;
    Quaternion initialRotation;

    Material initMat;
    public Material attackMat;

    public Animator myAnimator;

    float iniSpeed;
    float iniAngSpeed;
    float stunnedTimer = 0;
    public float maxStunnedTimer = 2;
    bool stunned = false;
    bool canAttack = true;

    public int lives = 2;

    float knockBackDistance = 15;
    bool knockBack = false;
    Vector3 direction;
    float iniAcc;
    float knokBackIniDisctance;

    public GameObject psStun;
    public GameObject psDamage;
    Vector3 psOffset = new Vector3(0, 4, 0);

    float deadTime = 0;
    float timeToDestroy = 3f;


    void Start()
    {
        gameManager = GameManager.Instance;

        if (patroler)
        {
            availableNodes = Grid.instance.AvailableNodesType(Node.Type.floor, 1, 1, Node.Type.enemy);
            patrolPoint = availableNodes[Random.Range(0, availableNodes.Count)];

            if (patrolPoint != null)
            {
                myAnimator.SetBool("Move", false);
                currentState = state.patrol;
            }
            else
            {
                print("No patrol points in patroler, changing to non-patroler");
                patroler = false;
            }
        }
        else
        {
            myAnimator.SetBool("Move", false);
            currentState = state.stay;
        }

        player = gameManager.player;
        playerScript = player.GetComponent<PlayerScript>();
        agent = GetComponent<NavMeshAgent>();

        if (!patroler)
        {
            initialPos = new Vector3(transform.position.x, 0, transform.position.z);
            initialRotation = transform.rotation;
        }

        iniSpeed = agent.speed;
        iniAngSpeed = agent.angularSpeed;
        iniAcc = agent.acceleration;
        knokBackIniDisctance = knockBackDistance;
    }

    void Update()
    {
        if (GameManager.startGame)
        {
            if (player != null)
                UpdateState(currentState);

            else
                player = GameObject.FindGameObjectWithTag("Player");

            if (stunned)
            {
                stunnedTimer += InputManager.deltaTime;
                if (stunnedTimer >= maxStunnedTimer)
                    OffStun();
            }
        }
    }

    void FixedUpdate()
    {
        if (knockBack)
        {
            agent.velocity = direction * knockBackDistance;//Knocks the enemy back when appropriate
        }
    }

    void ChangeState(state newState)
    {
        switch (newState)
        {
            case state.stay:
                agent.isStopped = false;
                myAnimator.SetBool("Move", true);
                break;

            case state.patrol:
                agent.isStopped = false;
                myAnimator.SetBool("Move", true);
                break;

            case state.chase:
                agent.isStopped = false;
                myAnimator.SetBool("Move", true);
                break;

            case state.attack:
                attackTimer = 0;
                agent.isStopped = true;
                myAnimator.SetBool("Move", false);
                break;

            case state.dead:
                SoundManager.PlayOneShot(SoundManager.DeathSound, this.transform.position);
                agent.isStopped = true;
                myAnimator.SetBool("Move", false);
                myAnimator.SetBool("Dead", true);
                agent.enabled = false;
                this.transform.GetChild(0).tag = "Untagged";
                this.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
                if (!patroler)
                    transform.parent.SendMessage("ChildKilled");
                break;
        }
        currentState = newState;
    }

    void UpdateState(state currentState)
    {
        switch (currentState)
        {
            case state.stay:
            case state.patrol:
                if (GetSqrDistanceXZToPosition(player.transform.position) <= chaseDistance && !gameManager.ignorePlayer)
                {
                    ChangeState(state.chase);
                    break;
                }

                else if (currentState == state.stay)
                    Stay();

                else
                    Patrol();

                break;

            case state.chase:
                if (gameManager.ignorePlayer)
                {
                    if (patroler)
                        ChangeState(state.patrol);
                    else
                        ChangeState(state.stay);
                    break;
                }

                if (playerScript.actualType != PlayerScript.playerType.sword && GetSqrDistanceXZToPosition(player.transform.position) > chaseDistance)
                {
                    if (patroler)
                    {
                        ChangeState(state.patrol);
                        break;
                    }
                    else
                    {
                        ChangeState(state.stay);
                        break;
                    }
                }

                else if (GetSqrDistanceXZToPosition(player.transform.position) <= attackDistance)
                {
                    ChangeState(state.attack);
                    break;
                }

                else
                    Chase();

                break;

            case state.attack:
                if (gameManager.ignorePlayer)
                {
                    if (patroler)
                        ChangeState(state.patrol);
                    else
                        ChangeState(state.stay);
                    break;
                }

                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

                if (GetSqrDistanceXZToPosition(player.transform.position) > attackDistance)
                {
                    ChangeState(state.chase);
                    break;
                }

                else if (canAttack)
                    Attack();

                break;

            case state.dead:
                transform.position -= Vector3.up * InputManager.deltaTime;

                deadTime += InputManager.deltaTime;

                if (deadTime >= timeToDestroy)
                    Destroy(this.gameObject);
                break;
        }
    }


    void Stay()
    {
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);

        if (Vector3.Distance(pos, initialPos) <= 0.2)
        {
            agent.isStopped = true;
            myAnimator.SetBool("Move", false);
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, InputManager.deltaTime * 10);
        }

        else
        {
            agent.SetDestination(initialPos);
        }
    }

    void Patrol()
    {
        if (!myAnimator.GetBool("Move"))
            myAnimator.SetBool("Move", true);

        if (patrolPoint != null)
            agent.SetDestination(patrolPoint.worldPosition);

        if (transform.position == patrolPoint.worldPosition || Vector3.Distance(transform.position, patrolPoint.worldPosition) < 10.0f || patrolPoint == null)
        {
            availableNodes = Grid.instance.AvailableNodesType(Node.Type.floor, 1, 1, Node.Type.enemy);
            patrolPoint = availableNodes[Random.Range(0, availableNodes.Count)];
            availableNodes.Clear();
        }
    }

    void Chase()
    {
        agent.SetDestination(player.transform.position);
    }


    void Attack()
    {
        attackTimer += InputManager.deltaTime;

        if (attackTimer >= attackCoolDown)
        {

            attackTimer = 0;
            myAnimator.SetTrigger("Attack");
            playerScript.Damage(transform.forward, true);
        }
    }

    public void Stun()
    {
        agent.speed = 0;
        agent.angularSpeed = 0;
        canAttack = false;
        stunned = true;
        stunnedTimer = 0;
        Instantiate(psStun, this.transform.position + psOffset, Quaternion.identity);
    }

    public void OffStun()
    {
        agent.speed = iniSpeed;
        agent.angularSpeed = iniAngSpeed;
        canAttack = true;
        stunned = false;
    }

    IEnumerator KnockBack()
    {
        knockBack = true;
        agent.speed = iniSpeed * 1.5f;
        agent.angularSpeed = 0;//Keeps the enemy facing forwad rther than spinning
        agent.acceleration = iniAcc * 1.5f;
        agent.baseOffset += 0.1f;

        yield return new WaitForSeconds(0.2f); //Only knock the enemy back for a short time    

        //Reset to default values
        knockBack = false;
        agent.speed = iniSpeed;
        agent.angularSpeed = iniAngSpeed;
        agent.acceleration = iniAcc;
        agent.baseOffset = 0;
        knockBackDistance = knokBackIniDisctance;
    }

    public void KnockBackActivated(Transform bomb)
    {
        direction = -(bomb.position - this.gameObject.transform.position);
        StartCoroutine(KnockBack());
    }


    private float GetSqrDistanceXZToPosition(Vector3 position)
    {
        Vector3 vector = position - transform.position;
        vector.y = 0;

        return vector.sqrMagnitude;
    }

    public void GetAttacked(Transform player, bool knockBack)
    {
        Instantiate(psDamage, this.transform.position + psOffset, Quaternion.identity);
        lives--;
        if (knockBack)
        {
            KnockBackActivated(player);
            knockBackDistance *= 0.5f;
        }

        if (lives < 1)
            ChangeState(state.dead);
        else
            SoundManager.PlayOneShot(SoundManager.EnemyHurtSound, this.transform.position);
    }

    public void GetAttackedByBomb()
    {
        lives--;
        if (lives < 1)
            ChangeState(state.dead);
    }
}