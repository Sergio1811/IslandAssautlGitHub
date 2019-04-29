using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour
{
    public float timeToComplete;
    public float radius;
    float speed;
    public float startAngle;
    float currentAngle;
    public float degreesToRotate;

    void Start()
    {
        currentAngle = startAngle;

        transform.position = new Vector3(radius * Mathf.Cos(currentAngle), radius * Mathf.Sin(currentAngle), transform.position.z);

        speed = (Mathf.Deg2Rad * degreesToRotate) / timeToComplete;
    }

    void Update()
    {
        currentAngle += Time.deltaTime * speed;

        transform.position = new Vector3(radius * Mathf.Cos(currentAngle), radius * Mathf.Sin(currentAngle), transform.position.z);

        transform.LookAt(Vector3.zero);
    }
}
