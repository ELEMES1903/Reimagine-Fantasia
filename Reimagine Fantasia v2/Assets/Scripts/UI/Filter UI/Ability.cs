using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Ability : MonoBehaviour
{
    public string Name;
    public string Type;
    public bool equipabble;
    public bool isEquipped;
    public  Button selectButton;

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

    [Header("Item Description UI")]
    public string itemProperties;
    public string itemDescription;
    private Button itemSelectButton;
    private FilterManager filterManager;
    private AttributeAndSkill attributeAndSkill;
    private ItemDescriptionUI itemDescriptionUI;

    void Start()
    {   
        selectButton = GetComponentInChildren<Button>();
        selectButton.onClick.AddListener(delegate {ItemSelected();});

        filterManager = FindObjectOfType<FilterManager>();
        attributeAndSkill = FindObjectOfType<AttributeAndSkill>();
        itemDescriptionUI = FindObjectOfType<ItemDescriptionUI>();

        if(itemSelectButton != null)
        {
            itemSelectButton.onClick.AddListener(ItemSelected);
        }
    }

    // Method to remove the ability from the unacquired list
    public void RemoveAbility()
    {
        transform.SetParent(filterManager.unacquiredStuffParent.transform);
        
        // Add the GameObject to the list
        filterManager.allUnacquiredStuff.Add(gameObject);

        //Remove itself from the ability filter list
        int index = filterManager.allAcquiredStuff.IndexOf(gameObject);
        filterManager.allAcquiredStuff.RemoveAt(index);

        itemDescriptionUI.UpdateWhenRemoved();
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
            if(Type == "Item")
            {
                transform.SetParent(filterManager.ItemsParent.transform);
            }
            else
            {
                var itemTypeToParentMap = new Dictionary<string, Transform>
                {
                    { "Set Ability", filterManager.SetAbilitiesParent.transform },
                    { "Background", filterManager.BackgroundsParent.transform },
                    { "Trait", filterManager.TraitsParent.transform },
                    { "Knowledge", filterManager.KnowledgeParent.transform },
                };

                if (itemTypeToParentMap.TryGetValue(abilityType, out Transform parentTransform))
                {
                    transform.SetParent(parentTransform);
                }
            }
            itemDescriptionUI.UpdateWhenAdded();

            filterManager.allAcquiredStuff.Add(gameObject);

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

        // Check if the item is currently unequipped
        if (!isEquipped)
        {
            // Set the parent based on the item type
            if (itemTypeToParentMap.TryGetValue(itemType, out var parentInfo))
            {
                Transform parentTransform = parentInfo.parent;
                int limit = parentInfo.limit;

                // Check if the parent has reached its child limit
                if (parentTransform.childCount < limit)
                {
                    transform.SetParent(parentTransform);
                    isEquipped = true;
                    itemDescriptionUI.UpdateWhenEquip();
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
            isEquipped = false;
            itemDescriptionUI.UpdateWhenUnequip();

            //Remove itself from the ability filter list
            int index = filterManager.allEquipedItems.IndexOf(gameObject);

            if(filterManager.allEquipedItems.Contains(gameObject))
            {
                filterManager.allEquipedItems.RemoveAt(index);
            }
        }
    }

    public void ItemSelected()
    {
        itemDescriptionUI.currentObject = gameObject;
        itemDescriptionUI.abilityScript = itemDescriptionUI.currentObject.GetComponent<Ability>();
        itemDescriptionUI.GlobalUpdate();
        
        itemDescriptionUI.Name.text = Name;
        itemDescriptionUI.Properties.text = itemProperties;
        itemDescriptionUI.Description.text = itemDescription;
    }
}