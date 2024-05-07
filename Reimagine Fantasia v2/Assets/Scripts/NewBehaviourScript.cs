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
            // Call TakeDamage method of HealthBar script
            healthBar.TakeDamage(10f, false); 
        }

        // Check for spacebar press
        if (Input.GetKeyDown(KeyCode.Return))
        {
            modifiersManager.RemoveModifier("Strength", "Bonus Damage", attributeAndSkill.attributes);
        }
    }
}
