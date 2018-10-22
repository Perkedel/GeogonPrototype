using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Test shanpe heal

public class Item4 : MonoBehaviour
{

    public SHanpe player;
    public bool stay = true;
    public float healer = 1;

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
                if (player.HealthPoint < 100)
                {
                    Debug.Log("ahhh");
                    player.HealthPoint += healer;
                }
                else Debug.Log("Health is full!");
                stayCount = stayCount - 0.25f;
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
