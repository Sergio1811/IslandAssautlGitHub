using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRandomChild : MonoBehaviour
{

    void Start()
    {
        int randomNumber = Random.Range(0, transform.childCount);
        transform.GetChild(randomNumber).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
