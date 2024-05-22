using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SettingsManager : MonoBehaviour
{
    public GameObject saveSettings;
    public GameObject statSettings;
    public GameObject Settings;
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;

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
    public void OpenSettings()
    {
    Settings.SetActive(true);
    }

    public void CloseSettings()
    {
        Settings.SetActive(false);
    }

    //SAVE 
    public void OpenSaveSettings()
    {
    saveSettings.SetActive(true);
    }

    public void CloseSaveSettings()
    {
        saveSettings.SetActive(false);
    }

    //STAT 
    public void OpenStatSettings()
    {
        statSettings.SetActive(true);
    }

    public void CloseStatSettings()
    {
        statSettings.SetActive(false);
    }

    // Method to open the Player's Handbook Link
    public void OpenWebsite(string url)
    {
        Application.OpenURL(url);
    }
}
