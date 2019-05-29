using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour
{
    public float timeToComplete;
    public float radius;
    public float speed;
    public float startMinimumAngle;
    public float startMaximumAngle;
    float currentAngle;
    float timer = 0;

    void Start()
    {
        currentAngle = Random.Range(startMinimumAngle, startMaximumAngle);

        transform.position = new Vector3(radius * Mathf.Cos(currentAngle), radius * Mathf.Sin(currentAngle), transform.position.z);

        transform.LookAt(Vector3.zero);
        
    }

    void Update()
    {
        if (GameManager.Instance.startGame)
            if (timer < timeToComplete)
            {
                timer += Time.deltaTime;

                currentAngle += Time.deltaTime * speed;
                transform.position = new Vector3(radius * Mathf.Cos(currentAngle), radius * Mathf.Sin(currentAngle), transform.position.z);

                transform.LookAt(Vector3.zero);
            }
    }
}
