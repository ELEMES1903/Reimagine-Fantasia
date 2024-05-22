using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class AbilityTextArray
{
    public TMP_InputField name;
    public TMP_InputField tierSetType;
    public TMP_InputField castRules;
    public TMP_InputField abilityText;
}

public class KitTextManager : MonoBehaviour
{
    public AbilityTextArray[] Ability;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
