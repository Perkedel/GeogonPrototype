using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SHanpe : MonoBehaviour {

    public enum Bentuk { eekSerkat, Lingkaran, Kotak, Segitiga};
    public Bentuk bentuk, currBentuk;

    //public GameObject[] Shappings;
    public GameObject Circling;
    public GameObject Squaring;
    public GameObject Triangling;
    public GameObject Dying;

    public float Sliding = 10f;
    public float Rolling = 10f;
    public float Jumping = 100f;

    float currSlide = 10f;
    float currRolls = 10f;
    float currJumps = 100f;

    public int jumpToken = 2; //Recomended 2! Not Recomended when > 2!
    [SerializeField][Range(0,2)] int currJumpToken = 2;

    //Parametrics
    public float timeToRestart = 5f;
    public enum whatToDoIfDead { ReloadLevel, Respawn};
    public whatToDoIfDead deadAction = whatToDoIfDead.Respawn;

    //Vital Conditions
    [Range(0, 100)] private float healthPoint = 100;
    [Range(0, 100)] private float armourPoint = 20;

    [SerializeField] bool grounded = false;

    public Rigidbody2D rb2D;

    //Extern Conditions
    [SerializeField] private bool eekSerkat = false; //eek Serkat means died
    [SerializeField] private float timerRestart = 0;

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

    //functionality
    public LevelLoader LevelLoadering;
    void restartLevel()
    {
        LevelLoadering.RestartLevel();
    }
    void respawn()
    {
        bentuk = initBentuk;
        currBentuk = initBentuk;
        HealthPoint = initHP;
        ArmourPoint = initArmour;
        eekSerkat = false;

        //https://answers.unity.com/questions/8291/how-to-move-a-gameobject-from-his-position-to-a-xy.html
        transform.position = new Vector2(initPosition.x, initPosition.y);
    }

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        //bentuk = Bentuk.Lingkaran;
        currBentuk = bentuk;
    }

    // Use this for initialization
    void Start () {
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

        if (!eekSerkat)
        {
            if (bentuk == Bentuk.eekSerkat) //resurrect from death
            {
                bentuk = currBentuk;
                currJumpToken = jumpToken;
            }

            //Controlling
            if (currSlide > 0) ControlSlide = Input.GetAxis("Horizontal") * currSlide; else ControlSlide = 0;
            if (currRolls > 0) ControlRolls = Input.GetAxis("Horizontal") * currRolls; else ControlRolls = 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currJumpToken > 0)
                {
                    rb2D.AddForce(Vector2.up * currJumps * 10f);
                    currJumpToken -= 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                bentuk = Bentuk.Lingkaran;
                currBentuk = Bentuk.Lingkaran;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                bentuk = Bentuk.Kotak;
                currBentuk = Bentuk.Kotak;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                bentuk = Bentuk.Segitiga;
                currBentuk = Bentuk.Segitiga;
            }
        } else //start to dead
        {
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
                }
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
            eekSerkat = true;
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
	}

    //Tony Morelli https://www.youtube.com/watch?v=cl201U1oUYs
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision Enter");
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
