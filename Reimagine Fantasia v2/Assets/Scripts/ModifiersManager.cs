using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class Modifier
{
    public string name;
    public int value;
}

public class ModifiersManager : MonoBehaviour
{
    public void AddModifierToElement(string elementName, string modifierName, int modifierValue, List<Attributes> attributes)
    {
        // Check if the element is an attribute
        Attributes attribute = attributes.FirstOrDefault(attr => attr.name == elementName);
        if (attribute != null)
        {
            AddModifierToAttribute(attribute, modifierName, modifierValue);
        }
        else
        {
            // Check if the element is a skill
            foreach (Attributes attr in attributes)
            {
                Skills skill = attr.skills.FirstOrDefault(s => s.name == elementName);
                if (skill != null)
                {
                    AddModifierToSkill(skill, modifierName, modifierValue);
                    return;
                }
            }
            Debug.LogWarning("Element with name '" + elementName + "' not found.");
        }
    }

    private void AddModifierToAttribute(Attributes attribute, string modifierName, int modifierValue)
    {
        // Find the Modifiers array for the attribute
        Modifier[] modifiers = attribute.modifiers;

        // Create a new modifier
        Modifier newModifier = new Modifier();
        newModifier.name = modifierName;
        newModifier.value = modifierValue;

        // Add the new modifier to the Modifiers array
        System.Array.Resize(ref modifiers, modifiers.Length + 1);
        modifiers[modifiers.Length - 1] = newModifier;

        // Assign the modified Modifiers array back to the attribute
        attribute.modifiers = modifiers;

        Debug.Log("Added modifier '" + modifierName + "' with value " + modifierValue + " to attribute '" + attribute.name + "'.");
    }

    private void AddModifierToSkill(Skills skill, string modifierName, int modifierValue)
    {
        // Find the Modifiers array for the skill
        Modifier[] modifiers = skill.modifiers;

        // Create a new modifier
        Modifier newModifier = new Modifier();
        newModifier.name = modifierName;
        newModifier.value = modifierValue;

        // Add the new modifier to the Modifiers array
        System.Array.Resize(ref modifiers, modifiers.Length + 1);
        modifiers[modifiers.Length - 1] = newModifier;

        // Assign the modified Modifiers array back to the skill
        skill.modifiers = modifiers;

        Debug.Log("Added modifier '" + modifierName + "' with value " + modifierValue + " to skill '" + skill.name + "'.");
    }

    public void RemoveModifier(string elementName, string modifierName, List<Attributes> attributes)
    {
        // Check if the element is an attribute
        Attributes attribute = attributes.FirstOrDefault(attr => attr.name == elementName);
        if (attribute != null)
        {
            RemoveModifierFromAttribute(attribute, modifierName);
        }
        else
        {
            // Check if the element is a skill
            foreach (Attributes attr in attributes)
            {
                Skills skill = attr.skills.FirstOrDefault(s => s.name == elementName);
                if (skill != null)
                {
                    RemoveModifierFromSkill(skill, modifierName);
                    return;
                }
            }
            Debug.LogWarning("Element with name '" + elementName + "' not found.");
        }
    }

    private void RemoveModifierFromAttribute(Attributes attribute, string modifierName)
    {
        // Find the modifier in the Modifiers array for the attribute
        Modifier[] modifiers = attribute.modifiers;
        int index = System.Array.FindIndex(modifiers, mod => mod.name == modifierName);

        if (index != -1)
        {
            // Remove the modifier from the Modifiers array
            List<Modifier> modifierList = modifiers.ToList();
            modifierList.RemoveAt(index);
            attribute.modifiers = modifierList.ToArray();

            Debug.Log("Removed modifier '" + modifierName + "' from attribute '" + attribute.name + "'.");
        }
        else
        {
            Debug.LogWarning("Modifier '" + modifierName + "' not found in attribute '" + attribute.name + "'.");
        }
    }

    private void RemoveModifierFromSkill(Skills skill, string modifierName)
    {
        // Find the modifier in the Modifiers array for the skill
        Modifier[] modifiers = skill.modifiers;
        int index = System.Array.FindIndex(modifiers, mod => mod.name == modifierName);

        if (index != -1)
        {
            // Remove the modifier from the Modifiers array
            List<Modifier> modifierList = modifiers.ToList();
            modifierList.RemoveAt(index);
            skill.modifiers = modifierList.ToArray();

            Debug.Log("Removed modifier '" + modifierName + "' from skill '" + skill.name + "'.");
        }
        else
        {
            Debug.LogWarning("Modifier '" + modifierName + "' not found in skill '" + skill.name + "'.");
        }
    }
}
