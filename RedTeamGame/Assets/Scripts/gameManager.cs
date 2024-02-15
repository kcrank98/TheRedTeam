using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject winMenu;

    [SerializeField] TMP_Text timer;

    public GameObject player;
    public Image damageFlash;
    public Image damagePersist;
    public GameObject shieldDamage;
    public playerController playerScript;
    public GameObject playerSpawnPos;
    public GameObject enemySpawnPos;
    public TimeSpan currentTime;// unity class to turn delta time into 
    public bool isPaused;
    int enemyCount;
    float time;
    bool timerOn;
    //awake will run before any other call crating this object before anything needs to use it
    void Awake()
    {
        //create the manager and find the object that will be our player in any given instance by tag
        //also find the players script and both enemy and player spawn postitons
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        timerStart();
        //playerSpawnPos = gameObject.FindWithTag("Player Spawn Pos");
        //enemySpawnPos = gameObject.FindWithTag("Enemy Spawn Pos");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)//if the escape key is pressed and there is no open menu
        {
            statePaused();//pause the game
            activeMenu = pauseMenu;//current menu is the pause menu
            activeMenu.SetActive(true);// bring it to the screen
        }
        timerUpdate();

    }
    public void statePaused()//sets the game state into a paused state and brings back mouse
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        timerStop();

    }
    public void stateUnPaused()//sets the game state into a running state and removes mouse
    {
        isPaused = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);//turns off the current menu
        activeMenu = null;// there is no menu open anymore
        timerStart();
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
    // pause the timer
    public void timerStart()
    {
        timerOn = !timerOn;
    }
    // start or resume timer
    public void timerStop()
    {
        timerOn = !timerOn;
    }
    //updates timer on screen and the current time 
    public void timerUpdate()
    {
        if (timerOn)
        {
            time += Time.deltaTime;
        }
        currentTime = TimeSpan.FromSeconds(time);
        timer.text = currentTime.Minutes + ":" + currentTime.Seconds + ":" + currentTime.Milliseconds;
    }

}
