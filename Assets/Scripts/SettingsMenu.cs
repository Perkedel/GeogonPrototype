using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

//by Brackey
//Options menu!!!
//optimised by Perkedel
//Ultimate Game UI is not free!!! Please wait for the newer Gratis Open Full one in undefined time.
//exclusive, on Hexagon Engine Asset store.
//and less likely on Unity Asset store.

public class SettingsMenu : MonoBehaviour {

    //Brackeys https://www.youtube.com/watch?v=YOaYQrN1oYQ

    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;

    Resolution[] resolutions;
    //QualityLevel[] qualityLevels;

    public GameObject[] hideThoseUI;

    void Start()
    {
        resolutions = Screen.resolutions;
        //qualitySettings = QualitySettings.GetQualityLevel;

        if (gameObject.activeSelf)
        {
            if (Input.GetButtonDown("Cancel"))
            {

            }
        }
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        List<string> optionq = new List<string>();

        int currentResolutionIndex = 0;
        int currentQualityIndex = 3;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
         }
        //for(int i = 0; i < qualitySettings.Length; i++)
        //{
        //    string optiqn = qualitySettings[i].name;
        //    optionq.Add(optiqn);

        //    if(qualitySettings[i].name == QualitySettings.GetQualityLevel)
        //    {

        //    }
        //}

        //qualityDropdown.AddOptions(optionq);
        //qualityDropdown.value = currentQualityIndex;
        //qualityDropdown.RefreshShownValue();
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //LoadSetting();
    }

    public void SetVolume(float volume)
    {
        //Debug.Log(volume);
        audioMixer.SetFloat("volume", volume);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Setting: ResolutionIndex", resolutionIndex);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Setting: QualityIndex", qualityIndex);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        int fullScreen;
        if (isFullscreen) fullScreen = 1; else fullScreen = 0;
        PlayerPrefs.SetInt("Setting: FullScreen", fullScreen);
    }

    public void LoadSetting()
    {
        Resolution resolution = resolutions[PlayerPrefs.GetInt("Setting: ResolutionIndex", 100000)];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Setting: QualityIndex", 4));

        bool isFullscreen;
        if (PlayerPrefs.GetInt("Setting: FullScreen", 1) != 0) isFullscreen = true; else isFullscreen = false;
        Screen.fullScreen = isFullscreen;
    }
}
