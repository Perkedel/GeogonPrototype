using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ECGcable : MonoBehaviour {

    public SHanpe target;
    [Range(0, 100)] public float HP = 100f;
    float lastHP = 100f;
    public Slider healthBar;
    public Slider prevHealthBar;
    private Color inColoring;
    public Image HPBarBGColor;
    public Image HPfillColor;
    public Text HPTextInfo;
    public GameObject EekSerkatText;
    public GameObject LevelCompleteText;
    public GameObject LevelFailedText;
    public Scrollbar ShapeStatusBar;
    public ShapeStatus refferShapeStatusScript;
    public Scrollbar HighlightStatusBar;
    public LevelLoader LevelManager;

    //Condition
    private bool startDelayDecrease;
    private float delayDecrease;
    [SerializeField] private bool hasBeenDedd;
    private SHanpe.Bentuk bentuk;
    [SerializeField] private float ShapeStatus;
    private float currShapeStatus;
    private float setShapeValue;

	// Use this for initialization
	void Start () {
        //Search for object with Player tag
        if (!target)
        {
            var go = GameObject.FindWithTag("Player");
            // Check we found an object with the player tag
            if (go)
            // Set the target to the object we found
            {
                if (go.CompareTag("Player"))
                    target = go.gameObject.transform.parent.GetComponent<SHanpe>();
            }
        }
        if (!LevelManager)
        {
            var go = GameObject.FindWithTag("LevelLoader");
            // Check we found an object with the player tag
            if (go)
            // Set the target to the object we found
            {
                if (go.CompareTag("LevelLoader"))
                    LevelManager = go.gameObject.GetComponent<LevelLoader>();
            }
        }
    }

    //Parametrics
    public bool doFreezeControllerLCompleteFail = true;
    public bool willGameOver = false;

    // Update is called once per frame
    void Update () {
        HP = getPlayerHP();
        healthBar.value = getPlayerHP();

        if(HP >= 100)
        {
            HPfillColor.color = Color.blue;
            HPTextInfo.color = Color.yellow;
            hasBeenDedd = false;
        } else if(HP < 100 && HP >= 50)
        {
            HPfillColor.color = Color.green;
            HPTextInfo.color = Color.magenta;
            hasBeenDedd = false;
        } else if(HP < 50 && HP >= 25)
        {
            HPfillColor.color = Color.yellow;
            HPTextInfo.color = Color.blue;
            hasBeenDedd = false;
        } else if(HP < 25 && HP > 0)
        {
            HPfillColor.color = Color.red;
            HPTextInfo.color = Color.cyan;
            hasBeenDedd = false;
        } else if(HP == 0)
        {
            HPfillColor.color = Color.grey;
            HPTextInfo.color = Color.white;
            hasBeenDedd = true;
        }

        //if (lastHP != HP) startDelayDecrease = true;
        //if (startDelayDecrease) delayDecrease += Time.deltaTime;
        //if (delayDecrease > 2f)
        //{
        //    startDelayDecrease = false;
        //    if (lastHP > HP) lastHP--;
        //    if (lastHP < HP) lastHP++;

        //    if (lastHP == HP) delayDecrease = 0;
        //}
        if (!target.healthWasChanged)
        {
            if (lastHP > HP) lastHP--;
            if (lastHP < HP) lastHP++;
        }
        prevHealthBar.value = lastHP;

        //HP Bar BG
        //https://docs.unity3d.com/ScriptReference/Color-ctor.html
        //color using value precisely
        inColoring = new Color((float)HP / 100f, (float)HP / 100f, (float)HP / 100f);
        HPBarBGColor.color = inColoring;
        if (HP == 100) HPTextInfo.text = "FULL";
        else if (HP <= 0) HPTextInfo.text = "DEDD";
        else HPTextInfo.text = Mathf.Floor((HP*1000))/1000 + "%";

        if (hasBeenDedd)
        {
            EekSerkatText.SetActive(true);
        } else
        {
            EekSerkatText.SetActive(false);
        }

        //Scrollbar of Shape Statusing. ShapeStatusBar
        if (!target.canChangeShapeWhileDead)
        {
            if (hasBeenDedd)
            {
                ShapeStatusBar.interactable = false;
            }
            else ShapeStatusBar.interactable = true;
        }
        else ShapeStatusBar.interactable = true;

        bentuk = target.currBentuk;
        //ShapeStatusBar.value = (float)bentuk/3f;
        switch (bentuk)
        {
            default:
                break;
            case SHanpe.Bentuk.Lingkaran:
                currShapeStatus = 0f;
                if (!refferShapeStatusScript.beingDragged)
                {
                    if (ShapeStatusBar.value > 0f) ShapeStatusBar.value -= Time.deltaTime;
                }
                break;
            case SHanpe.Bentuk.Kotak:
                currShapeStatus = .5f;
                if (!refferShapeStatusScript.beingDragged)
                {
                    if (ShapeStatusBar.value < .5f) ShapeStatusBar.value += Time.deltaTime;
                    if (ShapeStatusBar.value > .5f) ShapeStatusBar.value -= Time.deltaTime; //does not vibrate!!!
                }
                break;
            case SHanpe.Bentuk.Segitiga:
                currShapeStatus = 1f;
                if (!refferShapeStatusScript.beingDragged)
                {
                    if (ShapeStatusBar.value < 1f) ShapeStatusBar.value += Time.deltaTime;
                }
                break;
        }
        //ShapeStatusBar.value = currShapeStatus;
        //if(ShapeStatusBar.value > currShapeStatus)
        //{
        //    ShapeStatusBar.value -= Time.deltaTime;
        //} else if(ShapeStatusBar.value < currShapeStatus)
        //{
        //    ShapeStatusBar.value += Time.deltaTime;
        //} else if(ShapeStatusBar.value == currShapeStatus) //float is inaccurate! .5 is vibrating!
        //{
        //    //ShapeStatusBar.value = currShapeStatus;
        //}
        HighlightStatusBar.value = currShapeStatus;

        setShapeValue = ShapeStatusBar.value;
        if (refferShapeStatusScript.beingDragged)
        {
            if(setShapeValue < .25)
            {
                target.setShape(1);
            } else if(setShapeValue > .25 && setShapeValue < .75)
            {
                target.setShape(2);
            } else if(setShapeValue > .75)
            {
                target.setShape(3);
            }
        }

        if (LevelManager.levelIsCompleted)
        {
            CompleteTheLevel();
        }

	}

    //Templatics
    public float getPlayerHP()
    {
        return target.HealthPoint;
    }

    public void CompleteTheLevel()
    {
        LevelCompleteText.SetActive(true);
    }
}
