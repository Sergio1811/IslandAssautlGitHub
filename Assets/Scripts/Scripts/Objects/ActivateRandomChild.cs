using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRandomChild : MonoBehaviour
{
    void Start()
    {
        int childrenNumber = transform.childCount;
        int randomNumber = Random.Range(0, childrenNumber);
        
        for (int i = childrenNumber - 1; i >= 0; i--)
        {
            if (i != randomNumber)
                Destroy(transform.GetChild(i).gameObject);
            else
                transform.GetChild(randomNumber).gameObject.SetActive(true);
        }
    }
}
