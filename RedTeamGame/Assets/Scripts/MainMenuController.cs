using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController instance;
    // Start is called before the first frame update
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject options;
    [SerializeField] GameObject credits;
    bool menuToggled;
    void Start()
    {
        instance = this;
    }
    public void toggleOptions()//toggles options
    {
        mainMenu.SetActive(menuToggled);//turn off the main menu
        options.SetActive(!menuToggled);//turn on the options
        menuToggled = !menuToggled;//optins are on, toggle bool to true for menu up or false for menu down
        if(!menuToggled) 
        {
            dataManager.instance.saveOptions();
        }
    }
    public void toggleCredits()//toggles credits
    {
        mainMenu.SetActive(menuToggled);
        credits.SetActive(!menuToggled);
        menuToggled= !menuToggled;
    }
   

}
