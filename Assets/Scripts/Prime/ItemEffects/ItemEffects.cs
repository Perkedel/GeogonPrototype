using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class ItemEffects : MonoBehaviour {

    Collision2D whosTouching;
    GameObject genericToucher;
    public GameObject theGameObject;
    public SHanpe theSHanpeWhoIsTouching;
    public LevelLoader levelManager;
    public ECGcable theHUD;
    public Camera[] TheseCameras;
    public FollowPlayerCSharp SeriousMainCamera;

    //Parametering
    public float setHPvalue = 100;
    public float addHPvalue = 10;
    public float damageMeValue = 20; public float lavaDamageValue = 10;
    public float addArmorValue = 50;

    //Template Methods
    public bool doSetHealth;
    public void setHealth(float value)
    {
        theSHanpeWhoIsTouching.setHealth(value);
    }

    public bool doAddHealth;
    public void addHealth(float value)
    {
        theSHanpeWhoIsTouching.addHealth(value);
    }

    public bool doDamageMe;
    public bool doLavaDamage;
    public void damageMe(float value) //initial single time damage
    {
        theSHanpeWhoIsTouching.damageMe(value);
    }
    public void lavaDamage(float value) //continuous damage
    {
        //theSHanpeWhoIsTouching.damageMe(value);
        damageMe(value);
    }

    public bool singleUse = true; //Use this if you are sure no more need after the object has been "destroyed". This is recomended to save memory.
    public void destroySelf()
    {
        Destroy(gameObject);
    }
    public bool pseudoSingleUse = false; //Use this if something should still happening if the object has been "despawned". e.g., Audio cannot play if object is actually destroyed.
    [SerializeField] private bool hasBeenInvisibled = false;
    public void invisiblizeSelf()
    {
        if (!hasBeenInvisibled)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            GetComponent<Collider2D>().enabled = false;
            hasBeenInvisibled = true;
        }
    }

    public bool doSayDebug = false;
    public string whatDoesDebugSay;
    public void sayDebug()
    {
        Debug.Log(whatDoesDebugSay);
    }
    public bool doSayDebugDynamic = false;
    public void sayDebugDynamic(string something)
    {
        Debug.Log(something);
    }

    public bool doVibrate = true;
    public void Vibrates()
    {
        Vibration.Vibrate(50);
    }
    public bool doVibrateCertainTime = false;
    public long vibrateForHowLong = 10; //milliseconds
    public void VibrateCertainMillisecond()
    {
        Vibration.Vibrate(vibrateForHowLong);
    }
    public void Vibrates(long value) //Milliseconds
    {
        Vibration.Vibrate(value);
    }
    public void Vibrates(long[] pattern, int repeatFrom)
    {
        Vibration.Vibrate(pattern, repeatFrom);
    }

    public bool doLevelComplete = false;
    public void LevelComplete()
    {
        levelManager.CompleteTheLevel();
    }

    public bool doLevelFailed = false;
    public void LevelFailed() //or game over
    {
        levelManager.FailTheLevel();
    }

    public bool doLevelDevExited = false;
    public void LevelDevExited()
    {
        levelManager.DevExitTheLevel();
    }

    public bool doChangeShape = false;
    public void ChangeShape()
    {

    }

    public bool doSetShapeLock = false; //disable/enable change shape by hand
    public void SetShapeLock()
    {

    }

    public bool doSetSize = false;
    public void SetSize() //scaling local
    {

    }

    public bool doSetSpeed = false;
    public void SetSpeed()
    {

    }

    public bool doSetGravity = false;
    public float gravityNewValue = 1f;
    public void SetGravity(float newGravityValue)
    {
        theGameObject.GetComponent<Rigidbody2D>().gravityScale = newGravityValue;
    }

    public AudioSource ItemSelfSound;
    public AudioSource OtherSelfSound;
    public bool useOtherSelfSoundInstead = false;
    public bool doPlaySoundArray = false;
    public AudioClip[] SoundArray;
    [Range(0, 1)] public float volumeMultiple;
    public void PlaySoundArray()
    {
        for (int i = 0; i < SoundArray.Length; i++) {
            if (!useOtherSelfSoundInstead)
            {
                ItemSelfSound.PlayOneShot(SoundArray[i], volumeMultiple);
            }
            else
            {
                if (OtherSelfSound)
                {
                    OtherSelfSound.PlayOneShot(SoundArray[i], volumeMultiple);
                } else
                {
                    Debug.LogError("OtherSound is not connected to the Inspector!!! using ItemSound instead");
                    ItemSelfSound.PlayOneShot(SoundArray[i], volumeMultiple);
                }
            }
        }
    }
    public bool doPlaySoundWhichRand = false;
    public AudioClip[] SoundWhichRand;
    [Range(0, 1)] public float volumeSingle;
    public void PlaySoundWhichRand()
    {
        if (!useOtherSelfSoundInstead)
        {
            ItemSelfSound.PlayOneShot(SoundWhichRand[Random.Range(0, SoundWhichRand.Length)], volumeSingle);
        }
        else
        {
            if (OtherSelfSound)
            {
                OtherSelfSound.PlayOneShot(SoundWhichRand[Random.Range(0, SoundWhichRand.Length)], volumeSingle);
            } else
            {
                Debug.LogError("OtherSound is not connected to the Inspector!!! using ItemSound instead");
                ItemSelfSound.PlayOneShot(SoundWhichRand[Random.Range(0, SoundWhichRand.Length)], volumeSingle);
            }
        }
    }

    public ParticleSystem[] EmitParticles;
    public bool doEmitParticles = false;
    public void EmitTheseParticles()
    {
        for(int i=0; i<EmitParticles.Length; i++)
        {
            Instantiate(EmitParticles[i], GetComponent<Transform>().position, Quaternion.identity);
        }
    }

    public bool doEnableTheseObjects;
    public GameObject[] EnableTheseObjects;
    public void LetsEnableObjects()
    {
        for(int i = 0; i<EnableTheseObjects.Length; i++)
        {
            EnableTheseObjects[i].SetActive(true);
        }
    }

    public bool doDisableTheseObjects;
    public GameObject[] DisableTheseObjects;
    public void LetsDisableObjects()
    {
        for (int i = 0; i < DisableTheseObjects.Length; i++)
        {
            DisableTheseObjects[i].SetActive(false);
        }
    }

    public bool doSetCameraColor;
    public Color SetColorCamera = Color.gray;
    public void letsColorCamera()
    {
        for(int i = 0; i<TheseCameras.Length; i++)
        {
            TheseCameras[i].backgroundColor = SetColorCamera;
        }
    }

    public bool doSetCameraZoom;
    public float NewZoomLevel;
    public void letsZoomCamera()
    {
        SeriousMainCamera.Zoom = NewZoomLevel;
    }

    public bool doSetCheckPoint;
    public void letsCheckpoint()
    {
        theSHanpeWhoIsTouching.initPosition = GetComponent<Transform>().position;
    }

    //toDo list
    /*
     * Set camera zoom
     * Set camera rotation
     * Set camera effect
     * particle
     * medical
     * cynical
     * ah
     * im tierd
     * sleep
     * set position
     * set velocity (picking up this item will make the player run that fast)
     * disable movement
     * etc
    */

    //Base
    // Use this for initialization
    void Start () {
        //GetComponent<Rigidbody2D>().isKinematic = true;
        //Search for object with HUD tag
        if (!theHUD)
        {
            var go = GameObject.FindWithTag("HUD");
            // Check we found an object with the player tag
            if (go)
            // Set the target to the object we found
            {
                if (go.CompareTag("HUD"))
                    theHUD = go.gameObject.GetComponent<ECGcable>();
            }
        }
        if (!levelManager)
        {
            var go = GameObject.FindWithTag("LevelLoader");
            // Check we found an object with the player tag
            if (go)
            // Set the target to the object we found
            {
                if (go.CompareTag("LevelLoader"))
                    levelManager = go.gameObject.GetComponent<LevelLoader>();
            }
        }
        ItemSelfSound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    //OnCollision Detection
    //Make sure IsTrigger is ON!
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    //whosTouching = collision;
    //    //genericToucher = collision.gameObject;
    //    //theSHanpeWhoIsTouching = collision.gameObject.GetComponent<SHanpe>();
    //    //if(theSHanpeWhoIsTouching != null)
    //    //{
    //    //    if (doAddHealth)
    //    //    {
    //    //        addHealth(addHPvalue);
    //    //    }
    //    //    if (doSetHealth)
    //    //    {
    //    //        setHealth(setHPvalue);
    //    //    }
    //    //    if (singleUse)
    //    //    {
    //    //        destroySelf();
    //    //    }
    //    //}
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    whosTouching = collision;
    //    genericToucher = collision.gameObject;
    //    theSHanpeWhoIsTouching = collision.gameObject.GetComponent<SHanpe>();
    //    if (theSHanpeWhoIsTouching != null)
    //    {

    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!theSHanpeWhoIsTouching)
        //{
        //    //// Search for object with Player tag
        //    //var go = GameObject.FindWithTag("Player");
        //    //// Check we found an object with the player tag
        //    //if (go)
        //    //// Set the target to the object we found
        //    //{
        //    //    theGameObject = go;
        //    //    if(theGameObject.CompareTag("Player"))
        //    //    theSHanpeWhoIsTouching = go.gameObject.GetComponent<SHanpe>();
        //    //}
        //    theGameObject = collision.gameObject;
        //    theSHanpeWhoIsTouching = collision.gameObject.GetComponent<SHanpe>();
        //}

        //Get Parent gameobject! https://answers.unity.com/questions/12301/how-can-i-get-a-parent-gameobject-of-gameobject-us.html
        //Because children contact collide with item!!!
        theGameObject = collision.gameObject.transform.parent.gameObject;
        theSHanpeWhoIsTouching = theGameObject.GetComponent<SHanpe>();
        if (theSHanpeWhoIsTouching)
        {
            if (doPlaySoundArray)
            {
                PlaySoundArray();
            }
            if (doPlaySoundWhichRand)
            {
                PlaySoundWhichRand();
            }
            if (doVibrate)
            {
                Vibration.Vibrate(50); //android O has trouble with just calling vibrate. deprecated probably
            }
            if (doAddHealth)
            {
                addHealth(addHPvalue);
            }
            if (doSetHealth)
            {
                setHealth(setHPvalue);
            }
            if (doDamageMe)
            {
                damageMe(damageMeValue);
                //Debug.Log("OUCH");
            }
            if (doSayDebug)
            {
                sayDebug();
            }
            if (doVibrateCertainTime)
            {
                Vibrates(vibrateForHowLong);
            }
            if (doLevelComplete)
            {
                LevelComplete();
            }
            if (doLevelFailed)
            {
                LevelFailed();
            }
            if (doLevelDevExited)
            {
                LevelDevExited();
            }
            if (doSetGravity)
            {
                SetGravity(gravityNewValue);
            }
            if (doEmitParticles)
            {
                EmitTheseParticles();
            }
            if (doEnableTheseObjects)
            {
                LetsEnableObjects();
            }
            if (doDisableTheseObjects)
            {
                LetsDisableObjects();
            }
            if (doSetCameraColor)
            {
                letsColorCamera();
            }
            if (doSetCameraZoom)
            {
                letsZoomCamera();
            }
            if (doSetCheckPoint)
            {
                letsCheckpoint();
            }
            if (pseudoSingleUse)
            {
                invisiblizeSelf();
            }
            if (singleUse)
            {
                //anItem.destroySelf();
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (theGameObject.tag == "Player")
        {
            if (doLavaDamage)
            {
                //damageMe(lavaDamageValue);
                lavaDamage(lavaDamageValue);
                //Debug.Log("hot");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    //Non-Trigger Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        theGameObject = collision.transform.parent.gameObject;
        theSHanpeWhoIsTouching = theGameObject.GetComponent<SHanpe>();

        if (theSHanpeWhoIsTouching)
        {
            if (doVibrate)
            {
                Vibration.Vibrate(50); //android O has trouble with just calling vibrate. deprecated probably
            }
            if (doAddHealth)
            {
                addHealth(addHPvalue);
            }
            if (doSetHealth)
            {
                setHealth(setHPvalue);
            }
            if (doDamageMe)
            {
                damageMe(damageMeValue);
                //Debug.Log("OUCH");
            }
            if (singleUse)
            {
                //anItem.destroySelf();
                Destroy(gameObject);
            }
            if (doSayDebug)
            {
                sayDebug();
            }
            if (doVibrateCertainTime)
            {
                Vibrates(vibrateForHowLong);
            }
            if (doLevelComplete)
            {
                LevelComplete();
            }
            if (doLevelFailed)
            {
                LevelFailed();
            }
            if (doSetGravity)
            {
                SetGravity(gravityNewValue);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
