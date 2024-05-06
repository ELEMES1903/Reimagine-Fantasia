using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider hpSlider;
    public Slider maxHpSlider;

    private float maxHP;
    private float currentMaxHP;
    private float currentHP;

    private bool isHPvalue;

    void Start()
    {
        // Initialize values
        maxHP = 100f;
        currentMaxHP = maxHP;
        currentHP = maxHP;
        isHPvalue = true;

        // Set slider max values
        hpSlider.maxValue = maxHP;
        maxHpSlider.maxValue = maxHP;

        // Update slider values
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

            currentMaxHP -= heal;
            // Ensure current max HP doesn't exceed max HP
            currentMaxHP = Mathf.Max(currentMaxHP, maxHP);

        }

        // Update health bars
        UpdateHealthBars();
    }

    void UpdateHealthBars()
    {
        hpSlider.value = currentHP;
        maxHpSlider.value = currentMaxHP;
        
    }
}
