using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class LevelLoader : MonoBehaviour {

    //Brackeys https://www.youtube.com/watch?v=YMj2qPq9CP8

    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;
    public EventSystem eventSystem;
    public SHanpe player;
    public HajiyevMusicManager MusicPlayer;

    //Spare part variable
    private GameObject StoreSelected; //https://youtu.be/FRbRQFpVFxg Store button selection
    private AudioSource ItselfSound;

    //Customizable Variables
    public string MainMenuName = "SampleMenuScene";
    public string GameOverName = "SampleGameOver";
    public string[] DevExitPack;
    public float goingToNextTimer = 5f;
    public string NextLevelName;
    public string [] AltLevelNames;
    public bool playTheseSounds = true;
    [Range(0, 1)] public float volumeControl = 1;
    public AudioClip[] LevelCompleteSound;
    public AudioClip[] LevelFailedSound;
    public bool levelIsModuleBased = false;
    public GameObject[] LevelModules;
    public GameObject[] ModuleCheckpoints;

    //Statusing
    public bool levelIsCompleted = false;
    public bool levelIsFailed = false;
    public bool levelIsDevExited = false;

    //Loading Level
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

            if (slider.value >= 50)
            {
                //HajiyevMusicManager.instance.ForcePause();
                MusicPlayer.ForcePause();
            }

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

            if(slider.value >= 50)
            {
                //HajiyevMusicManager.instance.ForcePause();
                MusicPlayer.ForcePause();
            }

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

    public void GoToLevel()
    {
        LoadLevel(NextLevelName);
    }

    //Workaround to get the store selected selects button on newly activated menu part e.g. Menu -> Option
    //Fill it with the selectable Button GameObject!
    public void SetStoreSelected(GameObject newThing)
    {
        StoreSelected = newThing;
        eventSystem.SetSelectedGameObject(StoreSelected);
    }
    //End Template Method

    public void CompleteTheLevel()
    {
        levelIsCompleted = true;
        ItselfSound.PlayOneShot(LevelCompleteSound[Random.Range(0, LevelCompleteSound.Length)], volumeControl);
        player.controllerIsActive = false;
    }
    public void FailTheLevel()
    {
        Debug.Log("FailLevel");
        StoreScene.CurrSceneName = SceneManager.GetActiveScene().name;
        StoreScene.CurrSceneIndex = SceneManager.GetActiveScene().buildIndex;
        levelIsFailed = true;
        ItselfSound.PlayOneShot(LevelFailedSound[Random.Range(0, LevelFailedSound.Length)], volumeControl);
        player.controllerIsActive = false;
    }
    public void DevExitTheLevel()
    {
        StoreScene.CurrSceneName = SceneManager.GetActiveScene().name;
        StoreScene.CurrSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StoreScene.NextLevelNaming = DevExitPack[Random.Range(0, DevExitPack.Length)];
        levelIsDevExited = true;
        ItselfSound.PlayOneShot(LevelCompleteSound[Random.Range(0, LevelCompleteSound.Length)], volumeControl);
        player.controllerIsActive = false;
    }

    //Retry Level
    public void RetryLevel()
    {
        LoadLevel(StoreScene.CurrSceneName);
    }
    //same as above restart level lol

    //Basic Unity Method
    public void Start()
    {
        ItselfSound = GetComponent<AudioSource>();
        AltLevelNames = new string[AltLevelNames.Length];
        if (!player)
        {
            // Search for object with Player tag
            var go = GameObject.FindWithTag("Player");
            // Check we found an object with the player tag
            if (go)
                // Set the target to the object we found
                player = go.gameObject.GetComponent<SHanpe>();
        }
        StoreSelected = eventSystem.firstSelectedGameObject;

        //Load level from Module
        GameObject SelectedLevelModule;
        GameObject DirectToNextStage;
        if (StoreScene.checkStartPoint)
        {
            if (StoreScene.whichLevelModuleIndex == 0)
            {

            }
            else if (StoreScene.whichLevelModuleIndex > 0)
            {
                for (int i = 0; i < LevelModules.Length; i++)
                {
                    SelectedLevelModule = LevelModules[1];
                    //find with tag inside this Selected level module, those child, for Module Checkpoint
                    //so the player should teleport instant to that object position, and enable that module
                }
            }
        }
        //End load level from module
    }
    public void Update()
    {
        //https://youtu.be/FRbRQFpVFxg
        if (eventSystem.currentSelectedGameObject != StoreSelected)
        {
            if(eventSystem.currentSelectedGameObject == null)
            {
                eventSystem.SetSelectedGameObject(StoreSelected);
            } else
            {
                StoreSelected = eventSystem.currentSelectedGameObject;
            }
        }
        if (slider.value >= 50)
        {
            //HajiyevMusicManager.instance.ForcePause();
            MusicPlayer.ForcePause();
        }
        if (levelIsCompleted)
        {
            goingToNextTimer -= Time.deltaTime;
            if(goingToNextTimer <= 0)
            {
                LoadLevel(NextLevelName);
            }
        }
        if (levelIsFailed)
        {
            goingToNextTimer -= Time.deltaTime;
            if (goingToNextTimer <= 0)
            {
                LoadLevel(GameOverName);
            }
        }
        if (levelIsDevExited)
        {
            goingToNextTimer -= Time.deltaTime;
            if (goingToNextTimer <= 0)
            {
                //LoadLevel(StoreScene.DevExitScene1);
                LoadLevel(StoreScene.NextLevelNaming);
            }
        }
    }
}
