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
    public Toggle proficientToggle;
    public Toggle signatureToggle;
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
    public int profeciency = 1;
    public AttributeArray[] attributes;
    private OtherStat otherStat;
    private ModifiersManager modifiersManager;

    public TMP_Dropdown levelDropdown;

    void Start()
    {
        otherStat = GetComponent<OtherStat>();
        modifiersManager = GetComponent<ModifiersManager>();

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

        levelDropdown.onValueChanged.AddListener(delegate { levelChange();});
        
        UpdateAll();
        levelChange();
    }

    public void CalculateTotalValue(object obj)
    {
        string modifierToText;

        // Check if the object is an Attributes
        if (obj is AttributeArray)
        {
            AttributeArray attribute = (AttributeArray)obj;

            // Calculate sum of modifier values
            int totalModifierValue = attribute.modifiers.Sum(modifier => modifier.value);

            attribute.modifiedValue = attribute.baseValue + totalModifierValue;

            if(totalModifierValue > 0)
            {
                modifierToText = " ( +" + totalModifierValue.ToString("") + " )";
            }
            else if(totalModifierValue == 0)
            {
                modifierToText = "";
            }
            else
            {
                modifierToText = " ( " + totalModifierValue.ToString("") + " )";
            }

            attribute.inputField.text = attribute.baseValue.ToString() + modifierToText;

            foreach (SkillArray skills in attribute.skills)
            {
                skills.baseValue = attribute.modifiedValue;
                CalculateTotalValue(skills);
            }
        }
        // Check if the object is a Skills
        else if (obj is SkillArray)
        {
            SkillArray skill = (SkillArray)obj;

            // Calculate sum of modifier values
            int totalModifierValue = skill.modifiers.Sum(modifier => modifier.value);

            if(totalModifierValue > 0)
            {
                modifierToText = " ( +" + totalModifierValue.ToString("") + " )";
            }
            else if(totalModifierValue == 0)
            {
                modifierToText = "";
            }
            else
            {
                modifierToText = " ( " + totalModifierValue.ToString("") + " )";
            }

            skill.modifiedValue = skill.baseValue + totalModifierValue; 

            skill.modifiedValueText.text = skill.baseValue.ToString() + modifierToText;
        }
        otherStat.CalculateInstinctScore();
    }

    public void UpdateAll()
    {
        foreach(AttributeArray attribute in attributes)
        {
            CalculateTotalValue(attribute);
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

    void OnProficientToggleChanged(SkillArray skill)
    {
        modifiersManager.FindElement(skill.name, "Proficient", 0);

        if (skill.proficientToggle.isOn)
        {
            modifiersManager.FindElement(skill.name, "Proficient", profeciency);

            if(skill.signatureToggle.isOn)
            {
                skill.signatureToggle.isOn = false;
                modifiersManager.FindElement(skill.name, "Signature", 0);
            }
        }
    }

    void OnSignatureToggleChanged(SkillArray skill)
    {
        modifiersManager.FindElement(skill.name, "Signature", 0);

        if (skill.signatureToggle.isOn)
        {
            modifiersManager.FindElement(skill.name, "Signature", profeciency*2);

            if(skill.signatureToggle.isOn)
            {
                skill.proficientToggle.isOn = false;
                modifiersManager.FindElement(skill.name, "Proficient", 0);
            }
        }
    }

    public void levelChange()
    {
        int level = levelDropdown.value + 1 ;

        if(level < 6){ profeciency = 1;}
        else if(level < 10 && level > 5){profeciency = 2;}
        else{profeciency = 3;}

        foreach(AttributeArray attributes in attributes)
        {
            foreach(SkillArray skills in attributes.skills)
            {
                OnProficientToggleChanged(skills);
                OnSignatureToggleChanged(skills);
            }
        }
    }
}
