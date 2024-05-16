using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Modifier
{
    public string name;
    public int value;
}

public class ModifiersManager : MonoBehaviour
{
    public AttributeAndSkill attributeAndSkill;
    public StatManager statManager;
    

    void Start(){

        FindElement("Instinct", "crippled", -2);
    }
    public void FindElement(string elementName, string modifierName, int modifierValue)
    {
        object cherry = null;

        // Check if the element is a stat
        StatArray stats = statManager.stats.FirstOrDefault(s => s.name == elementName);
        if (stats != null)
        {
            cherry = stats;
        }
        else
        {
            // Check if the element is an attribute
            AttributeArray attribute = attributeAndSkill.attributes.FirstOrDefault(a => a.name == elementName);
            if (attribute != null)
            {
                cherry = attribute;
            }
            else
            {
                // Check if the element is a skill
                foreach (AttributeArray attr in attributeAndSkill.attributes)
                {
                    SkillArray skill = attr.skills.FirstOrDefault(s => s.name == elementName);

                    if (skill != null)
                    {
                        
                        cherry = skill;
                    } else {
                        Debug.LogWarning("Element with name '" + elementName + "' not found.");
                    }
                }
            }
        }

        if(modifierValue == 0){
            RemoveModifier(cherry, modifierName);
        } else {
            AddModifier(cherry, modifierName, modifierValue);
        }
        
    }

    private void AddModifier(object cherry, string modifierName, int modifierValue)
    {
        Modifier newModifier = new Modifier();
        newModifier.name = modifierName;
        newModifier.value = modifierValue;

        if (cherry is StatArray)
        {
            StatArray stats = (StatArray)cherry;
            Modifier[] modifiers = stats.modifiers;
            Array.Resize(ref modifiers, modifiers.Length + 1);
            modifiers[modifiers.Length - 1] = newModifier;
            stats.modifiers = modifiers;

            Debug.Log("Added modifier '" + modifierName + "' with value " + modifierValue + " to stat array '" + stats.name + "'.");
        }
        else if (cherry is AttributeArray)
        {
            AttributeArray attribute = (AttributeArray)cherry;
            Modifier[] modifiers = attribute.modifiers;
            Array.Resize(ref modifiers, modifiers.Length + 1);
            modifiers[modifiers.Length - 1] = newModifier;
            attribute.modifiers = modifiers;

            Debug.Log("Added modifier '" + modifierName + "' with value " + modifierValue + " to attribute array '" + attribute.name + "'.");
        }
        else if (cherry is SkillArray)
        {
            SkillArray skill = (SkillArray)cherry;
            Modifier[] modifiers = skill.modifiers;
            Array.Resize(ref modifiers, modifiers.Length + 1);
            modifiers[modifiers.Length - 1] = newModifier;
            skill.modifiers = modifiers;

            Debug.Log("Added modifier '" + modifierName + "' with value " + modifierValue + " to skill '" + skill.name + "'.");
        }
        else
        {
            Debug.LogWarning("Invalid element type.");
        }
    }

    private void RemoveModifier(object cherry, string modifierName)
    {
        if (cherry is StatArray)
        {
            StatArray statArray = (StatArray)cherry;
            Modifier[] modifiers = statArray.modifiers;
            int index = Array.FindIndex(modifiers, mod => mod.name == modifierName);

            if (index != -1)
            {
                List<Modifier> modifierList = modifiers.ToList();
                modifierList.RemoveAt(index);
                statArray.modifiers = modifierList.ToArray();

                Debug.Log("Removed modifier '" + modifierName + "' from stat array '" + statArray.name + "'.");
            }
            else
            {
                Debug.LogWarning("Modifier '" + modifierName + "' not found in stat array '" + statArray.name + "'.");
            }
        }
        else if (cherry is AttributeArray)
        {
            AttributeArray attributeArray = (AttributeArray)cherry;
            Modifier[] modifiers = attributeArray.modifiers;
            int index = Array.FindIndex(modifiers, mod => mod.name == modifierName);

            if (index != -1)
            {
                List<Modifier> modifierList = modifiers.ToList();
                modifierList.RemoveAt(index);
                attributeArray.modifiers = modifierList.ToArray();

                Debug.Log("Removed modifier '" + modifierName + "' from attribute array '" + attributeArray.name + "'.");
            }
            else
            {
                Debug.LogWarning("Modifier '" + modifierName + "' not found in attribute array '" + attributeArray.name + "'.");
            }
        }
        else if (cherry is SkillArray)
        {
            SkillArray skillArray = (SkillArray)cherry;
            Modifier[] modifiers = skillArray.modifiers;
            int index = Array.FindIndex(modifiers, mod => mod.name == modifierName);

            if (index != -1)
            {
                List<Modifier> modifierList = modifiers.ToList();
                modifierList.RemoveAt(index);
                skillArray.modifiers = modifierList.ToArray();

                Debug.Log("Removed modifier '" + modifierName + "' from skill array '" + skillArray.name + "'.");
            }
            else
            {
                Debug.LogWarning("Modifier '" + modifierName + "' not found in skill array '" + skillArray.name + "'.");
            }
        }
        else
        {
            Debug.LogWarning("Invalid element type.");
        }
    }
}
