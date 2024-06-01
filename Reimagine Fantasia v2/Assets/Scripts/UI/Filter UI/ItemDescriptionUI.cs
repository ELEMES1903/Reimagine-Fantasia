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

    public Button[] enableUIButtons;
    public Button[] disableUIButtons;

    void Start()
    {
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
    }
}
