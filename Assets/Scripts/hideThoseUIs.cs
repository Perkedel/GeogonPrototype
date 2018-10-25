using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VibrateOnButtonClick))]
public class hideThoseUIs : MonoBehaviour {

    public GameObject[] hideThoseObjects;
    public Button[] hideThoseButtons;
    public GameObject[] reshowThoseObjects;

    public LevelLoader levelManager;
    public GameObject setStoreSelected;

    public VibrateOnButtonClick letVibrate;

	// Use this for initialization
	void Start () {
        if (!levelManager)
        {
            var whereLevelManager = GameObject.FindWithTag("LevelLoader");
            if (whereLevelManager)
            {
                levelManager = whereLevelManager.GetComponent<LevelLoader>();
            }
        }
        letVibrate = GetComponent<VibrateOnButtonClick>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.activeSelf)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                for(int i= 0; i < hideThoseObjects.Length; i++)
                {
                    hideThoseObjects[i].SetActive(false);
                }
                for(int i= 0; i < reshowThoseObjects.Length; i++)
                {
                    reshowThoseObjects[i].SetActive(true);
                }
                levelManager.SetStoreSelected(setStoreSelected);
                letVibrate.VibrateIt();
            }
        }
		
	}
}
