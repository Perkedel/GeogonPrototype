using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibrateOnButtonClick : MonoBehaviour {

    //Patterns
    long[] DoubleClick = {0, 10, 100, 10 };
    long[] LongDouble = {0, 500, 250, 500 };
    long[] Nanang_Nang = { 0, 100, 50, 100, 250, 250 }; //Who made this monopoly.jar game? it was a sound of window transition!

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
                selecting = new long[] {0, 50 , 50, 50, 50, 50};
                break;
            case 0:
                selecting = DoubleClick;
                break;
            case 1:
                selecting = LongDouble;
                break;
            case 2:
                selecting = Nanang_Nang;
                break;
        }
        VibrateIt(selecting, -1); //-1 is out of bound repeat from. it will then stop
    }
    //https://proandroiddev.com/using-vibrate-in-android-b0e3ef5d5e07 Android vibrate

    public void VibrateStyle(int index, int repeat)
    {
        long[] selecting;
        switch (index)
        {
            default:
                selecting = new long[] {0, 50, 50, 50};
                break;
            case 0:
                selecting = DoubleClick;
                break;
            case 1:
                selecting = LongDouble;
                break;
            case 2:
                selecting = Nanang_Nang;
                break;
        }
        VibrateIt(selecting, repeat); //repeat means repeat from elements in array
    }

    //bonus AdamJason = Trademark_Quality(bit.64);
}
