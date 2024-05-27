using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityFilter : MonoBehaviour
{
    public TMP_InputField nameFilterInputField;      // Reference to the name filter TMP_InputField
    public TMP_InputField typeFilterInputField;      // Reference to the type filter TMP_InputField
    public Dropdown categoryDropdown;
    public Dropdown setDropdown;                      // Reference to the type filter Dropdown
    public GameObject scrollViewContent;             // The GameObject that holds the upgrades in the ScrollView
    public GameObject filteredUpgradesParent;        // The GameObject where filtered-out upgrades will be moved to

    private List<GameObject> allUpgrades;            // List to hold all the upgrades for easy access

    void Start()
    {
        allUpgrades = new List<GameObject>();

        // Get all upgrades from the ScrollView content
        foreach (Transform child in scrollViewContent.transform)
        {
            allUpgrades.Add(child.gameObject);
        }

        // Add listeners to the input fields and dropdown to trigger filtering
        nameFilterInputField.onValueChanged.AddListener(OnFilterChanged);
        typeFilterInputField.onValueChanged.AddListener(OnFilterChanged);
        categoryDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });
        setDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });
    }

    // Method called when the input field value or dropdown selection changes
    void OnFilterChanged(string filterText)
    {
        FilterUpgrades();
    }

    void FilterUpgrades()
    {
        string nameFilter = nameFilterInputField.text.ToLower();
        string typeFilter = typeFilterInputField.text.ToLower();
        string categoryFilter = categoryDropdown.options[categoryDropdown.value].text.ToLower();
        string setFilter = setDropdown.options[setDropdown.value].text.ToLower();

        foreach (GameObject upgrade in allUpgrades)
        {
            Upgrade upgradeScript = upgrade.GetComponent<Upgrade>();
            if (upgradeScript != null)
            {
                bool matchesName = string.IsNullOrEmpty(nameFilter) || upgradeScript.abilityName.ToLower().Contains(nameFilter);
                bool matchesType = string.IsNullOrEmpty(typeFilter) || upgradeScript.upgradeType.ToLower().Contains(typeFilter);
                bool matchesCategory = categoryFilter == "all" || categoryFilter == "" || upgradeScript.categoryType.ToLower().Equals(categoryFilter);
                bool matchesSet = setFilter == "all" || setFilter == "" || upgradeScript.setType.ToLower().Equals(setFilter);

                if (matchesName && matchesType && matchesCategory && matchesSet)
                {
                    // If the upgrade matches all filters, move it to the ScrollView content and activate it
                    upgrade.transform.SetParent(scrollViewContent.transform);
                    upgrade.SetActive(true);
                }
                else
                {
                    // If the upgrade does not match the filters, move it to the filtered upgrades parent and deactivate it
                    upgrade.transform.SetParent(filteredUpgradesParent.transform);
                    upgrade.SetActive(false);
                }
            }
        }
    }
}
