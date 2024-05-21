using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stress : MonoBehaviour
{
    private ModifiersManager modifiersManager;
    private AttributeAndSkill attributeAndSkill;
    private Conditions conditions;

    public Slider energySlider;
    public int maxEnergy;
    public int currentEnergy;

    int totalStress;
    public int heavyStress;
    public int normalStress;
    public int lightStress;
    public int weakened;

    bool isWeakened;
    bool isEnergetic;
    public bool isNormal;
    bool isTired;
    bool isExhausted;

    public TMP_Dropdown stressManagerDropdown;
    public Button applyStressActionButton;
    public TMP_Text stressAmountText;
    
    private enum StressType
    {
        Heavy,
        Normal,
        Light
    }

    void Start()
    {
        modifiersManager = GetComponent<ModifiersManager>();
        attributeAndSkill = GetComponent<AttributeAndSkill>();
        conditions = GetComponent<Conditions>();

        maxEnergy = 10;
        currentEnergy = maxEnergy;
        energySlider.maxValue = maxEnergy;
        energySlider.minValue = -6;

        lightStress = 0;
        normalStress = 0;
        heavyStress = 0;
        weakened = 0;


        // Initialize stress actions
        List<string> stressActions = new List<string>
        {
            "Gain Heavy Stress",
            "Gain Normal Stress",
            "Gain Light Stress",
            "Remove Heavy Stress",
            "Remove Normal Stress",
            "Remove Light Stress"
        };

        stressManagerDropdown.ClearOptions();
        stressManagerDropdown.AddOptions(stressActions);

        applyStressActionButton.onClick.AddListener(ApplyStressAction);
        CalculateStressAndEnergy();
    }

    private void ApplyStressAction()
    {
        bool isGaining = stressManagerDropdown.value < 3;

        StressType stressType = (StressType)(stressManagerDropdown.value % 3);

        AdjustStress(stressType, isGaining);
    }

    private void AdjustStress(StressType stressType, bool isGaining)
    {
        int stressChange = isGaining ? 1 : -1;

        if(totalStress >= 16 && isGaining)
        {
            if(lightStress > 0)
            {
                Mathf.Max(0, lightStress--);
                normalStress++;
            }
            else if (normalStress > 0)
            {
                Mathf.Max(0, normalStress--);
                heavyStress++;
            }
        }
        else
        {
            switch (stressType)
            {
                case StressType.Heavy:
                    heavyStress = Mathf.Clamp(heavyStress + stressChange, 0, 16);
                    break;
                case StressType.Normal:
                    normalStress = Mathf.Clamp(normalStress + stressChange, 0, 16);
                    break;
                case StressType.Light:
                    lightStress = Mathf.Clamp(lightStress + stressChange, 0, 16);
                    break;
            }
        }
        CalculateStressAndEnergy();
    }

    public void CalculateStressAndEnergy()
    {
        totalStress = heavyStress + normalStress + lightStress;
        if(totalStress > 16){ totalStress = 16;}

        currentEnergy = Mathf.Clamp(maxEnergy - totalStress, -6, maxEnergy);
        
        stressAmountText.text = $"Energy: {currentEnergy}\n\nHeavy: {heavyStress}\nNormal: {normalStress}\nLight: {lightStress}";

        applyEnergyLevelEffects();
        conditions.ConditionListUpdate();
        energySlider.value = currentEnergy;
    }

    public void DisplayCurrentStatus()
    {
        if(isEnergetic){ conditions.conditionList.text += $"\n<b>- <color=#FF6666>Energetic</color></b> (+2 to Checks)";}
        if(isTired){conditions.conditionList.text += $"\n<b>- <color=#FF6666>Tired</color></b> (-2 to Checks)";}
        if(isExhausted){conditions.conditionList.text += $"\n<b>- <color=#FF6666>Exhausted </color></b> (Disadvantage to Checks, Weigthed 1)";}
        if(isWeakened){conditions.conditionList.text += $"\n<b>- <color=#FF6666>Weakened {weakened}</color></b> (Only {weakened} AP/Turn)";}
    }
    
    private void applyEnergyLevelEffects()
    {
        isEnergetic = false;
        isNormal = false;
        isTired = false;
        isExhausted = false;
        isWeakened = false;

        foreach(AttributeArray attribute in attributeAndSkill.attributes)
        {
            if(currentEnergy > 8)
            {
                isEnergetic = true;
                modifiersManager.FindElement(attribute.name, "Energetic", +2);
            }
            else
            {
                isEnergetic = false;
                modifiersManager.FindElement(attribute.name, "Energetic", 0);
            }

            if(currentEnergy <= 8 && currentEnergy >= 4)
            {
                isNormal = true;
                attributeAndSkill.UpdateAll();
                //add disadvantage logic here
            }
            else
            {
                isNormal = false;
                attributeAndSkill.UpdateAll();
            }

            if(currentEnergy <= 4)
            {
                isTired = true;
                modifiersManager.FindElement(attribute.name, "Tired", -2);
            }
            else
            {
                isTired = false;
                modifiersManager.FindElement(attribute.name, "Tired", 0);
            }

            if(currentEnergy <= 2)
            {
                isExhausted = true;
                //add disadvantage logic here
            }
            else
            {
                isExhausted = false;
            }

            if(currentEnergy <= 0)
            {
                isWeakened = true;
                weakened = currentEnergy;
            }
            else
            {
                isWeakened = false;
            }
        }    
    }
}