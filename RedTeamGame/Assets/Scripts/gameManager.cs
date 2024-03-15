using System;
using System.Collections.Generic;
using TMPro;
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
    [Header("--shield--")]
    [SerializeField] Image LShield;
    [SerializeField] Image RShield;
    [Header("--prompt text--")]
    [SerializeField] TMP_Text popUp;
    [Header ("--gun ammo ui--")]
    [SerializeField] SpriteRenderer GunImg;
    [SerializeField] TMP_Text currentMagAmmo;
    [SerializeField] TMP_Text currentReserves;
    [Header("--timer--")]
    [SerializeField] TMP_Text timer;
    [SerializeField] TMP_Text winTime;
    [Header("--audio--")]
    [SerializeField] public AudioSource music;
    [SerializeField] public AudioClip[] backgroundMusic;
    [Range(0, 1)][SerializeField] public float backgroundMusicVol;
    [Header("--score--")]
    [SerializeField] int bonusScoreMultiplyer;
    [SerializeField] GameObject scoreParent;
    [SerializeField] TMP_Text scoreValue;
    [SerializeField] GameObject leaderBoard;
    [SerializeField] TMP_Text scorePreFap;
    [SerializeField] GameObject newScoreMenu;
    

    [Header("--player elements--")]
    //player related elements
    public GameObject player;
    public Image damageFlash;
    public Image damagePersist;
    public GameObject shieldDamage;
    public playerController playerScript;
    public GameObject playerSpawnPos;
    
    
    



    //public GameObject enemySpawnPos;
    public TimeSpan currentTime;// unity class to turn delta time into 
    public bool isPaused;
    public int currrentGunIndex;
    public int gunIndex;  
    public int enemyCount;
    public string playerInitals;
    int playerScore;
    float time;
    bool timerOn;
    bool popUpOn;
    bool leaderBoardOn;
    bool promptForInitalsOn;
    List<TMP_Text> scoreList;
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
        LShield.gameObject.SetActive(false);
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
        LShield.gameObject.SetActive(true);
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
    public void updateScore(int value)// updates the player score to the screen
    {
        playerScore += value;
        scoreValue.text = playerScore.ToString();
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
    public void updateAmmo()//updates the ammo ui
    {
        
        currentMagAmmo.text = playerScript.magazine.ToString();
        currentReserves.text = playerScript.reserves.ToString();
    }
    public void UpdateShiieldUi()//updates the player shield ui
    {
        float tmp = (int)playerScript.shieldAmount / playerScript.shieldAmountOrg;
        LShield.fillAmount = tmp;
        RShield.fillAmount = tmp;
    }
   
    public List<GameObject> findAllChild(GameObject parent)//find all the children of an object and return them as a list
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
        }
        return children;
    }

    public void popUpTxt(string text = "")//toggles a pop up text
    {
        popUp.gameObject.SetActive(!popUpOn);
        popUpOn = !popUpOn;
        popUp.text = text;
    }
    public void togglePromptNewScore()// toggles the score prompt
    {
        activeMenu.SetActive(promptForInitalsOn);
        newScoreMenu.SetActive(!promptForInitalsOn);
        promptForInitalsOn = !promptForInitalsOn;
    }
    public void toggleLeaderBoard()//toggles the leader board
    {
        activeMenu.SetActive(leaderBoard);
        leaderBoard.SetActive(!leaderBoardOn);
        leaderBoardOn = !leaderBoardOn;
    }
    public void saveScore(string s)
    {
        TMP_Text newScore = Instantiate(scorePreFap, scoreParent.transform);//create a new score
        
        double bonusScore = Math.Sqrt((int)currentTime.Minutes * 60) + Math.Sqrt((int)currentTime.Seconds);//gets sqrt of the current time as a numaric value in seconds(give or take a second)
        playerScore += (int)bonusScore * bonusScoreMultiplyer;//adds the bonus score multiplyed by a score multiplyer to the current player score
        newScore.text = s + " " + 1 + " " + playerScore + " " + time;//set the score to be the same as current score values
        scoreList.Add(newScore);//add the score refrence to the list
        sortByWinner();//re order the score board and list
        togglePromptNewScore();// turn off the prompt
        toggleLeaderBoard();// turn on the score board
    }
    public void sortByWinner()// sorts the leaderboard by highest score
    {
        List<int> tmpInt = new List<int>();
        List<TMP_Text> tmpScr = new List<TMP_Text>();
        foreach(TMP_Text score in scoreList)//for each score find the score and add it to a list
        {
            tmpInt.Add(findScore(score));
        }
        tmpInt.Sort();//sort the scores by lowest to highest
        tmpInt.Reverse();//reverse the scores to high to low
        tmpScr = scoreList;//make a copy of refrences to the scores
        scoreList.Clear();//clear out the refrences to the scores in score list
        int index = 0;//create an index to the first object
        while (index < tmpInt.Count)//while there is still a score
        {
            foreach (int score in tmpInt)//check each score
            {
                if(score == findScore(tmpScr[index]))//if the score is the current postion on the index
                {
                    tmpScr[index].text = alterPosistion(splitScore(tmpScr[index]), index);//change its postion on the leader board to be the index(so this will decrement from first place)
                    scoreList.Add(tmpScr[index]);//add it back to the score list 
                    ++index;//increase the current index
                }
            }
        }
        for (int i = scoreList.Count; i > 0; --i)//starting from the last score
        {
            scoreList[i].transform.SetAsFirstSibling();//set each score in assending order to be the frstplace (this will re arange them in the vertical list component)
        }
        
    }
    int findScore(TMP_Text score)//finds the score string and converts it to an int
    {
        string[] split = score.text.Split(new string[] { " ", "-" }, StringSplitOptions.None);
        return System.Convert.ToInt16(split[2]);
    }
    string[] splitScore(TMP_Text score)//splits a score into its base parts at delemanators
    {
        string[] split = score.text.Split(new string[] { " ", "-" }, StringSplitOptions.None);
        return split;
    }
    string alterPosistion(string[] strings, int pos)//takes a split score and alters the postition (first place, second place ext..)on the leader board by the passed in int
    {
        string returnString = "";// make a tmp string for return
        strings[1] = pos.ToString();//get the positon
        foreach (string s in strings)// for each part of the split string
        {
            returnString = returnString + " " + s;// add a space and the current string to itself
        }
        return returnString;// return the full stirng
    }
    public void saveScores()//saves scores on close
    {

    }
    public void loadScores()//loads scores on open
    { 

    }
}
