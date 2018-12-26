using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Derived from FollowPlayerCSharp.cs

public class AttachPlayerCSharp : MonoBehaviour
{
    public Transform target; //This will be your citizen
    public Camera otherCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        if (target)
            transform.position = new Vector3(target.position.x, target.position.y, target.position.z-10);

        GetComponent<Camera>().orthographicSize = otherCam.gameObject.GetComponent<Camera>().orthographicSize;
        GetComponent<Camera>().backgroundColor = otherCam.gameObject.GetComponent<Camera>().backgroundColor;
    }
}
