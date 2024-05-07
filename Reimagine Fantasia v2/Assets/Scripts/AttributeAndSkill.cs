using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using System.Collections.Generic;

[System.Serializable]
public class Skills
{
    public string name;
    public int modifiedValue;
    public Modifier[] modifiers;
    public Toggle proficientToggle; // Toggle for proficient
    public Toggle signatureToggle; // Toggle for signature
}

[System.Serializable]
public class Attributes
{
    public string name;
    public TMP_InputField inputField;
    public int baseValue;
    public int modifiedValue;
    public Modifier[] modifiers;
    public Skills[] skills;
}

public class AttributeAndSkill : MonoBehaviour
{
    public List<Attributes> attributes = new List<Attributes>(); // Using List instead of array
    public ModifiersManager modifiersManager; // Reference to ModifiersManager

    void Start()
    {
        modifiersManager.AddModifierToElement("Strength", "Bonus Damage", 10, attributes);
    }

}
