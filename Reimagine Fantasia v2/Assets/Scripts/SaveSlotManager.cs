using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class SaveSlotManager : MonoBehaviour
{
    public SaveSlot saveSlot; // Reference to the SaveSlot script

    void Awake(){

        LoadNames();
    }

    void OnApplicationQuit(){

        SaveNames();
    }
    // Method to save the names of all save slots
    public void SaveNames()
    {
        for (int i = 0; i < saveSlot.saveSlots.Length; i++)
        {
            string nameKey = "SlotName_" + i;
            PlayerPrefs.SetString(nameKey, saveSlot.saveSlots[i].nameText.text);
        }

        // Save PlayerPrefs data
        PlayerPrefs.Save();
    }

    // Method to load the names of all save slots
    public void LoadNames()
    {
        for (int i = 0; i < saveSlot.saveSlots.Length; i++)
        {
            string nameKey = "SlotName_" + i;
            if (PlayerPrefs.HasKey(nameKey))
            {
                string loadedName = PlayerPrefs.GetString(nameKey);
                saveSlot.saveSlots[i].nameText.text = loadedName;
            }
        }
    }
}