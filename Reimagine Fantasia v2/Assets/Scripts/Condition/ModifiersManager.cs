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
    private AttributeAndSkill attributeAndSkill;
    private OtherStat otherStat;
    
    void Start()
    {
        attributeAndSkill = GetComponent<AttributeAndSkill>();
        otherStat = GetComponent<OtherStat>();
    }

    public void FindElement(string elementName, string modifierName, int modifierValue)
    {
        object cherry = null;

        // Check if the element is a stat
        StatArray stats = otherStat.stats.FirstOrDefault(s => s.name == elementName);
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
                    } else
                    {
                        Debug.LogWarning("Element with name '" + elementName + "' not found.");
                    }
                }
            }
        }

        if(modifierValue == 0)
        {
            RemoveModifier(cherry, modifierName);
        } else
        {
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

            // Check if a modifier with the same name already exists
            if (modifiers.Any(mod => mod.name == modifierName))
            {
                Debug.LogWarning("Modifier '" + modifierName + "' already exists in skill array '" + stats.name + "'.");
                return;
            }

            Array.Resize(ref modifiers, modifiers.Length + 1);
            modifiers[modifiers.Length - 1] = newModifier;
            stats.modifiers = modifiers;
            attributeAndSkill.CalculateTotalValue(stats);

            Debug.Log("Added modifier '" + modifierName + "' with value " + modifierValue + " to stat array '" + stats.name + "'.");
        }
        else if (cherry is AttributeArray)
        {
            AttributeArray attribute = (AttributeArray)cherry;
            Modifier[] modifiers = attribute.modifiers;

            // Check if a modifier with the same name already exists
            if (modifiers.Any(mod => mod.name == modifierName))
            {
                Debug.LogWarning("Modifier '" + modifierName + "' already exists in skill array '" + attribute.name + "'.");
                return;
            }

            Array.Resize(ref modifiers, modifiers.Length + 1);
            modifiers[modifiers.Length - 1] = newModifier;
            attribute.modifiers = modifiers;

            attributeAndSkill.CalculateTotalValue(attribute);

            Debug.Log("Added modifier '" + modifierName + "' with value " + modifierValue + " to attribute array '" + attribute.name + "'.");
        }
        else if (cherry is SkillArray)
        {
            SkillArray skill = (SkillArray)cherry;
            Modifier[] modifiers = skill.modifiers;

            // Check if a modifier with the same name already exists
            if (modifiers.Any(mod => mod.name == modifierName))
            {
                Debug.LogWarning("Modifier '" + modifierName + "' already exists in skill array '" + skill.name + "'.");
                return;
            }

            Array.Resize(ref modifiers, modifiers.Length + 1);
            modifiers[modifiers.Length - 1] = newModifier;
            skill.modifiers = modifiers;
            attributeAndSkill.CalculateTotalValue(skill);

            Debug.Log("Added modifier '" + modifierName + "' with value " + modifierValue + " to skill '" + skill.name + "'.");
        }
        else
        {
            Debug.LogWarning("Invalid element type.");
        }
        otherStat.UpdateAll();
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
                attributeAndSkill.CalculateTotalValue(statArray);

                Debug.Log("Removed modifier '" + modifierName + "' from stat array '" + statArray.name + "'.");
            }
            else
            {
                if(modifierName == "Energetic" || modifierName == "Tired")
                {

                } else {
                    Debug.LogWarning("Modifier '" + modifierName + "' not found in stat array '" + statArray.name + "'.");
                }
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
                attributeAndSkill.CalculateTotalValue(attributeArray);

                Debug.Log("Removed modifier '" + modifierName + "' from attribute array '" + attributeArray.name + "'.");
            }
            else
            {
                if(modifierName == "Energetic" || modifierName == "Tired")
                {

                } else {
                    Debug.LogWarning("Modifier '" + modifierName + "' not found in stat array '" + attributeArray.name + "'.");
                }
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
                attributeAndSkill.CalculateTotalValue(skillArray);

                Debug.Log("Removed modifier '" + modifierName + "' from skill array '" + skillArray.name + "'.");
            }
            else
            {
                if(modifierName == "Energetic" || modifierName == "Tired")
                {

                } else {
                    Debug.LogWarning("Modifier '" + modifierName + "' not found in stat array '" + skillArray.name + "'.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Invalid element type.");
        }
        otherStat.UpdateAll();
    }

    public void RemoveAllModifiers()
    {
        foreach (StatArray stat in otherStat.stats)
        {
            stat.modifiers = new Modifier[0];
        }

        foreach (AttributeArray attribute in attributeAndSkill.attributes)
        {
            attribute.modifiers = new Modifier[0];

            foreach (SkillArray skill in attribute.skills)
            {
                skill.modifiers = new Modifier[0];
            }
        }

        Debug.Log("All modifiers have been removed from attributes, skills, and stats.");
    }

    
}
