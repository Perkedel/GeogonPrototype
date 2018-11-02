using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBack_Joystick : MonoBehaviour {
    public Joystick refferenceTouch;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float X = Mathf.Clamp((Input.GetAxis("Horizontal") + refferenceTouch.Horizontal) * 120, -120, 120);
        float Y = Mathf.Clamp((Input.GetAxis("Vertical") + refferenceTouch.Vertical) * 120, -120, 120);
        GetComponent<RectTransform>().anchoredPosition = new Vector3(X, Y);
	}
}
