using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearFieldCommunication : MonoBehaviour
{

    [SerializeField] private bool RFIDIsTouching = false;
    public bool mustStayOnIt = false;
    public GameObject[] toEnableThese;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (RFIDIsTouching)
        {
            for(int i = 0; i < toEnableThese.Length; i++)
            {
                toEnableThese[i].SetActive(true);
            }
        } else
        {
            for (int i = 0; i < toEnableThese.Length; i++)
            {
                toEnableThese[i].SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Trigger")) RFIDIsTouching = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(mustStayOnIt) RFIDIsTouching = false;
    }
}
