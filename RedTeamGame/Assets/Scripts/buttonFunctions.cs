using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()//resume the game from where it was paused if aplicable
    {
        gameManager.instance.stateUnPaused();//unpause
    }
    public void restart()//resets the current game
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//reload curent scene
        gameManager.instance.stateUnPaused();//unpause
    }
    public void quit()//closes application (change to load main menu)
    {
        Application.Quit();//self explanatory
        gameManager.instance.saveScores();
    }
    public void returnToMainMenu()
    {
        SceneManager.LoadScene("mainMenu");
        gameManager.instance.saveScores();
    }
    public void StartRun()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
        //need to change loading somehow to render all textures

    }
    public void options()
    {
        MainMenuController.instance.toggleOptions();
    }
    public void togglePrompt()
    {
        gameManager.instance.togglePromptNewScore();
    }
    public void toggleScoreBoard()//add to main menu
    {
        gameManager.instance.toggleLeaderBoard();
    }
    public void toggleCredits()
    {
        MainMenuController.instance.toggleCredits();
    }
}
