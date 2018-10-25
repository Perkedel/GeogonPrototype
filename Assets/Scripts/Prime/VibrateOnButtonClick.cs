using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibrateOnButtonClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void VibrateIt()
    {
#if UNITY_IOS || UNITY_ANDROID || UNITY_WSA
        Handheld.Vibrate();
#endif
    }
}
