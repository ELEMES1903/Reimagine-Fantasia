using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class HPArray
{
    public string name;
    public TMP_InputField hpInputField;
    public TMP_Text inputText;
    public TMP_Text finalHpText;
    public int hpValue;
    
}
public class HealthBar : MonoBehaviour
{
    public HPArray[] hpUI;

    // HP Bar UI components
    //public Slider hpSlider;
    //public Slider currentMaxHpSlider;
    //public Gradient gradient;   
    //public Image fill;

    public Button increaseHp;
    public Button decreaseHP;
    public Toggle isCurrentMaxHp;

    void Start()
    {
        //fill.color = gradient.Evaluate(1f);

        increaseHp.onClick.AddListener(delegate { ChangeHpBy1(true); });
        decreaseHP.onClick.AddListener(delegate { ChangeHpBy1(false); });

        foreach (HPArray element in hpUI)
        {
            element.hpInputField.onEndEdit.AddListener(delegate { OnHPInputValueChanged(element); });
            element.hpInputField.onSelect.AddListener(delegate { OnInputFieldFocused(element); });
            element.hpInputField.text = "10";
            element.inputText.text = "10";
            element.finalHpText.text = "10";
            element.hpValue = 10;

            OnInputFieldFocused(element);
        }
        UpdateHealthBars();
    }

    void OnInputFieldFocused(HPArray element)
    {
        element.inputText.gameObject.SetActive(false);
        element.finalHpText.gameObject.SetActive(true);
    }

    public void OnHPInputValueChanged(HPArray element)
    {
        element.inputText.gameObject.SetActive(true);
        element.finalHpText.gameObject.SetActive(false);

        if (int.TryParse(element.hpInputField.text, out int newValue))
        {
            if(element.name == "Shield")
            {
                if(newValue > element.hpValue)
                {
                    element.hpValue = newValue;
                }
            }
            element.hpValue = newValue;
        }
        UpdateHealthBars();
    }

    public void TakeDamage(int damage)
    {
        if (FindHPElement("Shield").hpValue > 0)
        {
            FindHPElement("Shield").hpValue -= damage;
        }
        else
        {
            ApplyDamageToHP(damage);
        }
        UpdateHealthBars();
    }

    private void ApplyDamageToHP(int damage)
    {        
        if (FindHPElement("HP").hpValue > 0)
        {
            FindHPElement("HP").hpValue -= damage;
        }
        else
        {
            ApplyDamageToMaxHP(damage);
        }
        UpdateHealthBars();
    }

    private void ApplyDamageToMaxHP(int damage)
    {
        FindHPElement("Current Max HP").hpValue -= damage;
        UpdateHealthBars();
    }

    public void HpHeal(int healAmount)
    {
        FindHPElement("HP").hpValue += healAmount;
        UpdateHealthBars();
    }

    public void CurrentMaxHpHeal(int healAmount)
    {
        FindHPElement("Current Max HP").hpValue += healAmount;
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
            if(isCurrentMaxHp.isOn)
            {
                ApplyDamageToMaxHP(1);
            }
            else
            {
                TakeDamage(1);
            }
        }
    }

    public void UpdateHealthBars()
    {
        HPArray HP = FindHPElement("HP");
        HPArray currentMaxHP = FindHPElement("Current Max HP");
        HPArray maxHP = FindHPElement("Max HP");
        HPArray shield = FindHPElement("Shield");
        
        currentMaxHP.hpValue = Mathf.Clamp(currentMaxHP.hpValue, 0, maxHP.hpValue);
        HP.hpValue = Mathf.Clamp(HP.hpValue, 0, currentMaxHP.hpValue);
        shield.hpValue = Mathf.Clamp(shield.hpValue, 0, maxHP.hpValue);

        foreach (HPArray element in hpUI)
        {
            if(element.name == "Shield")
            {
                element.finalHpText.text = "(" + element.hpValue.ToString() + ")";
            }
            else
            {
                element.finalHpText.text = element.hpValue.ToString();
            }
        }
        //hpSlider.value = currentHP;
        //currentMaxHpSlider.value = currentMaxHP;
        //fill.color = gradient.Evaluate(hpSlider.normalizedValue);
    }

    public HPArray FindHPElement(string hpType)
    {
        foreach (HPArray element in hpUI)
        {
            if(element.name == hpType)
            {
                return element;
            }
        }
        return null;
    }
}