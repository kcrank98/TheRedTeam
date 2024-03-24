using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class dataManager : MonoBehaviour
{   public static dataManager instance;
    public List<sliderController> optionsSliders;
    void Awake()
    {
        instance = this;//malke singleton
        optionsSliders = new List<sliderController>();//make a list of all the options sliders (they add them selfs)
    }
    public void saveOptions()
    {
        foreach (var sliderController in optionsSliders)//for each slider
        {
            PlayerPrefs.SetInt(sliderController.saveKey, sliderController.currentValue);//save the options by the set key
        }
        PlayerPrefs.Save();//save to disk
    }
    public void loadOptions()
    {
        foreach(var sliderController in optionsSliders)//for each slider
        {
            sliderController.slider.value = PlayerPrefs.GetInt(sliderController.saveKey);//get there previous values and set them visualy
            sliderController.currentValue = (int)sliderController.slider.value;//set the value copy as well
        }
    }
   
}
