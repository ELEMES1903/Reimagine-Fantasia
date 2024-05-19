using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;



public class SaveSystem : MonoBehaviour
{
    public HealthBar healthBar;
    public AttributeAndSkill attributeAndSkill;
    public OtherStat otherStat;
    public Stress stress;
    public CustomResource customResource;
    public Conditions conditions;
    public ModifiersManager modifiersManager;

    private string savePath;
    
    private string slotNameSavePath;

    // Reference to the SaveSlotManager
    public SaveSlot saveSlot;

    // Number of save slots
    public int numberOfSlots = 3;

    // File extension for save files
    private string fileExtension = ".json";

    // Directory path for save files
    private string saveDirectory;
    
    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        // Initialize save directory
        saveDirectory = Application.persistentDataPath; // or any other valid directory path

        // Ensure that saveDirectory exists
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
    }

    // Method to save the health and attribute data to a JSON file
    public void SaveData(int slotIndex)
    {
        string saveFileName = "saveData" + slotIndex + fileExtension;
        string saveFilePath = Path.Combine(saveDirectory, saveFileName);

        SaveDataStructure data = new SaveDataStructure
        {
            attributeData = new AttributeData[attributeAndSkill.attributes.Length],
            conditionData = new ConditionData[conditions.conditions.Length],

            healthData = new HealthData
            {
                maxHP = healthBar.maxHP,
                currentMaxHP = healthBar.currentMaxHP,
                currentHP = healthBar.currentHP
            },

            skillData = attributeAndSkill.attributes.SelectMany(a => a.skills).Select(s => new SkillData
            {
                name = s.name,
                baseValue = s.baseValue,
                modifiers = s.modifiers.Select(m => new ModifierData { name = m.name, value = m.value }).ToArray()
            }).ToArray(),

            statData = otherStat.stats.Select(stat => new StatData
            {
                modifiers = stat.modifiers.Select(m => new ModifierData { name = m.name, value = m.value }).ToArray()
            }).ToArray(),

            missScore = otherStat.missScore,
            armorScore = otherStat.armorScore,
            freeMovement = otherStat.freeMovement,

            heavyStress = stress.heavyStress,
            normalStress = stress.normalStress,
            lightStress = stress.lightStress,

            currentValue = customResource.customSlider1.currentValue,
            maxValue = customResource.customSlider1.maxValueInput.text,
            minValue = customResource.customSlider1.minValueInput.text,
            resourceName = customResource.customSlider1.resourceName.text,
        };

        for (int i = 0; i < conditions.conditions.Length; i++)
        {
            data.conditionData[i] = new ConditionData
            {
                conDuration = conditions.conditions[i].conDuration,
                isActive = conditions.conditions[i].isActive
            };
        }

        for (int i = 0; i < attributeAndSkill.attributes.Length; i++)
        {
            data.attributeData[i] = new AttributeData
            {
                name = attributeAndSkill.attributes[i].name,
                baseValue = attributeAndSkill.attributes[i].baseValue,
                modifiers = attributeAndSkill.attributes[i].modifiers.Select(m => new ModifierData { name = m.name, value = m.value }).ToArray()                
            };
        }

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, jsonData);

        Debug.Log("Data saved to slot " + slotIndex + ".");
    }

    // Method to load the health and attribute data from a JSON file
    public void LoadData(int slotIndex)
    {
        modifiersManager.RemoveAllModifiers();
        
        string saveFileName = "saveData" + slotIndex + fileExtension;
        string saveFilePath = Path.Combine(saveDirectory, saveFileName);

        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            SaveDataStructure data = JsonUtility.FromJson<SaveDataStructure>(jsonData);

            // Update the HealthBar script with the loaded health values
            healthBar.maxHP = data.healthData.maxHP;
            healthBar.currentMaxHP = data.healthData.currentMaxHP;
            healthBar.currentHP = data.healthData.currentHP;

            otherStat.missScore = data.missScore;
            otherStat.armorScore = data.armorScore;
            otherStat.freeMovement = data.freeMovement;

            stress.heavyStress = data.heavyStress;
            stress.normalStress = data.normalStress;
            stress.lightStress = data.lightStress;

            customResource.customSlider1.currentValue = data.currentValue;
            customResource.customSlider1.maxValueInput.text = data.maxValue;
            customResource.customSlider1.minValueInput.text = data.minValue;
            customResource.customSlider1.resourceName.text = data.resourceName;

            // Update the AttributeAndSkill script with the loaded attribute values
            for (int i = 0; i < data.attributeData.Length; i++)
            {
                attributeAndSkill.attributes[i].baseValue = data.attributeData[i].baseValue;
                attributeAndSkill.attributes[i].modifiers = data.attributeData[i].modifiers.Select(m => new Modifier { name = m.name, value = m.value }).ToArray();
            }

            foreach (var skill in data.skillData)
            {
                var skillArray = attributeAndSkill.attributes.SelectMany(a => a.skills).FirstOrDefault(s => s.name == skill.name);
                if (skillArray != null)
                {
                    skillArray.baseValue = skill.baseValue;
                    skillArray.modifiers = skill.modifiers.Select(m => new Modifier { name = m.name, value = m.value }).ToArray();
                }
            }

            foreach (var stat in data.statData)
            {
                var statArray = otherStat.stats.FirstOrDefault(s => s.name == stat.name);
                if (statArray != null)
                {
                    statArray.modifiers = stat.modifiers.Select(m => new Modifier { name = m.name, value = m.value }).ToArray();
                }
            }



            saveSlot.charInputField.text = saveSlot.saveSlots[slotIndex].nameText.text;

            UpdateAll();
            customResource.UpdateMinMax(customResource.customSlider1, true);
            customResource.UpdateMinMax(customResource.customSlider1, false);
            stress.CalculateStressAndEnergy();
            healthBar.UpdateHealthBars();
            otherStat.CalculateInstinctScore();

            Debug.Log("Data loaded from slot " + slotIndex + ".");
        }
        else
        {
            Debug.LogWarning("No save data found in slot " + slotIndex + ".");
        }
    }

    public void UpdateAll()
    {
        foreach (AttributeArray attribute in attributeAndSkill.attributes)
        {
            attributeAndSkill.CalculateTotalValue(attribute);

            foreach (SkillArray skill in attribute.skills)
            {
                attributeAndSkill.CalculateTotalValue(skill);
            }
        }
    }
}
[System.Serializable]
public class ModifierData
{
    public string name;
    public int value;
}

[System.Serializable]
public class SaveDataStructure
{
    public HealthData healthData;
    public AttributeData[] attributeData;
    public ConditionData[] conditionData;
    public SkillData[] skillData;
    public StatData[] statData;

    public int missScore;
    public int armorScore;
    public int heavyStress;
    public int normalStress;
    public int lightStress;
    public float currentValue;
    public string maxValue;
    public string minValue;
    public string resourceName;
    public int freeMovement;
}

[System.Serializable]
public class HealthData
{
    public int maxHP;
    public int currentMaxHP;
    public int currentHP;
}

[System.Serializable]
public class AttributeData
{
    public string name;
    public int baseValue;
    public ModifierData[] modifiers;

}
[System.Serializable]
public class ConditionData
{
    public int conDuration;
    public bool isActive;
    public ModifierData[] modifiers;
}

[System.Serializable]
public class StatData
{
    public string name;
    public ModifierData[] modifiers; // Add this field
}

[System.Serializable]
public class SkillData
{
    public string name;
    public int baseValue;
    public ModifierData[] modifiers; // Add this field
}