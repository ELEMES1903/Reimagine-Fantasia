using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilterManager : MonoBehaviour
{
    [Header("Filter Options UI Components")]
    public TMP_InputField nameFilterInputField;
    public TMP_Dropdown typeDropdown;
    public TMP_Dropdown itemTypeDropdown;
    public TMP_Dropdown abilityTypeDropdown;
    public TMP_Dropdown categoryDropdown;
    public TMP_Dropdown setDropdown;

    [Header("Stuff Parent GameObjects")]
    public GameObject unacquiredStuffParent;
    public GameObject filteredStuffParent;

    [Header("Ability Parent GameObjects")]
    public GameObject SetAbilitiesParent;
    public GameObject SkillAbilitiesParent;
    public GameObject unacquiredSkillAbilitiesParent;
    public GameObject TraitsParent;
    public GameObject BackgroundsParent;
    public GameObject KnowledgeParent;

    [Header("Item Parent GameObjects")]
    public GameObject ItemsParent;
    public GameObject ringsParent;
    public GameObject braceletsParent;
    public GameObject necklaceParent;

    public GameObject headwearParent;
    public GameObject bodywearParent;
    public GameObject handwearParent;
    public GameObject footwearParent;

    [Header("Lists")]
    public List<GameObject> allUnacquiredStuff = new List<GameObject>();
    public List<GameObject> allAcquiredStuff = new List<GameObject>();
    public List<GameObject> allSkillAbilities = new List<GameObject>();
    public List<GameObject> allEquipedItems = new List<GameObject>();

    public Toggle enableRemoveAbilityToggle;
    public TMP_Text enableToggleText;

    void Start()
    {
        AddChildrenToList(unacquiredStuffParent.transform, allUnacquiredStuff);

        AddChildrenToList(SetAbilitiesParent.transform, allAcquiredStuff);
        AddChildrenToList(TraitsParent.transform, allAcquiredStuff);
        AddChildrenToList(BackgroundsParent.transform, allAcquiredStuff);

        AddChildrenToList(unacquiredSkillAbilitiesParent.transform, allSkillAbilities);

        AddChildrenToList(headwearParent.transform, allEquipedItems);
        AddChildrenToList(bodywearParent.transform, allEquipedItems);
        AddChildrenToList(handwearParent.transform, allEquipedItems);
        AddChildrenToList(footwearParent.transform, allEquipedItems);
        AddChildrenToList(ringsParent.transform, allEquipedItems);
        AddChildrenToList(braceletsParent.transform, allEquipedItems);
        AddChildrenToList(necklaceParent.transform, allEquipedItems);

        // Add listeners to the input fields and dropdown to trigger filtering
        nameFilterInputField.onValueChanged.AddListener(OnFilterChanged);
        typeDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });

        abilityTypeDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });
        categoryDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });
        setDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });

        itemTypeDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });

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
            if(ability.gameObject.transform.parent.name != "Unacquired Stuff" && allEquipedItems.Contains(ability) == false)
            {
                Button abilityRemoveButton = ability.GetComponent<Ability>().removeButton;
                if(abilityRemoveButton != null)
                {
                    abilityRemoveButton.gameObject.SetActive(!isOn);
                }
                
            }
        }
        enableToggleText.text = isOn ? "Enable Ability Remove Option" : "Disable Ability Remove Option";
    }

    void FilterAbilities()
    {
        string nameFilter = nameFilterInputField.text.ToLower();
        string typeFilter = typeDropdown.options[abilityTypeDropdown.value].text.ToLower();

        string abilityTypeFilter = abilityTypeDropdown.options[abilityTypeDropdown.value].text.ToLower();
        string categoryFilter = categoryDropdown.options[categoryDropdown.value].text.ToLower();
        string setFilter = setDropdown.options[setDropdown.value].text.ToLower();

        string itemTypeFilter = itemTypeDropdown.options[itemTypeDropdown.value].text.ToLower();

        foreach (GameObject ability in allUnacquiredStuff)
        {
            Ability abilityScript = ability.GetComponent<Ability>();
            if (abilityScript != null)
            {
                bool matchesName = string.IsNullOrEmpty(nameFilter) || abilityScript.Name.ToLower().Contains(nameFilter);
                bool matchesType = string.IsNullOrEmpty(typeFilter) || abilityScript.Type.ToLower().Equals(typeFilter);

                bool matchesAbilityType = !abilityTypeDropdown.gameObject.activeSelf || string.IsNullOrEmpty(abilityTypeFilter) || abilityScript.abilityType.ToLower().Equals(abilityTypeFilter);
                bool matchesCategory = !categoryDropdown.gameObject.activeSelf || string.IsNullOrEmpty(categoryFilter) || abilityScript.categoryType.ToLower().Equals(categoryFilter);
                bool matchesSet = !setDropdown.gameObject.activeSelf || string.IsNullOrEmpty(setFilter) || abilityScript.setType.ToLower().Equals(setFilter);

                bool matchesItemType = !itemTypeDropdown.gameObject.activeSelf || string.IsNullOrEmpty(itemTypeFilter) || abilityScript.itemType.ToLower().Equals(itemTypeFilter);

                if (matchesName && matchesType && matchesAbilityType && matchesCategory && matchesSet && matchesItemType)
                {
                    // If the ability matches all filters, move it to the unacquired abilities parent and activate it
                    ability.transform.SetParent(unacquiredStuffParent.transform);
                    ability.SetActive(true);
                }
                else
                {
                    // If the ability does not match the filters, move it to the filtered upgrades parent and deactivate it
                    ability.transform.SetParent(filteredStuffParent.transform);
                    ability.SetActive(false);
                }
            }
        }
    }

    public List<string> GetAcquiredAbilityNames()
    {
        List<string> acquiredAbilityNames = new List<string>();
        foreach (GameObject ability in allAcquiredStuff)
        {
            Ability abilityScript = ability.GetComponent<Ability>();
            if (abilityScript != null)
            {
                acquiredAbilityNames.Add(abilityScript.Name);
            }
        }
        return acquiredAbilityNames;
    }

    public void LoadAcquiredAbilities(List<string> acquiredAbilityNames)
    {
        foreach (string abilityName in acquiredAbilityNames)
        {
            foreach (GameObject ability in allUnacquiredStuff)
            {
                Ability abilityScript = ability.GetComponent<Ability>();
                if (abilityScript != null && abilityScript.Name == abilityName)
                {
                    abilityScript.AddAbility();
                    abilityScript.CheckIfSkillAbilityEligible(abilityScript.nameOfSkill);
                    allAcquiredStuff.Add(ability);
                    allUnacquiredStuff.Remove(ability);
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