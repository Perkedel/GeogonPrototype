using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//sparkzbarca https://answers.unity.com/questions/331545/double-click-mouse-detection-.html

public class DoubleClick : MonoBehaviour
{
    //CUstom
    public FollowPlayerCSharp targetCameraScript;

    //Internal
    [SerializeField] bool one_click = false;
    [SerializeField] bool timer_running;
    [SerializeField] float timer_for_double_click;

    //how long delay should be
    public float delay = 1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (targetCameraScript.TwoClick) //Input.GetMouse(0);
        {
            if (!one_click) // first click no previous clicks
            {
                one_click = true;

                timer_for_double_click = Time.time; // save the current time
                                                    // do one click things;
            }
            else
            {
                one_click = false; // found a double click, now reset

                //do double click things
                //Debug.Log("Clickk");
                targetCameraScript.ResetCamera();
            }
        }
        if (one_click)
        {
            // if the time now is delay seconds more than when the first click started. 
            if ((Time.time - timer_for_double_click) > delay)
            {

                //basically if thats true its been too long and we want to reset so the next click is simply a single click and not a double click.

                one_click = false;

            }



        }
    }
}
