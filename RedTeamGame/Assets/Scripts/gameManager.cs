using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject winMenu;

    public GameObject player;
    public bool isPaused;
    int enemyCount;

    //awake will run before any other call crating this object before anything needs to use it
    void Awake()
    {
        //create the manager and find the object that will be our player in any given instance by tag
        instance = this;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel") && activeMenu == null)//if the escape key is pressed and there is no open menu
        {
            statePaused();//pause the game
            activeMenu = pauseMenu;//current menu is the pause menu
            activeMenu.SetActive(isPaused);// bring it to the screen
        }
    }
    public void statePaused()//sets the game state into a paused state and brings back mouse
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void stateUnPaused()//sets the game state into a running state and removes mouse
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);//turns off the current menu
        activeMenu = null;// there is no menu open anymore
    }
    public void updateGameGoal(int score)//will activly alter the total score until win or loss (same code as class for now)
    {
        enemyCount += score;// determent the current score(in this case number of enemys)
        if (enemyCount <= 0)// if there are no enemys its a win
        {
            activeMenu = winMenu;
            activeMenu.SetActive(true);
            statePaused();
        }
    }
    public void youLose()//on a player death show a loss menu
    {
        statePaused();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }
}
