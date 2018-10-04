using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

    //is this Brackeys as well?

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}
