using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTurnTable : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Rotate(this.gameObject.transform.up, 15 *Time.deltaTime); 
    }
}
