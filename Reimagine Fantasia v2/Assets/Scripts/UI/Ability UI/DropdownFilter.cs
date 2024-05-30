using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownFilter : MonoBehaviour
{
    public TMP_Dropdown categoryDropdown;
    public TMP_Dropdown setDropdown;

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
}
