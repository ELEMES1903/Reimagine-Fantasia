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
    public HPUIElement[] hpUI;

    // HP Bar UI components
    public Slider hpSlider;
    public Slider currentMaxHpSlider;
    public Gradient gradient;   
    public Image fill;

    // Integers
    public int maxHP;
    public int currentMaxHP;
    public int currentHP;
    public int shield;

    // UI Components
    public TMP_InputField hpInput;
    public TMP_InputField currentMaxHpInput;
    public TMP_InputField maxHpInput;
    public TMP_Text allHpText;
    public TMP_InputField hpChangeField;
    public TMP_Dropdown hpChangeDropdown;
    public Button hpChangeApply;
    public Button increaseHp;
    public Button decreaseHP;
    public Toggle isCurrentMaxHp;
    void Start()
    {
        // Initialize values
        maxHP = 10;
        currentMaxHP = maxHP;
        currentHP = maxHP;

        fill.color = gradient.Evaluate(1f);

        hpChangeApply.onClick.AddListener(delegate { ApplyHpChange(); });
        increaseHp.onClick.AddListener(delegate { ChangeHpBy1(true); });
        decreaseHP.onClick.AddListener(delegate { ChangeHpBy1(false); });

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
        //If there is shield
        if (shield > 0)
        {
            //shield takes damage first
            shield -= damage;
            //if there is still more damage shield cant take
            if (shield < 0)
            {
                //find the amount of left over damage
                int leftOverDmg = -shield;
                shield = 0;
                //apply leftover damage to Hp
                ApplyDamageToHP(leftOverDmg);
            }
        }
        else
        {
            ApplyDamageToHP(damage);
        }
        UpdateHealthBars();
    }

    private void ApplyDamageToHP(int damage)
    {
        if (currentHP > 0)
        {
            currentHP -= damage;
            if (currentHP < 0)
            {
                int leftOverDmg = -currentHP;
                currentHP = 0;
                ApplyDamageToMaxHP(leftOverDmg);
            }
        }
        else
        {
            ApplyDamageToMaxHP(damage);
        }
    }

    private void ApplyDamageToMaxHP(int damage)
    {
        currentMaxHP -= damage;
        currentMaxHP = Mathf.Clamp(currentMaxHP, 0, maxHP);
    }

    public void HpHeal(int healAmount)
    {
        currentHP += healAmount;
        currentHP = Mathf.Clamp(currentHP, 0, currentMaxHP);
        UpdateHealthBars();
    }

    public void CurrentMaxHpHeal(int healAmount)
    {
        currentMaxHP += healAmount;
        currentMaxHP = Mathf.Clamp(currentMaxHP, 0, maxHP);
        UpdateHealthBars();
    }

    public void ChangeHpBy1(bool isAdding)
    {
        if(isAdding)
        {
            if(isCurrentMaxHp.isOn)
            {
                CurrentMaxHpHeal(1);
            }
            else
            {
                HpHeal(1);
            }
        }
        else
        {
            TakeDamage(1);
        }
    }

    public void GainShield(int shieldAmount)
    {
        if(shield < shieldAmount)
        {
            shield = shieldAmount;
        }
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

        fill.color = gradient.Evaluate(hpSlider.normalizedValue);

        allHpText.text = $"( <b><color=#00FFFF>{shield}</color></b> )  <b><color=#FF6666>{currentHP}</color></b> / <b><color=#FF6666>{currentMaxHP}</color></b> / <b><color=#FF6666>{maxHP}</color></b>";
    }

    public void ApplyHpChange()
    {
        if (int.TryParse(hpChangeField.text, out int newValue))
        {
            if(hpChangeDropdown.value == 0)
            {
                TakeDamage(newValue);
            }

            if(hpChangeDropdown.value == 1)
            {
                HpHeal(newValue);
            }
            if(hpChangeDropdown.value == 2)
            {
                CurrentMaxHpHeal(newValue);
            }
            if(hpChangeDropdown.value == 3)
            {
                GainShield(newValue);
            }
        }
    }
}