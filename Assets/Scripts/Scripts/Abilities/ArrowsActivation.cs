using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsActivation : MonoBehaviour
{
    public bool twoDirections;
    float speed = 0.2f;
    float timer;
    int childsNumber;
    int activatedChildren;


    void Start()
    {
        childsNumber = transform.childCount;
        timer = 0;
        activatedChildren = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= speed)
        {
            timer = 0;

            if (activatedChildren == 0 && twoDirections)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                activatedChildren++;
            }
            transform.GetChild(activatedChildren).gameObject.SetActive(true);
            activatedChildren++;

            if (activatedChildren == childsNumber)
                enabled = false;
        }
    }
}