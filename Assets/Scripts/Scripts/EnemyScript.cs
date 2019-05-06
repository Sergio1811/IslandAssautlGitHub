//using System;
using System.Collections.Generic;
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
        initMat = transform.GetComponentInChildren<Renderer>().material;
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
                        if (attackMat != null) transform.GetComponentInChildren<Renderer>().material = attackMat;
                        else print("No attack material attached in inspector");
                        break;
                    }

                    else
                        Chase();

                    break;

                case state.attack:
                    if (GetSqrDistanceXZToPosition(player.transform.position) > attackDistance)
                    {
                        currentState = state.chase;
                        transform.GetComponentInChildren<Renderer>().material = initMat;
                        break;
                    }

                    else
                        Attack();

                    break;
            }
        }
        else
            player = GameObject.FindGameObjectWithTag("Player");
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
        else if (transform.position == patrolPoint.worldPosition || Vector3.Distance(transform.position, patrolPoint.worldPosition) < 10.0f || patrolPoint == null)
        {
            print("AHHHHHH");
            availableNodes = Grid.instance.AvailableNodesType(Node.Type.floor, 1, 1, Node.Type.enemy);
            patrolPoint = availableNodes[Random.Range(0, availableNodes.Count)];
            availableNodes.Clear();
        }

        print(transform.position + "      " + patrolPoint.worldPosition);
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

    private float GetSqrDistanceXZToPosition(Vector3 position)
    {
        Vector3 vector = position - transform.position;
        vector.y = 0;

        return vector.sqrMagnitude;
    }
}
