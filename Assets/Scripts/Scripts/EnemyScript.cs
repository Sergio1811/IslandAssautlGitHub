//using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public GameObject player;
    public Node patrolPoint;
    List<Node> availableNodes;
    private NavMeshAgent agent;

    public bool patroler = false;

    public enum state { stay, patrol, chase, attack }
    public state currentState;

    public float chaseDistance;
    public float attackDistance;
    public float attackCoolDown;
    private float attackTimer = 0;

    public float repathTime;
    private float repathTimer = 0;

    Vector3 goingPos;
    Vector3 initialPos;

    Material initMat;
    public Material attackMat;

    
    float iniSpeed;
    float iniAngSpeed;
    float stunnedTimer = 0;
    public float maxStunnedTimer = 2;
    bool stunned = false;
    bool canAttack = true;

    int lives = 2;

    float knockBackDistance = 8;
    bool knockBack = false;
    Vector3 direction;
    float iniAcc;

    void Start()
    {
        if (patroler)
        {
            availableNodes = Grid.instance.AvailableNodesType(Node.Type.floor, 1, 1, Node.Type.enemy);
            patrolPoint = availableNodes[Random.Range(0, availableNodes.Count)];

            if (patrolPoint != null)
                currentState = state.patrol;
            else
            {
                print("No patrol points in patroler, changing to non-patroler");
                patroler = false;
            }
        }
        else
            currentState = state.stay;

        //player = GameManager.Instance.player;
        agent = GetComponent<NavMeshAgent>();
        initialPos = transform.position;

        initMat = transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;

        iniSpeed = agent.speed;
        iniAngSpeed = agent.angularSpeed;
        iniAcc = agent.acceleration;
    }

    void Update()
    {
        if (player != null)
        {
            switch (currentState)
            {
                case state.stay:
                case state.patrol:
                    if (GetSqrDistanceXZToPosition(player.transform.position) <= chaseDistance)
                    {
                        goingPos = player.transform.position;
                        currentState = state.chase;
                        break;
                    }

                    else if (currentState == state.stay) Stay();
                    else Patrol();

                    break;

                case state.chase:
                    if (player.GetComponent<Movement>().actualType != Movement.playerType.sword && GetSqrDistanceXZToPosition(player.transform.position) > chaseDistance)
                    {
                        if (patroler) currentState = state.patrol;
                        else currentState = state.stay;
                        break;
                    }

                    else if (GetSqrDistanceXZToPosition(player.transform.position) <= attackDistance)
                    {
                        attackTimer = 0;
                        currentState = state.attack;
                        if (attackMat != null) transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = attackMat;
                        else print("No attack material attached in inspector");
                        break;
                    }

                    else
                        Chase();

                    break;

                case state.attack:
                    transform.LookAt(new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z));

                    if (GetSqrDistanceXZToPosition(player.transform.position) > attackDistance)
                    {
                        currentState = state.chase;
                        transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = initMat;
                        break;
                    }

                    else if (canAttack)
                        Attack();

                    break;
            }
        }
        else
            player = GameObject.FindGameObjectWithTag("Player");

        if (stunned)
        {
            stunnedTimer += Time.deltaTime;
            if (stunnedTimer >= maxStunnedTimer)
                OffStun();
        }
    }

    void FixedUpdate()
    {
        if (knockBack)
        {
            agent.velocity = direction * knockBackDistance;//Knocks the enemy back when appropriate
        }
    }


    void Stay()
    {
        agent.isStopped = false;

        agent.SetDestination(initialPos);

        if (Vector3.Distance(transform.position, initialPos) < 0.2f)
        {
            agent.isStopped = true;
        }
    }

    void Patrol()
    {
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
        agent.isStopped = false;

        agent.SetDestination(player.transform.position);

        repathTimer += Time.deltaTime;

        if (repathTimer >= repathTime)
        {
            goingPos = player.transform.position;
            repathTimer = 0;
        }
    }


    void Attack()
    {
        agent.isStopped = true;

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCoolDown)
        {
            attackTimer = 0;
            player.GetComponent<Movement>().Damage(transform.forward);
            //player.SendMessage("Damage");
            //GameManager.Instance.Damage();
        }
    }

    public void Stun()
    {
        agent.speed = 0;
        agent.angularSpeed = 0;
        canAttack = false;
        stunned = true;
        stunnedTimer = 0;
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
    }

    public void KnockBackActivated(Transform bomb)
    {
        direction = bomb.position - this.gameObject.transform.position;
        StartCoroutine(KnockBack());
    }


    private float GetSqrDistanceXZToPosition(Vector3 position)
    {
        Vector3 vector = position - transform.position;
        vector.y = 0;

        return vector.sqrMagnitude;
    }

    public void GetAttacked()
    {
        lives--;
        if(lives < 1)
            Destroy(this.gameObject);
    }
}
