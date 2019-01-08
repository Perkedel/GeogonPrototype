using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCore : MonoBehaviour
{
    public bool isTouchingSensor = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RFID"))
        {
            isTouchingSensor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTouchingSensor = false;
    }
}
