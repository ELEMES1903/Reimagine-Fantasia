using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SettingsManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject saveSettings;
    public GameObject Settings;
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;

    [Header("Health")]
    public GameObject healthTab;
    public GameObject healthSettings;

    [Header("Attribute")]
    public GameObject attributeTab;
    public GameObject profeciencySettings;

    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && 
            resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void Quit()
    {
        Debug.Log("see ya");
        Application.Quit();
    }

    //SETTINGS 
    public void OpenSettings() { Settings.SetActive(true); }
    public void CloseSettings() { Settings.SetActive(false); }

    //SAVE 
    public void OpenSaveSettings() { saveSettings.SetActive(true); }
    public void CloseSaveSettings() { saveSettings.SetActive(false); }

    // Method to open the Player's Handbook Link
    public void OpenWebsite(string url) { Application.OpenURL(url); }

    //HEALTH
    public void OpenHealthSettings() { healthSettings.SetActive(true); healthTab.SetActive(false); }
    public void CloseHealthSettings() { healthSettings.SetActive(false); healthTab.SetActive(true); }

    //PROFECIENCY
    public void OpenProfeciencySettings() { profeciencySettings.SetActive(true); attributeTab.SetActive(false); }
    public void CloseProfeciencySettings() { profeciencySettings.SetActive(false); attributeTab.SetActive(true); }
}
