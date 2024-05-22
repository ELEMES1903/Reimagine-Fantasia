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
        public float currentValue;
        public float maxValue;
        public float minValue;
        public TMP_InputField resourceName;
        public int resourceNumber;
    }
public class CustomResource : MonoBehaviour
{

    public CustomResourceArray[] customResource;
    public TMP_Dropdown selectResource;
    public TMP_Dropdown resourceUpdateOptions;
    public TMP_InputField resourceUpdateInput;
    public Button applyResourceChange;
    void Start()
    {
        resourceUpdateInput.characterLimit = 4;
        applyResourceChange.onClick.AddListener(delegate{UpdateResource();});

        // Create a list of custom entries
        List<string> options = new List<string>();
        options.Add("Set Max");
        options.Add("Set Min");
        options.Add("Set CUrrent");
        resourceUpdateOptions.ClearOptions(); // Clear existing options
        resourceUpdateOptions.AddOptions(options); // Add the custom entries to the dropdown

        // Create a list of custom entries
        List<string> resources = new List<string>();
        resources.Add("1");
        resources.Add("2");
        resources.Add("3");
        resources.Add("4");
        selectResource.ClearOptions(); // Clear existing options
        selectResource.AddOptions(resources); // Add the custom entries to the dropdown

        foreach (CustomResourceArray resource in customResource)
        {
            resource.slider.onValueChanged.AddListener((float newValue) => UpdateSliderValue(resource, newValue));
            resource.currentValue = 0;
        } 
    }

    void UpdateSliderValue(CustomResourceArray resource, float newValue)
    {
        resource.currentValue = newValue;
        UpdateCustomText(); 
    }
    public void UpdateResource()
    {
        foreach(CustomResourceArray resource in customResource)
        {
            if(selectResource.value + 1 == resource.resourceNumber)
            {
                if (int.TryParse(resourceUpdateInput.text, out int newValue))
                {
                    if(resourceUpdateOptions.value == 0)
                    {
                        resource.maxValue = newValue;
                    }
                    else if(resourceUpdateOptions.value == 1)
                    {
                        resource.minValue = newValue;
                    }
                    else if (resourceUpdateOptions.value == 2)
                    {
                        resource.currentValue = newValue;
                    }
                }
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
