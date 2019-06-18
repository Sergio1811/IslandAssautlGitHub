using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOff : MonoBehaviour
{
    void Start()
    {
        if (ClimaRandom.Rain)
            Destroy(this.gameObject);
    }
}
