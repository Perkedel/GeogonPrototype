using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    //Brackeys https://www.youtube.com/watch?v=zc8ac_qUXQY&t=3s
    public bool isPauseGame = false;
    [SerializeField] CanvasCore canvasCore;
    //playScene
    public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        canvasCore.PlayTheLevel("SampleScene");
    }
    public void PlayGame(string passName)
    {
        //SceneManager.LoadScene(passName);
        canvasCore.PlayTheLevel(passName);
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
