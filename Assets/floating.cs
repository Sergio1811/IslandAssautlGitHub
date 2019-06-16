using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floating : MonoBehaviour
{
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }
    
    void Update()
    {
        transform.localPosition = startPosition + new Vector3(0.0f, Mathf.Sin(Time.time * 3f) * 0.05f, 0.0f);
    }
}
