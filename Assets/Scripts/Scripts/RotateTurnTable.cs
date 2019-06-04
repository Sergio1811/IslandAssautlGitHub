using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTurnTable : MonoBehaviour
{
    float speed;
   
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Rotate(this.gameObject.transform.up, speed *Time.deltaTime); 
    }
}
