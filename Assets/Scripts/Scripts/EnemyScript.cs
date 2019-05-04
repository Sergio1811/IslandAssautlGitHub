//using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public GameObject player;
    public GameObject[] patrolPoints; //random points in map
    private int patrolPointID;
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
            patrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");

            if(patrolPoints.Length>0)
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
                        if(patroler) currentState = state.patrol;
                        else currentState = state.stay;
                        break;
                    }

                    else if (GetSqrDistanceXZToPosition(player.transform.position) <= attackDistance)
                    {
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
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[patrolPointID].transform.position);

            if (transform.position == patrolPoints[patrolPointID].transform.position || Vector3.Distance(transform.position, patrolPoints[patrolPointID].transform.position) < 10.0f)
            {
                patrolPointID = Random.Range(0, patrolPoints.Length);
            }
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

    private float GetSqrDistanceXZToPosition(Vector3 position)
    {
        Vector3 vector = position - transform.position;
        vector.y = 0;

        return vector.sqrMagnitude;
    }
}
