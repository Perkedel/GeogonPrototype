using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    //Brackeys https://www.youtube.com/watch?v=JivuXdrIHK0

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject [] hideThoseUI;

    //Controller Configurations
    private void PauseButton()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (GameIsPaused)
            {
                Resume();
                for(int i = 0; i < hideThoseUI.Length; i++)
                {
                    hideThoseUI[i].SetActive(true);
                }
                //hideThoseUI.SetActive(true);
            }
            else
            {
                Pause();
                for (int i = 0; i < hideThoseUI.Length; i++)
                {
                    hideThoseUI[i].SetActive(false);
                }
                //hideThoseUI.SetActive(false);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        PauseButton();
	}

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        //Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleMenuScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
        
    }
}
