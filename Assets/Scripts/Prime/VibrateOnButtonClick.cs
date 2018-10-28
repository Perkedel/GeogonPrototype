using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibrateOnButtonClick : MonoBehaviour {

    //Patterns
    long[] DoubleClick = { 10, 10 };
    long[] LongDouble = { 500, 500 };

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void VibrateIt()
    {
        Vibration.Vibrate(50);
    }

    public void VibrateIt(long milliseconds)
    {
        Vibration.Vibrate(milliseconds);
    }

    public void VibrateIt(long[] patterns)
    {
        Vibration.Vibrate(patterns, 0);
    }

    public void VibrateIt(long[] patterns, int repeat)
    {
        Vibration.Vibrate(patterns, repeat);
    }

    public void VibrateStyle(int index)
    {
        long[] selecting;
        switch (index)
        {
            default:
                selecting = new long[] { 50 , 50, 50};
                break;
            case 0:
                selecting = DoubleClick;
                break;
            case 1:
                selecting = LongDouble;
                break;
        }
        VibrateIt(selecting, 1);
    }

    public void VibrateStyle(int index, int repeat)
    {
        long[] selecting;
        switch (index)
        {
            default:
                selecting = new long[] { 50, 50, 50};
                break;
            case 0:
                selecting = DoubleClick;
                break;
            case 1:
                selecting = LongDouble;
                break;
        }
        VibrateIt(selecting, repeat);
    }
}
