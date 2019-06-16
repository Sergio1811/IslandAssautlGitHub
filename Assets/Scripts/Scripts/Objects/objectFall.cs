using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class objectFall : MonoBehaviour
{
    Rigidbody myRigidbody;
    float velocity = 4;
    float finalPositionY;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.down);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "IslandCollision")
                finalPositionY = hit.point.y;
        }
    }


    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -10f, transform.position.z), velocity * Time.deltaTime);
        if (transform.position.y <= finalPositionY)
        {
            transform.position = new Vector3(transform.position.x, finalPositionY , transform.position.z);
            enabled = false;
        }
    }
}