using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Ability : MonoBehaviour
{
    public string Name;
    public string Type;

    [Header("Ability Info")]
    public string abilityType;
    public string categoryType;
    public string setType;
    public int tier;

    [Header("Skill Ability Info")]
    public string nameOfSkill;
    public int skillAbilityLevel;

    [Header("Item Info")]
    public string itemType;
    public string weightCost;
    public string goldCost;

    [Header("UI Buttons")]
    public Button removeButton;
    private Button addButton;
    private Button equipButton;
    private TMP_Text equipButtonText;
    public bool isEquipped;

    [Header("Item Description UI")]
    public string itemProperties;
    public string itemDescription;
    private Button itemSelectButton;
    private FilterManager filterManager;
    private AttributeAndSkill attributeAndSkill;
    private ItemDescriptionUI itemDescriptionUI;

    void Start()
    {   
        removeButton = transform.Find("remove Button").GetComponent<Button>();
        addButton = transform.Find("add Button").GetComponent<Button>();
        equipButton = transform.Find("equip Button").GetComponent<Button>();
        itemSelectButton = transform.Find("item select Button").GetComponent<Button>();

        filterManager = FindObjectOfType<FilterManager>();
        attributeAndSkill = FindObjectOfType<AttributeAndSkill>();
        itemDescriptionUI = FindObjectOfType<ItemDescriptionUI>();

        removeButton.onClick.AddListener(RemoveAbility);
        addButton.onClick.AddListener(AddAbility);
        
        if(equipButton != null)
        {
            equipButton.onClick.AddListener(EquipItem);
        }

        if(itemSelectButton != null)
        {
            itemSelectButton.onClick.AddListener(ItemSelected);
        }
        
        if(transform.parent.name != "Unacquired Abilities")
        {
            addButton.gameObject.SetActive(false);
        }
        else
        {
            addButton.gameObject.SetActive(true);
            removeButton.gameObject.SetActive(false);
        }
    }

    // Method to remove the ability from the unacquired list
    void RemoveAbility()
    {
        removeButton.gameObject.SetActive(false);
        addButton.gameObject.SetActive(true);

        transform.SetParent(filterManager.unacquiredStuffParent.transform);
        
        // Add the GameObject to the list
        filterManager.allUnacquiredStuff.Add(gameObject);

        //Remove itself from the ability filter list
        int index = filterManager.allAcquiredStuff.IndexOf(gameObject);
        filterManager.allAcquiredStuff.RemoveAt(index);

    }

    // Method to add the ability to the acquired list based on type
    public void AddAbility()
    {
        if(isEquipped)
        {
            EquipItem();
        }
        else
        {
            addButton.gameObject.SetActive(false);

            var itemTypeToParentMap = new Dictionary<string, Transform>
            {
                { "Set Ability", filterManager.SetAbilitiesParent.transform },
                { "Background", filterManager.BackgroundsParent.transform },
                { "Trait", filterManager.TraitsParent.transform },
                { "Knowledge", filterManager.KnowledgeParent.transform },
                { "Item", filterManager.KnowledgeParent.transform }
            };

            if (itemTypeToParentMap.TryGetValue(itemType, out Transform parentTransform))
            {
                transform.SetParent(parentTransform);
            }
            
            if(filterManager.enableRemoveAbilityToggle.isOn)
            {
                removeButton.gameObject.SetActive(false);
            }
            else
            {
                removeButton.gameObject.SetActive(true);
            }

            // Add the GameObject to the list
            filterManager.allAcquiredStuff.Add(gameObject);

            //Remove itself from the ability filter list
            int index = filterManager.allUnacquiredStuff.IndexOf(gameObject);
            filterManager.allUnacquiredStuff.RemoveAt(index);
        }
    }

    public int FindSkillBaseValue(string skillName)
    {
        foreach (AttributeArray attribute in attributeAndSkill.attributes)
        {
            foreach(SkillArray skill in attribute.skills)
            {
                if(skillName == skill.name)
                {
                    return skill.baseValue;
                }
            }
        }
        return 0;
    }

    public void CheckIfSkillAbilityEligible(string skillName)
    {
        if(abilityType == "Skill Ability" && skillName == nameOfSkill)
        {
            int skillBaseValue = FindSkillBaseValue(skillName);

            if(skillBaseValue >= skillAbilityLevel)
            {
                transform.SetParent(filterManager.SkillAbilitiesParent.transform);
                gameObject.SetActive(true);     
            }
            else
            {
                transform.SetParent(filterManager.unacquiredSkillAbilitiesParent.transform);     
                gameObject.SetActive(false);           
            }
        }
    }

    public void EquipItem()
    {
        // Dictionary to map item types to their corresponding parents and their limits
        var itemTypeToParentMap = new Dictionary<string, (Transform parent, int limit)>
        {
            { "Headwear", (filterManager.headwearParent.transform, 1) },
            { "Bodywear", (filterManager.bodywearParent.transform, 1) },
            { "Handwear", (filterManager.handwearParent.transform, 1) },
            { "Footwear", (filterManager.footwearParent.transform, 1) },
            { "Ring", (filterManager.ringsParent.transform, 4) },
            { "Bracelet", (filterManager.braceletsParent.transform, 2) },
            { "Necklace", (filterManager.necklaceParent.transform, 1) },
        };

        // Check if the item is currently equipped
        if (isEquipped)
        {
            removeButton.gameObject.SetActive(true);

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

                    filterManager.allEquipedItems.Add(gameObject);
                }
                else
                {
                    Debug.Log("Cannot equip item. Parent has reached its child limit.");
                }
            }
        }
        else
        {
            transform.SetParent(filterManager.ItemsParent.transform);
            equipButtonText.text = "Equip";
            isEquipped = true;

            //Remove itself from the ability filter list
            int index = filterManager.allEquipedItems.IndexOf(gameObject);
            filterManager.allEquipedItems.RemoveAt(index);
        }
    }

    public void ItemSelected()
    {
        itemDescriptionUI.Name.text = Name;
        itemDescriptionUI.Properties.text = itemProperties;
        itemDescriptionUI.Description.text = itemDescription;
    }
}