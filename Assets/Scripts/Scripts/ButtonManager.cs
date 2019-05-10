using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static List<string> disabledButtonsList = new List<string>();

    void Awake()
    {
        for (int i = 0; i < disabledButtonsList.Count; i++)
        {
            GameObject.Find(disabledButtonsList[i]).GetComponentInChildren<Button>().interactable = false;
        }
    }
}
