using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject options;
    [SerializeField] GameObject credits;
    bool menuToggled;
    void Start()
    {
        
    }
    public void toggleOptions()//toggles options
    {
        mainMenu.SetActive(menuToggled);
        options.SetActive(!menuToggled);
        menuToggled = !menuToggled;
    }
    public void toggleCredits()//toggles credits
    {
        mainMenu.SetActive(menuToggled);
        credits.SetActive(!menuToggled);
        menuToggled= !menuToggled;

    }
    public void saveOptions()
    {
        //options to save out
    }
    private void loadOptions()
    {
        //set options from file
    }

}
