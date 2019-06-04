using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalLayoutMovement : MonoBehaviour
{
    public VerticalLayoutGroup verticalLayout;
    int counter;

    void Start()
    {
        counter = 4;
    }
    
    void Update()
    {
        if (counter > 0)
        {
            counter--;
            verticalLayout.childForceExpandHeight = true;
            verticalLayout.childForceExpandHeight = false;
        }
    }
}