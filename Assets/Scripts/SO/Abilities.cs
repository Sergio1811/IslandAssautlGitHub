using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = ("Ability"), menuName = ("Ability"))]
public class Abilities : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Thumbnail;
}
