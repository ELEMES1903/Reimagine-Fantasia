using System.Collections.Generic;
using UnityEngine;

public class AbilityOrganizer : MonoBehaviour
{
    public GameObject abilitiesParent;  // Parent GameObject containing the Ability GameObjects

    public void OrganizeAbilities()
    {
        // Get all Ability GameObjects under the abilitiesParent
        Ability[] abilities = abilitiesParent.GetComponentsInChildren<Ability>();

        // Sort the abilities
        List<Ability> sortedAbilities = new List<Ability>(abilities);
        sortedAbilities.Sort((a, b) =>
        {
            int categoryTypeComparison = a.categoryType.CompareTo(b.categoryType);
            if (categoryTypeComparison != 0)
                return categoryTypeComparison;

            int setTypeComparison = a.setType.CompareTo(b.setType);
            if (setTypeComparison != 0)
                return setTypeComparison;

            int tierComparison = a.tier.CompareTo(b.tier);
            if (tierComparison != 0)
                return tierComparison;

            return string.Compare(a.abilityName, b.abilityName);
        });

        // Reparent the sorted abilities in the hierarchy
        foreach (Ability ability in sortedAbilities)
        {
            ability.transform.SetParent(null); // Unparent temporarily to avoid hierarchy issues
        }

        foreach (Ability ability in sortedAbilities)
        {
            ability.transform.SetParent(abilitiesParent.transform);
        }
    }

    void Start()
    {
        OrganizeAbilities();
    }
}
