using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Condition
{
    public string name;
    public int conMaxDuration;
    public int conDuration;
    public string modifierString;
    public Toggle isActivated;
}

public class Conditions : MonoBehaviour
{
    public Condition[] conditions;
    
    public TMP_Dropdown conDropdown;
    
    //public TMP_Dropdown durationDropdown;
    public TMP_InputField conInput;
    public Button addConButton;
    public Button removeConButton;

    public TMP_Text conditionList;
    private List<string> allDropdownOptions; // All options in the dropdown

    // Start is called before the first frame update
    void Start()
    {
        allDropdownOptions = new List<string>();

        // Populate dropdown options with condition names
        List<string> dropdownOptions = new List<string>();
        foreach (Condition condition in conditions)
        {
            dropdownOptions.Add(condition.name);
            allDropdownOptions.Add(condition.name); 
        }
        conDropdown.ClearOptions();
        conDropdown.AddOptions(dropdownOptions);

        conInput.onValueChanged.AddListener(OnInputValueChanged);
        addConButton.onClick.AddListener(AddCondition);
        //removeConButton.onClick.AddListener(RemoveCondition);
    }

    void Update()
    {
        foreach(Condition condition in conditions){

            if(condition.isActivated.isOn){


            }
        }
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
            if (option.ToLower().Contains(newValue.ToLower())) // Case-insensitive check
            {
                filteredOptions.Add(option);
            }
        }
        conDropdown.ClearOptions();
        conDropdown.AddOptions(filteredOptions);
    }

    // Listener for conButton's onClick event
    void AddCondition()
    {
        // Get the selected condition name from the dropdown
        string selectedConditionName = conDropdown.options[conDropdown.value].text;
        //string durationValue = durationDropdown.options[durationDropdown.value].text;

        // Find the condition with the selected name
        Condition selectedCondition = Array.Find(conditions, condition => condition.name == selectedConditionName);

        //int duration = int.Parse(durationValue);
        
        // If the condition is found, invoke its Execute method using reflection
        if (selectedCondition != null)
        {
            selectedCondition.isActivated.isOn = true;
            selectedCondition.conDuration = selectedCondition.conMaxDuration;
            //selectedCondition.conDuration = duration;

            System.Reflection.MethodInfo method = typeof(Conditions).GetMethod(selectedConditionName);
            if (method != null)
            {
                method.Invoke(this, null);
            }
            else
            {
                Debug.LogWarning($"Method {selectedConditionName} not found.");
            }
        }
        else
        {
            Debug.LogWarning($"Condition {selectedConditionName} not found.");
        }
    }

}