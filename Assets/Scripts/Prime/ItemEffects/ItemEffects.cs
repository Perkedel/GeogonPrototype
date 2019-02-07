using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XInputDotNetPure;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class ItemEffects : MonoBehaviour {

    //Controller
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

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

    //Allowed Shapes
    [SerializeField] private bool Deserves = true;
    public bool canBeCircle = true;
    public bool canBeSquare = true;
    public bool canBeTriangle = true;
    public bool canBeDedd = false;
    [SerializeField] private bool isCircle;
    [SerializeField] private bool isSquare;
    [SerializeField] private bool isTriangle;
    [SerializeField] private bool isEikSerkat;

    //StatusShape
    [SerializeField] private SHanpe.Bentuk currentShape;

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
        SpriteRenderer CheckSpriteRenderer = GetComponent<SpriteRenderer>();
        if (!hasBeenInvisibled)
        {
            if(CheckSpriteRenderer) CheckSpriteRenderer.color = new Color(1, 1, 1, 0);
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
        CallVibrationGround(vibrateForHowLong, 1f, 1f);
    }
    public void Vibrates(long value) //Milliseconds
    {
        Vibration.Vibrate(value);
        CallVibrationGround(value, 1f, 1f);
    }
    public void Vibrates(long[] pattern, int repeatFrom)
    {
        Vibration.Vibrate(pattern, repeatFrom);
        CallVibrationGround(1f, 1f, 1f);
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
        for (int i = 0; i < TheseCameras.Length; i++)
        {
            if (TheseCameras[i])
            {
                TheseCameras[i].backgroundColor = SetColorCamera;
            }
        }
    }

    public bool doSetCameraZoom;
    public bool doSetNewZoomADefault = true;
    public float NewZoomLevel;
    public void letsZoomCamera()
    {
        SeriousMainCamera.Zoom = NewZoomLevel;
        if(doSetNewZoomADefault) SeriousMainCamera.initialZoom = NewZoomLevel;
    }

    public bool doTotalResetCamera = false;
    public void LetsTotalResetCamera()
    {
        SeriousMainCamera.ResetCamera();
    }

    public bool doSetCheckPoint;
    public bool doSetCheckPointBasedOnCurrShanpePosition = true;
    public void letsCheckpoint()
    {
        if (!doSetCheckPointBasedOnCurrShanpePosition)
        {
            theSHanpeWhoIsTouching.initPosition = GetComponent<Transform>().position;
        } else
        {
            theSHanpeWhoIsTouching.initPosition = theSHanpeWhoIsTouching.transform.position;
        }
    }

    public bool doDislodgeJoints;
    [SerializeField] private bool ShanpeIsTriangle;
    public Joint2D[] DislodgeTheseJoints;
    public Collider2D[] DisableTheseWeapons;
    public void letsDislodgeJoints()
    {

        //GameObject TriangleShape = null;
        //Collider2D TriangleCollision;
        //for(int i = 0; i < theSHanpeWhoIsTouching.transform.childCount; i++)
        //{
        //    if(theSHanpeWhoIsTouching.transform.GetChild(i).name == "SHanpe_Triangle")
        //    {
        //        TriangleShape = theSHanpeWhoIsTouching.transform.GetChild(i).gameObject;
        //        TriangleCollision = TriangleShape.GetComponent<Collider2D>();
        //        break;
        //    }
        //}
        //if (transform.childCount > 0)
        //{
        //    GameObject refferChild;
        //    Joint2D ChildJoint;
        //    Collider2D ChildCollider2D;
        //    for (int i = 0; i < transform.childCount; i++)
        //    {
        //        refferChild = transform.GetChild(i).gameObject;
        //        ChildJoint = refferChild.GetComponent<Joint2D>();
        //        ChildCollider2D = refferChild.GetComponent<Collider2D>();
        //        if (refferChild)
        //        {
        //            if (ChildJoint)
        //            {
        //                //ChildJoint.connectedBody = GetComponent<Rigidbody2D>();
        //                ChildJoint.breakForce = .001f;
        //                ChildJoint.breakTorque = .001f;
        //            }
        //            if (ChildCollider2D)
        //            {
        //                if (refferChild.CompareTag("SenjataMusuh"))
        //                {
        //                    ChildCollider2D.enabled = false;
        //                }
        //            }
        //        }
        //    }
        //}

        for (int i = 0; i < DislodgeTheseJoints.Length; i++)
        {
            if (DislodgeTheseJoints[i])
            {
                DislodgeTheseJoints[i].breakForce = .001f;
                DislodgeTheseJoints[i].breakTorque = .001f;
            }
        }
        for (int i = 0; i < DisableTheseWeapons.Length; i++)
        {
            if (DisableTheseWeapons[i])
            {
                DisableTheseWeapons[i].enabled = false;
            }
        }
    }

    public bool doJockeyMachine;
    public WheelJoint2D[] wheelLists;
    public JointMotor2D motorProperty;
    public float MotorSpeedingScale = 1000f;
    public float MotorTorqueingScale = 100f;
    public float GasPedal = 0f;
    public void letsJockeyMachine()
    {
        if (!theSHanpeWhoIsTouching.isRidingVehicle)
        {
            theSHanpeWhoIsTouching.JockeyIsThat = true;
            theSHanpeWhoIsTouching.JockeyMachine = gameObject.GetComponent<ItemEffects>();
            //motorProperty.motorSpeed = MotorSpeedingScale;
            motorProperty.maxMotorTorque = MotorTorqueingScale;

            theSHanpeWhoIsTouching.gasPedalScale = MotorSpeedingScale;
            theSHanpeWhoIsTouching.motorForcingScale = MotorTorqueingScale;
        } else
        {
            theSHanpeWhoIsTouching.JockeyIsThat = true;
        }
    }
    public void letsNotJockeyMachine()
    {
        if (!theSHanpeWhoIsTouching.isRidingVehicle)
        {
            theSHanpeWhoIsTouching.JockeyIsThat = false;
        }
    }

    public bool doTriggerButton = false;
    public bool IamBeingTouched = false;
    public bool TargetBeingTouched = false;
    public bool doBeingFallenObject = false;
    public bool killColliderAfterTouchdown = false;
    public void letsKillColliderAfterTouchdown()
    {
        GetComponent<Collider2D>().enabled = false;
    }
    public ItemEffects targetMachine;
    public void letsTriggerButton()
    {
        IamBeingTouched = true;
        if (targetMachine)
        {
            targetMachine.TombolMangsa();
        }
        if (targetTargetHasItemEffect)
        {
            TriggerIsTouched = true;
        }
    }
    public void letsNotTriggerButton()
    {
        IamBeingTouched = false;
        TriggerIsTouched = false;
    }
    public void TombolMangsa()
    {
        //GetComponent<FixedJoint2D>().enabled = false;
        transform.parent.GetComponent<FixedJoint2D>().enabled = false;
    }

    public bool doThisThingIsMovingTowards = false;
    public Vector3 initPositional;
    //use doTriggerButton
    public bool TriggerIsTouched;
    public bool ThingIsMoving = false;
    public bool LaunchIsMovingProgram = false;
    public GameObject targetTarget;
    public ItemEffects targetTargetHasItemEffect;
    public Vector3 gotoMove;
    public void letsThisThingThingIsMovingTowards() {
        LaunchIsMovingProgram = true;
        //if (targetTarget)
        //{
        //    targetTargetHasItemEffect = targetTarget.GetComponent<ItemEffects>();
        //    if (targetTargetHasItemEffect)
        //    {
                
        //        if (targetTargetHasItemEffect.TriggerIsTouched || targetTargetHasItemEffect.IamBeingTouched)
        //        {
        //            ThingIsMoving = true;
        //            gotoMove = new Vector3(0, -2);
        //            if (transform.position.y > targetTarget.transform.position.y)
        //            {
        //                GetComponent<FixedJoint2D>().enabled = false;
        //                //GetComponent<Rigidbody2D>().AddForce(gotoMove);
        //                transform.position += gotoMove*Time.deltaTime;

        //            } else if(transform.position.y <= targetTarget.transform.position.y)
        //            {
        //                GetComponent<FixedJoint2D>().enabled = true;
        //            }
        //        } else
        //        {
        //            ThingIsMoving = false;
        //            gotoMove = new Vector2(0, 2);
        //            if (transform.position.y < initPositional.y)
        //            {
        //                GetComponent<FixedJoint2D>().enabled = false;
        //                //GetComponent<Rigidbody2D>().AddForce(gotoMove);
        //                transform.position += gotoMove * Time.deltaTime;
        //            } else if(transform.position.y >= initPositional.y)
        //            {
        //                GetComponent<FixedJoint2D>().enabled = true;
        //            }
        //        }
        //    }
        //}
    }
    public void letsNotThisThingTHingIsMovingTowards()
    {
        LaunchIsMovingProgram = false;
    }


    //toDo list
    /*
     * Set camera zoom (ok)
     * Set camera rotation
     * Set camera effect (color)
     * particle (ok)
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
    private void Awake()
    {
        //Deserves = true;
        //if (!theSHanpeWhoIsTouching)
        //{
        //    //theSHanpeWhoIsTouching = FindObjectOfType<SHanpe>();
        //}
    }

    //Special
    bool PressVibrateGroundButton = true;
    float timingBrate = 0f;
    float timingBring = 0f;
    float StrengthLeft = 1f; //0..1
    float StrengthRight = 1f; //0..1
    bool letsbegin = false;
    public void CallVibrationGround(float howLong)
    {
        StrengthLeft = 1f;
        StrengthRight = 1f;
        PressVibrateGroundButton = true;
        timingBring = howLong;
    }
    public void CallVibrationGround(float howLong, float howStrongLeft, float howStrongRight)
    {
        StrengthLeft = howStrongLeft;
        StrengthRight = howStrongRight;
        PressVibrateGroundButton = true;
        timingBring = howLong;
    }
    public void LetsVibratorGround() //always on
    {
        if (PressVibrateGroundButton)
        {
            timingBrate = timingBring;
            PressVibrateGroundButton = false;
        }
        if (timingBrate > 0)
        {
            if (!letsbegin)
            {
                //Debug.Log("VibrateON");
                GamePad.SetVibration(playerIndex, StrengthLeft, StrengthRight);
                letsbegin = true;
            }
        }
        else
        {
            if (letsbegin)
            {
                //Debug.Log("VibrateOFF");
                GamePad.SetVibration(playerIndex, 0f, 0f);
                letsbegin = false;
            }
        }
        timingBrate -= Time.deltaTime;
    }
    //end Special

    private void FixedUpdate()
    {
        LetsVibratorGround();
    }

    void Start () {
        initPositional = transform.position;
        bool foundShanpe =false;
        if (!foundShanpe)
        {
            // Search for object with Player tag
            var go = GameObject.FindWithTag("Player");
            // Check we found an object with the player tag
            if (go)
            // Set the target to the object we found
            {
                //theGameObject = go;
                if (go.CompareTag("Player"))
                {
                    theSHanpeWhoIsTouching = go.gameObject.GetComponent<SHanpe>();
                    foundShanpe = true;
                }
            }
        }
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
        if (theSHanpeWhoIsTouching)
        {
            currentShape = theSHanpeWhoIsTouching.currBentuk;

            if (theSHanpeWhoIsTouching.currBentuk == SHanpe.Bentuk.Lingkaran)
            {
                isCircle = true;
            } else
            {
                isCircle = false;
            }

            if (theSHanpeWhoIsTouching.currBentuk == SHanpe.Bentuk.Kotak)
            {
                isSquare = true;
            }
            else
            {
                isSquare = false;
            }

            if (theSHanpeWhoIsTouching.currBentuk == SHanpe.Bentuk.Segitiga)
            {
                ShanpeIsTriangle = true;
                isTriangle = true;
            }
            else
            {
                ShanpeIsTriangle = false;
                isTriangle = false;
            }

            if (theSHanpeWhoIsTouching.currBentuk == SHanpe.Bentuk.eekSerkat)
            {
                isEikSerkat = true;
            }
            else
            {
                isEikSerkat = false;
            }

            #region riding vehcicle
            if (theSHanpeWhoIsTouching.isRidingVehicle)
            {
                //GasPedal = theSHanpeWhoIsTouching.gasPedal * MotorSpeedingScale;
                //motorProperty.motorSpeed = GasPedal;
                //motorProperty.maxMotorTorque = MotorTorqueingScale;
                //if(theSHanpeWhoIsTouching.gasPedal > 0 || theSHanpeWhoIsTouching.gasPedal < 0)
                //{
                //    for(int i = 0; i < wheelLists.Length; i++)
                //    {
                //        wheelLists[i].useMotor = true;
                //        wheelLists[i].motor = motorProperty;
                //    }
                //} else
                //{
                //    for (int i = 0; i < wheelLists.Length; i++)
                //    {
                //        wheelLists[i].useMotor = false;
                //        wheelLists[i].motor = motorProperty;
                //    }
                //}
            } else
            {

            }

            #endregion riding vehicle

            #region Hydraulic Press
            if (targetTargetHasItemEffect) TargetBeingTouched = targetTargetHasItemEffect.IamBeingTouched;
            //if (LaunchIsMovingProgram)
            //{
            //    if (targetTarget)
            //    {
            //        if(!targetTargetHasItemEffect) targetTargetHasItemEffect = targetTarget.GetComponent<ItemEffects>();
            //        if (targetTargetHasItemEffect)
            //        {

            //            if (targetTargetHasItemEffect.TriggerIsTouched || TargetBeingTouched)
            //            {
            //                ThingIsMoving = true;
            //                gotoMove = new Vector3(0, -2);
            //                if (transform.position.y > targetTarget.transform.position.y)
            //                {
            //                    if (GetComponent<FixedJoint2D>()) GetComponent<FixedJoint2D>().enabled = false;
            //                    //GetComponent<Rigidbody2D>().AddForce(gotoMove);
            //                    transform.position += gotoMove * Time.deltaTime;

            //                }
            //                else if (transform.position.y <= targetTarget.transform.position.y)
            //                {
            //                    if (GetComponent<FixedJoint2D>()) GetComponent<FixedJoint2D>().enabled = true;
            //                }
            //            }
            //        }
            //    }
            //} else
            //{
            //    ThingIsMoving = false;
            //    gotoMove = new Vector2(0, 2);
            //    if (transform.position.y < initPositional.y)
            //    {
            //        if(GetComponent<FixedJoint2D>()) GetComponent<FixedJoint2D>().enabled = false;
            //        //GetComponent<Rigidbody2D>().AddForce(gotoMove);
            //        transform.position += gotoMove * Time.deltaTime;
            //    }
            //    else if (transform.position.y >= initPositional.y)
            //    {
            //        if(GetComponent<FixedJoint2D>()) GetComponent<FixedJoint2D>().enabled = true;
            //    }
            //}
            if (TargetBeingTouched)
            {
                ThingIsMoving = true;
                gotoMove = new Vector3(0, -2);
                if (transform.position.y > targetTarget.transform.position.y)
                {
                    if (GetComponent<FixedJoint2D>()) GetComponent<FixedJoint2D>().enabled = false;
                    //GetComponent<Rigidbody2D>().AddForce(gotoMove);
                    transform.position += gotoMove * Time.deltaTime;

                }
                else if (transform.position.y <= targetTarget.transform.position.y)
                {
                    if (GetComponent<FixedJoint2D>()) GetComponent<FixedJoint2D>().enabled = true;
                }
            } else
            {
                ThingIsMoving = false;
                gotoMove = new Vector2(0, 2);
                if (transform.position.y < initPositional.y)
                {
                    if (GetComponent<FixedJoint2D>()) GetComponent<FixedJoint2D>().enabled = false;
                    //GetComponent<Rigidbody2D>().AddForce(gotoMove);
                    transform.position += gotoMove * Time.deltaTime;
                }
                else if (transform.position.y >= initPositional.y)
                {
                    if (GetComponent<FixedJoint2D>()) GetComponent<FixedJoint2D>().enabled = true;
                }
            }
            #endregion
        }

        #region Can Be What Shape

        if (canBeCircle && canBeSquare && canBeTriangle && canBeDedd)
        {
            Deserves = true;
        }
        else if (canBeCircle && canBeSquare && canBeTriangle && !canBeDedd)
        {
            if (isEikSerkat)
            {
                Deserves = false;
            }
            else if (isCircle || isSquare || isTriangle)
            {
                Deserves = true;
            }
        }
        else if (canBeCircle && canBeSquare && !canBeTriangle && canBeDedd)
        {
            if (isTriangle)
            {
                Deserves = false;
            }
            else if (isCircle || isSquare || isEikSerkat)
            {
                Deserves = true;
            }
        }
        else if (canBeCircle && canBeSquare && !canBeTriangle && !canBeDedd)
        {
            if (isTriangle || isEikSerkat)
            {
                Deserves = false;
            }
            else if (isCircle || isSquare)
            {
                Deserves = true;
            }
        }
        else if (canBeCircle && !canBeSquare && canBeTriangle && canBeDedd)
        {
            if (isSquare)
            {
                Deserves = false;
            }
            else if (isCircle || isTriangle || isEikSerkat)
            {
                Deserves = true;
            }
        }
        else if (canBeCircle && !canBeSquare && canBeTriangle && !canBeDedd)
        {
            if (isSquare)
            {
                Deserves = false;
            }
            else if (isCircle || isTriangle)
            {
                Deserves = true;
            }
        }
        else if (canBeCircle && !canBeSquare && !canBeTriangle && canBeDedd)
        {
            if (isSquare || isTriangle)
            {
                Deserves = false;
            }
            else if (isCircle || isEikSerkat)
            {
                Deserves = true;
            }
        }
        else if (canBeCircle && !canBeSquare && !canBeTriangle && !canBeDedd)
        {
            if (isSquare || isTriangle || isEikSerkat)
            {
                Deserves = false;
            }
            else if (isCircle)
            {
                Deserves = true;
            }
        }
        else if (!canBeCircle && canBeSquare && canBeTriangle && canBeDedd)
        {
            if (isCircle)
            {
                Deserves = false;
            }
            else if (isSquare || isTriangle || isEikSerkat)
            {
                Deserves = true;
            }
        }
        else if (!canBeCircle && canBeSquare && canBeTriangle && !canBeDedd)
        {
            if (isCircle || isEikSerkat)
            {
                Deserves = false;
            }
            else if (isSquare || isTriangle)
            {
                Deserves = true;
            }
        }
        else if (!canBeCircle && canBeSquare && !canBeTriangle && canBeDedd)
        {
            if (isCircle || isTriangle)
            {
                Deserves = false;
            }
            else if (isSquare || isEikSerkat)
            {
                Deserves = true;
            }
        }
        else if (!canBeCircle && canBeSquare && !canBeTriangle && !canBeDedd)
        {
            if (isCircle || isTriangle || isEikSerkat)
            {
                Deserves = false;
            }
            else if (isSquare)
            {
                Deserves = true;
            }
        }
        else if (!canBeCircle && !canBeSquare && canBeTriangle && canBeDedd)
        {
            if (isCircle || isSquare)
            {
                Deserves = false;
            }
            else if (isTriangle || isEikSerkat)
            {
                Deserves = true;
            }
        }
        else if (!canBeCircle && !canBeSquare && canBeTriangle && !canBeDedd)
        {
            if (isCircle || isSquare || isEikSerkat)
            {
                Deserves = false;
            }
            else if (isTriangle)
            {
                Deserves = true;
            }
        }
        else if (!canBeCircle && !canBeSquare && !canBeTriangle && canBeDedd)
        {
            if (isCircle || isSquare || isTriangle)
            {
                Deserves = false;
            }
            else if (isEikSerkat)
            {
                Deserves = true;
            }
        }
        else if (!canBeCircle && !canBeSquare && !canBeTriangle && !canBeDedd)
        {
            Deserves = true; //if none of them allowed, then it treated as allow all.
        }
        #endregion

        

    }

    private void Executo()
    {
        //Basically trigger for ItemEffect activation to be called by Collision and Trigger
        //considering should it be put
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
        if (doTotalResetCamera)
        {
            LetsTotalResetCamera();
        }
        if (doSetCheckPoint)
        {
            letsCheckpoint();
        }
        if (doDislodgeJoints)
        {
            letsDislodgeJoints();
        }
        if (doJockeyMachine)
        {
            letsJockeyMachine();
        }
        if (doTriggerButton)
        {
            letsTriggerButton();
        }
        if (doTriggerButton)
        {
            letsThisThingThingIsMovingTowards();
        }

        //Destroy Item
        if (killColliderAfterTouchdown)
        {
            letsKillColliderAfterTouchdown();
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
    private void ExecutoHold()
    {
        //for Collider/Trigger Hold
        if (doLavaDamage)
        {
            //damageMe(lavaDamageValue);
            lavaDamage(lavaDamageValue);
            //Debug.Log("hot");
        }
    }
    private void ExecutoExit()
    {
        if (doJockeyMachine)
        {
            letsNotJockeyMachine();
        }
        if (doTriggerButton)
        {
            letsNotTriggerButton();
        }
        if (doThisThingIsMovingTowards)
        {
            //letsNotTriggerButton();
            letsNotThisThingTHingIsMovingTowards();
        }
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
        bool ActualDeserves;
        if (collision.transform.parent)
        {
            theGameObject = collision.transform.parent.gameObject;
        }
        if (theGameObject)
        {
        }
        if (theSHanpeWhoIsTouching)
        {
            if (theSHanpeWhoIsTouching.gameObject == theGameObject)
            {

                if (Deserves) ActualDeserves = true; else ActualDeserves = false;
                //if (!MustBeTriangleShape)
                //{
                //    Deserves = true;
                //} else if(MustBeTriangleShape && ShanpeIsTriangle)
                //{
                //    Deserves = true;
                //} else
                //{
                //    Deserves = false;
                //}
                if (ActualDeserves)
                {
                    Executo();
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (theSHanpeWhoIsTouching)
        {
            ExecutoHold();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ExecutoExit();
    }

    //Non-Trigger Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool ActualDeserves;
        if (collision.transform.parent)
        {
            theGameObject = collision.transform.parent.gameObject;
        }
        if (theGameObject)
        {
        }
        if (theSHanpeWhoIsTouching)
        {
            if (theSHanpeWhoIsTouching.gameObject == theGameObject)
            {

                if (Deserves) ActualDeserves = true; else ActualDeserves = false;
                //if (!MustBeTriangleShape)
                //{
                //    Deserves = true;
                //} else if(MustBeTriangleShape && ShanpeIsTriangle)
                //{
                //    Deserves = true;
                //} else
                //{
                //    Deserves = false;
                //}
                if (ActualDeserves)
                {
                    Executo();
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (theSHanpeWhoIsTouching)
        {
            ExecutoHold();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ExecutoExit();
    }
}
