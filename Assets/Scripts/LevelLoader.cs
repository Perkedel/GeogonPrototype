using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    //Brackeys https://www.youtube.com/watch?v=YMj2qPq9CP8

    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;

    //Customizable Variables
    public string MainMenuName = "SampleMenuScene";
    public string GameOverName = "SampleGameOver";

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void LoadLevel(string sceneName) //This is JOELwindows7's mod. sometimes you will reffer scene by its name in the build.
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            //Debug.Log(progress);

            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }

    IEnumerator LoadAsynchronously(string sceneName) //JOELwindows7
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            //Debug.Log(progress);

            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }

    ///////////////////////////////////////Template Methods/////////////////////////////////////////////
    //https://forum.unity.com/threads/how-to-get-the-loaded-scene-name.86910/
    //jasonjoh for get scene name
    public void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        LoadLevel(MainMenuName);
        //By Default, the Main Menu is placed on 0th scene index.
        //Sometimes 0th index may be a logo scene.
    }

    public void GameOver()
    {
        LoadLevel(GameOverName);
    }

    public void NextLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PrevLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
