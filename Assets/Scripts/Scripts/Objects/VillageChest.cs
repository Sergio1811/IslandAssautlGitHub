using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageChest : MonoBehaviour
{
    public GameObject enemiesGroup;
    public GameObject chest;
    public GameObject psPoof;
    int children;

    private void Start()
    {
        children = enemiesGroup.transform.childCount;
    }

    void Update()
    {
        if (children <= 0)
        {
            SoundManager.PlayOneShot(SoundManager.OpenChestSound, this.transform.position);
            Instantiate(psPoof, transform.position, Quaternion.identity);
            chest.SetActive(true);
            this.enabled = false;
        }
    }

    public void ChildKilled()
    {
        children--;
    }
}
