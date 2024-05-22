using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public ButtonSounds buttonSound;
    public float delayBeforeLoad = 0.5f; // Thời gian chờ trước khi load game
    void Start()
    {
        buttonSound = FindObjectOfType<ButtonSounds>(); // Tìm và gán đối tượng ButtonSound trong Scene
    }

    public void Pause()
    {
        if (buttonSound != null)
        {
            buttonSound.PlayButtonClickSound(); // Phát âm thanh
        }
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void Resume()
    {
        if (buttonSound != null)
        {
            buttonSound.PlayButtonClickSound(); // Phát âm thanh
        }
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void MainMenu()
    {
        if (buttonSound != null)
        {
            buttonSound.PlayButtonClickSound(); // Phát âm thanh
            StartCoroutine(MainMenuAfterDelay());

        }
        Time.timeScale = 1;

    }
    public void Restart()
    {
        if (buttonSound != null)
        {
            buttonSound.PlayButtonClickSound(); // Phát âm thanh
            StartCoroutine(RestartAfterDelay());
        }
        Time.timeScale = 1;
    }
    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad); // Chờ một khoảng thời gian
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    IEnumerator MainMenuAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad); // Chờ một khoảng thời gian
        SceneManager.LoadScene("MainMenu");

    }
}

