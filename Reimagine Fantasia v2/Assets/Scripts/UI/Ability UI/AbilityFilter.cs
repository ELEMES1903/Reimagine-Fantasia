using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityFilter : MonoBehaviour
{
    [Header("Filter Options UI Components")]
    public TMP_InputField nameFilterInputField;
    public TMP_Dropdown typeDropdown;
    public TMP_Dropdown categoryDropdown;
    public TMP_Dropdown setDropdown;

    [Header("Parent GameObjects")]
    public GameObject unacquiredAbilitiesParent;
    public GameObject filteredAbilitiesParent;
    public GameObject acquiredSetAbilitiesParent;
    public GameObject acquiredSkillAbilitiesParent;
    public GameObject acquiredTraitsParent;
    public GameObject acquiredBackgroundsParent;

    [Header("Other")]
    public List<GameObject> allUnacquiredAbilities = new List<GameObject>();
    public List<GameObject> allAcquiredAbilities = new List<GameObject>();
    public Toggle enableRemoveAbilityToggle;
    public TMP_Text enableToggleText;

    private AbilityOrganizer abilityOrganizer;

    void Start()
    {
        abilityOrganizer = GetComponent<AbilityOrganizer>();

        // Get all abilities from the unacquired abilities parent
        AddChildrenToList(unacquiredAbilitiesParent.transform, allUnacquiredAbilities);
        
        AddChildrenToList(acquiredSetAbilitiesParent.transform, allAcquiredAbilities);
        AddChildrenToList(acquiredTraitsParent.transform, allAcquiredAbilities);
        AddChildrenToList(acquiredBackgroundsParent.transform, allAcquiredAbilities);

        // Add listeners to the input fields and dropdown to trigger filtering
        nameFilterInputField.onValueChanged.AddListener(OnFilterChanged);
        typeDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });
        categoryDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });
        setDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });

        if (allAcquiredAbilities.Count > 0)
        {
            enableRemoveAbilityToggle.onValueChanged.AddListener(delegate { UpdateRemoveAbilityToggle(enableRemoveAbilityToggle.isOn); });
            UpdateRemoveAbilityToggle(enableRemoveAbilityToggle.isOn);
        }
        else
        {
            Debug.LogWarning("No child abilities found under allAcquiredAbilitiesParent.");
        }
    }

    void AddChildrenToList(Transform parent, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            list.Add(child.gameObject);
        }
    }

    void OnFilterChanged(string filterText)
    {
        FilterAbilities();
    }

    void UpdateRemoveAbilityToggle(bool isOn)
    {
        foreach (GameObject ability in allAcquiredAbilities)
        {
            Button removeButton = ability.GetComponent<Ability>().removeAbilityButton;
            if (removeButton != null)
            {
                removeButton.gameObject.SetActive(!isOn);
            }
        }
        enableToggleText.text = isOn ? "Enable Ability Remove Option" : "Disable Ability Remove Option";
    }

    void FilterAbilities()
    {
        string nameFilter = nameFilterInputField.text.ToLower();
        string typeFilter = typeDropdown.options[typeDropdown.value].text.ToLower();
        string categoryFilter = categoryDropdown.options[categoryDropdown.value].text.ToLower();
        string setFilter = setDropdown.options[setDropdown.value].text.ToLower();

        foreach (GameObject ability in allUnacquiredAbilities)
        {
            Ability abilityScript = ability.GetComponent<Ability>();
            if (abilityScript != null)
            {
                bool matchesName = string.IsNullOrEmpty(nameFilter) || abilityScript.abilityName.ToLower().Contains(nameFilter);
                bool matchesType = typeFilter == "All Types" || typeFilter == "" || abilityScript.abilityType.ToLower().Equals(typeFilter);
                bool matchesCategory = categoryFilter == "All Categories" || categoryFilter == "" || abilityScript.categoryType.ToLower().Equals(categoryFilter);
                bool matchesSet = setFilter == "All Sets" || setFilter == "" || abilityScript.setType.ToLower().Equals(setFilter);

                if (matchesName && matchesType && matchesCategory && matchesSet)
                {
                    // If the ability matches all filters, move it to the unacquired abilities parent and activate it
                    ability.transform.SetParent(unacquiredAbilitiesParent.transform);
                    ability.SetActive(true);
                }
                else
                {
                    // If the ability does not match the filters, move it to the filtered upgrades parent and deactivate it
                    ability.transform.SetParent(filteredAbilitiesParent.transform);
                    ability.SetActive(false);
                }
            }
        }
        abilityOrganizer.OrganizeAbilities();
    }

    public List<string> GetAcquiredAbilityNames()
    {
        List<string> acquiredAbilityNames = new List<string>();
        foreach (GameObject ability in allAcquiredAbilities)
        {
            Ability abilityScript = ability.GetComponent<Ability>();
            if (abilityScript != null)
            {
                acquiredAbilityNames.Add(abilityScript.abilityName);
            }
        }
        return acquiredAbilityNames;
    }

    public void LoadAcquiredAbilities(List<string> acquiredAbilityNames)
    {
        foreach (string abilityName in acquiredAbilityNames)
        {
            foreach (GameObject ability in allUnacquiredAbilities)
            {
                Ability abilityScript = ability.GetComponent<Ability>();
                if (abilityScript != null && abilityScript.abilityName == abilityName)
                {
                    abilityScript.AddAbility();
                    allAcquiredAbilities.Add(ability);
                    allUnacquiredAbilities.Remove(ability);
                    break;
                }
            }
        }
    }
}