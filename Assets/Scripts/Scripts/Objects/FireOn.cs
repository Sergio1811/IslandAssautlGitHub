using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOn : MonoBehaviour
{
    void Start()
    {
        if (!ClimaRandom.Night)
            gameObject.SetActive(false);
    }
}
