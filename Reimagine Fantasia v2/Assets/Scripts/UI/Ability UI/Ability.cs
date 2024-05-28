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

    [Header("UI Buttons")]
    public Button removeAbilityButton;
    private Button addAbilityButton;

    // Reference to the AbilityFilter script
    private AbilityFilter abilityFilter;

    // Set up references and event listeners
    void Start()
    {   
        removeAbilityButton = transform.Find("remove Button").GetComponent<Button>();
        addAbilityButton = transform.Find("add Button").GetComponent<Button>();
        abilityFilter = FindObjectOfType<AbilityFilter>();

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
        int index = abilityFilter.allAcquiredAbilities.IndexOf(gameObject);
        abilityFilter.allAcquiredAbilities.RemoveAt(index);

    }

    // Method to add the ability to the acquired list based on type
    public void AddAbility()
    {
        addAbilityButton.gameObject.SetActive(false);

        if (abilityType == "Trait")
        {
            transform.SetParent(abilityFilter.acquiredTraitsParent.transform);
        }
        else if (abilityType == "Background")
        {
            transform.SetParent(abilityFilter.acquiredBackgroundsParent.transform);
        }
        else 
        { 
            transform.SetParent(abilityFilter.acquiredSetAbilitiesParent.transform);
        }

        // Add the GameObject to the list
        abilityFilter.allAcquiredAbilities.Add(gameObject);

        //Remove itself from the ability filter list
        int index = abilityFilter.allUnacquiredAbilities.IndexOf(gameObject);
        abilityFilter.allUnacquiredAbilities.RemoveAt(index);
    }
}