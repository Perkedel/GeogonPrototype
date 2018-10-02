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

    float currSlide = 10f;
    float currRolls = 10f;

    public Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        float ControlSlide;
        float ControlRolls;

        if (currSlide > 0) ControlSlide = Input.GetAxis("Horizontal") * currSlide; else ControlSlide = 0;
        if (currRolls > 0) ControlRolls = Input.GetAxis("Horizontal") * currRolls; else ControlRolls = 0;

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
}
