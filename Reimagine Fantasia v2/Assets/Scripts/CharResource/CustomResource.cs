using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomResource : MonoBehaviour
{
    [System.Serializable]
    public class CustomSlider
    {
        public Slider slider;
        public TMP_Text text;
        public TMP_InputField maxValueInput;
        public TMP_InputField minValueInput;
        public float currentValue;
        public TMP_InputField resourceName;
    }

    public CustomSlider customSlider1;
    //public CustomSlider customSlider2;

    void Start()
    {
        customSlider1.maxValueInput.onEndEdit.AddListener(delegate { UpdateMinMax(customSlider1, true); });
        customSlider1.minValueInput.onEndEdit.AddListener(delegate { UpdateMinMax(customSlider1, false); });

        //customSlider2.maxValueInput.onEndEdit.AddListener(delegate { UpdateMinMax(customSlider2, true); });
        //customSlider2.minValueInput.onEndEdit.AddListener(delegate { UpdateMinMax(customSlider2, false); });

        customSlider1.slider.onValueChanged.AddListener(value => UpdateSlider(customSlider1, value));
        //customSlider2.slider.onValueChanged.AddListener(value => UpdateSlider(customSlider2, value));

        customSlider1.currentValue = 0;
        customSlider1.minValueInput.text = "0";
        customSlider1.maxValueInput.text = "10";
    }

    public void UpdateSlider(CustomSlider customSlider, float value)
    {
        customSlider.currentValue = value;
        UpdateCustomText(customSlider);
    }

    public void UpdateMinMax(CustomSlider customSlider, bool isMax)
    {
        TMP_InputField inputField = isMax ? customSlider.maxValueInput : customSlider.minValueInput;

        if (int.TryParse(inputField.text, out int newValue))
        {
            if (isMax)
            {
                customSlider.slider.maxValue = newValue;
            }
            else
            {
                customSlider.slider.minValue = newValue;
            }
        }

        UpdateCustomText(customSlider);
    }

    void UpdateCustomText(CustomSlider customSlider)
    {
        customSlider.text.text = $"{customSlider.currentValue} / {customSlider.slider.maxValue}";
    }
}
