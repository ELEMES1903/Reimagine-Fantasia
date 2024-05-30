using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName;
    public string itemType;
    public string itemCost;
    public int weightCost;

    [Header("UI Buttons")]
    public Button removeItemButton;
    private Button addItemButton;
    private Button equipButton;
    private TMP_Text equipButtonText;
    public bool isEquipped;

    private ItemFilter itemFilter;
    private AbilityFilter abilityFilter;

    void Start()
    {
        removeItemButton = transform.Find("remove Button").GetComponent<Button>();
        addItemButton = transform.Find("add Button").GetComponent<Button>();
        equipButton = transform.Find("equip Button").GetComponent<Button>();

        // Assuming the TMP_Text is a child of the equip button
        equipButtonText = equipButton.GetComponentInChildren<TMP_Text>();

        itemFilter = FindObjectOfType<ItemFilter>(); 

        removeItemButton.onClick.AddListener(RemoveItem);
        addItemButton.onClick.AddListener(AddItem);
        equipButton.onClick.AddListener(EquipItem);
    }

    void RemoveItem()
    {
        removeItemButton.gameObject.SetActive(false);
        addItemButton.gameObject.SetActive(true);

        transform.SetParent(itemFilter.unacquiredItemsParent.transform);
        
        // Add the GameObject to the list
        abilityFilter.allUnacquiredAbilities.Add(gameObject);

        //Remove itself from the ability filter list
        abilityFilter.allAcquiredStuff.Remove(gameObject);
    }

    public void AddItem()
    {
        addItemButton.gameObject.SetActive(false);
        transform.SetParent(itemFilter.ItemsParent.transform);

        if (abilityFilter.enableRemoveAbilityToggle.isOn)
        {
            removeItemButton.gameObject.SetActive(false);
        }
        else
        {
            removeItemButton.gameObject.SetActive(true);
        }

        // Add the GameObject to the list
        abilityFilter.allAcquiredStuff.Add(gameObject);

        //Remove itself from the ability filter list
        abilityFilter.allUnacquiredAbilities.Remove(gameObject);
    }

    public void EquipItem()
    {
        // Dictionary to map item types to their corresponding parents and their limits
        var itemTypeToParentMap = new Dictionary<string, (Transform parent, int limit)>
        {
            //{ "Headwear", (itemFilter.headwearParent.transform, 5) }, // Example with limit 5
            { "Bodywear", (itemFilter.bodywearParent.transform, 1) },
            //{ "Handwear", (itemFilter.handwearParent.transform, 5) },
            //{ "Footwear", (itemFilter.footwearParent.transform, 5) },
            //{ "Ring", (itemFilter.ringsParent.transform, 5) },
            //{ "Bracelet", (itemFilter.braceletsParent.transform, 5) },
            //{ "Necklace", (itemFilter.necklaceParent.transform, 5) },
        };

        // Check if the item is currently equipped
        if (isEquipped)
        {
            removeItemButton.gameObject.SetActive(true);

            // Set the parent based on the item type
            if (itemTypeToParentMap.TryGetValue(itemType, out var parentInfo))
            {
                Transform parentTransform = parentInfo.parent;
                int limit = parentInfo.limit;

                // Check if the parent has reached its child limit
                if (parentTransform.childCount < limit)
                {
                    transform.SetParent(parentTransform);
                    equipButtonText.text = "Unequip";
                    isEquipped = false;
                }
                else
                {
                    // Optionally show a message or visual feedback that the parent is full
                    Debug.Log("Cannot equip item. Parent has reached its child limit.");
                }
            }
        }
        else
        {
            // Set the parent to ItemsParent and mark as equipped
            transform.SetParent(itemFilter.ItemsParent.transform);
            equipButtonText.text = "Equip";
            isEquipped = true;
        }
    }
}
