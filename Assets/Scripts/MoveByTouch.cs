using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Brackeys Touch Control

public class MoveByTouch : MonoBehaviour {

    public Joystick joystick;

    Vector3 mover;

    public float MoveX = 0f;
    public float MoveY = 0f;

    // Update is called once per frame
    void Update() {
        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    //touch.phase
        //    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        //    touchPosition.z = 0f;
        //    transform.position = touchPosition;
        //}

        for(int i = 0; i < Input.touchCount; i++)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
            //Debug.DrawLine(Vector3.zero, touchPosition, Color.red);
        }

        //Smooth move
        //MoveX = joystick.Horizontal * Time.deltaTime;
        //MoveY = joystick.Vertical * Time.deltaTime;

        //Hard move
        if (joystick.Horizontal >= .2f)
        {
            MoveX = 1f;
        }
        else if (joystick.Horizontal <= -.2f)
        {
            MoveX = -1f;
        }
        else MoveX = 0f;
        if (joystick.Vertical >= .2f)
        {
            MoveY = 1f * Time.deltaTime;
        }
        else if (joystick.Vertical <= -.2f)
        {
            MoveY = -1f * Time.deltaTime;
        }
        else MoveY = 0f;

        transform.Translate(MoveX, MoveY, 0, Space.World);

        
	}
}
