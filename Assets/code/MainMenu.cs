using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void loadGame()
    {
        SceneManager.LoadScene("Demo");

    }
    //public void HowToPlay()
    //{
    //    HowToPlay.SetActive(true);
    //    Time.timeScale = 0;
    //    SceneManager.LoadScene("HowToPlay");

    //}
    public void exitGame()
    {
        Application.Quit();

    }

}
