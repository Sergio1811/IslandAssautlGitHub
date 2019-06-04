//using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public GameObject player;
    PlayerScript playerScript;
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

    public Animator myAnimator;
    Vector3 originalAnimationPosition;

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

    void Start()
    {
        originalAnimationPosition = myAnimator.transform.localPosition;

        if (patroler)
        {
            availableNodes = Grid.instance.AvailableNodesType(Node.Type.floor, 1, 1, Node.Type.enemy);
            patrolPoint = availableNodes[Random.Range(0, availableNodes.Count)];

            if (patrolPoint != null)
            {
                myAnimator.SetBool("Move", true);
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

        player = GameManager.Instance.player;
        playerScript = player.GetComponent<PlayerScript>();
        agent = GetComponent<NavMeshAgent>();
        initialPos = new Vector3(transform.position.x, 0, transform.position.z);

        iniSpeed = agent.speed;
        iniAngSpeed = agent.angularSpeed;
        iniAcc = agent.acceleration;
        knokBackIniDisctance = knockBackDistance;
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
                        currentState = state.chase; myAnimator.SetBool("Move", true);
                        myAnimator.transform.localPosition = originalAnimationPosition;
                        break;
                    }

                    else if (currentState == state.stay) Stay();
                    else Patrol();

                    break;

                case state.chase:
                    if (playerScript.actualType != PlayerScript.playerType.sword && GetSqrDistanceXZToPosition(player.transform.position) > chaseDistance)
                    {
                        if (patroler)
                        {
                            currentState = state.patrol; myAnimator.SetBool("Move", true);
                            myAnimator.transform.localPosition = originalAnimationPosition;
                        }
                        else
                        {
                            currentState = state.stay; myAnimator.SetBool("Move", false);
                            myAnimator.transform.localPosition = originalAnimationPosition;
                        }
                        break;
                    }

                    else if (GetSqrDistanceXZToPosition(player.transform.position) <= attackDistance)
                    {
                        attackTimer = 0;
                        currentState = state.attack; myAnimator.SetBool("Move", false);
                        myAnimator.transform.localPosition = originalAnimationPosition;
                        break;
                    }

                    else
                        Chase();

                    break;

                case state.attack:
                    transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

                    if (GetSqrDistanceXZToPosition(player.transform.position) > attackDistance)
                    {
                        currentState = state.chase; myAnimator.SetBool("Move", true);
                        myAnimator.transform.localPosition = originalAnimationPosition;
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
            myAnimator.SetTrigger("Attack");
            playerScript.Damage(transform.forward, true);
            //player.SendMessage("Damage");
            //GameManager.Instance.Damage();
        }
    }

    public void Stun()
    {
        myAnimator.SetBool("Stay", true);
        myAnimator.transform.localPosition = originalAnimationPosition;
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
        myAnimator.SetBool("Stay", false);
        myAnimator.transform.localPosition = originalAnimationPosition;
    }

    IEnumerator KnockBack()
    {
        myAnimator.SetBool("Stay", true);
        myAnimator.transform.localPosition = originalAnimationPosition;
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
        myAnimator.SetBool("Stay", false);
        myAnimator.transform.localPosition = originalAnimationPosition;
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
            Destroy(this.gameObject);
    }

    public void GetAttackedByBomb()
    {
        lives--;
        if (lives < 1)
            Destroy(this.gameObject);
    }
}
