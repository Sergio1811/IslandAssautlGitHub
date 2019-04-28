using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour
{
    public float timeToComplete;
    public float radius;
    float speed;
    float currentAngle;

    void Start()
    {
        speed = (Mathf.PI * 2) / timeToComplete;
    }

    void Update()
    {
        currentAngle += Time.deltaTime * speed;
        float x = radius * Mathf.Cos(currentAngle);
        float y = radius * Mathf.Sin(currentAngle);
        transform.position = new Vector3(x, y, transform.position.z);

        transform.LookAt(Vector3.zero);
    }
}
