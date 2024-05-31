using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemFilter : MonoBehaviour
{
    [Header("Filter Options UI Components")]
    public TMP_InputField nameFilterInputField;
    public TMP_Dropdown typeDropdown;

    [Header("Parent GameObjects")]
    public GameObject unacquiredItemsParent;
    public GameObject filteredItemsParent;
    public GameObject ItemsParent;
    public GameObject ringsParent;
    public GameObject braceletsParent;
    public GameObject necklaceParent;

    public GameObject headwearParent;
    public GameObject bodywearParent;
    public GameObject handwearParent;
    public GameObject footwearParent;

    [Header("Other")]
    public List<GameObject> allUnacquiredItems = new List<GameObject>();
    public List<GameObject> allAcquiredItems = new List<GameObject>();
    public List<GameObject> allEquipedItems = new List<GameObject>();

    void Start()
    {
        nameFilterInputField.onValueChanged.AddListener(OnFilterChanged);
        typeDropdown.onValueChanged.AddListener(delegate { OnFilterChanged(""); });

        AddChildrenToList(unacquiredItemsParent.transform, allUnacquiredItems);

        AddChildrenToList(headwearParent.transform, allEquipedItems);
        AddChildrenToList(bodywearParent.transform, allEquipedItems);
        AddChildrenToList(handwearParent.transform, allEquipedItems);
        AddChildrenToList(footwearParent.transform, allEquipedItems);

        AddChildrenToList(ringsParent.transform, allEquipedItems);
        AddChildrenToList(braceletsParent.transform, allEquipedItems);
        AddChildrenToList(necklaceParent.transform, allEquipedItems);

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
        FilterItems();
    }

    void UpdateEquipedItems()
    {
        foreach (GameObject item in allEquipedItems)
        {
            
        }
    }

    void FilterItems()
    {
        string nameFilter = nameFilterInputField.text.ToLower();
        string typeFilter = typeDropdown.options[typeDropdown.value].text.ToLower();

        foreach (GameObject item in allUnacquiredItems)
        {
            Item itemScript = item.GetComponent<Item>();
            if (itemScript != null)
            {
                bool matchesName = string.IsNullOrEmpty(nameFilter) || itemScript.itemName.ToLower().Contains(nameFilter);
                bool matchesType = string.IsNullOrEmpty(typeFilter) || itemScript.itemType.ToLower().Equals(typeFilter);

                if (matchesName && matchesType)
                {
                    // If the ability matches all filters, move it to the unacquired abilities parent and activate it
                    item.transform.SetParent(unacquiredItemsParent.transform);
                    item.SetActive(true);
                }
                else
                {
                    // If the ability does not match the filters, move it to the filtered upgrades parent and deactivate it
                    item.transform.SetParent(filteredItemsParent.transform);
                    item.SetActive(false);
                }
            }
        }
    }
}
