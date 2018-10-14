//https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter2D.html
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleItem2 : MonoBehaviour
{
    private float spriteMove;

    void Awake()
    {
        SpriteRenderer sprRend;
        sprRend = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sprRend.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);

        BoxCollider2D bc;
        bc = gameObject.AddComponent<BoxCollider2D>() as BoxCollider2D;
        bc.size = new Vector2(1.3f, 1.3f);
        bc.isTrigger = true;
    }

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("circle");
        gameObject.transform.Translate(-4.0f, 0.0f, 0.0f);
        spriteMove = 0.1f;
    }

    void FixedUpdate()
    {
        gameObject.transform.Translate(spriteMove, 0.0f, 0.0f);

        if (gameObject.transform.position.x < -4.0f)
        {
            // move GameObject2 to the right
            spriteMove = 0.1f;
        }
    }

    // when the GameObjects collider arrange for this GameObject to travel to the left of the screen
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        spriteMove = -0.1f;
    }
}