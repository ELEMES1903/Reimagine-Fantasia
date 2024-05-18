using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using System.Collections.Generic;

[System.Serializable]
public class SkillArray
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
public class AttributeArray
{
    public string name;
    public TMP_InputField inputField;
    public int baseValue;
    public int modifiedValue;
    public Modifier[] modifiers;
    public SkillArray[] skills;

}

public class AttributeAndSkill : MonoBehaviour
{
    public int profeciency = 2;
    public AttributeArray[] attributes;
    public OtherStat otherStat;
    public ModifiersManager modifiersManager;

    void Start()
    {
        // Initialize input field listeners
        foreach (AttributeArray attribute in attributes)
        {
            attribute.inputField.characterLimit = 2; // Set character limit to 2
            attribute.inputField.onEndEdit.AddListener(delegate { UpdateBaseValue(attribute); });

            // Add listener for proficientToggle changes
            foreach (SkillArray skill in attribute.skills)
            {
                skill.proficientToggle.onValueChanged.AddListener((bool value) => OnProficientToggleChanged(skill));
                skill.signatureToggle.onValueChanged.AddListener((bool value) => OnSignatureToggleChanged(skill));
            }
        }
        UpdateText();
    }

    public void CalculateTotalValue(object obj)
    {
        // Check if the object is an Attributes
        if (obj is AttributeArray)
        {
            AttributeArray attribute = (AttributeArray)obj;

            // Calculate sum of modifier values
            int totalModifierValue = attribute.modifiers.Sum(modifier => modifier.value);

            // Calculate total value (base value + sum of modifier values)
            int totalValue = attribute.baseValue + totalModifierValue;

            // Set the totalValue to a new property in Attributes
            attribute.modifiedValue = totalValue;

            foreach (SkillArray skills in attribute.skills)
            {
                skills.baseValue = attribute.modifiedValue;
                CalculateTotalValue(skills);
                skills.modifiedValueText.text = skills.modifiedValue.ToString();
            }
            
        }
        // Check if the object is a Skills
        else if (obj is SkillArray)
        {
            SkillArray skill = (SkillArray)obj;

            // Calculate sum of modifier values
            int totalModifierValue = skill.modifiers.Sum(modifier => modifier.value);

            // Calculate total value (modified value + sum of modifier values)
            int totalValue = skill.baseValue + totalModifierValue;

            skill.modifiedValue = totalValue;  
            skill.modifiedValueText.text = skill.modifiedValue.ToString();
        }
        UpdateText();
        otherStat.UpdateInstinctScore();
    }

    public void UpdateAll(){


        foreach(AttributeArray attribute in attributes)
        {
            CalculateTotalValue(attribute);

            foreach(SkillArray skill in attribute.skills)
            {
                CalculateTotalValue(attribute.skills);
            }
        }
    }

    // Method to update base value when input field value changes
    void UpdateBaseValue(AttributeArray attribute)
    {
        if (int.TryParse(attribute.inputField.text, out int newValue))
        {
            attribute.baseValue = newValue;
            CalculateTotalValue(attribute);
        }
    }

    void UpdateText()
    {
        foreach (AttributeArray attribute in attributes)
        {
            attribute.inputField.text = attribute.baseValue.ToString() + " (" + attribute.modifiedValue.ToString() + ")";
        }
    }

    void OnProficientToggleChanged(SkillArray skill)
    {
        if (skill.proficientToggle.isOn)
        {
            modifiersManager.FindElement(skill.name, "Proficient", profeciency);
        }
        else
        {
            modifiersManager.FindElement(skill.name, "Proficient", 0);
        }
    }

    void OnSignatureToggleChanged(SkillArray skill)
    {
        if (skill.signatureToggle.isOn)
        {
            modifiersManager.FindElement(skill.name, "Signature", profeciency*2);
        }
        else
        {
            modifiersManager.FindElement(skill.name, "Signature", 0);
        }
    }
}
