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
        typeDropdown.onValueChanged.AddListener(delegate { OnTypeChanged(); OnFilterChanged(""); });

        abilityTypeDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });
        categoryDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });
        setDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });

        itemTypeDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });
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

    void FilterAbilities()
    {
        string nameFilter = nameFilterInputField.text.ToLower();
        string typeFilter = typeDropdown.options[typeDropdown.value].text.ToLower();

        string abilityTypeFilter = abilityTypeDropdown.gameObject.activeSelf ? abilityTypeDropdown.options[abilityTypeDropdown.value].text.ToLower() : string.Empty;
        string categoryFilter = categoryDropdown.gameObject.activeSelf ? categoryDropdown.options[categoryDropdown.value].text.ToLower() : string.Empty;
        string setFilter = setDropdown.gameObject.activeSelf ? setDropdown.options[setDropdown.value].text.ToLower() : string.Empty;
        string itemTypeFilter = itemTypeDropdown.gameObject.activeSelf ? itemTypeDropdown.options[itemTypeDropdown.value].text.ToLower() : string.Empty;

        foreach (GameObject ability in allUnacquiredStuff)
        {
            Ability abilityScript = ability.GetComponent<Ability>();
            if (abilityScript != null)
            {
                bool matchesName = string.IsNullOrEmpty(nameFilter) || abilityScript.Name.ToLower().Contains(nameFilter);
                bool matchesType = string.IsNullOrEmpty(typeFilter) || abilityScript.Type.ToLower().Equals(typeFilter);

                bool matchesAbilityType = string.IsNullOrEmpty(abilityTypeFilter) || abilityScript.abilityType.ToLower().Equals(abilityTypeFilter);
                bool matchesCategory = string.IsNullOrEmpty(categoryFilter) || abilityScript.categoryType.ToLower().Equals(categoryFilter);
                bool matchesSet = string.IsNullOrEmpty(setFilter) || abilityScript.setType.ToLower().Equals(setFilter);

                bool matchesItemType = string.IsNullOrEmpty(itemTypeFilter) || abilityScript.itemType.ToLower().Equals(itemTypeFilter);

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

    void OnTypeChanged()
    {
        string selectedType = typeDropdown.options[typeDropdown.value].text.ToLower();

        if (selectedType == "item")
        {
            abilityTypeDropdown.gameObject.SetActive(false);
            categoryDropdown.gameObject.SetActive(false);
            setDropdown.gameObject.SetActive(false);
            itemTypeDropdown.gameObject.SetActive(true);
        }
        else if (selectedType == "ability")
        {
            abilityTypeDropdown.gameObject.SetActive(true);
            categoryDropdown.gameObject.SetActive(true);
            setDropdown.gameObject.SetActive(true);
            itemTypeDropdown.gameObject.SetActive(false);
        }

        FilterAbilities();
    }

    public List<string> GetAcquiredAbilityNames()
    {
        List<string> acquiredAbilityNames = new List<string>();

        if (allAcquiredStuff != null)
        {
            foreach (GameObject ability in allAcquiredStuff)
            {
                if (ability != null)
                {
                    Ability abilityScript = ability.GetComponent<Ability>();
                    if (abilityScript != null && !string.IsNullOrEmpty(abilityScript.Name))
                    {
                        acquiredAbilityNames.Add(abilityScript.Name);
                    }
                }
            }
        }

        return acquiredAbilityNames;
    }

    public void LoadAcquiredAbilities(List<string> acquiredAbilityNames)
    {
        if (acquiredAbilityNames == null || allUnacquiredStuff == null || allAcquiredStuff == null)
        {
            return;
        }

        foreach (string abilityName in acquiredAbilityNames)
        {
            if (!string.IsNullOrEmpty(abilityName))
            {
                foreach (GameObject ability in allUnacquiredStuff)
                {
                    if (ability != null)
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
