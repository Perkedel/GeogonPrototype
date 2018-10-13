using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item2 : MonoBehaviour {

    public SHanpe player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.IsTickled = true;
        //Debug.Log("Tickle");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.IsTickled = false;
        //Debug.Log("left");
    }
}
