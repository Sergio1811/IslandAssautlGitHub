using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsActivation : MonoBehaviour
{
    public bool twoDirections;
    public float speed = 0.5f;
    float timer;
    int childsNumber;
    int activatedChildren;


    void Start()
    {
        childsNumber = transform.childCount;
        if (twoDirections)
            childsNumber--;
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

            if (activatedChildren == childsNumber - 1)
                enabled = false;
        }
    }
}