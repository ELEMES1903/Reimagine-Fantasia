using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ConditionArray
{
    public string name;
    public int conMaxDuration;
    public int conDuration;
    public string affectingStatName;
    public int modifier;
    public string conEffectInText;
    public bool isActive;
}

public class Conditions : MonoBehaviour
{
    public ConditionArray[] conditions;
    private ModifiersManager modifiersManager;
    private Stress stress;

    public TMP_InputField conInput;
    public Button applyButton;
    public TMP_Text conditionList;

    private List<string> allDropdownOptions; // All options in the dropdown
    public TMP_Dropdown conDropdown;
    public TMP_Dropdown durationDropdown;
    
    // Start is called before the first frame update
    void Start()
    {
        modifiersManager = GetComponent<ModifiersManager>();
        stress = GetComponent<Stress>();

        // Condition Options Dropdown Set Up
        allDropdownOptions = new List<string>();
        List<string> dropdownOptions = new List<string>();

        foreach (ConditionArray condition in conditions)
        {
            dropdownOptions.Add(condition.name);
            allDropdownOptions.Add(condition.name); 
        }
        conDropdown.ClearOptions();
        conDropdown.AddOptions(dropdownOptions);

        // Condition Duration Option Dropdown Set Up
        List<string> durationDropdownOptions = new List<string>();
        durationDropdownOptions.Add("Add");
        durationDropdownOptions.Add("Set to 1 Turn");
        durationDropdownOptions.Add("Set to 2 Turn");
        durationDropdownOptions.Add("Set to 3 Turn");
        durationDropdownOptions.Add("Set to 4 Turn");
        durationDropdownOptions.Add("Remove");
        
        durationDropdown.ClearOptions();
        durationDropdown.AddOptions(durationDropdownOptions);

        // UI Listener Set Up
        conInput.onValueChanged.AddListener(OnInputValueChanged);
        applyButton.onClick.AddListener(AddRemoveCondition);
    }

    // Listener for conInput's onValueChanged event
    void OnInputValueChanged(string newValue)
    {
        // If the input field is empty, show all dropdown options
        if (string.IsNullOrEmpty(newValue))
        {
            conDropdown.ClearOptions();
            conDropdown.AddOptions(allDropdownOptions);
            return;
        }
        // Filter dropdown options based on the input text
        List<string> filteredOptions = new List<string>();

        foreach (string option in allDropdownOptions)
        {
            if (option.ToLower().Contains(newValue.ToLower()))
            {
                filteredOptions.Add(option);
            }
        }
        conDropdown.ClearOptions();
        conDropdown.AddOptions(filteredOptions);
    }   

    void AddRemoveCondition()
    {
        // Get the selected condition name from the dropdown
        string selectedConditionName = conDropdown.options[conDropdown.value].text;

        // Find the condition with the selected name
        ConditionArray con = Array.Find(conditions, condition => condition.name == selectedConditionName);
        
        // If the condition is found, invoke its Execute method using reflection
        if (con != null)
        {
            if(durationDropdown.value == 0){
                
                // If dropdown set to "Normal", set conDuration to conMaxDuration
                con.conDuration = con.conMaxDuration;
                con.isActive = true;

                // Add condition to the designated element to affect it
                modifiersManager.FindElement(con.affectingStatName, con.name, con.modifier);

            } else if(durationDropdown.value == 5){

                // If dropdown set to "Remove", find the condition and remove it from the array
                modifiersManager.FindElement(con.affectingStatName, con.name, 0);
                con.isActive = false;

            } else {

                // If dropdown set to "Set to X turn", set conDuration to X but not above conMaxDuration
                con.conDuration = durationDropdown.value;
                if(con.conDuration > con.conMaxDuration){con.conDuration = con.conMaxDuration;}
            }
            // Update condition list text
            ConditionListUpdate();
        }
        else
        {
            Debug.LogWarning($"Condition {selectedConditionName} not found.");
        }
    }

    public void DecreaseConDuration()
    {
        foreach (ConditionArray condition in conditions)
        {
            if(condition.isActive == true)
            {
                condition.conDuration -= 1;

                if(condition.conDuration == 0)
                {
                    condition.conDuration = condition.conMaxDuration;
                    condition.isActive = false;
                    modifiersManager.FindElement(condition.affectingStatName, condition.name, 0);
                }
            }
            ConditionListUpdate();
        }
    }

    public void ConditionListUpdate()
    {
        conditionList.text = "";
        stress.DisplayCurrentStatus();

        foreach (ConditionArray condition in conditions)
        {
            if(condition.isActive == true)
            {
                conditionList.text += $"\n<b>- <color=#FF6666>{condition.name}</color> for <color=#FF6666>{condition.conDuration}</color> turns.</b>\n     ({condition.conEffectInText})";
            }
        }
    }
}   