using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HPUIElement
{
    public string name;
    public TMP_InputField hitPointsInput;
    public GameObject inputText;
    public TMP_Text hitPointsText;
    
}

public class HealthBar : MonoBehaviour
{
    public Slider hpSlider;
    public Slider currentMaxHpSlider;
    public HPUIElement[] hpUI;

    public float maxHP;
    public float currentMaxHP;
    public float currentHP;
    public TMP_InputField hpInput;
    public TMP_InputField currentMaxHpInput;
    public TMP_InputField maxHpInput;
    

    void Start()
    {
        // Initialize values
        maxHP = 10f;
        currentMaxHP = maxHP;
        currentHP = maxHP;
        

        foreach (HPUIElement element in hpUI)
        {
            // Add listener for input field changes
            element.hitPointsInput.onEndEdit.AddListener(delegate { OnHPInputValueChanged(element); });
        }

        // Update slider values
        UpdateHealthBars();
    }

    void Update(){

        foreach (HPUIElement element in hpUI)
        {
            if(element.hitPointsInput.isFocused){

                element.hitPointsText.gameObject.SetActive(false);
                element.inputText.SetActive(true);

            } else {
                element.hitPointsText.gameObject.SetActive(true);
                element.inputText.SetActive(false);
            }
        }

    }


    public void OnHPInputValueChanged(HPUIElement element)
    {
        if (int.TryParse(element.hitPointsInput.text, out int newValue))
        {
            if(element.name == "HP"){ currentHP = newValue;}
            if(element.name == "Current Max HP"){ currentMaxHP = newValue;}
            if(element.name == "Max HP"){ maxHP = newValue;}
        }

        UpdateHealthBars();
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
        UpdateHealthBars();
    }

    public void UpdateHealthBars()
    {
        if(currentHP > currentMaxHP){ currentHP = currentMaxHP; }
        if(currentMaxHP > maxHP){ currentMaxHP = maxHP; }

        foreach (HPUIElement element in hpUI)
        {
            if(element.name == "HP"){ element.hitPointsText.text = currentHP.ToString();}
            if(element.name == "Current Max HP"){ element.hitPointsText.text = currentMaxHP.ToString();}
            if(element.name == "Max HP"){ element.hitPointsText.text = maxHP.ToString();}
        }

        hpSlider.value = currentHP;
        hpSlider.maxValue = maxHP;
        currentMaxHpSlider.value = currentMaxHP;
        currentMaxHpSlider.maxValue = maxHP;

    }
}