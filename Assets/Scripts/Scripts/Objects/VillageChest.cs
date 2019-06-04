using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageChest : MonoBehaviour
{
    public GameObject enemiesGroup;
    public GameObject chest;
    public GameObject psPoof;
    bool chestOut = false;

    void Update()
    {
        if (!chestOut)
        {
            if (enemiesGroup.transform.childCount <= 0)
            {
                Instantiate(psPoof, transform.position, Quaternion.identity);
                chest.SetActive(true);
                chestOut = true;
            }
        }
    }
}
