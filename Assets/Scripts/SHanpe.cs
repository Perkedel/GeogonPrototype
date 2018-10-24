﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput; //https://www.youtube.com/watch?v=DNLAuV-d4sA 
using UnityEngine.Audio; //https://docs.unity3d.com/ScriptReference/AudioSource.Play.html

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class SHanpe : MonoBehaviour {

    //Shape of SHanpe
    public enum Bentuk { eekSerkat, Lingkaran, Kotak, Segitiga};
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

    public float Sliding = 10f;
    public float Rolling = 10f;
    public float Jumping = 100f;

    float currSlide = 10f;
    float currRolls = 10f;
    float currJumps = 100f;

    public int jumpToken = 2; //Recomended 2! Not Recomended when > 2!
    [SerializeField][Range(0,2)] int currJumpToken = 2;

    //Parametrics, Customizable variables
    public float timeToRestart = 5f;
    public enum whatToDoIfDead { ReloadLevel, Respawn, quitToMenu, GameOverScreen, DoNothing};
    public whatToDoIfDead deadAction = whatToDoIfDead.Respawn;
    public bool resetRotationOnRespawn = true;
    public bool resetShapeOnRespawn = false;
    public bool canChangeShapeWhileDead = true;

    //Vital Conditions
    [Range(0, 100)] private float healthPoint = 100;
    [Range(0, 100)] private float armourPoint = 20;

    [SerializeField] private bool grounded = false;

    public Rigidbody2D rb2D;

    //Extern Conditions
    [SerializeField] private bool eekSerkat = false; //eek Serkat means died
    [SerializeField] private float timerRestart = 0;
    [SerializeField] public bool healthWasChanged = false;
    private float timerHealthChanged;

    //View Conditions
    [SerializeField][Range(0, 100)] private float healthMonitor = 100;
    [SerializeField][Range(0, 100)] private float armourMonitor = 20;

    //Item Effects
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

    public float HealthPoint
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
    private Vector2 initPosition;
    private Bentuk initBentuk;
    private float initHP;
    private float initArmour;

    //Controller Configurations, Hold Functionality template
    public Joystick SHanpedJoystick;
    
    private void changeShapeButton()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetButtonDown("Fire1"))
        {
            if(!eekSerkat) bentuk = Bentuk.Lingkaran;
            currBentuk = Bentuk.Lingkaran;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)|| Input.GetButtonDown("Fire2"))
        {
            if(!eekSerkat) bentuk = Bentuk.Kotak;
            currBentuk = Bentuk.Kotak;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetButtonDown("Fire3"))
        {
            if(!eekSerkat) bentuk = Bentuk.Segitiga;
            currBentuk = Bentuk.Segitiga;
        }
    }

    public void LetsJump()
    {
        if (currJumpToken > 0)
        {
            itSelfSound.PlayOneShot(JumpSounds[Random.Range(0, JumpSounds.Length)], .05f);
            rb2D.AddForce(Vector2.up * currJumps * 10f);
            currJumpToken -= 1;
        }
    }
    private void JumpButton()
    {
        //CrossPlatformInputManager is incompatible with PC (Windows) standalone!
        if (Input.GetButtonDown("Jump") && !TheCameraAction.isMovingCamera) //Jump button
        {
            LetsJump();
        }
    }

    //functionality, Template Methods
    //Hard To figure out Jump By touch button
    //Devin Curry https://www.youtube.com/watch?v=gKjKFZ30684 , Learn Everything Fast https://www.youtube.com/watch?v=JoyjDac-oJY
    public int JumpingCable = 0;
    [SerializeField] private bool hasJumpPressed = false;
    public void JumpByHand()
    {
        if (!TheCameraAction.isMovingCamera)
        {
            if (!hasJumpPressed && JumpingCable == 0)
            {
                JumpingCable = 1;
                hasJumpPressed = true;

                //Insert Jump Methodings
                LetsJump();
            }
            if (hasJumpPressed && JumpingCable != 0)
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

    public void setShape(int index)
    {
        if(!eekSerkat) bentuk = (Bentuk) index;
        currBentuk = (Bentuk)index;
    }

    public LevelLoader LevelLoadering;
    public void restartLevel()
    {
        LevelLoadering.RestartLevel();
    }
    public void respawn()
    {
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

    //Check Something from other objects
    public FollowPlayerCSharp TheCameraAction;

    //Begin
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        //bentuk = Bentuk.Lingkaran;
        currBentuk = bentuk;
        itSelfSound = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
        JumpingCable = 0;
        currJumpToken = jumpToken;

        //catching initial parameters for respawning
        initPosition = new Vector2(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y);
        initBentuk = bentuk;
        initHP = HealthPoint;
        initArmour = armourPoint;
    }
	
	// Update is called once per frame
	void Update () {

        float ControlSlide = 0;
        float ControlRolls = 0;

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
                if (currSlide > 0) ControlSlide = (Input.GetAxis("Horizontal") + SHanpedJoystick.Horizontal) * currSlide; else ControlSlide = 0;
                if (currRolls > 0) ControlRolls = (Input.GetAxis("Horizontal") + SHanpedJoystick.Horizontal) * currRolls; else ControlRolls = 0;
                //To allow Keyboard controll, you must have Axes Horizontal and Vertical that set to Type as Key or Mouse Button
                //Then add another same set for Joystick. Types are Joystick Axis.

                //jump button
                JumpButton();

                //change shape
                changeShapeButton();

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
        //itSelfSound.Play(0);
        //Debug.Log("Collision Enter");
        itSelfSound.PlayOneShot(CollisionSounds[Random.Range(0, CollisionSounds.Length)], .01f);
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            currJumpToken = jumpToken; //Reset jump token when touch ground! Joel's addition.
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("Collision Exit");
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
            //currJumpToken -= 1; //uncomment to make him lose 1 jump token in air.
        }
    }
}
