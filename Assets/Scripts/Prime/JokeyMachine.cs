using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JokeyMachine : MonoBehaviour
{
    //Connector
    public WheelJoint2D[] wheelSets;
    public GameObject theHUD;
    public Button jokeyButton;
    public GameObject whoIsTouching;
    public SHanpe rider;

    //Storage
    public float riderMass = 0;
    public float riderGravity = 0;

    //Status
    [SerializeField] private bool isPossessed;
    [SerializeField] private bool beingTouched;
    [SerializeField] private bool canBeRode;
    [SerializeField] private bool beingRode;
    [SerializeField] private bool prepareResetIfDiedOnRoad;

    // Start is called before the first frame update
    void Start()
    {
        if (!jokeyButton)
        {
            var findHUD = GameObject.FindWithTag("HUD");
            if (findHUD)
            {
                theHUD = findHUD;
                jokeyButton = GameObject.FindWithTag("JockeyButton").GetComponent<Button>();
                jokeyButton.onClick.AddListener(pressToRide);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rider)
        {
            if (!rider.eekSerkat)
            {
                if (canBeRode)
                {
                    jokeyButton.interactable = true;
                    //jokeyButton.onClick.AddListener(delegate { pressToRide(); });
                    if (beingRode)
                    {
                        //rider.GetComponent<Rigidbody2D>().gravityScale = 0f;
                        Vector3 positronal = new Vector3(transform.position.x, transform.position.y);
                        rider.GetComponent<Transform>().SetPositionAndRotation(positronal, transform.rotation);
                        prepareResetIfDiedOnRoad = true;
                        if (rider.eekSerkat) resetRiderCondition();
                    }
                }
                else
                {
                    resetRiderCondition();
                    //jokeyButton.onClick.RemoveAllListeners();

                }
            }
            else
            {
                resetRiderCondition();
                //jokeyButton.onClick.RemoveAllListeners();
            }
        } else
        {
            resetRiderCondition();
            //jokeyButton.onClick.RemoveAllListeners();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        beingTouched = true;
        whoIsTouching = other.transform.gameObject;
        rider = whoIsTouching.transform.parent.GetComponent<SHanpe>();
        //jokeyButton = rider.GetComponent<Button>();

        if (rider)
        {
            if (rider.gameObject == whoIsTouching.transform.parent.gameObject)
            {
                canBeRode = true;
                jokeyButton.interactable = true;
            } else
            {
                canBeRode = false;
                jokeyButton.interactable = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //whoIsTouching = null;
        //rider = null;
        beingTouched = false;
        canBeRode = false;
        jokeyButton.interactable = false;
    }

    public void pressToRide()
    {
        Debug.Log("Pressed Button Jockey");
        if (!beingRode)
        {
            //riderMass = rider.GetComponent<Rigidbody2D>().mass;
            //rider.GetComponent<Rigidbody2D>().mass = 0f;
            //riderGravity = rider.GetComponent<Rigidbody2D>().gravityScale;
            //rider.GetComponent<Rigidbody2D>().gravityScale = 0f;
            beingRode = true;
        } else if(beingRode)
        {
            //rider.GetComponent<Rigidbody2D>().mass = riderMass;
            //rider.GetComponent<Rigidbody2D>().gravityScale = riderGravity;
            beingRode = false;
        }

    }

    public void resetRiderCondition()
    {
        if (rider)
        {
            //rider.GetComponent<Rigidbody2D>().mass = riderMass;
            //rider.GetComponent<Rigidbody2D>().gravityScale = riderGravity;
        }
        if (jokeyButton)
        {
            jokeyButton.interactable = false;
        }
        prepareResetIfDiedOnRoad = false;
        beingRode = false;
        canBeRode = false;
        beingTouched = false;
        isPossessed = false;
    }
}
