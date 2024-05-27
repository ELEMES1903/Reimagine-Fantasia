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
    public string otherStatName1;
    public string otherStatName2;
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
    public TMP_Text profeciencyValue;

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
            int totalAttributeModifierValue = attribute.modifiers.Sum(modifier => modifier.value);

            attribute.modifiedValue = attribute.baseValue + totalAttributeModifierValue;

            if(totalAttributeModifierValue > 0)
            {
                modifierToText = " ( +" + totalAttributeModifierValue.ToString("") + " )";
            }
            else if(totalAttributeModifierValue == 0)
            {
                modifierToText = "";
            }
            else
            {
                modifierToText = " ( " + totalAttributeModifierValue.ToString("") + " )";
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

            int profeciencyBonus;
            if(skill.proficientToggle.isOn)
            {
                profeciencyBonus = profeciency;
            }
            else if (skill.signatureToggle.isOn)
            {
                profeciencyBonus = profeciency*2;
            }
            else
            {
                profeciencyBonus = 0;
            }

            // Calculate sum of modifier values
            int totalSkillModifierValue =  skill.modifiers.Sum(modifier => modifier.value);

            if(totalSkillModifierValue > 0)
            {
                modifierToText = " ( +" + totalSkillModifierValue.ToString("") + " )";
            }
            else if(totalSkillModifierValue == 0)
            {
                modifierToText = "";
            }
            else
            {
                modifierToText = " ( " + totalSkillModifierValue.ToString("") + " )";
            }

            skill.modifiedValue = skill.baseValue + totalSkillModifierValue; 
            skill.baseValue += profeciencyBonus;
            skill.modifiedValueText.text = skill.baseValue.ToString() + modifierToText;
            skill.baseValue -= profeciencyBonus;
        }

        otherStat.UpdateAll();
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
            otherStat.UpdateAll();
        }
    }

    void OnProficientToggleChanged(SkillArray skill)
    {
        if (skill.proficientToggle.isOn)
        {
            if(skill.signatureToggle.isOn)
            {
                skill.signatureToggle.isOn = false;
            }
        }
        CalculateTotalValue(skill);
    }

    void OnSignatureToggleChanged(SkillArray skill)
    {
        if (skill.signatureToggle.isOn)
        {
            if(skill.signatureToggle.isOn)
            {
                skill.proficientToggle.isOn = false;
            }
        }
        CalculateTotalValue(skill);
    }

    public void levelChange()
    {
        int level = levelDropdown.value + 1 ;

        if(level < 6){ profeciency = 1;}
        else if(level < 10 && level > 5){profeciency = 2;}
        else{profeciency = 3;}

        profeciencyValue.text = "Profeciency Bonus (+" + profeciency.ToString() + ")";
    }
}
