using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public GameObject player;
    public Transform[] patrolPoints;
    private int patrolPointID;
    private NavMeshAgent agent;

    public enum state { patrol, chase, attack}
    public state currentState = state.patrol;

    public float chaseDistance;
    public float attackDistance;
    public float attackCoolDown;
    private float attackTimer = 0;

    public float repathTime;
    private float repathTimer = 0;

    Vector3 goingPos;
    Vector3 initialPos;

    void Start()
    {
        //player = GameManager.Instance.player;
        agent = GetComponent<NavMeshAgent>();
        initialPos = transform.position;
    }

    void Update()
    {
        if (player != null)
        {
            switch (currentState)
            {
                case state.patrol:
                    if (GetSqrDistanceXZToPosition(player.transform.position) <= chaseDistance)
                    {
                        goingPos = player.transform.position;
                        currentState = state.chase;
                        break;
                    }

                    else
                        Patrol();

                    break;
                case state.chase:
                    if (player.GetComponent<Movement>().actualType != Movement.playerType.sword && GetSqrDistanceXZToPosition(player.transform.position) > chaseDistance)
                    {
                        currentState = state.patrol;
                        break;
                    }

                    else if (GetSqrDistanceXZToPosition(player.transform.position) <= attackDistance)
                    {
                        currentState = state.attack;
                        break;
                    }

                    else
                        Chase();

                    break;

                case state.attack:
                    if (GetSqrDistanceXZToPosition(player.transform.position) > attackDistance)
                    {
                        currentState = state.chase;
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


    void Patrol()
    {
        agent.isStopped = false;

        agent.SetDestination(initialPos);

        if(Vector3.Distance(transform.position, initialPos) < 0.2f)
        {
            agent.isStopped = true;
        }

        //if (patrolPoints.Length > 0)
        //{
        //    agent.SetDestination(patrolPoints[patrolPointID].position);

        //    if (transform.position == patrolPoints[patrolPointID].position || Vector3.Distance(transform.position, patrolPoints[patrolPointID].position) < 0.2f)
        //    {
        //        patrolPointID++;
        //    }

        //    if (patrolPointID >= patrolPoints.Length)
        //    {
        //        patrolPointID = 0;
        //    }
        //}
    }

    void Chase()
    {
        agent.isStopped = false;

        agent.SetDestination(player.transform.position);

        repathTimer += Time.deltaTime;

        if(repathTimer>=repathTime)
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
