using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDisplay : MonoBehaviour
{
    public Abilities Ability;

    public Image Icon;
    public Text Name;
    public Text Description;
    public Button Button;


    void Start()
    {
        Icon.sprite = Ability.Thumbnail;

        Name.text = Ability.Name;

        Description.text = Ability.Description;
    }
    
}
