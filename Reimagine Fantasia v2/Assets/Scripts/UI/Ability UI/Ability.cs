using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    [Header("Ability Info")]
    public string abilityName;
    public string abilityType;
    public string categoryType;
    public string setType;
    public int tier;

    [Header("Skill Ability Info")]

    public string skillAbilitySkill;
    public int skillAbilityLevel;

    [Header("UI Buttons")]
    public Button removeAbilityButton;
    private Button addAbilityButton;

    // Reference to the AbilityFilter script
    private AbilityFilter abilityFilter;
    private AttributeAndSkill attributeAndSkill;

    // Set up references and event listeners
    void Start()
    {   
        removeAbilityButton = transform.Find("remove Button").GetComponent<Button>();
        addAbilityButton = transform.Find("add Button").GetComponent<Button>();
        abilityFilter = FindObjectOfType<AbilityFilter>();
        attributeAndSkill = FindObjectOfType<AttributeAndSkill>();

        removeAbilityButton.onClick.AddListener(RemoveAbility);
        addAbilityButton.onClick.AddListener(AddAbility);

        if(transform.parent.name != "Unacquired Abilities")
        {
            addAbilityButton.gameObject.SetActive(false);
        }
        else
        {
            addAbilityButton.gameObject.SetActive(true);
            removeAbilityButton.gameObject.SetActive(false);
        }
    }

    // Method to remove the ability from the unacquired list
    void RemoveAbility()
    {
        removeAbilityButton.gameObject.SetActive(false);
        addAbilityButton.gameObject.SetActive(true);

        transform.SetParent(abilityFilter.unacquiredAbilitiesParent.transform);
        
        // Add the GameObject to the list
        abilityFilter.allUnacquiredAbilities.Add(gameObject);

        //Remove itself from the ability filter list
        int index = abilityFilter.allAcquiredStuff.IndexOf(gameObject);
        abilityFilter.allAcquiredStuff.RemoveAt(index);

    }

    // Method to add the ability to the acquired list based on type
    public void AddAbility()
    {
        addAbilityButton.gameObject.SetActive(false);

        if (abilityType == "Trait")
        {
            transform.SetParent(abilityFilter.TraitsParent.transform);
        }
        else if (abilityType == "Background")
        {
            transform.SetParent(abilityFilter.BackgroundsParent.transform);
        }
        else 
        { 
            transform.SetParent(abilityFilter.SetAbilitiesParent.transform);
        }
        
        if(abilityFilter.enableRemoveAbilityToggle.isOn)
        {
            removeAbilityButton.gameObject.SetActive(false);
        }
        else
        {
            removeAbilityButton.gameObject.SetActive(true);
        }

        // Add the GameObject to the list
        abilityFilter.allAcquiredStuff.Add(gameObject);

        //Remove itself from the ability filter list
        int index = abilityFilter.allUnacquiredAbilities.IndexOf(gameObject);
        abilityFilter.allUnacquiredAbilities.RemoveAt(index);
        
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
        if(abilityType == "Skill Ability" && skillName == skillAbilitySkill)
        {
            int skillBaseValue = FindSkillBaseValue(skillName);

            if(skillBaseValue >= skillAbilityLevel)
            {
                transform.SetParent(abilityFilter.SkillAbilitiesParent.transform);
                gameObject.SetActive(true);     
            }
            else
            {
                transform.SetParent(abilityFilter.unacquiredSkillAbilitiesParent.transform);     
                gameObject.SetActive(false);           
            }
        }
    }
}