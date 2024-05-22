using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;



public class SaveSystem : MonoBehaviour
{
    private HealthBar healthBar;
    private AttributeAndSkill attributeAndSkill;
    private OtherStat otherStat;
    private Stress stress;
    private CustomResource customResource;
    private Conditions conditions;
    private ModifiersManager modifiersManager;

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
    void Start()
    {
        attributeAndSkill = GetComponent<AttributeAndSkill>();
        otherStat = GetComponent<OtherStat>();
        healthBar = GetComponent<HealthBar>();
        stress = GetComponent<Stress>();
        customResource = GetComponent<CustomResource>();
        conditions = GetComponent<Conditions>();
        modifiersManager = GetComponent<ModifiersManager>();
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

            customResourceData = customResource.customResource.Select(cr => new CustomResourceData
            {
                currentValue = cr.currentValue,
                maxValue = cr.maxValue,
                minValue = cr.minValue,
                resourceName = cr.resourceName.text
            }).ToArray()
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

            for (int i = 0; i < data.customResourceData.Length; i++)
            {
                customResource.customResource[i].currentValue = data.customResourceData[i].currentValue;
                customResource.customResource[i].maxValue = data.customResourceData[i].maxValue;
                customResource.customResource[i].minValue = data.customResourceData[i].minValue;
                customResource.customResource[i].resourceName.text = data.customResourceData[i].resourceName;
            }
            
            saveSlot.charInputField.text = saveSlot.saveSlots[slotIndex].nameText.text;

            customResource.UpdateCustomText();
            attributeAndSkill.UpdateAll();
            stress.CalculateStressAndEnergy();
            healthBar.UpdateHealthBars();

            Debug.Log("Data loaded from slot " + slotIndex + ".");
        }
        else
        {
            Debug.LogWarning("No save data found in slot " + slotIndex + ".");
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
    public CustomResourceData[] customResourceData; // Add this field

    public int missScore;
    public int armorScore;
    public int heavyStress;
    public int normalStress;
    public int lightStress;
    public int freeMovement;
}

[System.Serializable]
public class CustomResourceData
{
    public float currentValue;
    public float maxValue;
    public float minValue;
    public string resourceName;
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