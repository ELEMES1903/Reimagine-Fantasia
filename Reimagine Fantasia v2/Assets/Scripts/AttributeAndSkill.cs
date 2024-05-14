using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using System.Collections.Generic;

[System.Serializable]
public class Skills
{
    public string name;
    public int baseValue;
    public int modifiedValue;
    public TMP_Text modifiedValueText;
    public Modifier[] modifiers;
    public Toggle proficientToggle; // Toggle for proficient
    public Toggle signatureToggle; // Toggle for signature
}

[System.Serializable]
public class AttributeStat
{
    public string name;
    public TMP_InputField inputField;
    public int baseValue;
    public int modifiedValue;
    public Modifier[] modifiers;
    public Skills[] skills;

}

public class AttributeAndSkill : MonoBehaviour
{
    public int profeciency = 2;
    public AttributeStat[] attributes;

    void Start()
    {

        // Initialize input field listeners
        foreach (AttributeStat attribute in attributes)
        {
            attribute.inputField.onEndEdit.AddListener(delegate { UpdateBaseValue(attribute); });

            // Add listener for proficientToggle changes
            foreach (Skills skill in attribute.skills)
            {
                skill.proficientToggle.onValueChanged.AddListener((bool value) => OnProficientToggleChanged(skill, value));
                skill.signatureToggle.onValueChanged.AddListener((bool value) => OnSignatureToggleChanged(skill, value));
            }
        }

        UpdateText();
        
    }

    public void AddModifier(string arrayName, string modifierName, int modifierValue, bool isAdding)
    {
        // Search for the attribute or skill array with the same string as the parameter
        foreach (AttributeStat attribute in attributes)
        {
            // Check if the attribute name matches
            if (attribute.name == arrayName)
            {
                attribute.modifiers = AddModifierToArray(attribute.modifiers, modifierName, modifierValue, isAdding);
                CalculateTotalValue(attribute);
                return;
            }

            // Check if any skill array name matches
            foreach (Skills skill in attribute.skills)
            {
                if (skill.name == arrayName)
                {
                    skill.modifiers = AddModifierToArray(skill.modifiers, modifierName, modifierValue, isAdding);
                    CalculateTotalValue(skill);
                    return;
                }
            }
        }

        // Print a message if the attribute or skill with the specified name was not found
        Debug.LogWarning("Attribute or skill with name " + arrayName + " not found.");
    }

    // Helper method to add or remove a modifier to/from an array and return the modified array
    private Modifier[] AddModifierToArray(Modifier[] array, string modifierName, int modifierValue, bool isAdding)
    {
        if (isAdding)
        {
            if (array == null)
            {
                return new Modifier[] { new Modifier { name = modifierName, value = modifierValue } };
            }
            else
            {
                int newSize = array.Length + 1;
                Modifier[] newArray = new Modifier[newSize];
                array.CopyTo(newArray, 0);
                newArray[newSize - 1] = new Modifier { name = modifierName, value = modifierValue };
                return newArray;
            }
        }
        else
        {
            // Remove the modifier with the specified name, if it exists
            List<Modifier> newList = new List<Modifier>(array);
            Modifier modifierToRemove = newList.Find(modifier => modifier.name == modifierName);
            if (modifierToRemove != null)
            {
                newList.Remove(modifierToRemove);
                return newList.ToArray();
            }
            else
            {
                Debug.LogWarning("Modifier with name " + modifierName + " not found.");
                return array; // Return the original array if the modifier to remove was not found
            }
        }        
    }

    public void CalculateTotalValue(object obj)
    {
        // Check if the object is an Attributes
        if (obj is AttributeStat)
        {
            AttributeStat attribute = (AttributeStat)obj;

            // Calculate sum of modifier values
            int totalModifierValue = attribute.modifiers.Sum(modifier => modifier.value);

            // Calculate total value (base value + sum of modifier values)
            int totalValue = attribute.baseValue + totalModifierValue;

            // Set the totalValue to a new property in Attributes
            attribute.modifiedValue = totalValue;

            foreach (Skills skills in attribute.skills){

                skills.baseValue = attribute.modifiedValue;
                CalculateTotalValue(skills);
                skills.modifiedValueText.text = skills.modifiedValue.ToString();
            }
            
        }
        // Check if the object is a Skills
        else if (obj is Skills)
        {
            Skills skill = (Skills)obj;

            // Calculate sum of modifier values
            int totalModifierValue = skill.modifiers.Sum(modifier => modifier.value);

            // Calculate total value (modified value + sum of modifier values)
            int totalValue = skill.baseValue + totalModifierValue;

            skill.modifiedValue = totalValue;  
            skill.modifiedValueText.text = skill.modifiedValue.ToString();
        }

        UpdateText();
    }

    // Method to update base value when input field value changes
    void UpdateBaseValue(AttributeStat attribute)
    {
        if (int.TryParse(attribute.inputField.text, out int newValue))
        {
            attribute.baseValue = newValue;
            CalculateTotalValue(attribute);
        }
    }

    void UpdateText()
    {
        foreach (AttributeStat attribute in attributes){
            attribute.inputField.text = attribute.baseValue.ToString() + " (" + attribute.modifiedValue.ToString() + ")";
        }
    }

    void OnProficientToggleChanged(Skills skill, bool value)
    {
        if (value)
        {
            AddModifier(skill.name, "Proficient", profeciency, true);
        }
        else
        {
            AddModifier(skill.name, "Proficient", profeciency, false);
        }
    }

    void OnSignatureToggleChanged(Skills skill, bool value)
    {
        if (value)
        {
            AddModifier(skill.name, "Signature", profeciency*2, true);
        }
        else
        {
            AddModifier(skill.name, "Signature", profeciency*2, false);
        }
    }
}
