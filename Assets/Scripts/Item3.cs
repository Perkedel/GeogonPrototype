using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item3 : MonoBehaviour {

    public SHanpe player;
    public bool stay = true;
    public float damager = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    //check Item.cs script!
    private float stayCount = 0.0f;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (stay)
        {
            if (stayCount > 0.25f)
            {
                Debug.Log("ouch!");
                stayCount = stayCount - 0.25f;
                player.HealthPoint -= damager;
            }
            else
            {
                stayCount = stayCount + Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
