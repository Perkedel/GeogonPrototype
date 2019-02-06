using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput; //https://www.youtube.com/watch?v=DNLAuV-d4sA 
using UnityEngine.Audio; //https://docs.unity3d.com/ScriptReference/AudioSource.Play.html
using XInputDotNetPure;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(FixedJoint2D))]
public class SHanpe : MonoBehaviour {

    //Shape of SHanpe
    public enum Bentuk { eekSerkat, Lingkaran, Kotak, Segitiga };
    public Bentuk bentuk, currBentuk;

    //public GameObject[] Shappings;
    public GameObject Circling;
    public GameObject Squaring;
    public GameObject Triangling;
    public GameObject Dying;

    public AudioSource itSelfSound;
    public AudioClip[] CollisionSounds;
    public AudioClip[] JumpSounds;
    public AudioClip[] RollingSounds;
    public AudioClip[] DeathSounds;

    public ParticleSystem[] Exploded;
    [SerializeField] private bool hasBeenExploded = false;

    public float Sliding = 10f;
    public float Rolling = 10f;
    public float Jumping = 100f;

    float currSlide = 10f;
    float currRolls = 10f;
    float currJumps = 100f;

    public float gasPedal;
    public float motorForcing;

    public int jumpToken = 2; //Recomended 2! Not Recomended when > 2!
    [SerializeField] [Range(0, 2)] int currJumpToken = 2;

    //Parametrics, Customizable variables
    public float timeToRestart = 5f;
    public enum whatToDoIfDead { ReloadLevel, Respawn, quitToMenu, GameOverScreen, DoNothing };
    public whatToDoIfDead deadAction = whatToDoIfDead.Respawn;
    public bool resetRotationOnRespawn = true;
    public bool resetShapeOnRespawn = false;
    public bool canChangeShapeWhileDead = true;
    public bool controllerIsActive = true;
    public float gasPedalScale = 1000f;
    public float motorForcingScale = 100f;

    //Initial Conditions
    [Range(0, 100)] public float InitHealthPoint = 100;
    [Range(0, 100)] public float InitArmourPoint = 20;

    //Vital Conditions
    [Range(0, 100)] private float healthPoint = 100; //This is real health point
    [Range(0, 100)] private float armourPoint = 20; //there is no extra armour by reproductive exposeness because SHanpe is not Human!

    [SerializeField] private bool grounded = false;

    public Rigidbody2D rb2D;

    //Extern Conditions
    public bool eekSerkat = false; //eek Serkat means died
    [SerializeField] private float timerRestart = 0;
    [SerializeField] public bool healthWasChanged = false;
    private float timerHealthChanged;
    public ItemEffects JockeyMachine;
    public bool JockeyIsThat = false;
    public bool isRidingVehicle = false;
    public JointMotor2D motorKey;

    //View Conditions
    [SerializeField] [Range(0, 100)] private float healthMonitor = 100;
    [SerializeField] [Range(0, 100)] private float armourMonitor = 20;
    [SerializeField] private bool VibrationGrounded = false;

    //Controller
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    //Item Effects (Deprecating)
    private bool isTickled = false;

    public bool IsTickled
    {
        get
        {
            return isTickled;
        }

        set
        {
            isTickled = value;
            //Debug.Log("setTickled");
        }
    }

    public float HealthPoint //and this is how to get set it
    {
        get
        {
            return healthPoint;
        }

        set
        {
            healthPoint = value;
            healthMonitor = value;
            healthWasChanged = true;
            timerHealthChanged = 0;
        }
    }

    public float ArmourPoint
    {
        get
        {
            return armourPoint;
        }

        set
        {
            armourPoint = value;
            armourMonitor = value;
        }
    }

    public bool viewTickled;

    //Initial parametrics to be caught by Awake
    public Vector2 initPosition;
    public Bentuk initBentuk;
    public float initHP;
    public float initArmour;

    //Controller Configurations, Hold Functionality template
    public Joystick SHanpedJoystick;

    private void changeShapeButton()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetButtonDown("Fire1"))
        {
            if (!eekSerkat) bentuk = Bentuk.Lingkaran;
            currBentuk = Bentuk.Lingkaran;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetButtonDown("Fire2"))
        {
            if (!eekSerkat) bentuk = Bentuk.Kotak;
            currBentuk = Bentuk.Kotak;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetButtonDown("Fire3"))
        {
            if (!eekSerkat) bentuk = Bentuk.Segitiga;
            currBentuk = Bentuk.Segitiga;
        }
        if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3)) ||
            (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3")))
        {
            Vibration.Vibrate(50);
            CallVibrationGround(.1f, .4f, .4f);
        }
    }

    public void LetsJump()
    {
        if (currJumpToken > 0)
        {
            itSelfSound.PlayOneShot(JumpSounds[Random.Range(0, JumpSounds.Length)], .05f);
            Vibration.Vibrate(75);
            CallVibrationGround(.1f, 1f, 1f);
            rb2D.AddForce(Vector2.up * currJumps * 10f);
            currJumpToken -= 1;
        }
    }
    private void JumpButton()
    {
        //CrossPlatformInputManager is incompatible with PC (Windows) standalone!
        if (Input.GetButtonDown("Jump") && !TheCameraAction.isMovingCamera) //Jump button
        {
            if (!isRidingVehicle)
            {
                LetsJump();
            }
        }
    }

    //functionality, Template Methods
    //Hard To figure out Jump By touch button
    //Devin Curry https://www.youtube.com/watch?v=gKjKFZ30684 , Learn Everything Fast https://www.youtube.com/watch?v=JoyjDac-oJY
    public int JumpingCable = 0;
    [SerializeField] private bool hasJumpPressed = false;
    public void JumpByHand()
    {
        if (controllerIsActive)
        {
            if (!TheCameraAction.isMovingCamera)
            {
                if (!hasJumpPressed && JumpingCable == 0)
                {
                    JumpingCable = 1;
                    hasJumpPressed = true;

                    //Insert Jump Methodings
                    if (!isRidingVehicle)
                    {
                        LetsJump();
                    }
                }
                if (hasJumpPressed && JumpingCable != 0)
                {
                    JumpingCable = 0;
                }
            }
            else
            {
                JumpingCable = 0;
            }
        } else
        {
            JumpingCable = 0;
        }
    }
    public void JumpByNone()
    {
        JumpingCable = 0;
        hasJumpPressed = false;
    }

    [SerializeField]private bool hasJockeyPressed = false;
    public int RidingCable = 0;
    private void ToggleIsRiding()
    {
        if (JockeyIsThat)
        {
            if (!isRidingVehicle && !hasJockeyPressed)
            {
                Debug.Log("isRidingVehicle calk" + isRidingVehicle);
                isRidingVehicle = true;
                hasJockeyPressed = true;
                Debug.Log("isRidingVehicle talk" + isRidingVehicle);

            }
        }
        if (isRidingVehicle && !hasJockeyPressed)
        {
            Debug.Log("isRidingVehicle walk" + isRidingVehicle);
            isRidingVehicle = false;
            hasJockeyPressed = true;
            Debug.Log("isRidingVehicle nalk" + isRidingVehicle);
        }
        
    }
    public void PressToRide()
    {
        //if (controllerIsActive)
        //{
        //    if (!TheCameraAction.isMovingCamera)
        //    {
        //        if (!hasJockeyPressed)
        //        {
        //            Debug.Log("PressRide");
        //            if (true)
        //            {
        //                Debug.Log("Jockey machine");
        //                ToggleIsRiding();
        //            }
        //            hasJockeyPressed = true;
        //        }
        //    }
        //}
        //if (!hasJockeyPressed)
        //{
        //    if (controllerIsActive)
        //    {
        //        Debug.Log("Pressed");
        //        ToggleIsRiding();
        //    }
        //    hasJockeyPressed = true;
        //}
        if (controllerIsActive)
        {
            if (!TheCameraAction.isMovingCamera)
            {
                if (!hasJockeyPressed && RidingCable == 0)
                {
                    //Insert Jump Methodings
                    ToggleIsRiding();
                    //Debug.Log("gamein");
                    RidingCable = 1;
                    hasJockeyPressed = true;
                }
                if (hasJockeyPressed && RidingCable != 0)
                {
                    RidingCable = 0;
                }
            }
            else
            {
                RidingCable = 0;
            }
        }
        else
        {
            RidingCable = 0;
        }
    }
    public void DepressToStay()
    {
        RidingCable = 0;
        hasJockeyPressed = false;
    }

    public void setShape(int index)
    {
        //Vibration.Vibrate(50);
        if(!eekSerkat) bentuk = (Bentuk) index;
        currBentuk = (Bentuk)index;
    }

    public void setHealth(float value)
    {
        HealthPoint = value;
    }

    public void setArmour(float value)
    {
        ArmourPoint = value;
    }

    public void addHealth(float value) //do not hurt SHanpe using this!
    {
        HealthPoint += value;
    }

    public void addArmour(float value)
    {
        ArmourPoint += value;
    }

    public void damageMe(float value) //To hurt SHanpe, this is the correct method that should be used instead!
    {
        if (value > 0)
        {
            Vibration.Vibrate(150);
            CallVibrationGround(.2f, 1f, 1f);
            HealthPoint -= (value - ArmourPoint/3f);
        } else
        {
            Debug.LogError("Damage Value = " + value + "\n<color=red>Damage Value cannot be minus or zero a.k.a <= 0!!!</color>");
        }
        if(HealthPoint <= 0)
        {
            Vibration.Vibrate(2000);
            CallVibrationGround(5f, 100f, 100f);
        }
    }

    public void comSayFromItem(string value)
    {
        //https://docs.unity3d.com/Manual/StyledText.html Guide for stylished text
        //https://docs.unity3d.com/ScriptReference/Debug.Log.html This is example of Debug Log
        Debug.Log(value);
    }

    public void vibrateDevice()
    {
#if UNITY_IOS || UNITY_ANDROID || UNITY_WSA
        //Handheld.Vibrate();
#endif
    }

    public LevelLoader LevelLoadering;
    public void restartLevel()
    {
        LevelLoadering.RestartLevel();
    }
    public void respawn()
    {
        Vibration.Cancel();
        if (resetRotationOnRespawn)
        {
            //https://answers.unity.com/questions/208447/set-rotation-of-a-transform.html
            //bbrode
            transform.localEulerAngles = new Vector3(0f, 0f);
        }
        if (resetShapeOnRespawn)
        {
            bentuk = initBentuk;
            currBentuk = initBentuk;
        }
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
        HealthPoint = initHP;
        ArmourPoint = initArmour;
        eekSerkat = false;

        //https://answers.unity.com/questions/8291/how-to-move-a-gameobject-from-his-position-to-a-xy.html
        transform.position = new Vector2(initPosition.x, initPosition.y);
    }
    public void quitToMenu()
    {
        LevelLoadering.GoToMenu();
    }
    public void GameOverScreen()
    {
        LevelLoadering.GameOver();
    }

    public bool SelfDestructButton = false;
    void ScronchSelf()
    {
        SelfDestructButton = false;
        HealthPoint = 0;
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

    //Check Something from other objects
    public FollowPlayerCSharp TheCameraAction;

    //Begin
    private void Awake()
    {
        //isRidingVehicle = false;
        GetComponent<FixedJoint2D>().enabled = false;
        rb2D = GetComponent<Rigidbody2D>();
        //bentuk = Bentuk.Lingkaran;
        currBentuk = bentuk;
        itSelfSound = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
        //Let's INit
        setHealth(InitHealthPoint);
        setArmour(InitArmourPoint);
        JumpingCable = 0;
        currJumpToken = jumpToken;

        //catching initial parameters for respawning
        initPosition = new Vector2(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y);
        initBentuk = bentuk;
        initHP = HealthPoint;
        initArmour = armourPoint;

        //find Object
        if (!LevelLoadering)
        {
            var go = GameObject.FindWithTag("LevelLoader");
            // Check we found an object with the player tag
            if (go)
            // Set the target to the object we found
            {
                if (go.CompareTag("LevelLoader"))
                    LevelLoadering = go.gameObject.GetComponent<LevelLoader>();
            }
        }
        if (!TheCameraAction)
        {
            var go = GameObject.FindWithTag("MainCamera");
            // Check we found an object with the player tag
            if (go)
            // Set the target to the object we found
            {
                if (go.CompareTag("MainCamera"))
                    TheCameraAction = go.gameObject.GetComponent<FollowPlayerCSharp>();
            }
        }

    }

    private void FixedUpdate()
    {
        LetsVibratorGround();
    }

    bool JointReseted = false;
    // Update is called once per frame
    void Update () {
        
        //Copied from XInputTestCS.cs
        if (!playerIndexSet || !prevState.IsConnected)
        {
            //for (int i = 0; i < 4; ++i)
            //{
            //    PlayerIndex testPlayerIndex = (PlayerIndex)i;
            //    GamePadState testState = GamePad.GetState(testPlayerIndex);
            //    if (testState.IsConnected)
            //    {
            //        Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
            //        playerIndex = testPlayerIndex;
            //        playerIndexSet = true;
            //    }
            //}
            PlayerIndex testPlayerIndex = (PlayerIndex)0;
            GamePadState testState = GamePad.GetState(testPlayerIndex);
            if (testState.IsConnected)
            {
                playerIndex = testPlayerIndex;
                playerIndexSet = true;
            }
        }
        //End Copy of XInputTestCS.cs

        float ControlSlide = 0;
        float ControlRolls = 0;

        int randSound = Random.Range(0, DeathSounds.Length);

        //for (int i = 0; i < Input.touchCount; i++)
        //{
        //    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
        //    Debug.DrawLine(Vector3.zero, touchPosition, Color.red);
        //}

        if (!eekSerkat)
        {
            if (bentuk == Bentuk.eekSerkat) //resurrect from death
            {
                bentuk = currBentuk;
                currJumpToken = jumpToken;
            }
            if (!TheCameraAction.isMovingCamera)
            {
                //Controlling
                if (controllerIsActive)
                {
                    if (!isRidingVehicle)
                    {
                        if (currSlide > 0) ControlSlide = (Input.GetAxis("Horizontal") + SHanpedJoystick.Horizontal) * currSlide; else ControlSlide = 0;
                        if (currRolls > 0) ControlRolls = (Input.GetAxis("Horizontal") + SHanpedJoystick.Horizontal) * currRolls; else ControlRolls = 0;

                        //To allow Keyboard controll, you must have Axes Horizontal and Vertical that set to Type as Key or Mouse Button
                        //Then add another same set for Joystick. Types are Joystick Axis.

                        
                    } else if(isRidingVehicle)
                    {
                        currJumpToken = jumpToken;
                        ControlSlide = 0f;
                        ControlRolls = 0f;
                    }

                    //jump button
                    JumpButton();

                    //change shape
                    changeShapeButton();
                }
                //Rolling
                if (grounded)
                {
                    if (!itSelfSound.isPlaying)
                    {
                        //itSelfSound.PlayOneShot(RollingSounds[Random.Range(0, RollingSounds.Length)], rb2D.velocity.magnitude * .01f);
                    }
                } else
                {
                    
                }
            }
        } else //start to dead
        {
            //Handheld.Vibrate(); //https://docs.unity3d.com/ScriptReference/Handheld.Vibrate.html
            //Vibration.Vibrate(2000); //Just use the basic 2s vibrate! this character is not human being that uses heartbeat dying pattern!!!

            isRidingVehicle = false;

            bentuk = Bentuk.eekSerkat;
            currJumpToken = 0;

            timerRestart += Time.deltaTime;
            if(timerRestart >= timeToRestart) //restart this SHanpe
            {
                switch (deadAction)
                {
                    default:
                        Debug.Log("No Dead Action/Unknown Dead Action!!!");
                        break;
                    case whatToDoIfDead.ReloadLevel:
                        restartLevel();
                        break;
                    case whatToDoIfDead.Respawn:
                        respawn();
                        break;
                    case whatToDoIfDead.quitToMenu:
                        quitToMenu();
                        break;
                    case whatToDoIfDead.GameOverScreen:
                        GameOverScreen();
                        break;
                    case whatToDoIfDead.DoNothing:
                        break;
                }
                timerRestart = 0;
            }
            if (canChangeShapeWhileDead)
            {
                changeShapeButton();
            }
        }

        currJumps = Jumping; //download jump force from serial field

        switch (bentuk)
        {
            case Bentuk.eekSerkat:
                currRolls = 0f;
                currSlide = 0f;
                Circling.SetActive(false);
                Squaring.SetActive(false);
                Triangling.SetActive(false);
                Dying.SetActive(true);
                break;
            case Bentuk.Lingkaran:
                currRolls = Rolling;
                currSlide = 0f;
                Circling.SetActive(true);
                Squaring.SetActive(false);
                Triangling.SetActive(false);
                Dying.SetActive(false);
                break;
            case Bentuk.Kotak:
                currRolls = 0f;
                currSlide = Sliding;
                Circling.SetActive(false);
                Squaring.SetActive(true);
                Triangling.SetActive(false);
                Dying.SetActive(false);
                break;
            case Bentuk.Segitiga:
                currRolls = Rolling * 2f;
                currSlide = Sliding;
                Circling.SetActive(false);
                Squaring.SetActive(false);
                Triangling.SetActive(true);
                Dying.SetActive(false);
                break;
            default:
                currRolls = Rolling;
                currSlide = Sliding;
                Circling.SetActive(true);
                Squaring.SetActive(false);
                Triangling.SetActive(false);
                Dying.SetActive(false);
                break;
        }

        
        if (isRidingVehicle)
        {
            JointReseted = false;
            GetComponent<FixedJoint2D>().enabled = true;
            if (JockeyMachine)
            {
                GetComponent<FixedJoint2D>().connectedBody = JockeyMachine.GetComponent<Rigidbody2D>();
                GetComponent<Rigidbody2D>().mass = 0;
                Vector3 hookPosition = new Vector3(JockeyMachine.transform.position.x, JockeyMachine.transform.position.y);
                transform.SetPositionAndRotation(hookPosition, JockeyMachine.transform.rotation);

                //https://answers.unity.com/questions/821148/modifying-wheel-joint-2d-motor-speed.html Glued Brain
                gasPedal = Mathf.Clamp((Input.GetAxis("Horizontal") + SHanpedJoystick.Horizontal),-1,1);
                motorKey.motorSpeed = gasPedal * gasPedalScale;
                motorForcing = motorForcingScale;
                motorKey.maxMotorTorque = motorForcing;
                if (gasPedal > 0 || gasPedal < 0)
                {
                    for(int i = 0; i < JockeyMachine.wheelLists.Length; i++)
                    {
                        JockeyMachine.wheelLists[i].useMotor = true;
                        JockeyMachine.wheelLists[i].motor = motorKey;
                    }
                }
                else
                {
                    for (int i = 0; i < JockeyMachine.wheelLists.Length; i++)
                    {
                        JockeyMachine.wheelLists[i].useMotor = false;
                        JockeyMachine.wheelLists[i].motor = motorKey;
                    }
                }
            }
        } else
        {
            gasPedal = 0f;
            if (!JointReseted)
            {
                //GetComponent<FixedJoint2D>().breakForce = 0;
                //GetComponent<FixedJoint2D>().breakForce = float.PositiveInfinity;
                GetComponent<Rigidbody2D>().mass = 1f;
                GetComponent<FixedJoint2D>().connectedBody = null;
                GetComponent<FixedJoint2D>().enabled = false;
                JointReseted = true;
            }
        }

        if (Dying.activeSelf)
        {
            if (!hasBeenExploded)
            {
                for (int i = 0; i < Exploded.Length; i++)
                {
                    Instantiate(Exploded[i].gameObject, GetComponent<Transform>().position, Quaternion.identity);
                }
                itSelfSound.PlayOneShot(DeathSounds[randSound], 1);
                hasBeenExploded = true;
            }
        } else
        {
            //Destroy(Exploded);
            hasBeenExploded = false;
        }

        rb2D.AddForce(Vector2.right * ControlSlide);
        rb2D.AddTorque(ControlRolls * -1f);

        //interclamper
        if(healthPoint > 100)
        {
            healthPoint = 100;
        }
        if(healthPoint < 0)
        {
            healthPoint = 0;
            //eekSerkat = true;
        }

        if (armourPoint > 100)
        {
            armourPoint = 100;
        }
        if (armourPoint < 0)
        {
            armourPoint = 0;
        }

        if (healthMonitor > 100)
        {
            healthMonitor = 100;
        }
        if (healthMonitor < 0)
        {
            healthMonitor = 0;
        }
        if (armourMonitor > 100)
        {
            armourMonitor = 100;
        }
        if (armourMonitor < 0)
        {
            armourMonitor = 0;
        }

        //tickled
        if (!isTickled)
        {
            viewTickled = false;
        } else
        {
            viewTickled = true;
        }

        //Dedd
        if(healthPoint == 0)
        {
            eekSerkat = true;
        } else
        {
            eekSerkat = false;
        }

        //SelfDestroy
        if (SelfDestructButton)
        {
            SelfDestructButton = false;
            ScronchSelf();
        }

        //health was changed
        if (healthWasChanged)
        {
            timerHealthChanged += Time.deltaTime;

            if(timerHealthChanged > 2)
            {
                healthWasChanged = false;
            }
        }
	}

    //Tony Morelli https://www.youtube.com/watch?v=cl201U1oUYs
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vibration.Vibrate(50);
        CallVibrationGround(.15f);
        //Touched the Ground
        //itSelfSound.Play(0);
        //Debug.Log("Collision Enter");
        itSelfSound.PlayOneShot(CollisionSounds[Random.Range(0, CollisionSounds.Length)], .01f);
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            currJumpToken = jumpToken; //Reset jump token when touch ground! Joel's addition.
        }

        //Picked an Item
        //GameObject anItem = collision.gameObject;
        //ItemEffects anItemEffect = anItem.GetComponent<ItemEffects>();
        //if(collision.gameObject.CompareTag("Item") && !!collision.gameObject.GetComponent<Collider2D>().isTrigger) //if you collide with item and is not trigger
        //{
        //    Debug.Log(anItem.gameObject.name);
        //    if (anItemEffect.doAddHealth)
        //    {
        //        addHealth(anItemEffect.addHPvalue);
        //    }
        //    if (anItemEffect.doSetHealth)
        //    {
        //        setHealth(anItemEffect.setHPvalue);
        //    }
        //    if (anItemEffect.singleUse)
        //    {
        //        //anItem.destroySelf();
        //        Destroy(anItem.gameObject);
        //    }
        //    //collision.gameObject.SetActive(false);
        //}

        //Jockey-able vehicles
        //ItemEffects verifyingJockey = collision.gameObject.GetComponent<ItemEffects>();
        //if (verifyingJockey.doJockeyMachine)
        //{
        //    JockeyMachine = collision.gameObject.GetComponent<ItemEffects>();
        //}
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        PressVibrateGroundButton = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Left the Ground
        //Debug.Log("Collision Exit");
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
            //currJumpToken -= 1; //uncomment to make him lose 1 jump token in air.

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Picked an Item
        ItemEffects anItemEffect = collision.GetComponent<ItemEffects>();
        if (anItemEffect != null) //if you collide with item
        {
            //Debug.Log(anItemEffect.gameObject.name);
            //if (anItemEffect.doVibrate)
            //{
            //    Vibration.Vibrate();
            //}
            //if (anItemEffect.doAddHealth)
            //{
            //    addHealth(anItemEffect.addHPvalue);
            //}
            //if (anItemEffect.doSetHealth)
            //{
            //    setHealth(anItemEffect.setHPvalue);
            //}
            //if (anItemEffect.doDamageMe)
            //{
            //    damageMe(anItemEffect.damageMeValue);
            //}
            //if (anItemEffect.singleUse)
            //{
            //    //anItem.destroySelf();
            //    Destroy(anItemEffect.gameObject);
            //}
            //if (anItemEffect.doSayDebug)
            //{
            //    anItemEffect.sayDebug(anItemEffect.whatDoesDebugSay);
            //}
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //stayed on item
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //left from item
    }
}
