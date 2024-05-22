using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Maintenence : MonoBehaviour
{

    public Toggle didEat;
    public Toggle didSleep;
    public int unhealthy;
    public TMP_Text unhealthyText;
    public int fatigued;
    public TMP_Text fatigueText;
    public TMP_Dropdown selectDebuff;
    public Button addStack;
    public Button removeStack;
    public Button maintenenceCheck;
    private Stress stress;

    // Start is called before the first frame update
    void Start()
    {
        stress = GetComponent<Stress>();
        maintenenceCheck.onClick.AddListener( delegate { MaintenenceCheck();});

        addStack.onClick.AddListener( delegate {AddRemoveStack(true);});
        removeStack.onClick.AddListener( delegate {AddRemoveStack(false);});

            // Initialize stress actions
        List<string> debuffs = new List<string>
        {
            "Unhealthy",
            "Fatigued",
        };
        selectDebuff.ClearOptions();
        selectDebuff.AddOptions(debuffs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddRemoveStack(bool isAdding)
    {
        int value;

        if(isAdding)
        {
            value = 1;
        }
        else
        {
            value = -1;
        }
        
        if(selectDebuff.value == 0)
        {
            unhealthy += value;
            unhealthy = Math.Clamp(unhealthy, 0, 10);
        }
        else
        {
            fatigued += value;
            fatigued = Math.Clamp(fatigued, 0, 10);
        }
        UpdateTexts();
    }
    void MaintenenceCheck()
    {
        if (didEat.isOn)
        {
            didEat.isOn = false;
        }
        else
        {
            unhealthy ++;
            unhealthy = Mathf.Clamp(unhealthy, 0, 10);   
        }
        if (didSleep.isOn)
        {
            didEat.isOn = false;
        }
        else
        {
            fatigued ++;
            fatigued = Mathf.Clamp(fatigued, 0, 10);
        }

        stress.currentEnergy -= fatigued;
        stress.currentEnergy = Mathf.Clamp(stress.currentEnergy, 0, 10);
        stress.currentEnergy -= unhealthy;
        stress.currentEnergy = Mathf.Clamp(stress.currentEnergy, -6, 10);
        stress.CalculateStressAndEnergy();

        UpdateTexts();
    }

    void UpdateTexts()
    {
        fatigueText.text = $"Fatigued: {fatigued}/10";
        unhealthyText.text = $"Unhealthy: {unhealthy}/10";
    }
}
