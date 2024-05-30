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

        equipButtonText = GetComponentInChildren<TMP_Text>();

        itemFilter = GetComponent<ItemFilter>(); 
        abilityFilter = GetComponent<AbilityFilter>();

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
        int index = abilityFilter.allAcquiredStuff.IndexOf(gameObject);
        abilityFilter.allAcquiredStuff.RemoveAt(index);

    }

    // Method to add the ability to the acquired list based on type
    public void AddItem()
    {
        addItemButton.gameObject.SetActive(false);

        transform.SetParent(itemFilter.ItemsParent.transform);

        if(abilityFilter.enableRemoveAbilityToggle.isOn)
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
        int index = abilityFilter.allUnacquiredAbilities.IndexOf(gameObject);
        abilityFilter.allUnacquiredAbilities.RemoveAt(index);
        
    }

    public void EquipItem()
    {
        // Dictionary to map item types to their corresponding parents
        var itemTypeToParentMap = new Dictionary<string, Transform>
        {
            { "Headwear", itemFilter.headwearParent.transform },
            { "Bodywear", itemFilter.bodywearParent.transform },
            { "Handwear", itemFilter.handwearParent.transform },
            { "Footwear", itemFilter.footwearParent.transform },
            { "Ring", itemFilter.ringsParent.transform },
            { "Bracelet", itemFilter.braceletsParent.transform },
            { "Necklace", itemFilter.necklaceParent.transform },
        };

        // Check if the item is currently equipped
        if (isEquipped)
        {
            removeItemButton.gameObject.SetActive(true);

            // Set the parent based on the item type
            if (itemTypeToParentMap.TryGetValue(itemType, out Transform parentTransform))
            {
                transform.SetParent(parentTransform);
            }
            
            equipButtonText.text = "Unequip";
            isEquipped = false;    
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
