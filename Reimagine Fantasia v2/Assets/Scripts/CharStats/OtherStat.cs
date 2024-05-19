using System;
using System.Linq;
using TMPro;
using UnityEngine;

[System.Serializable]
public class StatArray
{
    public string name;
    public Modifier[] modifiers;
}

public class OtherStat : MonoBehaviour
{
    public StatArray[] stats;

    public TMP_InputField baseMissScoreInput;
    public TMP_InputField armorScoreInput;
    public TMP_InputField freeMovementInput;

    public int baseMissScore = 3;
    public int instinctScore;
    public int missScore;
    public int armorScore;
    public int protectionScore;
    public int freeMovement;
    public AttributeAndSkill attributeAndSkill;

    private void Start()
    {
        baseMissScoreInput.characterLimit = 2;
        armorScoreInput.characterLimit = 2;

        baseMissScoreInput.onEndEdit.AddListener(delegate { UpdateMissScore(); });
        armorScoreInput.onEndEdit.AddListener(delegate { UpdateProtectionScore(); });
        freeMovementInput.onEndEdit.AddListener(delegate { UpdateFreeMovement(); });

    }
    public void UpdateAll()
    {
        UpdateMissScore();
        UpdateProtectionScore();
        UpdateFreeMovement();
    }
    public void CalculateInstinctScore()
    {
        instinctScore = attributeAndSkill?.attributes
            .SelectMany(attribute => attribute.skills)
            .Where(skill => skill.name == "Instinct")
            .Sum(skill => skill.modifiedValue) ?? 0;
    }

    private void UpdateMissScore()
    {
        CalculateInstinctScore();

        if (int.TryParse(baseMissScoreInput.text, out int newValue))
        {
            baseMissScore = newValue;
        }

        int totalValue = GetTotalModifierValue("Miss Score");
        missScore = baseMissScore + instinctScore + totalValue;
        baseMissScoreInput.text = $"{missScore}";
    }

    private void UpdateProtectionScore()
    {
        if (int.TryParse(armorScoreInput.text, out int newValue))
        {
            armorScore = newValue;
        }

        int totalValue = GetTotalModifierValue("Armor Score");
        protectionScore = armorScore + missScore + totalValue;
        armorScoreInput.text = $"{armorScore} ({protectionScore})";
    }

    private void UpdateFreeMovement()
    {
        if (int.TryParse(freeMovementInput.text, out int newValue))
        {
            freeMovement = newValue;
        }

        int totalValue = GetTotalModifierValue("Free Movement");
        freeMovement = freeMovement + totalValue;
        freeMovementInput.text = $"{freeMovement} meters";
    }

    private int GetTotalModifierValue(string name)
    {
        StatArray stat = stats.FirstOrDefault(s => s.name == name);
        return stat?.modifiers.Sum(modifier => modifier.value) ?? 0;
    }
}
