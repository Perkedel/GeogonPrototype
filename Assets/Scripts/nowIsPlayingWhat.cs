using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class nowIsPlayingWhat : MonoBehaviour
{
    //Derives from Hajiyev's Music Player Asset, Now Playing!
    public TextMeshProUGUI nowPlayingText;
    public Text SourceText;
    public Text CopyrightText;
    public Text LicenseText;
    public Slider musicLength;

    //Manual Fill
    public string ArtistName;
    public string SourceFrom;
    public string Copyright;
    public string License;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(HajiyevMusicManager.instance.CurrentTrackNumber() >= 0)
        {
            nowPlayingText.text = "" + ArtistName + " - " + HajiyevMusicManager.instance.NowPlaying().name;
            SourceText.text = "" + SourceFrom;
            CopyrightText.text = "© " + Copyright;
            LicenseText.text = "" + License;


            musicLength.value = HajiyevMusicManager.instance.TimeInSeconds();
            musicLength.maxValue = HajiyevMusicManager.instance.LengthInSeconds();
        }
        else
        {
            nowPlayingText.text = "No Song!";
        }
    }
}
