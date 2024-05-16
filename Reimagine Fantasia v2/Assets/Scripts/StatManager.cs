using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
    public class StatArray
    {
        public string name;
        public TMP_InputField statInput;
        public Modifier[] modifiers;
    }

public class StatManager : MonoBehaviour
{
    public StatArray[] stats;

    void Start (){

        
    }
}
