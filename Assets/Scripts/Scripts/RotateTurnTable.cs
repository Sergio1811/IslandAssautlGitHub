using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTurnTable : MonoBehaviour
{
    float speed;
   
    void Start()
    {
        speed = Random.Range(0.5f, 1.5f);
    }
    
    void Update()
    {
        this.gameObject.transform.Rotate(this.gameObject.transform.up, speed *InputManager.deltaTime); 
    }
}
