using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class OtherStat : MonoBehaviour
{

    //Miss, Armor, and Protection Score
    int baseMissScore;
    public TMP_InputField baseMissScoreInput;
    public int instinctScore;
    public int missScore;
    
    public int armorScore;
    public TMP_InputField armorScoreInput;
    int protectionScore;

    public AttributeAndSkill attributeAndSkill;

    // Start is called before the first frame update
    void Start()
    {   
        baseMissScoreInput.characterLimit = 2;
        armorScoreInput.characterLimit = 2;

        baseMissScore = 3;
        baseMissScoreInput.onEndEdit.AddListener(delegate { UpdateMissScore(); });
        armorScoreInput.onEndEdit.AddListener(delegate { UpdateProtectionScore(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInstinctScore()
    {
        // Check if attributeAndSkill is not null and it has attributes
        if (attributeAndSkill != null && attributeAndSkill.attributes != null)
        {
            // Loop through each attribute
            foreach (AttributeArray attribute in attributeAndSkill.attributes)
            {
                // Loop through each skill in the current attribute
                foreach (SkillArray skill in attribute.skills)
                {
                    if (skill.name == "Instinct")
                    {
                        instinctScore = skill.modifiedValue;
                    } else
                    {
                        instinctScore = 0;
                    }
                }
            }
        }
        UpdateMissScore();
        UpdateProtectionScore();
    } 

    public void UpdateMissScore(){

        if (int.TryParse(baseMissScoreInput.text, out int newValue))
        {
            baseMissScore = newValue;
        }
        missScore = baseMissScore + instinctScore;
        baseMissScoreInput.text = $"{missScore}";
    }

    public void UpdateProtectionScore()
    {
        if (int.TryParse(armorScoreInput.text, out int newValue))
        {
            armorScore = newValue;
        }
        protectionScore = armorScore + missScore;
        armorScoreInput.text = $"{armorScore} ({protectionScore})";
    }
}
