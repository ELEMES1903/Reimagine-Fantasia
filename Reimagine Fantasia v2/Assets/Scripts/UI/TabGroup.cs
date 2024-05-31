using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public TabButton selectedTab;

    public List<GameObject> objectsToSwap;
    public void Subscribe (TabButton button)
    {

        if(tabButtons == null)
        {

            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.sprite = tabActive;
        int index = button.transform.GetSiblingIndex();

        for(int i = 0; i < objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }

        foreach (TabButton tabButton in tabButtons)
        {
            if (tabButton.buttonText != null)
            {
                if (tabButton == selectedTab)
                {
                    tabButton.buttonText.fontStyle = FontStyles.Underline;
                }
                else
                {
                    tabButton.buttonText.fontStyle = FontStyles.Normal;
                }
            }
        }
    }

    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if(selectedTab != null && button == selectedTab) { continue;}
            button.background.sprite = tabIdle;

            // Reset the text underline for non-selected tabs
            if (button.buttonText != null)
            {
                button.buttonText.fontStyle = FontStyles.Normal;
            }
        }
    }

}
    
