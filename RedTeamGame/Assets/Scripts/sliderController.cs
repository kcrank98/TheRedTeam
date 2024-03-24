using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class sliderController : MonoBehaviour
{
    [SerializeField] TMP_Text textToChange;
    [SerializeField] TMP_Text lable;
    [SerializeField] public Slider slider;
    [Header("key to save/load option under")]
    [SerializeField] public string saveKey;
    public int currentValue;
    private void Start()
    {
        dataManager.instance.optionsSliders.Add(this);
        lable.text = saveKey;
        dataManager.instance.loadOptions();
    }

    public void setValue()
    { 
        if (textToChange != null)
        {
            textToChange.text = ((int)slider.value).ToString();
            currentValue = (int)slider.value;
        }
    }

}
