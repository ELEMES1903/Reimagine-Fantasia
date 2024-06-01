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

    public int hp;
    public int currentHp;
    public int maxHp;
    public int shield;

    void Start()
    {
        //fill.color = gradient.Evaluate(1f);

        increaseHp.onClick.AddListener(delegate { ChangeHpBy1(true); });
        decreaseHP.onClick.AddListener(delegate { ChangeHpBy1(false); });

        foreach (HPArray element in hpUI)
        {
            element.hpInputField.onEndEdit.AddListener(delegate { OnHPInputValueChanged(element); });

            element.hpInputField.onEndEdit.AddListener(delegate { OnInputFieldFocusLost(element); });
            element.hpInputField.onSelect.AddListener(delegate { OnInputFieldFocused(element); });

            OnInputFieldFocusLost(element);
        }

        UpdateHealthBars();
    }

    void OnInputFieldFocused(HPArray element)
    {
        element.inputText.gameObject.SetActive(true);
        element.finalHpText.gameObject.SetActive(false);
    }

    void OnInputFieldFocusLost(HPArray element)
    {
        element.inputText.gameObject.SetActive(false);
        element.finalHpText.gameObject.SetActive(true);
    }

    public void OnHPInputValueChanged(HPArray element)
    {
        if (int.TryParse(element.hpInputField.text, out int newValue))
        {
            if(element.name == "Shield")
            {
                if(shield < newValue)
                {
                    shield = newValue;
                }
            }
            else if(element.name == "HP")
            {
                hp = newValue;
            }
            else if(element.name == "Current Max HP")
            {
                currentHp = newValue;
            }
            else if(element.name == "Max HP")
            {
                maxHp = newValue;
            }
            
        }
        UpdateHealthBars();
    }

    public void ChangeHpBy1(bool isAdding)
    {
        if(isAdding)
        {
            if(isCurrentMaxHp.isOn)
            {
                currentHp++;
            }
            else
            {
                hp++;
            }
        }
        else
        {
            if(isCurrentMaxHp.isOn)
            {
                currentHp--;
            }
            else
            {
                hp--;
            }
        }
        UpdateHealthBars();
    }

    public void UpdateHealthBars()
    {
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        hp = Mathf.Clamp(hp, 0, currentHp);
        shield = Mathf.Clamp(shield, 0, 100);

        FindHPElement("HP").hpValue = hp;
        FindHPElement("Current Max HP").hpValue = currentHp;
        FindHPElement("Max HP").hpValue = maxHp;
        FindHPElement("Shield").hpValue = shield;

        foreach(HPArray element in hpUI)
        {
            element.finalHpText.text = element.hpValue.ToString();
        }
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