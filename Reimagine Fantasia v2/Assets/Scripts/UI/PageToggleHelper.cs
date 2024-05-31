using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PageToggleHelper : MonoBehaviour
{
    public Button mainButton;
    public Button optionButton;
    public GameObject mainPage;
    public GameObject optionsPage;

    void Start()
    {
        mainButton.onClick.AddListener(OpenMainPage);
        mainButton.onClick.AddListener(OpenOptionPage);

        mainPage.SetActive(true);
        optionsPage.SetActive(false);
    }

    void OpenMainPage()
    {
        mainPage.SetActive(true);
        optionsPage.SetActive(false);
    }
    void OpenOptionPage()
    {
        mainPage.SetActive(false);
        optionsPage.SetActive(true);
    }

}
