using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HPUIElement
{
    public string name;
    public TMP_InputField hpInputField;
    public TMP_Text inputText;
    
}

public class HealthBar : MonoBehaviour
{
    public Slider hpSlider;
    public Slider currentMaxHpSlider;
    public HPUIElement[] hpUI;

    public float maxHP;
    public float currentMaxHP;
    public float currentHP;

    void Start()
    {
        // Initialize values
        maxHP = 100f;
        currentMaxHP = maxHP;
        currentHP = maxHP;
        

        foreach (HPUIElement element in hpUI)
        {
            // Add listener for input field changes
            element.hpInputField.onValueChanged.AddListener(delegate { OnHPInputValueChanged(element); });
        }

        // Set slider max values
        hpSlider.maxValue = maxHP;
        currentMaxHpSlider.maxValue = maxHP;

        // Update slider values
        UpdateHealthBars();
    }

    void OnHPInputValueChanged(HPUIElement element)
    {
        if (int.TryParse(element.hpInputField.text, out int newValue))
        {
            if(element.name == "HP"){
                currentHP = newValue;
            }
            if(element.name == "Current Max HP"){
                currentMaxHP = newValue;
            }
            if(element.name == "Max HP"){
                maxHP = newValue;
            }
            UpdateHealthBars();
        }
    }

    public void TakeDamage(float damage, bool isHPvalue)
    {

        if(isHPvalue == true){

            currentHP -= damage;
            // Ensure HP doesn't go below 0
            currentHP = Mathf.Max(currentHP, 0f);
            
        } else {

            currentMaxHP -= damage;
            // Ensure current max HP doesn't go below 0
            currentMaxHP = Mathf.Max(currentMaxHP, 0f);

        }
        
        if(currentHP > currentMaxHP){
            currentHP = currentMaxHP;
        }

        // Update health bars
        UpdateHealthBars();
    }

    public void Heal(float heal, bool isHPvalue)
    {

        if(isHPvalue == true){

            currentHP += heal;
            // Ensure HP doesn't exceed current max HP
            currentHP = Mathf.Min(currentHP, currentMaxHP);
            
        } else {

            currentMaxHP += heal;
            // Ensure current max HP doesn't exceed max HP
            currentMaxHP = Mathf.Max(currentMaxHP, maxHP);

        }

        // Update health bars
        UpdateHealthBars();
    }

    void UpdateHealthBars()
    {

        if(currentHP > currentMaxHP){ currentHP = currentMaxHP; }
        if(currentMaxHP > maxHP){ currentMaxHP = maxHP; }

        hpSlider.value = currentHP;
        hpSlider.maxValue = maxHP;
        currentMaxHpSlider.value = currentMaxHP;
        currentMaxHpSlider.maxValue = maxHP;
    }
}