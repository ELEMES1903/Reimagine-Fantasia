using UnityEngine;
using System.IO;
using Unity.VisualScripting;

public class SaveSystem : MonoBehaviour
{
    public HealthBar healthBar;
    public AttributeAndSkill attributeAndSkill;
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
        savePath = Path.Combine(Application.persistentDataPath, "healthData.json");
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
        string saveFileName = "healthData_" + slotIndex + fileExtension;
        string saveFilePath = Path.Combine(saveDirectory, saveFileName);

        SaveDataStructure data = new SaveDataStructure
        {
            healthData = new HealthData
            {
                maxHP = healthBar.maxHP,
                currentMaxHP = healthBar.currentMaxHP,
                currentHP = healthBar.currentHP
            },

            attributeData = new AttributeData[attributeAndSkill.attributes.Length]
        };

        for (int i = 0; i < attributeAndSkill.attributes.Length; i++)
        {
            data.attributeData[i] = new AttributeData
            {
                name = attributeAndSkill.attributes[i].name,
                baseValue = attributeAndSkill.attributes[i].baseValue
            };
        }

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, jsonData);

        Debug.Log("Data saved to slot " + slotIndex + ".");
    }

    // Method to load the health and attribute data from a JSON file
    public void LoadData(int slotIndex)
    {
        string saveFileName = "healthData_" + slotIndex + fileExtension;
        string saveFilePath = Path.Combine(saveDirectory, saveFileName);

        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            SaveDataStructure data = JsonUtility.FromJson<SaveDataStructure>(jsonData);

            // Update the HealthBar script with the loaded health values
            healthBar.maxHP = data.healthData.maxHP;
            healthBar.currentMaxHP = data.healthData.currentMaxHP;
            healthBar.currentHP = data.healthData.currentHP;

            // Update the AttributeAndSkill script with the loaded attribute values
            for (int i = 0; i < data.attributeData.Length; i++)
            {
                attributeAndSkill.attributes[i].baseValue = data.attributeData[i].baseValue;
            }

            UpdateAll();
            healthBar.UpdateHealthBars();

            Debug.Log("Data loaded from slot " + slotIndex + ".");
        }
        else
        {
            Debug.LogWarning("No save data found in slot " + slotIndex + ".");
        }
    }

    public void UpdateAll()
    {
        foreach (AttributeStat attribute in attributeAndSkill.attributes)
        {
            attributeAndSkill.CalculateTotalValue(attribute);

            foreach (Skills skill in attribute.skills)
            {
                attributeAndSkill.CalculateTotalValue(skill);
            }
        }
    }
    
}

[System.Serializable]
public class SaveDataStructure
{
    public HealthData healthData;
    public AttributeData[] attributeData;
}

[System.Serializable]
public class HealthData
{
    public float maxHP;
    public float currentMaxHP;
    public float currentHP;
}

[System.Serializable]
public class AttributeData
{
    public string name;
    public int baseValue;
}

