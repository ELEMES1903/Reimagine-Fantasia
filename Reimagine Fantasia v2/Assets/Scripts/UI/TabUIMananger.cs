using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabUIManager : MonoBehaviour
{
    public GameObject saveSettings;
    public GameObject statSettings;

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
}
