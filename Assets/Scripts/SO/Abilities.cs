using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = ("Ability"), menuName = ("Ability"))]
public class Abilities : ScriptableObject
{
    public string nameAbility;
    public string description;
    public float price;
    public bool materialNeeded;
    public Node.Type materialType;
    public float materialPrice;
    public Sprite icono;
    public Sprite iconoByN;
    public string saverString;
    public bool isBought;
    public bool isUnlocked;
}
