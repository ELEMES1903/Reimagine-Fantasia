using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SaveSlot : MonoBehaviour
{
    [System.Serializable]
    public class SlotElement
    {
        public string name;
        public TMP_Text nameText;
        public int slotNumber;
        public Button slotButton;
        public TMP_Text saveTime;
        public RawImage slotPortrait;
        public TMP_InputField notesInputField;
    }

    public SlotElement[] saveSlots;
    private SaveSystem saveSystem;
    private AttributeAndSkill attributeAndSkill;
    private LoadImageFromURL loadImageFromURL;
    int selectedIndex = 0;
    int selectedSlotIndex = 0;

    public TMP_InputField charInputField;
    public Toggle isAutoSaving;
    public TMP_Dropdown dropdown;

    void OnApplicationQuit()
    {
        if (isAutoSaving.isOn)
        {
            SaveDataForSelectedSlot();
        }
        SaveSlotData();
    }

    void Start()
    {
        attributeAndSkill = GetComponent<AttributeAndSkill>();
        saveSystem = GetComponent<SaveSystem>();
        loadImageFromURL = GetComponent<LoadImageFromURL>();    

        LoadSlotData();
   
        // Add listener to each slot button
        foreach (SlotElement slot in saveSlots)
        {
            int capturedSlotNumber = slot.slotNumber - 1;
            slot.slotButton.onClick.AddListener(() => OnSlotButtonClick(slot)); // Pass slot directly
        }
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        // Highlight the initially selected slot
        UpdateSelection();
        UpdateDropdownEntries();
    }

    void Update()
    {
        // Handle arrow key inputs for navigation
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSelection(1);
        }
    }

    // Method to update the names of dropdown entries
    public void UpdateDropdownEntries()
    {
        List<string> dropdownOptions = new List<string>();
        foreach (SlotElement slot in saveSlots)
        {
            string dropdownEntry = $"{slot.name}: {slot.nameText.text}"; // Format the dropdown entry
            dropdownOptions.Add(dropdownEntry);
        }
        dropdown.ClearOptions(); // Clear existing options
        dropdown.AddOptions(dropdownOptions); // Add updated options
    }

    void ChangeSelection(int direction)
    {
        // Update the selected index based on the direction
        selectedIndex = (selectedIndex + direction + saveSlots.Length) % saveSlots.Length;
        // Highlight the newly selected slot
        UpdateSelection();
    }

    void UpdateSelection()
    {
        // Update the visual representation to highlight the selected slot
        for (int i = 0; i < saveSlots.Length; i++)
        {
            bool isSelected = (i == selectedIndex);
            Color highlightColor = isSelected ? Color.yellow : Color.white;
            saveSlots[i].slotButton.colors = GetButtonColors(highlightColor);
        }
    }

    void OnSlotButtonClick(SlotElement clickedSlot)
    {
        // Update the selected index to the clicked slot number
        selectedIndex = clickedSlot.slotNumber;
        // Highlight the newly selected slot
        UpdateSelection();
    }

    ColorBlock GetButtonColors(Color highlightColor)
    {
        // Helper method to set button colors
        ColorBlock colors = ColorBlock.defaultColorBlock;
        colors.normalColor = highlightColor;
        colors.highlightedColor = highlightColor;
        colors.pressedColor = highlightColor;
        colors.selectedColor = highlightColor;
        return colors;
    }

    public void Save()
    {
        saveSystem.SaveData(selectedIndex);

        if (string.IsNullOrWhiteSpace(charInputField.text) || charInputField.text.Trim().Length == 0)
        {
            saveSlots[selectedIndex].nameText.text = "Unnamed Character";
            charInputField.text = "Unnamed Character";
        }
        else
        {
            saveSlots[selectedIndex].nameText.text = charInputField.text;
        }
        // Set the save time text
        DateTime currentTime = DateTime.Now;
        saveSlots[selectedIndex].saveTime.text = currentTime.ToString("MMM dd, HH:mm:ss");
    }

    public void Load()
    {
        saveSystem.LoadData(selectedIndex);
    }

    public void SaveSlotData()
    {
        for (int i = 0; i < saveSlots.Length; i++)
        {
            string nameKey = "SlotName_" + i;
            string timeKey = "SlotTime_" + i;
            string noteKey = "SlotNote_" + i;
            string portraitKey = "SlotPortrait_" + i;

            PlayerPrefs.SetString(nameKey, saveSlots[i].nameText.text);
            PlayerPrefs.SetString(timeKey, saveSlots[i].saveTime.text);
            PlayerPrefs.SetString(noteKey, saveSlots[i].notesInputField.text);

            if (saveSlots[i].slotPortrait.texture != null)
            {
                string base64Portrait = TextureHelper.TextureToBase64(saveSlots[i].slotPortrait.texture as Texture2D);
                PlayerPrefs.SetString(portraitKey, base64Portrait);
            }
        }
        // Save the status of the isAutoSaving toggle
        PlayerPrefs.SetInt("AutoSavingStatus", isAutoSaving.isOn ? 1 : 0);
        // Save PlayerPrefs data
        PlayerPrefs.Save();
    }

    // Method to load the names and save times of all save slots
    public void LoadSlotData()
    {
        for (int i = 0; i < saveSlots.Length; i++)
        {
            string nameKey = "SlotName_" + i;
            string timeKey = "SlotTime_" + i;
            string noteKey = "SlotNote_" + i;
            string portraitKey = "SlotPortrait_" + i;

            if (PlayerPrefs.HasKey(nameKey))
            {
                string loadedName = PlayerPrefs.GetString(nameKey);
                string loadedTime = PlayerPrefs.GetString(timeKey);
                string loadedNote = PlayerPrefs.GetString(noteKey);
                string loadedPortrait = PlayerPrefs.GetString(portraitKey);

                saveSlots[i].nameText.text = loadedName;
                saveSlots[i].saveTime.text = loadedTime;
                saveSlots[i].notesInputField.text = loadedNote;

                if (!string.IsNullOrEmpty(loadedPortrait))
                {
                    Texture2D texture = TextureHelper.Base64ToTexture(loadedPortrait);
                    saveSlots[i].slotPortrait.texture = texture;
                }
            }
        }
        // Load the status of the isAutoSaving toggle
        isAutoSaving.isOn = PlayerPrefs.GetInt("AutoSavingStatus", 1) == 1;
    }

    // Method called when the dropdown's value changes
    void OnDropdownValueChanged(int index)
    {
        selectedSlotIndex = index;
    }

    // Method to save data into the selected save slot
    void SaveDataForSelectedSlot()
    {
        if (charInputField.text == "Enter Character Name...")
        {
        }
        else
        {
            saveSlots[selectedSlotIndex].nameText.text = charInputField.text;
        }
        saveSystem.SaveData(selectedSlotIndex);
    }

    public int SelectedIndex
    {
        get { return selectedIndex; }
    }
}

public static class TextureHelper
{
    public static string TextureToBase64(Texture2D texture, bool useJpeg = false)
    {
        byte[] textureBytes;
        if (useJpeg)
        {
            textureBytes = texture.EncodeToJPG();
        }
        else
        {
            textureBytes = texture.EncodeToPNG();
        }
        return Convert.ToBase64String(textureBytes);
    }

    public static Texture2D Base64ToTexture(string base64)
    {
        byte[] textureBytes = Convert.FromBase64String(base64);
        Texture2D texture = new Texture2D(2, 2); // The size will be updated when loading the image
        texture.LoadImage(textureBytes);
        return texture;
    }
}
