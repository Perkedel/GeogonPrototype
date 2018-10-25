using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    //Brackeys https://www.youtube.com/watch?v=zc8ac_qUXQY&t=3s

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUITTED!!!");
        Application.Quit();
    }

    public void Update()
    {
        if(gameObject.activeSelf)
        {
            if(Input.GetButtonDown("Cancel"))
            {
                QuitGame();
            }
        }
    }
}
