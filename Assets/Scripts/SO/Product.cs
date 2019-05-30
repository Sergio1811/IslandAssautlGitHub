using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = ("Product"), menuName = ("Product"))]
public class Product : ScriptableObject
{
    public string nameProduct;
    public string description;
    public float price;
    public float reward;
    public string idName;
}
