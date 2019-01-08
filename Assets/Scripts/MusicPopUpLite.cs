using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicPopUpLite : MonoBehaviour
{
    //Derives from Now is Playing What
    public TapeBlanker RefferTheTape;
    public TextMeshProUGUI nowPlayingText;
    public Image SourceImager;
    public Text SourceText;
    public Text CopyrightText;
    public Text LicenseText;
    public Slider musicLength;

    //Manual Fill
    public string SongName;
    public string ArtistName;
    public string SourceFrom;
    public string Copyright;
    public string License;
    public float TimeFill;
    public float LengthFill;
    public float ShowTimeRemaining = 10f;
    public bool justReceive = true;

    // Start is called before the first frame update
    void Start()
    {
        if (!justReceive)
        {
            if (RefferTheTape)
            {
                ArtistName = RefferTheTape.ArtistName;
                SourceFrom = RefferTheTape.SourceFrom;
                Copyright = RefferTheTape.Copyright;
                License = RefferTheTape.License;

                nowPlayingText.text = RefferTheTape.nowPlayingText.text;
                SourceText.text = RefferTheTape.SourceFrom;
                CopyrightText.text = RefferTheTape.Copyright;
                LicenseText.text = RefferTheTape.License;
                musicLength.value = RefferTheTape.musicLength.value;

                ArtistName = RefferTheTape.ArtistName;
                SourceFrom = RefferTheTape.SourceFrom;
                Copyright = RefferTheTape.Copyright;
                License = RefferTheTape.License;
            }
            else
            {
                nowPlayingText.text = HajiyevMusicManager.instance.NowPlaying().name;
                SourceText.text = SourceFrom;
                CopyrightText.text = Copyright;
                LicenseText.text = License;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (justReceive)
        {
            nowPlayingText.text = "" + ArtistName + " - " + SongName;
            SourceText.text = SourceFrom;
            CopyrightText.text = "© " + Copyright;
            LicenseText.text = License;
            musicLength.maxValue = LengthFill;
            musicLength.value = TimeFill;
        }

        ShowTimeRemaining -= Time.deltaTime;
        if(ShowTimeRemaining <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
