using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PageToggleHelper : MonoBehaviour
{
    public Button button; // Reference to the Toggle component
    public TMP_Text buttonText;
    public GameObject mainPage; // The first GameObject
    public string mainPageButtonText;
    public GameObject options; // The second GameObject
    public string optionsButtonText;

    private int number = 1;

    void Start()
    {
        // Add a listener to the toggle to call ToggleObjects when the value changes
        button.onClick.AddListener(ToggleObjects);

        mainPage.SetActive(true);
        options.SetActive(false);
        buttonText.text = mainPageButtonText;
    }

    void ToggleObjects()
    {
        if (number == 1)
        {
            mainPage.SetActive(true);
            options.SetActive(false);
            number = 0;
            buttonText.text = mainPageButtonText;
        }
        else
        {
            mainPage.SetActive(false);
            options.SetActive(true);
            number = 1;
            buttonText.text = optionsButtonText;
        }
    }

}
