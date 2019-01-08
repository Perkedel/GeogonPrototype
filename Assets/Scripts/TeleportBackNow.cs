using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TeleportBackNow : MonoBehaviour
{
    public Transform EndTeleportLocator;

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

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
        if (collision.CompareTag("Cloud"))
        {
            collision.GetComponent<Transform>().position = new Vector3(EndTeleportLocator.transform.position.x, collision.transform.position.y);
        }
    }
}
