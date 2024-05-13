using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
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
    }

    public SlotElement[] saveSlots;
    public SaveSystem saveSystem;
    public AttributeAndSkill attributeAndSkill;
    public int selectedIndex = 0;
    public int selectedSlotIndex = 0;

    public GameObject charInputText;
    public TMP_Text charText;
    public TMP_InputField charInputField;
    public string charName;
    public Toggle isAutoSaving;
    public TMP_Dropdown dropdown; // Reference to the TMP dropdown

    public GameObject saveSettingsMenu;

    void OnApplicationQuit(){
        
        if(isAutoSaving.isOn){
            SaveDataForSelectedSlot();
        }

        SaveSlotData();
    }

    void Start()
    {
        LoadSlotData();

        // Set the placeholder text of the input field
        charInputField.placeholder.GetComponent<TMP_Text>().text = "Your placeholder text here";
        
        // Add listener to each slot button
        foreach (SlotElement slot in saveSlots)
        {
            int capturedSlotNumber = slot.slotNumber - 1;
            slot.slotButton.onClick.AddListener(() => OnSlotButtonClick(slot)); // Pass slot directly
        }

        charInputField.onEndEdit.AddListener(delegate { UpdateCharacterName(); });
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

        if(charInputField.isFocused){

            charText.gameObject.SetActive(false);
            charInputText.SetActive(true);

        } else {

            charText.gameObject.SetActive(true);
            charInputText.SetActive(false);

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
            //saveSlots[i].nameText.color = highlightColor;
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

    public void Save(){

        saveSystem.SaveData(selectedIndex);

        if(string.IsNullOrWhiteSpace(charName) || charName.Trim().Length == 0){

            saveSlots[selectedIndex].nameText.text = " Unnamed Character";
            charName = "Unnamed Character";

            } else {

            saveSlots[selectedIndex].nameText.text = charName;
        }


         // Set the save time text
        DateTime currentTime = DateTime.Now;
        saveSlots[selectedIndex].saveTime.text = currentTime.ToString("MMM dd, HH:mm:ss");
    }

    public void Load(){

        saveSystem.LoadData(selectedIndex);

    }

    void UpdateCharacterName(){

        charName = charInputField.text;
        UpdateCharacterNameText();

    }

    public void UpdateCharacterNameText(){

        charText.text = charName;

    }

    public void SaveSlotData()
    {
        for (int i = 0; i < saveSlots.Length; i++)
        {
            string nameKey = "SlotName_" + i;
            string timeKey = "SlotTime_" + i;

            PlayerPrefs.SetString(nameKey, saveSlots[i].nameText.text);
            PlayerPrefs.SetString(timeKey, saveSlots[i].saveTime.text);
        }

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

            if (PlayerPrefs.HasKey(nameKey))
            {
                string loadedName = PlayerPrefs.GetString(nameKey);
                string loadedTime = PlayerPrefs.GetString(timeKey);

                saveSlots[i].nameText.text = loadedName;
                saveSlots[i].saveTime.text = loadedTime;
            }
        }
    }

    // Method called when the dropdown's value changes
    void OnDropdownValueChanged(int index)
    {
        selectedSlotIndex = index;
    }

    // Method to save data into the selected save slot
    void SaveDataForSelectedSlot()
    {

        if(charText.text == "Enter Character Name..."){

        } else {
            saveSlots[selectedSlotIndex].nameText.text = charText.text; 
        }
        
        saveSystem.SaveData(selectedSlotIndex);
    }

    public void OpenSaveSettings(){

        saveSettingsMenu.SetActive(true);
    }

    public void CloseSaveSettings(){

        saveSettingsMenu.SetActive(false);
    }
}