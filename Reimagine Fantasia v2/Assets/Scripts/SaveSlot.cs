using UnityEngine;
using TMPro;
using UnityEngine.UI;
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
    }

    public SlotElement[] saveSlots;
    public SaveSystem saveSystem;
    public AttributeAndSkill attributeAndSkill;
    public int selectedIndex = 0;


    void Start()
    {
        // Add listener to each slot button
        foreach (SlotElement slot in saveSlots)
        {
            int capturedSlotNumber = slot.slotNumber - 1;
            slot.slotButton.onClick.AddListener(() => OnSlotButtonClick(slot)); // Pass slot directly
        }

        // Highlight the initially selected slot
        UpdateSelection();
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
            saveSlots[i].nameText.color = highlightColor;
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
        saveSlots[selectedIndex].nameText.text = attributeAndSkill.characterName;

         // Set the save time text
        DateTime currentTime = DateTime.Now;
        saveSlots[selectedIndex].saveTime.text = currentTime.ToString("MMM dd, HH:mm");
    }

    public void Load(){
        saveSystem.LoadData(selectedIndex);
    }

}