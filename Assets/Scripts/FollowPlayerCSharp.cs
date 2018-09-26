using UnityEngine;
using System.Collections;

//by Digital-Phantom
//https://answers.unity.com/questions/930656/how-to-prevent-camera-rotation.html
//Modification needed for 2D, so in this case, change Distance to 10.

public class FollowPlayerCSharp : MonoBehaviour
{
    public Transform target; //This will be your citizen
    public float distance;


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
            transform.position = new Vector3(target.position.x, target.position.y, target.position.z - distance);
            //transform.position = new Vector3(target.position.x, target.position.y + 25, target.position.z - distance); // original. in 2D, Y must face right at it. So remove +25 on the Y vector.
    }
}