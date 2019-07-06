using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//by Digital-Phantom
//https://answers.unity.com/questions/930656/how-to-prevent-camera-rotation.html
//Modification needed for 2D, so in this case, change Distance to 10.

public class FollowPlayerCSharp : MonoBehaviour
{
    //Explicit Blocking Refferences
    public Joystick[] BlockJoystick;
    public Scrollbar[] BlockShapeStatus;
    public ShapeStatus[] BlockShapeStatusScript;
    public Button[] BlockButton;

    //Expose Function Variables
    public bool isMovingCamera;
    public bool TwoClick;

    //Camera Settings
    private float relativeX, relativeY;
    [Range(10, 1000000)] public float constrainXleft = -100, constrainYdown = -100, constrainXright = 100, constrainYup = 100;

    [Range(1, 1000000)] public float Zoom; //Modification has just happened! (JOELwindows7)
    public float initialZoom;
    [Range(1, 1000000)] public float constrainZoomMax = 100f, constrainZoomMin = 1f;

    //Conditions
    public Transform target; //This will be your citizen
    public float distance;

    //Initialisation
    public float startUpZoom = 10f;

    [SerializeField] private bool JoystickIsTouched = false;
    public void SetJoystickIsTouched(bool value)
    {
        JoystickIsTouched = value;
    }

    //Template Methods
    public void ResetCamera()
    {
        relativeX = 0; relativeY = 0; Zoom = initialZoom;
    }

    //https://answers.unity.com/questions/386592/using-delta-to-find-mouse-movement.html delta mouse position
    //https://forum.unity.com/threads/input-getmousebutton-0-deltaposition.180701/ delta mouse position
    Vector2 oldMousePosition = Vector2.zero;
    Vector2 curMousePosition = Vector2.zero;
    float[] deltaMousePosition = { 0, 0, 0 };

    public GameObject[] BlockThoseObjects;

    private void Start()
    {
        initialZoom = startUpZoom;
        Zoom = startUpZoom;
        initialZoom = Zoom;
    }

    void FixedUpdate()
    {
        //MouseClick Heurestics

        float [] LineDirectioning = { 0f, 0f, 0f };
        bool DualTouched = false ;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
            //Debug.DrawLine(Vector3.zero, touchPosition, Color.red);
            Debug.DrawLine(Camera.main.transform.position + (Vector3.forward * 10), touchPosition + (Vector3.forward * 10), Color.red);
        }

        if (!target)
        {
            // Search for object with Player tag
            var go = GameObject.FindWithTag("Player");
            // Check we found an object with the player tag
            if (go)
                // Set the target to the object we found
                target = go.transform;
        }
        if(Input.touchCount == 2)
        {
            TwoClick = true;
        } else
        {
            TwoClick = false;
        }
        if (Input.touchCount >= 2 && !JoystickIsTouched)
        {
            DualTouched = true;
            //https://www.youtube.com/watch?v=0G4vcH9N0gc As Told By Waldo

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            //if (!JoystickIsTouched)
            //{
            //    touchZero = Input.GetTouch(0);
            //    touchOne = Input.GetTouch(1);
            //}
            //else if (JoystickIsTouched)
            //{
            //    touchZero = Input.GetTouch(1);
            //    touchOne = Input.GetTouch(2);
            //}

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            //zoom(difference * 0.01f);
            Zoom -= difference * 0.01f;

            Vector3 LinePositional = (touchZero.position - touchOne.position);
            Vector3 PrevLinePositional = (touchZeroPrevPos - touchOnePrevPos);
            Vector3 MouseLine = (touchZero.deltaPosition + touchOne.deltaPosition);
            
            //LineStart = LinePositional;
            LinePositional.z = 0;
            //Vector3 LineDifference = LinePositional - PrevLinePositional;
            //Vector3 LineDirection = Camera.main.ScreenToWorldPoint(LineStart) + Camera.main.ScreenToWorldPoint(LinePositional);
            //Camera.main.transform.position += LineDirection;
            LineDirectioning[0] = MouseLine.x * .01f;
            LineDirectioning[1] = MouseLine.y * .01f;
            Debug.DrawLine(Camera.main.ScreenToWorldPoint(touchOne.position) + (Vector3.forward * 10), Camera.main.ScreenToWorldPoint(touchZero.position) + (Vector3.forward * 10), Color.yellow);
        }
        else DualTouched = false;
        if (DualTouched)
        {
            isMovingCamera = true;
            for(int i = 0; i < BlockButton.Length; i++)
            {
                BlockButton[i].interactable = false;
            }
            for (int i = 0; i < BlockJoystick.Length; i++)
            {
                
            }
            for (int i = 0; i < BlockShapeStatusScript.Length; i++)
            {
                BlockShapeStatusScript[i].beingBlocked = true;
            }
            for (int i = 0; i < BlockShapeStatus.Length; i++)
            {
                BlockShapeStatus[i].interactable = false;
            }
        } else
        {
            isMovingCamera = false;
            for (int i = 0; i < BlockButton.Length; i++)
            {
                BlockButton[i].interactable = true;
            }
            for (int i = 0; i < BlockJoystick.Length; i++)
            {

            }
            for (int i = 0; i < BlockShapeStatusScript.Length; i++)
            {
                BlockShapeStatusScript[i].beingBlocked = false;
            }
            for (int i = 0; i < BlockShapeStatus.Length; i++)
            {
                BlockShapeStatus[i].interactable = true;
            }
        }

        //DeltaMousePostition
        if (oldMousePosition.x == -999.99f && oldMousePosition.y == -999.99f)
        {
            oldMousePosition = Event.current.mousePosition;
        }
        curMousePosition = Input.mousePosition;
        deltaMousePosition[0] = -(oldMousePosition.x - curMousePosition.x) * Time.deltaTime;
        deltaMousePosition[1] = -(oldMousePosition.y - curMousePosition.y) * Time.deltaTime;
        oldMousePosition = curMousePosition;

        //https://gamedev.stackexchange.com/questions/104693/how-to-use-input-getaxismouse-x-y-to-rotate-the-camera Fuzzy Logic
        if (!JoystickIsTouched)
        {
            if ((Input.GetMouseButton(2) || Input.GetMouseButton(1)) && !DualTouched)
            {
                //relativeX -= (Zoom / 10) * Input.GetAxis("Mouse X"); //Deprecated! conflict with Android Touchscreen (treated as Mouse movement)
                //relativeY -= (Zoom / 10) * Input.GetAxis("Mouse Y");
                relativeX -= (Zoom /10f) * deltaMousePosition[0];
                relativeY -= (Zoom /10f) * deltaMousePosition[1];
            }
            else if (DualTouched && !(Input.GetMouseButton(2) || Input.GetMouseButton(1)))
            {
                relativeX -= (Zoom / 10f) * LineDirectioning[0];
                relativeY -= (Zoom / 10f) * LineDirectioning[1];
            }
            else if (DualTouched && (Input.GetMouseButton(2) || Input.GetMouseButton(1)))
            {
                //Debug.Log("<color=red>Woi!! satu satu! jangang berebutan!!!</color>");
                relativeX -= (Zoom / 10f) * LineDirectioning[0];
                relativeY -= (Zoom / 10f) * LineDirectioning[1];
            }
            else
            {

            }
        }
        //Don't use keycode mouse 2 something like that! this code is also treated on Android e.g.
        //nvm. Unity succ! cannot differentiate mouse click and touchscreen on Editor at all!
        if ((Input.GetAxis("MoveCamX") > 0 || Input.GetAxis("MoveCamY") > 0) || (Input.GetAxis("MoveCamX") < 0 || Input.GetAxis("MoveCamY") < 0)) //don't mess with relatives if none right analog stick axes moved!
        {
            relativeX += (Zoom / 10) * Input.GetAxis("MoveCamX");
            relativeY += (Zoom / 10) * Input.GetAxis("MoveCamY");
        }
        if (relativeX <= constrainXleft) relativeX = constrainXleft; if (relativeX >= constrainXright) relativeX = constrainXright;
        if (relativeY <= constrainYdown) relativeY = constrainYdown; if (relativeY >= constrainYup) relativeY = constrainYup;

        if (Input.GetButtonDown("CamReset")) //Reset camera (JOELwindows7)
        {
            ResetCamera();
        }

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
        Zoom += Input.GetAxis("Zoom");
        if (Zoom <= constrainZoomMin) Zoom = constrainZoomMin;
        if (Zoom >= constrainZoomMax) Zoom = constrainZoomMax;
        Camera.main.orthographicSize = Zoom;

        //Here is better and easier function by Told by Waldo https://www.youtube.com/watch?v=0G4vcH9N0gc
        //Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - Zoom, constrainZoomMin, constrainZoomMax);
        //unfortunately, I have made it already. Please don't overhaul it! we are in Unity (Proprietary Subscription only) remember?!
        //Next time.

        if (target)
            transform.position = new Vector3(target.position.x + relativeX, target.position.y + relativeY, target.position.z - distance);
            //transform.position = new Vector3(target.position.x, target.position.y + 25, target.position.z - distance); // original. in 2D, Y must face right at it. So remove +25 on the Y vector.
    }

    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, constrainZoomMin, constrainZoomMax);
    }

    
}