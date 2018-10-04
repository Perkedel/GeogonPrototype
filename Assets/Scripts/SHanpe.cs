using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHanpe : MonoBehaviour {

    public enum Bentuk { Lingkaran, Kotak, Segitiga};
    public Bentuk bentuk;

    //public GameObject[] Shappings;
    public GameObject Circling;
    public GameObject Squaring;
    public GameObject Triangling;

    public float Sliding = 10f;
    public float Rolling = 10f;
    public float Jumping = 100f;

    float currSlide = 10f;
    float currRolls = 10f;
    float currJumps = 100f;

    public int jumpToken = 2; //Recomended 2! Not Recomended when > 2!
    [SerializeField][Range(0,2)] int currJumpToken = 2;

    bool grounded = false;

    public Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        
        
    }

    // Use this for initialization
    void Start () {
        currJumpToken = jumpToken;
    }
	
	// Update is called once per frame
	void Update () {

        float ControlSlide;
        float ControlRolls;

        if (currSlide > 0) ControlSlide = Input.GetAxis("Horizontal") * currSlide; else ControlSlide = 0;
        if (currRolls > 0) ControlRolls = Input.GetAxis("Horizontal") * currRolls; else ControlRolls = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(currJumpToken > 0)
            {
                rb2D.AddForce(Vector2.up * currJumps * 10f);
                currJumpToken -= 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bentuk = Bentuk.Lingkaran;
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            bentuk = Bentuk.Kotak;
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            bentuk = Bentuk.Segitiga;
        }

        currJumps = Jumping;

        switch (bentuk)
        {
            case Bentuk.Lingkaran:
                currRolls = Rolling;
                currSlide = 0f;
                Circling.SetActive(true);
                Squaring.SetActive(false);
                Triangling.SetActive(false);
                break;
            case Bentuk.Kotak:
                currRolls = 0f;
                currSlide = Sliding;
                Circling.SetActive(false);
                Squaring.SetActive(true);
                Triangling.SetActive(false);
                break;
            case Bentuk.Segitiga:
                currRolls = Rolling * 2f;
                currSlide = Sliding;
                Circling.SetActive(false);
                Squaring.SetActive(false);
                Triangling.SetActive(true);
                break;
            default:
                currRolls = Rolling;
                currSlide = Sliding;
                Circling.SetActive(true);
                Squaring.SetActive(false);
                Triangling.SetActive(false);
                break;
        }

        rb2D.AddForce(Vector2.right * ControlSlide);
        rb2D.AddTorque(ControlRolls * -1f);
	}

    //Tony Morelli https://www.youtube.com/watch?v=cl201U1oUYs
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Enter");
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            currJumpToken = jumpToken; //Reset jump token when touch ground! Joel's addition.
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Collision Exit");
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
            //currJumpToken -= 1; //uncomment to make him lose 1 jump token in air.
        }
    }
}
