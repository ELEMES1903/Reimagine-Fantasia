using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public HealthBar healthBar;
    public AttributeAndSkill attributeAndSkill;

    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "healthData.json");
    }

    

    // Method to save the health and attribute data to a JSON file
    public void SaveData()
    {
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
        File.WriteAllText(savePath, jsonData);

        Debug.Log("Data saved.");
    }

    // Method to load the health and attribute data from a JSON file
    public void LoadData()
    {
        if (File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);
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

            updateAll();
            healthBar.UpdateHealthBars();
            
            Debug.Log("Data loaded.");  
        }
        else
        {
            Debug.LogWarning("No save data found.");
        }
    }

    public void updateAll(){

        foreach (Attributes attribute in attributeAndSkill.attributes)
        {
            attributeAndSkill.CalculateTotalValue(attribute);

            foreach (Skills skill in attribute.skills){

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