using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class HPUIElement
{
    public string name;
    public TMP_InputField hpInputField;
    
}

public class HealthBar : MonoBehaviour
{
    public Slider hpSlider;
    public Slider currentMaxHpSlider;
    public HPUIElement[] hpUI;

    public int maxHP;
    public int currentMaxHP;
    public int currentHP;
    public int shield;
    public TMP_InputField hpInput;
    public TMP_InputField currentMaxHpInput;
    public TMP_InputField maxHpInput;
    public TMP_Text allHpText;

    public TMP_InputField hpChangeField;
    public TMP_Dropdown hpChangeDropdown;
    
    void Start()
    {
        // Initialize values
        maxHP = 10;
        currentMaxHP = maxHP;
        currentHP = maxHP;
        
        hpChangeField.onEndEdit.AddListener(delegate { HpChangeCalc(); });

        foreach (HPUIElement element in hpUI)
        {
            // Add listener for input field changes
            element.hpInputField.onEndEdit.AddListener(delegate { OnHPInputValueChanged(element); });
        }

        // Create a list of custom entries
        List<string> options = new List<string>();
        options.Add("Take Damage");
        options.Add("Heal HP");
        options.Add("Heal Current Max HP");
        options.Add("Gain Shield");

        hpChangeDropdown.ClearOptions(); // Clear existing options
        // Add the custom entries to the dropdown
        hpChangeDropdown.AddOptions(options);


        // Update slider values
        UpdateHealthBars();
    }

    public void OnHPInputValueChanged(HPUIElement element)
    {
        if (int.TryParse(element.hpInputField.text, out int newValue))
        {
            if(element.name == "HP"){ currentHP = newValue;}
            if(element.name == "Current Max HP"){ currentMaxHP = newValue;}
            if(element.name == "Max HP"){ maxHP = newValue;}
        }

        UpdateHealthBars();
    }

    public void TakeDamage(int damage)
    {
        //shield takes damage if there is a shield
        if(shield > 0){
            shield -= damage;

            //calculate left over damage
            if(shield < 0){
                int leftOverDmg = Mathf.Abs(shield);
                shield = 0;
                //HP takes leftover damage if there is HP, if not, current max HP takes leftover damage
                if(currentHP > 0){
                    currentHP -= leftOverDmg;
                    currentHP = Mathf.Max(currentHP, 0);
                } else {
                    currentMaxHP -= leftOverDmg;
                    currentMaxHP = Mathf.Max(currentMaxHP, 0);
                }
            }
        } else {
            //HP takes  damage if there is HP, if not, current max HP takes damage
           if(currentHP > 0){
                    currentHP -= damage;
                    currentHP = Mathf.Max(currentHP, 0);
                } else {
                    currentMaxHP -= damage;
                    currentMaxHP = Mathf.Max(currentMaxHP, 0);
                }
        }
        UpdateHealthBars();
    }

    public void HpHeal(int heal)
    {
        currentHP += heal;
        currentHP = Mathf.Min(currentHP, currentMaxHP);
        
        UpdateHealthBars();
    }

    public void CurrentMaxHpHeal(int heal)
    {
        currentMaxHP += heal;
        currentMaxHP = Mathf.Min(currentMaxHP, maxHP);
        
        UpdateHealthBars();
    }


    public void UpdateHealthBars()
    {
        if(currentHP > currentMaxHP){ currentHP = currentMaxHP; }
        if(currentMaxHP > maxHP){ currentMaxHP = maxHP; }

        foreach (HPUIElement element in hpUI)
        {
            if(element.name == "HP"){ element.hpInputField.text = currentHP.ToString();}
            if(element.name == "Current Max HP"){ element.hpInputField.text = currentMaxHP.ToString();}
            if(element.name == "Max HP"){ element.hpInputField.text = maxHP.ToString();}
        }

        hpSlider.value = currentHP;
        hpSlider.maxValue = maxHP;
        currentMaxHpSlider.value = currentMaxHP;
        currentMaxHpSlider.maxValue = maxHP;

        allHpText.text = $"( <b><color=#00FFFF>{shield}</color></b> )  <b><color=#FF6666>{currentHP}</color></b> / <b><color=#FF6666>{currentMaxHP}</color></b> / <b><color=#FF6666>{maxHP}</color></b>";

    }

    public void HpChangeCalc(){

        if (int.TryParse(hpChangeField.text, out int newValue))
        {
            if(hpChangeDropdown.value == 0){

                TakeDamage(newValue);
            }

            if(hpChangeDropdown.value == 1){

                HpHeal(newValue);
            }
            if(hpChangeDropdown.value == 2){

                CurrentMaxHpHeal(newValue);
            }
            if(hpChangeDropdown.value == 3){

                shield += newValue;
            }
        }
    }
}