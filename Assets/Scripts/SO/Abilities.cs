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
    [HideInInspector]
    public bool isBought;
    [HideInInspector]
    public bool isUnlocked;
}

[CreateAssetMenu(fileName = ("Product"), menuName = ("Product"))]
public class Product : ScriptableObject
{
    public string nameProduct;
    public string description;
    public float price;
    public float reward;
    public string idName;
}
