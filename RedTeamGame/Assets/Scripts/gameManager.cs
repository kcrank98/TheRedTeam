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
    [SerializeField] GameObject shieldUI;
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
    [SerializeField] TMP_Text winScore;
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
    public TimeSpan currentTime;// unity class to turn delta time into sec,min, ext...
    public bool isPaused;
    public int currrentGunIndex;
    public int gunIndex;  
    public int enemyCount;
    public string playerInitals;
    int playerScore;
    float time;
    bool timerOn;
    bool popUpOn;
    bool menuToggled;
    List<TMP_Text> scoreList;
    //awake will run before any other call crating this object before anything needs to use it
    void Awake()
    {
        //create the manager and find the object that will be our player in any given instance by tag
        //also find the players script and both enemy and player spawn postitons
        instance = this;//any refreance to this game manager is this
        //enemyCount = -1;
        player = GameObject.FindWithTag("Player");//get the player
        if(player != null)
        playerScript = player.GetComponent<playerController>();//get the player script
        toggleTimer();//turn on the timer
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");//get the player spawn
        scoreList = new List<TMP_Text>();
        loadScores();
        backgroundMusicVol = dataManager.instance.GetOption("volume");
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
        if(timer != null)
        timerUpdate();// update the timer

    }
    public void statePaused()//sets the game state into a paused state and brings back mouse
    {
        isPaused = true;//is now paused
        Time.timeScale = 0;//set time to zero
        Cursor.visible = true;//return cursor to visable
        Cursor.lockState = CursorLockMode.None;//unlick the cursor
        shieldUI.gameObject.SetActive(false);//tirn off the shield
        toggleTimer();//toggle the timer

    }
    public void stateUnPaused()//sets the game state into a running state and removes mouse
    {
        isPaused = false;//is not paused
        Time.timeScale = 1;//resume normal time
        Cursor.visible = false;// turn off the cusor
        Cursor.lockState = CursorLockMode.Locked;// lock the cursor
        activeMenu.SetActive(false);//turns off the current menu
        activeMenu = null;// there is no menu open anymore
        shieldUI.gameObject.SetActive(true);
        toggleTimer();//toggle the timer
    }
    public void updateGameGoal(int enemyTotal)//will activly alter the total score until win or loss (same code as class for now)
    {
        enemyCount += enemyTotal;// determent the current score(in this case number of enemys)
        if (enemyCount <= 0)// if there are no enemys its a win
        {
           
            activeMenu = winMenu;//set the active menu to win menu
            winScore.text = bonusScoreCalc(playerScore).ToString(); //get the score and set it to the win text
            activeMenu.SetActive(true);

            statePaused();
        }
    }
    public void updateScore(int value)// updates the player score to the screen
    {
        playerScore += value;//add the pased in value to the player score
        scoreValue.text = playerScore.ToString();//update the player score text 
    }
  
    public void youLose()//on a player death show a loss menu
    {
        statePaused();//pause the game
        activeMenu = loseMenu;//set active menu to the lose menu
        activeMenu.SetActive(true);//turn on the active menu
    }
    public void toggleTimer() //toggle the timmer
    {
        timerOn = !timerOn;//set the timer bool to off skipping the update for the timer class
    }
    public void timerUpdate()//updates timer on screen and the current time 
    {
        if (timerOn)//if the timer is on
        {
            time += Time.deltaTime;//add time 
        }
        currentTime = TimeSpan.FromSeconds(time);//update the current time variable
        timer.text = currentTime.Minutes + ":" + currentTime.Seconds;//show the current time to the screen
    }
    public void setActiveGun()//sets the current gun both for ammo tracking and ui
    {
      //GunImg.sprite = playerScript.gunUI
        if(!currentMagAmmo.IsActive())//if current mag ammo is not active
        {
            currentMagAmmo.gameObject.SetActive(true);//current mag ammo is turned on
            currentReserves.gameObject.SetActive(true);//current reserves is turned on
        }
        updateAmmo();//update the ammo count
    }
    public void updateAmmo()//updates the ammo ui
    {
        
        currentMagAmmo.text = playerScript.magazine.ToString();//set magazine text = to the players current magazine
        currentReserves.text = playerScript.reserves.ToString();//set reserve text = to the players current reserves
    }
    public void UpdateShiieldUi()//updates the player shield ui
    {
        float tmp = (float)playerScript.shieldAmount / playerScript.shieldAmountOrg;//devide the player shield current by player shield orginal
        LShield.fillAmount = tmp;//set both shield halfs to the percent that is returned
        RShield.fillAmount = tmp;
    }
   
    public List<GameObject> findAllChild(GameObject parent)//find all the children of an object and return them as a list
    {
        List<GameObject> children = new List<GameObject>();//create a list of type game object
        foreach (Transform child in parent.transform)//for each child in the parents child list
        {
            children.Add(child.gameObject);//add the game objects to a list of children
        }
        return children;//return the childeren list
    }

    public void togglePopUpTxt(string text = "")//toggles a pop up text
    {
        popUp.gameObject.SetActive(!popUpOn);//if pop up is off turn on if on turn off
        popUpOn = !popUpOn;//toggle pop up bool
        popUp.text = text;//set pop up equal to passed in text, if none, will be an empty string
    }
    public void togglePromptNewScore()// toggles the score prompt
    {
        activeMenu.SetActive(menuToggled);//turn on or off the preveus menu based on if promp is on or off
        newScoreMenu.SetActive(!menuToggled);//toggle leader board on if its off, its not turning on, if on it turns off
        menuToggled = !menuToggled;//toggle the bool to match the state of prompt
    }
    public void toggleLeaderBoard()//toggles the leader board
    {
        activeMenu.SetActive(menuToggled);//turn off or on the previous menu if the leader board is on
        leaderBoard.SetActive(!menuToggled);//if leader bord is not on turn on, otherwise turn off
        menuToggled = !menuToggled;//toggle the leader board bool
        if (!menuToggled)//if the leader board is turning off
            saveScores();//save the scores
    }
    public void togglePauseOptions()
    {
        activeMenu.SetActive(menuToggled);
        //options set active
        menuToggled = !menuToggled;
    }
    public void saveScore(string s)
    {
        scoreList.Add(createScore(s.ToUpper() + " " + 0.ToString() + " " + bonusScoreCalc(playerScore).ToString() + " " + time.ToString()));//set the score to be the same as current score values and add it to the list
        sortByWinner();//re order the score board and list
        togglePromptNewScore();// turn osff the prompt
        toggleLeaderBoard();// turn on the score board
    }
    public int bonusScoreCalc(int score)
    {
        double bonusScore = Math.Sqrt((int)currentTime.Minutes * 60) + Math.Sqrt((int)currentTime.Seconds);//gets sqrt of the current time as a numaric value in seconds(give or take a second for rounding)
        return score + ((int)bonusScore * bonusScoreMultiplyer);// the full score after bonus
    }
    public void sortByWinner()// sorts the leaderboard by highest score
    {
        List<int> tmpInt = new List<int>();
        foreach(TMP_Text score in scoreList)//for each score find the score and add it to a list
        {
            tmpInt.Add(findScore(score));
        }
        tmpInt.Sort();//sort the scores by lowest to highest
        tmpInt.Reverse();//reverse the scores to high to low
        int index = 0;//create an index to the first object
        while (index < tmpInt.Count)//while there is still a score
        {
            foreach (int score in tmpInt)//check each score
            {
                if(score == findScore(scoreList[index]))//if the score is the current postion on the index
                {
                    scoreList[index].text = alterPosistion(splitScore(scoreList[index]), index);//change its postion on the leader board to be the index(so this will decrement from first place)
                    ++index;//increase the current index
                }
            }
        }
        for (int i = scoreList.Count -1; i >= 0; --i)//starting from the last score
        {
            scoreList[i].transform.SetAsFirstSibling();//set each score in assending order to be the frstplace (this will re arange them in the vertical list component)
        }
    }
    int findScore(TMP_Text score)//finds the score string and converts it to an int
    {
        return System.Convert.ToInt16(splitScore(score)[2]);//split and get the score, always in the third index postion
    }
    string[] splitScore(TMP_Text score)//splits a score into its base parts at delemanators
    {
        string[] split = score.text.Split(new string[] { " ", "-" }, StringSplitOptions.None);//split the score
        return split;//return the split score
    }
    string alterPosistion(string[] strings, int pos)//takes a split score and alters the postition (first place, second place ext..)on the leader board by the passed in int
    {
        bool skipSpace = true;//skip the first space
        string returnString = "";// make a tmp string for return
        strings[1] = pos.ToString();//get the positon
        foreach (string s in strings)// for each part of the split string
        {
            if (skipSpace == true)
            {
                returnString += s;//skip the first space
                skipSpace = false;//set skip space to false
            }
            else
            returnString += " " + s;// add a space and the current string to itself I + " " + AM, I AM + " " + SMART?
        }
        return returnString;// return the full stirng
    }
    public void saveScores()//saves scores on close
    {
        PlayerPrefs.SetInt("ScoreSize", scoreList.Count);//save the size of the current score board
        foreach(TMP_Text s in scoreList)//for each score in score list
        {
            PlayerPrefs.SetString("score" + splitScore(s)[1], s.text);//save each score with a key of score(posistion)
        }
        PlayerPrefs.Save();//save the set strings
    }
    public void loadScores()//loads scores on open
    {
        int scoreListSize = PlayerPrefs.GetInt("ScoreSize");//get the size of the preveious score board
        if (scoreListSize == 0)// if the score list is empty
        {
            return;//break out of this function
        } 
        
        for(int i = 0; i < scoreListSize; i++)//while index is not the score list size
        {
            scoreList.Add(createScore(PlayerPrefs.GetString("score" + i.ToString())));//create a score from the score in the player prefs with a key of score(index) to match score(position)
        }
    }
    TMP_Text createScore(string score)
    {
        TMP_Text newScore = Instantiate(scorePreFap, scoreParent.transform);//instantiate a new score from the pre fap under the score board parent
        newScore.text = score;//set this scores text equal to the score string
        return newScore;//return the new score
    }
   
}
