using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IvalueAugment : MonoBehaviour
{
    [SerializeField] TMP_Text textToChange;
    [SerializeField] Slider slider;
    public int sliderCurrentValue;

 
    public void setValue()
    { 
        if (textToChange != null)
        {
            textToChange.text = ((int)slider.value).ToString();
            sliderCurrentValue = (int)slider.value;
        }
    }

}
