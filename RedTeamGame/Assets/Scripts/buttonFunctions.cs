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
    public void quit()//closes application
    {
        Application.Quit();//self explanatory
    }
    public void StartRun()
    {
        SceneManager.LoadScene("MainScene");
        //need to change loading somehow to render all textures

    }
}
