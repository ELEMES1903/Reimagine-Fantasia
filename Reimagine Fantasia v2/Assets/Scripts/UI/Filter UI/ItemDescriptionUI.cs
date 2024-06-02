using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionUI : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text Properties;
    public TMP_Text Description;
    public GameObject UI;
    public GameObject currentObject;
    public Button[] enableUIButtons;
    public Button[] disableUIButtons;

    public Button addButton;
    public Button removeButton;
    public Button equipButton;
    private TMP_Text equipButtonText;

    private FilterManager filterManager;
    public Ability abilityScript;

    void Start()
    {
        addButton.onClick.AddListener(delegate {Add();});
        removeButton.onClick.AddListener(delegate {Remove();});
        equipButton.onClick.AddListener(delegate {Equip();});

        filterManager = GetComponent<FilterManager>();
        equipButtonText = equipButton.GetComponentInChildren<TMP_Text>();

        // Assign the methods to the onClick events for arrayA
        foreach (Button button in enableUIButtons)
        {
            button.onClick.AddListener(() => EnableUI(button));
        }

        // Assign the methods to the onClick events for arrayB
        foreach (Button button in disableUIButtons)
        {
            button.onClick.AddListener(() => DisableUI(button));
        }
    }

    // Method to be called when a button from arrayA is clicked
    void EnableUI(Button button)
    {
        UI.gameObject.SetActive(true);
    }

    // Method to be called when a button from arrayB is clicked
    void DisableUI(Button button)
    {
        UI.gameObject.SetActive(false);
        Reset();
    }
    public void GlobalUpdate()
    {
        if(filterManager.allEquipedItems.Contains(gameObject))
        {
            addButton.gameObject.SetActive(false);
            removeButton.gameObject.SetActive(false);

            equipButton.gameObject.SetActive(true);
            equipButtonText.text = "Unequip";
        }

        if(filterManager.allUnacquiredStuff.Contains(gameObject))
        {
            addButton.gameObject.SetActive(true);
            removeButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
        }

        if(filterManager.allAcquiredStuff.Contains(gameObject))
        {
            addButton.gameObject.SetActive(false);
            removeButton.gameObject.SetActive(true);

            if(abilityScript.equipabble && abilityScript.Type == "Item")
            {
                equipButton.gameObject.SetActive(true);
                equipButtonText.text = "Equip";
            }
            else
            {
                equipButton.gameObject.SetActive(false);
            }
        }
    }
    
    public void UpdateWhenRemoved()
    {
        addButton.gameObject.SetActive(true);
        removeButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
    }

    public void UpdateWhenAdded()
    {
        addButton.gameObject.SetActive(false);
        removeButton.gameObject.SetActive(true);

        if(abilityScript.equipabble && abilityScript.Type == "Item")
        {
            equipButton.gameObject.SetActive(true);
            equipButtonText.text = "Equip";
        }
        else
        {
            equipButton.gameObject.SetActive(false);
        }
    }

    public void UpdateWhenEquip()
    {
        addButton.gameObject.SetActive(false);
        removeButton.gameObject.SetActive(false);

        equipButton.gameObject.SetActive(true);
        equipButtonText.text = "Unequip";
    }

    public void UpdateWhenUnequip()
    {
        addButton.gameObject.SetActive(false);
        removeButton.gameObject.SetActive(true);

        equipButton.gameObject.SetActive(true);
        equipButtonText.text = "Equip";
    }

    public void Reset()
    {
        addButton.gameObject.SetActive(false);
        addButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);

        Name.text = " ";
        Properties.text = " ";
        Description.text = " ";
    }

    void Add()
    {
        abilityScript.AddAbility();
    }

    void Remove()
    {
        abilityScript.RemoveAbility();
    }

    void Equip()
    {
        abilityScript.EquipItem();
    }
}
