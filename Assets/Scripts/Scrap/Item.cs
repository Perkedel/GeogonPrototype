using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sample from Unity page

public class Item : MonoBehaviour {

    public bool enter = true;
    public bool stay = true;
    public bool exit = true;
    public float moveSpeed;

    public GameObject PickerObject;


    private void Awake()
    {
        var PolCollider = gameObject.GetComponent<PolygonCollider2D>();
        PolCollider.isTrigger = true;

        moveSpeed = 0f;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //https://docs.unity3d.com/ScriptReference/Collider.OnTriggerEnter.html
    void OnTriggerEnter2D(Collider2D other)
    {
        if (enter)
        {
            Debug.Log("entered");
        }
    }

    // stayCount allows the OnTriggerStay to be displayed less often
    // than it actually occurs.
    private float stayCount = 0.0f;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (stay)
        {
            if (stayCount > 0.25f)
            {
                Debug.Log("staying");
                stayCount = stayCount - 0.25f;
            }
            else
            {
                stayCount = stayCount + Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (exit)
        {
            Debug.Log("exit");
        }
    }
}
