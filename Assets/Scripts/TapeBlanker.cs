using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using UnityEngine.Audio;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(HajiyevMusicManager))]
public class TapeBlanker : MonoBehaviour
{
    //Derives from Now Is Playing What
    public GameObject CanvasBlanker;
    public MusicPopUpLite MusicPopUpper;
    public Text nowPlayingText;
    public Text ArtistText;
    public Text TimeText;
    public Text LengthText;
    public Image SongProfileImager;
    public Slider musicLength;
    public TriggerCore NFCSticker;
    public PauseMenu PauseDetector;

    public string ArtistName;
    public string SourceFrom;
    public string Copyright;
    public string License;
    public Color TapeColor;

    [SerializeField] private bool enabledTape = false;
    [SerializeField] private bool MusicPoppedup = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
        CanvasBlanker.SetActive(false);

        if (MusicPopUpper)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool HasPaused = false;
        if (HajiyevMusicManager.instance.CurrentTrackNumber() >= 0)
        {
            nowPlayingText.text = "" + HajiyevMusicManager.instance.NowPlaying().name;
            ArtistText.text = "" + ArtistName;
            TimeText.text = "" + SecondsToMS(HajiyevMusicManager.instance.TimeInSeconds());
            LengthText.text = "" + SecondsToMS(HajiyevMusicManager.instance.LengthInSeconds());

            musicLength.value = HajiyevMusicManager.instance.TimeInSeconds();
            musicLength.maxValue = HajiyevMusicManager.instance.LengthInSeconds();

            MusicPopUpper.LengthFill = HajiyevMusicManager.instance.LengthInSeconds();
            MusicPopUpper.TimeFill = HajiyevMusicManager.instance.TimeInSeconds();
            MusicPopUpper.SourceImager.sprite = SongProfileImager.sprite;
            MusicPopUpper.ArtistName = ArtistName;
            MusicPopUpper.SongName = HajiyevMusicManager.instance.NowPlaying().name;
            MusicPopUpper.SourceFrom = SourceFrom;
            MusicPopUpper.License = License;
            MusicPopUpper.Copyright = Copyright;
        }
        else
        {
            nowPlayingText.text = "No Song!";
            MusicPopUpper.SongName = "No Song!";
        }

        if (NFCSticker.isTouchingSensor) enabledTape = true;
        if (enabledTape)
        {
            if (!PauseDetector.PauseStatus)
            {
                    GetComponent<HajiyevMusicManager>().Play();
                    HasPaused = false;
            } else
            {
                if (!HasPaused)
                {
                    GetComponent<HajiyevMusicManager>().ForcePause();
                    HasPaused = true;
                }
            }

            GetComponent<SpriteRenderer>().color = TapeColor;
            CanvasBlanker.SetActive(true);
            if (!MusicPoppedup)
            {
                GetComponent<HajiyevMusicManager>().Play();
                MusicPopUpper.gameObject.SetActive(true);
                MusicPoppedup = true;
            }
        } else
        {

        }
    }

    string SecondsToMS(float seconds)
    {
        return string.Format("{0:D2}:{1:D2}", ((int)seconds) / 60, ((int)seconds) % 60);
    }
}
