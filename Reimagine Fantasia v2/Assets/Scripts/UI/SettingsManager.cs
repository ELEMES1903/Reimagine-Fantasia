using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject saveSettings;
    public GameObject Settings;
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;

    public GameObject deleteSettings;
    public GameObject enableDeleteButton;

    [Header("Health")]
    public GameObject healthTab;
    public GameObject healthSettings;

    [Header("Attribute")]
    public GameObject attributeTab;
    public GameObject profeciencySettings;

    void Start()
    {
        // Get the available screen resolutions
        resolutions = Screen.resolutions;

        // Clear any existing options in the dropdown
        resolutionDropdown.ClearOptions();

        // Create a list of resolution strings
        List<string> options = new List<string>();

        // Add each unique resolution (without refresh rate variations) to the options list
        foreach (Resolution resolution in resolutions)
        {
            string option = resolution.width + " x " + resolution.height + " " + resolution.refreshRateRatio + "Hz";
            if (!options.Contains(option))
            {
                options.Add(option);
            }
        }

        // Add the resolution options to the dropdown
        resolutionDropdown.AddOptions(options);

        // Set the current resolution as the default selected option
        int currentResolutionIndex = GetCurrentResolutionIndex();
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private int GetCurrentResolutionIndex()
    {
        Resolution currentResolution = Screen.currentResolution;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == currentResolution.width && resolutions[i].height == currentResolution.height)
            {
                return i;
            }
        }

        return 0; // Default to the first resolution if current resolution is not found
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        // Adjust the game window size to match the resolution
        if (!Screen.fullScreen)
        {
            int windowWidth = resolution.width;
            int windowHeight = resolution.height;
            AdjustScreenSize(resolution.width, resolution.height);
        }
    }

    void AdjustScreenSize(int width, int height)
    {
        // Adjust the size of the game window based on resolution
        float aspectRatio = (float)width / height;
        int targetWidth = width;
        int targetHeight = height;

        // You can adjust these values according to your needs
        int padding = 50; // Padding for the window borders
        if (Screen.fullScreen)
        {
            // If fullscreen, adjust the window size to match the screen resolution
            targetWidth += padding;
            targetHeight += padding;
        }

        Screen.SetResolution(targetWidth, targetHeight, Screen.fullScreen);
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

    //DELETE OPTIONS
    public void OpenDeleteSettings() { deleteSettings.SetActive(true); enableDeleteButton.SetActive(false); }
    public void CloseDeleteSettings() { deleteSettings.SetActive(false); enableDeleteButton.SetActive(true); }
}