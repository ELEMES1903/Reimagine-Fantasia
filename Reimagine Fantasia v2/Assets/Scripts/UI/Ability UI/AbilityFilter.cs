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
    public GameObject SetAbilitiesParent;
    public GameObject SkillAbilitiesParent;
    public GameObject unacquiredSkillAbilitiesParent;
    public GameObject TraitsParent;
    public GameObject BackgroundsParent;

    [Header("Other")]
    public List<GameObject> allUnacquiredAbilities = new List<GameObject>();
    public List<GameObject> allAcquiredStuff = new List<GameObject>();
    public List<GameObject> allSkillAbilities = new List<GameObject>();
    public Toggle enableRemoveAbilityToggle;
    public TMP_Text enableToggleText;

    private AbilityOrganizer abilityOrganizer;

    void Start()
    {
        abilityOrganizer = GetComponent<AbilityOrganizer>();

        // Get all abilities from the unacquired abilities parent
        AddChildrenToList(unacquiredAbilitiesParent.transform, allUnacquiredAbilities);
        
        AddChildrenToList(SetAbilitiesParent.transform, allAcquiredStuff);
        AddChildrenToList(TraitsParent.transform, allAcquiredStuff);
        AddChildrenToList(BackgroundsParent.transform, allAcquiredStuff);

        AddChildrenToList(unacquiredSkillAbilitiesParent.transform, allSkillAbilities);

        // Add listeners to the input fields and dropdown to trigger filtering
        nameFilterInputField.onValueChanged.AddListener(OnFilterChanged);
        typeDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });
        categoryDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });
        setDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });

        enableRemoveAbilityToggle.onValueChanged.AddListener(delegate { UpdateRemoveAbilityToggle(enableRemoveAbilityToggle.isOn); });
        UpdateRemoveAbilityToggle(enableRemoveAbilityToggle.isOn);

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
        foreach (GameObject ability in allAcquiredStuff)
        {
            if(ability.gameObject.transform.parent.name == "SetAbilities" || ability.gameObject.transform.parent.name == "Items")
            {
                Button removeButton = ability.GetComponent<Ability>().removeAbilityButton;
                if (removeButton != null)
                {
                    removeButton.gameObject.SetActive(!isOn);
                }
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
                bool matchesType = string.IsNullOrEmpty(typeFilter) || abilityScript.abilityType.ToLower().Equals(typeFilter);
                bool matchesCategory = string.IsNullOrEmpty(categoryFilter) || abilityScript.categoryType.ToLower().Equals(categoryFilter);
                bool matchesSet = string.IsNullOrEmpty(setFilter) || abilityScript.setType.ToLower().Equals(setFilter);

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
        //abilityOrganizer.OrganizeAbilities();
    }

    public List<string> GetAcquiredAbilityNames()
    {
        List<string> acquiredAbilityNames = new List<string>();
        foreach (GameObject ability in allAcquiredStuff)
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
                    abilityScript.CheckIfSkillAbilityEligible(abilityScript.skillAbilitySkill);
                    allAcquiredStuff.Add(ability);
                    allUnacquiredAbilities.Remove(ability);
                    break;
                }
            }
        }
    }

    public void CheckAndUpdateSkillAbilities(string skillName)
    {
        foreach (GameObject skillAbility in allSkillAbilities)
        {
            Ability abilityScript = skillAbility.GetComponent<Ability>();

            abilityScript.CheckIfSkillAbilityEligible(skillName);
        }
    }
}