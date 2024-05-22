using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;


[System.Serializable]
    public class CustomResourceArray
    {
        public Slider slider;
        public TMP_Text text;
        public TMP_InputField sliderUpdateInput;
        public TMP_Dropdown sliderUpdateOptions;
        public float currentValue;
        public float maxValue;
        public float minValue;
        public TMP_InputField resourceName;
    }
public class CustomResource : MonoBehaviour
{
    public CustomResourceArray[] customResource;

    void Start()
    {
        foreach (CustomResourceArray resource in customResource)
        {
            resource.sliderUpdateInput.onEndEdit.AddListener((string newValue) => UpdateSliderRange(resource));
            resource.slider.onValueChanged.AddListener((float newValue) => UpdateSliderValue(resource, newValue));
            resource.currentValue = 0;

            // Create a list of custom entries
            List<string> options = new List<string>();
            options.Add("Set Max");
            options.Add("Set Min");
            options.Add("Set CUrrent");
            
            resource.sliderUpdateOptions.ClearOptions(); // Clear existing options
            // Add the custom entries to the dropdown
            resource.sliderUpdateOptions.AddOptions(options);
        } 
    }

    void UpdateSliderValue(CustomResourceArray resource, float newValue)
    {
        resource.currentValue = newValue;
        UpdateCustomText(); 
    }
     public void UpdateSliderRange(CustomResourceArray resource)
    {

        if (int.TryParse(resource.sliderUpdateInput.text, out int newValue))
        {
            if(resource.sliderUpdateOptions.value == 0)
            {
                resource.maxValue = newValue;
            } 
            else if(resource.sliderUpdateOptions.value == 1)
            {
                resource.minValue = newValue;
            }
            else
            {
                resource.currentValue = newValue;
            }
        }

        UpdateCustomText();
    }

    public void UpdateCustomText()
    {
        foreach(CustomResourceArray resource in customResource)
        {
            resource.slider.maxValue = resource.maxValue;
            resource.slider.minValue = resource.minValue;
            resource.slider.value = resource.currentValue;
            resource.text.text = $"{resource.currentValue} / {resource.maxValue}";
        }
    }
}
