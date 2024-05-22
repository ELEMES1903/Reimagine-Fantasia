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
    public TMP_InputField initiativeInput;
    public TMP_InputField critGapInput;

    public int baseMissScore = 3;
    public int missScore;
    public int armorScore;
    public int protectionScore;
    public int freeMovement;
    public int initiative;
    public int critGap;
    public int baseCritGap;
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
    }

    private void Start()
    {
        baseMissScoreInput.characterLimit = 2;
        armorScoreInput.characterLimit = 2;

        baseMissScoreInput.onEndEdit.AddListener(delegate { UpdateMissScore(); });
        armorScoreInput.onEndEdit.AddListener(delegate { UpdateProtectionScore(); });
        critGapInput.onEndEdit.AddListener(delegate { UpdateCritGap(); });
        freeMovementInput.onEndEdit.AddListener(delegate { UpdateFreeMovement(); });
        initiativeInput.onEndEdit.AddListener(delegate { UpdateInitiative(); });
        
        UpdateAll();
    }

    void Update()
    {

    }
    public void UpdateAll()
    {
        UpdateMissScore();
        UpdateProtectionScore();
        UpdateFreeMovement();
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
    private void UpdateMissScore()
    {
        int instinctScore = GetSkillScore("Instinct");

        if (int.TryParse(baseMissScoreInput.text, out int newValue))
        {
            if(missScore  == newValue){ }else{baseMissScore = newValue;}
        }

        int totalValue = GetTotalModifierValue("Miss Score");
        missScore = baseMissScore + instinctScore + totalValue;
        baseMissScoreInput.text = $"{missScore}";
    }
    private void UpdateProtectionScore()
    {
        if (int.TryParse(armorScoreInput.text, out int newValue))
        {
            if(protectionScore  == newValue){ }else{armorScore = newValue;}
        }

        int totalValue = GetTotalModifierValue("Armor Score");

        protectionScore = missScore + armorScore + totalValue;
        armorScoreInput.text = $"{protectionScore}";
    }
    private void UpdateFreeMovement()
    {
        int agilityScore = GetSkillScore("Agility") / 2;
        
        if (int.TryParse(freeMovementInput.text, out int newValue))
        {
            if(freeMovement  == newValue){ }else{freeMovement = newValue;}
        }

        int totalValue = GetTotalModifierValue("Free Movement");
        
        freeMovement = freeMovement + totalValue + agilityScore;
        freeMovementInput.text = $"{freeMovement} meters";
    }
    private void UpdateInitiative()
    {
        int intuitionScore = GetSkillScore("Inuition");
        int instinctScore = GetSkillScore("Instinct");

        if (int.TryParse(armorScoreInput.text, out int newValue))
        {
            if(initiative == newValue){ }else{initiative = newValue;}
        }

        int totalValue = GetTotalModifierValue("Initiative");

        initiative = instinctScore + intuitionScore*2 + totalValue;
        armorScoreInput.text = $"{protectionScore}";
    }
    private void UpdateCritGap()
    {
        
        if (int.TryParse(critGapInput.text, out int newValue))
        {
            if(baseCritGap == newValue){ }else{baseCritGap = newValue;}
        }

        int totalValue = GetTotalModifierValue("Crit Gap");
        critGap = baseCritGap + totalValue;
        critGapInput.text = $"{critGap}";
    }
    private int GetTotalModifierValue(string name)
    {
        StatArray stat = stats.FirstOrDefault(s => s.name == name);
        return stat?.modifiers.Sum(modifier => modifier.value) ?? 0;
    }
}
