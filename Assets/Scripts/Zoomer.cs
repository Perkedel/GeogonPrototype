using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoomer : MonoBehaviour {

    float camX, camY;
    public float Speed = 1f;
    [RangeAttribute(1, 100)] public float Zoom = 10f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //GetComponent<Camera>().orthographicSize = 9;
        //https://answers.unity.com/questions/20228/mouse-wheel-zoom.html AnaRhisT
        if (Input.GetAxis("Mouse ScrollWheel") > 0) //forward
        {
            //Camera.main.orthographicSize--;
            Zoom--;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) //backward
        {
            //Camera.main.orthographicSize++;
            Zoom++;
        }
        if (Zoom < 1f) Zoom = 1f;
        Camera.main.orthographicSize = Zoom;

        //https://gamedev.stackexchange.com/questions/104693/how-to-use-input-getaxismouse-x-y-to-rotate-the-camera Fuzzy Logic
        camX -= Speed * Input.GetAxis("Mouse X");
        camY -= Speed * Input.GetAxis("Mouse Y");

        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse2))
        {
            //transform.position = new Vector3(camX, camY, -10);
            //https://docs.unity3d.com/ScriptReference/Transform.Translate.html Unity Translate
            transform.Translate(new Vector3(Input.GetAxis("Mouse X") * -1 *(Zoom/10f), Input.GetAxis("Mouse Y") * -1 *(Zoom/10f)), Space.World); //helpful!
        }

        //https://answers.unity.com/questions/10006/mouse-camera-camera-mouse.html Brian Kehrer
        //transform.position = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), -10f);
    }
}
