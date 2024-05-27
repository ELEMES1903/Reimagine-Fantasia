using System;
using System.Linq;
using TMPro;
using UnityEngine;
using System.Reflection;

[System.Serializable]
public class StatArray
{
    public string name;
    public Modifier[] modifiers;
    //public TMP_Text statText;
    public TMP_InputField inputField;
    public TMP_Text finalValueText;
    public TMP_Text inputText;
    public TMP_Text totalModifierText;
    public int baseValue;
    public int totalValue;
}

public class OtherStat : MonoBehaviour
{
    public StatArray[] stats;
    private AttributeAndSkill attributeAndSkill;
    private ModifiersManager modifiersManager;
    //public TMP_Text missScoreText;

    private void Awake()
    {
        // Get references in Awake to ensure they are ready before Start is called
        attributeAndSkill = GetComponent<AttributeAndSkill>();
        modifiersManager = GetComponent<ModifiersManager>();

        if (attributeAndSkill == null)
        {
            Debug.LogError("AttributeAndSkill component is missing on the GameObject.");
        }

        foreach (StatArray stat in stats)
        {
            if(stat.inputField != null)
            {
                stat.inputField.onEndEdit.AddListener( delegate{UpdateInput(stat);});
                stat.inputField.onSelect.AddListener(delegate { OnInputFieldFocused(stat); });
                stat.inputField.onEndEdit.AddListener(delegate { OnInputFieldFocusLost(stat); });
            }
            else
            {
                
            }

        }
    }

    void OnInputFieldFocused(StatArray stat)
    {
        stat.inputText.gameObject.SetActive(true);
        stat.finalValueText.gameObject.SetActive(false);
    }

    void OnInputFieldFocusLost(StatArray stat)
    {
        stat.inputText.gameObject.SetActive(false);
        stat.finalValueText.gameObject.SetActive(true);
    }

    public void UpdateAll()
    {
        foreach (StatArray stat in stats)
        {
            ExecuteMethod(stat.name);
        }
    }

    public void UpdateStatModifierText()
    {
        foreach (StatArray stat in stats)
        {
            int totalModifierValue = GetTotalModifierValue(stat.name);
            if(totalModifierValue == 0)
            {
                stat.totalModifierText.text = "";
            }
            else if(totalModifierValue > 0)
            {
                stat.totalModifierText.text = "(+" + totalModifierValue.ToString() + ")";
            }
            else
            {
                stat.totalModifierText.text = "(" + totalModifierValue.ToString() + ")";
            }
            
        }
    }

    void ExecuteMethod(string methodName)
    {
        string name = "Update" + methodName;
        MethodInfo methodInfo = GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        
        if (methodInfo != null)
        {
            methodInfo.Invoke(this, null);
        }
        else
        {
            Debug.LogError("Method not found: " + name);
        }
    }

    public void UpdateInput(StatArray stat)
    {
        if(int.TryParse(stat.inputField.text, out int newValue))
        {
            stat.baseValue = newValue;
        }
        ExecuteMethod(stat.name);
    }

    public void UpdateMissScore()
    {
        int agilityScore = GetSkillScore("Agility")/2;
        int insinctScore = GetSkillScore("Instinct")/2;
        StatArray missScore = GetStatElement("MissScore");

        missScore.totalValue = missScore.baseValue + agilityScore + insinctScore;
        missScore.finalValueText.text = $"{missScore.totalValue}";
    }

    public void UpdateProtectionScore()
    {
        StatArray protectionScore = GetStatElement("ProtectionScore");
        StatArray missScore = GetStatElement("MissScore");

        protectionScore.totalValue = missScore.totalValue + protectionScore.baseValue;
        protectionScore.finalValueText.text = $"{protectionScore.totalValue}";
    }

    public void UpdateCritGap()
    {
        StatArray critGap = GetStatElement("CritGap");

        critGap.totalValue = critGap.baseValue;
        critGap.finalValueText.text = $"{critGap.totalValue}";
    }

    public void UpdateFreeMovement()
    {
        int agilityScore = GetSkillScore("Agility")/2;
        StatArray freeMovement = GetStatElement("FreeMovement");

        freeMovement.totalValue = freeMovement.baseValue + agilityScore;
        freeMovement.finalValueText.text = $"{freeMovement.totalValue} meters";
    }

    public void UpdateInitiative()
    {
        int intuitionScore = GetSkillScore("Intuition")/2;
        StatArray initiative = GetStatElement("Initiative");

        initiative.totalValue = initiative.baseValue + intuitionScore;
        initiative.finalValueText.text = $"{initiative.totalValue}";
    }

    private int GetTotalModifierValue(string name)
    {
        StatArray stat = stats.FirstOrDefault(s => s.name == name);
        return stat?.modifiers.Sum(modifier => modifier.value) ?? 0;
    }

    public int GetSkillScore(string skillName)
    {
        foreach(AttributeArray attributes in attributeAndSkill.attributes)
        {
            foreach (SkillArray skill in attributes.skills)
            {
                if(skill.name == skillName)
                {
                    return skill.baseValue;
                }
            }
        }
        return 0;
    }

    public StatArray GetStatElement(string statName)
    {
        foreach(StatArray stat in stats)
        {
            if(stat.name == statName)
            {
                return stat;
            }
        }
        return null;
    }
}
