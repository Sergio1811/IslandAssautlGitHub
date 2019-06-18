using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInSeconds : MonoBehaviour
{
    public float seconds;
   
    void Update()
    {
        seconds -= InputManager.deltaTime;

        if (seconds <=0)
            Destroy(this.gameObject);
    }
}
