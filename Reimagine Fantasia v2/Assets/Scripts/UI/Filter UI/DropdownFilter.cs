using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownFilter : MonoBehaviour
{
    public TMP_Dropdown typeDropdown;
    public TMP_Dropdown abilityTypeDropdown;
    public TMP_Dropdown categoryDropdown;
    public TMP_Dropdown setDropdown;
    public TMP_Dropdown itemTypeDropdown;

    private Dictionary<string, List<string>> categoryToSets = new Dictionary<string, List<string>>()
    {
        { "Sword Styles", new List<string> { "Wilting Sakura", "Blossoming Iris", "Thorned Rose" } },
        { "Marksman", new List<string> { "Shooter", "Set2_2" } },
        { "Category3", new List<string> { "Set3_1", "Set3_2", "Set3_3", "Set3_4" } },
        // Add more categories and sets as needed
    };

    void Start()
    {
        // Add listener for category dropdown changes
        categoryDropdown.onValueChanged.AddListener(OnCategoryChanged);

        // Populate category dropdown with initial values
        PopulateCategoryDropdown();

        // Initialize set dropdown based on the default category
        if (categoryDropdown.options.Count > 0)
        {
            OnCategoryChanged(categoryDropdown.value);
        }

        // Set up listener for the typeDropdown to trigger the method OnTypeDropdownChanged
        typeDropdown.onValueChanged.AddListener(OnTypeDropdownChanged);
        // Initial check to set the correct state of dropdowns
        OnTypeDropdownChanged(typeDropdown.value);
    }

    void PopulateCategoryDropdown()
    {
        List<string> categories = new List<string>(categoryToSets.Keys);
        categoryDropdown.ClearOptions();
        categoryDropdown.AddOptions(categories);
    }

    void OnCategoryChanged(int index)
    {
        string selectedCategory = categoryDropdown.options[index].text;

        // Get the sets for the selected category
        List<string> sets;
        if (categoryToSets.TryGetValue(selectedCategory, out sets))
        {
            // Update set dropdown with the sets for the selected category
            setDropdown.ClearOptions();
            setDropdown.AddOptions(sets);
        }
        else
        {
            // If no sets are found for the selected category, clear the set dropdown
            setDropdown.ClearOptions();
        }
    }

    void OnTypeDropdownChanged(int index)
    {
        string selectedType = typeDropdown.options[index].text.ToLower();

        if (selectedType == "item")
        {
            // Disable ability-related dropdowns
            abilityTypeDropdown.gameObject.SetActive(false);
            categoryDropdown.gameObject.SetActive(false);
            setDropdown.gameObject.SetActive(false);
            // Enable item-related dropdown
            itemTypeDropdown.gameObject.SetActive(true);
        }
        else if (selectedType == "ability")
        {
            // Enable ability-related dropdowns
            abilityTypeDropdown.gameObject.SetActive(true);
            categoryDropdown.gameObject.SetActive(true);
            setDropdown.gameObject.SetActive(true);
            // Disable item-related dropdown
            itemTypeDropdown.gameObject.SetActive(false);
        }
    }
}
