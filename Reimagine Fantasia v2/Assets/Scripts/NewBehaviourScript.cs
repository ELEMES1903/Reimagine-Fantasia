using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public HealthBar healthBar;
    public ModifiersManager modifiersManager; // Reference to ModifiersManager
    public AttributeAndSkill attributeAndSkill; // Reference to ModifiersManager


    void Update()
    {
        // Check for spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            attributeAndSkill.AddModifier("Agility", "dsdsd ", 6, true);
        }

        // Check for spacebar press
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            attributeAndSkill.AddModifier("Strength", "Bonus Damage", 10, false);
        }
    }
}
