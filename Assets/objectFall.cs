using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class objectFall : MonoBehaviour
{
    Rigidbody myRigidbody;
    float velocity = 4;
    bool moving;
    public bool isEnemy;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        moving = true;
    }


    void Update()
    {
        if (moving)
        {
            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), time);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 0, transform.position.z), velocity* Time.deltaTime);

            if (isEnemy)
            {
                GetComponent<EnemyScript>().enabled = true;
                GetComponent<NavMeshAgent>().enabled = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Island")
        {
            moving = false;
        }
    }
}
