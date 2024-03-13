using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    //menu elements
    [Header ("--menu elements--")]
    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] Image LShield;
    [SerializeField] Image RShield;
    //gun ui elements
    [Header ("--gun ammo ui--")]
    [SerializeField] SpriteRenderer GunImg;
    [SerializeField] TMP_Text currentMagAmmo;
    [SerializeField] TMP_Text currentReserves;

    //timer
    [Header("--timer--")]
    [SerializeField] TMP_Text timer;
    [SerializeField] TMP_Text winTime;

    //music
    [Header("--audio--")]
    [SerializeField] public AudioSource music;
    [SerializeField] public AudioClip[] backgroundMusic;
    [Range(0, 1)][SerializeField] public float backgroundMusicVol;

    //score
    [Header("--score--")]
    [SerializeField] TMP_Text scoreValue;

    [Header("--player elements--")]
    //player related elements
    public GameObject player;
    public Image damageFlash;
    public Image damagePersist;
    public GameObject shieldDamage;
    public playerController playerScript;
    public GameObject playerSpawnPos;
   
    //gun elements
    //public Dictionary<GameObject,List<GameObject>> gunMags = new Dictionary<GameObject, List<GameObject>>();//refreince to gun mags
    //public Dictionary<GameObject,int> currentMagBullet = new Dictionary<GameObject,int>();//current ammo used per gun
    //public List<GameObject> guns = new List<GameObject>();//refrence to each ui element




    //public GameObject enemySpawnPos;
    public TimeSpan currentTime;// unity class to turn delta time into 
    public bool isPaused;
    public int currrentGunIndex;
    public int gunIndex;  
    public int enemyCount;
    int score;
    float time;
    bool timerOn;
    //awake will run before any other call crating this object before anything needs to use it
    void Awake()
    {
        //create the manager and find the object that will be our player in any given instance by tag
        //also find the players script and both enemy and player spawn postitons
        instance = this;
        //enemyCount = -1;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        timerStart();
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        
        //enemySpawnPos = GameObject.FindWithTag("Enemy Spawn Pos");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)//if the escape key is pressed and there is no open menu
        {
            statePaused();//pause the game
            activeMenu = pauseMenu;//current menu set to pause menu
            activeMenu.SetActive(true);// bring it to the screen
        }
        if(Input.GetButtonDown("R") && !isPaused)//if the reload key is pressed and there is an active gun
        {
            playerScript.reloadGun();
            //reloadMag();//reload gun(will not work while paused)
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
    public void updateGameGoal(int enemyTotal)//will activly alter the total score until win or loss (same code as class for now)
    {
        enemyCount += enemyTotal;// determent the current score(in this case number of enemys)
        //floor.enemyCount--;
        if (enemyCount <= 0)// if there are no enemys its a win
        {
           
            activeMenu = winMenu;
            winTime.text = currentTime.Minutes + ":" + currentTime.Seconds; 
            activeMenu.SetActive(true);

            statePaused();
        }
    }
    public void updateScore(int value)
    {
        score += value;
        scoreValue.text = score.ToString();
    }
  
    public void youLose()//on a player death show a loss menu
    {
        statePaused();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }
    public void timerStart() //pause the timer
    {
        timerOn = !timerOn;
    }
    public void timerStop()// start or resume timer
    {
        timerOn = !timerOn;
    }
    public void timerUpdate()//updates timer on screen and the current time 
    {
        if (timerOn)
        {
            time += Time.deltaTime;
        }
        currentTime = TimeSpan.FromSeconds(time);
        timer.text = currentTime.Minutes + ":" + currentTime.Seconds;
    }
    public void setActiveGun()//sets the current gun both for ammo tracking and ui
    {
      //GunImg.sprite = playerScript.gunUI
        if(!currentMagAmmo.IsActive())
        {
            currentMagAmmo.gameObject.SetActive(true);
            currentReserves.gameObject.SetActive(true);
        }
        updateAmmo();
      
    }
    public void updateAmmo()
    {
        
        currentMagAmmo.text = playerScript.magazine.ToString();
        currentReserves.text = playerScript.reserves.ToString();
    }
    public void UpdateShiieldUi()
    {
        float tmp = playerScript.shieldAmount / playerScript.shieldAmountOrg;
        LShield.fillAmount = tmp;
        RShield.fillAmount = tmp;
    }
   
    List<GameObject> findAllChild(GameObject parent)//find all the children of an object and return them as a list
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
        }
        return children;
    }
   
}
