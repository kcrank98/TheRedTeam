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
    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject winMenu;
    //gun ui elements
    [SerializeField] GameObject activeGun;
    [SerializeField] GameObject pistol;
    [SerializeField] GameObject rifle;
    [SerializeField] GameObject sniper;
    [SerializeField] GameObject shotgun;
    //timer
    [SerializeField] TMP_Text timer;
    [SerializeField] TMP_Text winTime;
    [SerializeField] AudioSource music;
    [SerializeField] floorManager floor;
    //score
    [SerializeField] TMP_Text scoreValue;
    //player related elements
    public GameObject player;
    public Image damageFlash;
    public Image damagePersist;
    public GameObject shieldDamage;
    public playerController playerScript;
    public GameObject playerSpawnPos;
   
    //gun elements
    public Dictionary<GameObject,List<GameObject>> gunMags = new Dictionary<GameObject, List<GameObject>>();//refreince to gun mags
    public Dictionary<GameObject,int> currentMagBullet = new Dictionary<GameObject,int>();//current ammo used per gun
    public List<GameObject> guns = new List<GameObject>();//refrence to each ui element




    //public GameObject enemySpawnPos;
    public TimeSpan currentTime;// unity class to turn delta time into 
    public bool isPaused;
    public int currrentGunIndex;
    public int gunIndex;  
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
        if(Input.GetButtonDown("R") && activeGun != null)//if the reload key is pressed and there is an active gun
        {
            reloadMag();//reload gun(will not work while paused)
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
        floor.enemyCount--;
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
        scoreValue.text = value.ToString();
    }
    public void updateFloor()
    {
        floor.enemyCount--;
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
    public void setActiveGun(GameObject gun)//sets the current gun both for ammo tracking and ui
    {
        if(activeGun != null)//if there is an active gun
        {
            activeGun.SetActive(false);//turn off the active gun
        }
        activeGun = gun;//set the active gun to the new gun
        activeGun.SetActive(true);//turn on the new gun
    }
    public bool updateBullet()//call in an "if" statment if the gun has ammo or not returns a bool to reflect
        //will also return false if there is no active gun
    {
        bool hasAmmo = true;
        if (activeGun != null && !isPaused)//if there is a gun and game is not paused
        {
            if (hasAmmo)//if the current bullet is not more than the mags maximum
            {
                gunMags[activeGun][currentMagBullet[activeGun]].SetActive(false);//turn off the current bullet chambered
                if (currentMagBullet[activeGun] != gunMags[activeGun].Count() - 1)
                {
                    currentMagBullet[activeGun]++;
                }
                //set the current bullet for active guns mag
                return hasAmmo;//return that the gun still has ammo
            }
        }
        return !hasAmmo;//else return there is no ammo
    }
    public bool hasAmmo()
    {
        if (currentMagBullet[activeGun] <= gunMags[activeGun].Count())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void reloadMag()//re-sets a mag for the active weapon
    {
        if(activeGun != null && !isPaused)//if there is a gun and the game is not paused
        {
            for (int i = 0; i < gunMags[activeGun].Count(); i++)//while the iterator is not the last bullet
            {
                gunMags[activeGun][i].SetActive(true);//set the current iterated bullet to on
            }
            currentMagBullet[activeGun] = 0;//when the mag is done resetng reset the current bullet counter
        }
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
    public GameObject addGunUi(string gunName)
    {
       
            if(gunName ==  "Pistol")
            {
                gunMags.Add(pistol, findAllChild(pistol.transform.GetChild(0).gameObject));
                currentMagBullet.Add(pistol, 0);
                guns.Add(pistol);
                return pistol;
            }
            else if(gunName == "Rifle")
            {
                gunMags.Add(rifle,findAllChild(rifle.transform.GetChild(0).gameObject));
                currentMagBullet.Add(rifle, 0);
                guns.Add(rifle);
                return rifle;
            }
            else if(gunName == "Shotgun")
            {
                gunMags.Add(shotgun,findAllChild(shotgun.transform.GetChild(0).gameObject));
                currentMagBullet.Add(shotgun, 0);
                guns.Add(shotgun);
                return shotgun;
            }
            else if(gunName == "Sniper")
            {
                gunMags.Add(sniper,findAllChild(sniper.transform.GetChild(0).gameObject));
                currentMagBullet.Add(sniper, 0);
                guns.Add(sniper);
                return sniper;
            }
        return null;
    }
}
