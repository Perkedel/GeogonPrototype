using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

[RequireComponent(typeof(VibrateOnButtonClick))]
public class PauseMenu : MonoBehaviour {

    //Brackeys https://www.youtube.com/watch?v=JivuXdrIHK0

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject [] hideThoseUI;

    public GameObject InsertResumeButtonObjectHere;
    public LevelLoader levelLoader;

    public VibrateOnButtonClick vibratings;

    //Parametrics
    public string sceneMenu = "SampleMenuScene";

    public void Start()
    {
        if (!vibratings)
        {
            vibratings = GetComponent<VibrateOnButtonClick>();
        }
    }

    //Controller Configurations
    private void PauseButton()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (GameIsPaused)
            {
                Resume();
                //hideThoseUI.SetActive(true);
            }
            else
            {
                Pause();
                //hideThoseUI.SetActive(false);
                levelLoader.SetStoreSelected(InsertResumeButtonObjectHere);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        PauseButton();
	}

    public void Resume()
    {
        //Vibration.Vibrate();
        vibratings.VibrateIt();
        pauseMenuUI.SetActive(false);
        for (int i = 0; i < hideThoseUI.Length; i++)
        {
            hideThoseUI[i].SetActive(true);
        }
        Time.timeScale = 1f;
        GameIsPaused = false;
        Debug.Log("resume");
    }

    public void Pause()
    {
        vibratings.VibrateStyle(2);
        pauseMenuUI.SetActive(true);
        for (int i = 0; i < hideThoseUI.Length; i++)
        {
            hideThoseUI[i].SetActive(false);
        }
        Time.timeScale = 0f;
        GameIsPaused = true;
        Debug.Log("pause");
    }

    public void LoadMenu()
    {
        vibratings.VibrateStyle(0);
        //Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene(sceneMenu);
    }

    public void QuitGame()
    {
        vibratings.VibrateStyle(0);
        GameIsPaused = false;
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
