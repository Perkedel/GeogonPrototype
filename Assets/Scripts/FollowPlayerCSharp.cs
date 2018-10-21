using UnityEngine;
using System.Collections;

//by Digital-Phantom
//https://answers.unity.com/questions/930656/how-to-prevent-camera-rotation.html
//Modification needed for 2D, so in this case, change Distance to 10.

public class FollowPlayerCSharp : MonoBehaviour
{
    private float relativeX, relativeY;
    [Range(10,1000000)]public float constrainXleft = -100, constrainYdown = -100, constrainXright = 100, constrainYup = 100;

    [Range(1, 1000000)] public float Zoom = 10f; //Modification has just happened! (JOELwindows7)
    private float initialZoom;
    [Range(1, 1000000)] public float constrainZoomMax = 100f, constrainZoomMin = 1f;

    public Transform target; //This will be your citizen
    public float distance;

    private void Start()
    {
        initialZoom = Zoom;
    }

    void Update()
    {
        if (!target)
        {
            // Search for object with Player tag
            var go = GameObject.FindWithTag("Player");
            // Check we found an object with the player tag
            if (go)
                // Set the target to the object we found
                target = go.transform;
        }
        //https://gamedev.stackexchange.com/questions/104693/how-to-use-input-getaxismouse-x-y-to-rotate-the-camera Fuzzy Logic
        if (Input.GetKey(KeyCode.Mouse2) || Input.GetKey(KeyCode.Mouse1))
        {
            relativeX -= (Zoom / 10) * Input.GetAxis("Mouse X");
            relativeY -= (Zoom / 10) * Input.GetAxis("Mouse Y");
        }
        relativeX += Input.GetAxis("MoveCamX"); relativeY += Input.GetAxis("MoveCamY");
        if (relativeX <= constrainXleft) relativeX = constrainXleft; if (relativeX >= constrainXright) relativeX = constrainXright;
        if (relativeY <= constrainYdown) relativeY = constrainYdown; if (relativeY >= constrainYup) relativeY = constrainYup;

        if (Input.GetButtonDown("CamReset")) //Reset camera (JOELwindows7)
        {
            relativeX = 0; relativeY = 0; Zoom = initialZoom;
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

        if (target)
            transform.position = new Vector3(target.position.x + relativeX, target.position.y + relativeY, target.position.z - distance);
            //transform.position = new Vector3(target.position.x, target.position.y + 25, target.position.z - distance); // original. in 2D, Y must face right at it. So remove +25 on the Y vector.
    }
}