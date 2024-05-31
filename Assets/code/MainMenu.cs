using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public ButtonSounds buttonSound;
    public float delayBeforeLoad = 0.5f; // Thời gian chờ trước khi load game
    void Start()
    {
        buttonSound = FindObjectOfType<ButtonSounds>(); // Tìm và gán đối tượng ButtonSound trong Scene
    }
    public void PlayButtonClicked()
    {
        if (buttonSound != null)
        {
            buttonSound.PlayButtonClickSound(); // Phát âm thanh
            StartCoroutine(LoadGameAfterDelay()); // Bắt đầu Coroutine để load game sau một khoảng thời gian
        }
    }
    IEnumerator LoadGameAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad); // Chờ một khoảng thời gian
        SceneManager.LoadScene("Demo"); // Load game sau khi chờ
    }
    //public void HowToPlay()
    //{
    //    HowToPlay.SetActive(true);
    //    Time.timeScale = 0;
    //    SceneManager.LoadScene("HowToPlay");

    //}
    void PlayButtonClickSoundAndExit()
    {
        if (buttonSound != null)
        {
            buttonSound.PlayButtonClickSound(); // Phát âm thanh
            Application.Quit(); // Thoát game
        }
    }
    public void ExitButtonClicked()
    {
        PlayButtonClickSoundAndExit();
    }
}
