﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ItemEffects : MonoBehaviour {

    Collision2D whosTouching;
    GameObject genericToucher;
    public GameObject theGameObject;
    public SHanpe theSHanpeWhoIsTouching;
    public LevelLoader levelManager;
    public ECGcable theHUD;

    //Parametering
    public float setHPvalue = 100;
    public float addHPvalue = 10;
    public float damageMeValue = 20; public float lavaDamageValue = 10;
    public float addArmorValue = 50;

    //Template Methods
    public bool doSetHealth;
    public void setHealth(float value)
    {
        theSHanpeWhoIsTouching.setHealth(value);
    }

    public bool doAddHealth;
    public void addHealth(float value)
    {
        theSHanpeWhoIsTouching.addHealth(value);
    }

    public bool doDamageMe;
    public bool doLavaDamage;
    public void damageMe(float value) //initial single time damage
    {
        theSHanpeWhoIsTouching.damageMe(value);
    }
    public void lavaDamage(float value) //continuous damage
    {
        theSHanpeWhoIsTouching.damageMe(value);
    }

    public bool singleUse = true;
    public void destroySelf()
    {
        Destroy(gameObject);
    }

    public bool doSayDebug = false;
    public string whatDoesDebugSay;
    public void sayDebug()
    {
        Debug.Log(whatDoesDebugSay);
    }
    public bool doSayDebugDynamic = false;
    public void sayDebugDynamic(string something)
    {
        Debug.Log(something);
    }

    public bool doVibrate = true;
    public void Vibrates()
    {
        Vibration.Vibrate(50);
    }
    public bool doVibrateCertainTime = false;
    public long vibrateForHowLong = 10; //milliseconds
    public void VibrateCertainMillisecond()
    {
        Vibration.Vibrate(vibrateForHowLong);
    }
    public void Vibrates(long value) //Milliseconds
    {
        Vibration.Vibrate(value);
    }
    public void Vibrates(long[] pattern, int repeatFrom)
    {
        Vibration.Vibrate(pattern, repeatFrom);
    }

    public bool doLevelComplete = false;
    public void LevelComplete()
    {
        levelManager.CompleteTheLevel();
    }

    public bool doLevelFailed = false;
    public void LevelFailed() //or game over
    {

    }

    public bool doChangeShape = false;
    public void ChangeShape()
    {

    }

    public bool doSetShapeLock = false; //disable/enable change shape by hand
    public void SetShapeLock()
    {

    }

    public bool doSetSize = false;
    public void SetSize() //scaling local
    {

    }

    public bool doSetSpeed = false;
    public void SetSpeed()
    {

    }

    public bool doSetGravity = false;
    public float gravityNewValue = 1f;
    public void SetGravity()
    {

    }

    //toDo list
    /*
     * Set camera zoom
     * Set camera rotation
     * Set camera effect
     * particle
     * medical
     * cynical
     * ah
     * im tierd
     * sleep
     * set position
     * set velocity (picking up this item will make the player run that fast)
     * disable movement
     * etc
    */

    //Base
	// Use this for initialization
	void Start () {
        //GetComponent<Rigidbody2D>().isKinematic = true;
        //Search for object with HUD tag
        if (!theHUD)
        {
            var go = GameObject.FindWithTag("HUD");
            // Check we found an object with the player tag
            if (go)
            // Set the target to the object we found
            {
                if (go.CompareTag("HUD"))
                    theHUD = go.gameObject.GetComponent<ECGcable>();
            }
        }
        if (!levelManager)
        {
            var go = GameObject.FindWithTag("LevelLoader");
            // Check we found an object with the player tag
            if (go)
            // Set the target to the object we found
            {
                if (go.CompareTag("LevelLoader"))
                    levelManager = go.gameObject.GetComponent<LevelLoader>();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    //OnCollision Detection
    //Make sure IsTrigger is ON!
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    //whosTouching = collision;
    //    //genericToucher = collision.gameObject;
    //    //theSHanpeWhoIsTouching = collision.gameObject.GetComponent<SHanpe>();
    //    //if(theSHanpeWhoIsTouching != null)
    //    //{
    //    //    if (doAddHealth)
    //    //    {
    //    //        addHealth(addHPvalue);
    //    //    }
    //    //    if (doSetHealth)
    //    //    {
    //    //        setHealth(setHPvalue);
    //    //    }
    //    //    if (singleUse)
    //    //    {
    //    //        destroySelf();
    //    //    }
    //    //}
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    whosTouching = collision;
    //    genericToucher = collision.gameObject;
    //    theSHanpeWhoIsTouching = collision.gameObject.GetComponent<SHanpe>();
    //    if (theSHanpeWhoIsTouching != null)
    //    {

    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!theSHanpeWhoIsTouching)
        //{
        //    //// Search for object with Player tag
        //    //var go = GameObject.FindWithTag("Player");
        //    //// Check we found an object with the player tag
        //    //if (go)
        //    //// Set the target to the object we found
        //    //{
        //    //    theGameObject = go;
        //    //    if(theGameObject.CompareTag("Player"))
        //    //    theSHanpeWhoIsTouching = go.gameObject.GetComponent<SHanpe>();
        //    //}
        //    theGameObject = collision.gameObject;
        //    theSHanpeWhoIsTouching = collision.gameObject.GetComponent<SHanpe>();
        //}

        //Get Parent gameobject! https://answers.unity.com/questions/12301/how-can-i-get-a-parent-gameobject-of-gameobject-us.html
        //Because children contact collide with item!!!
        theGameObject = collision.gameObject.transform.parent.gameObject;
        theSHanpeWhoIsTouching = theGameObject.GetComponent<SHanpe>();
        if (theSHanpeWhoIsTouching)
        {
            if (doVibrate)
            {
                Vibration.Vibrate(50); //android O has trouble with just calling vibrate. deprecated probably
            }
            if (doAddHealth)
            {
                addHealth(addHPvalue);
            }
            if (doSetHealth)
            {
                setHealth(setHPvalue);
            }
            if (doDamageMe)
            {
                damageMe(damageMeValue);
                Debug.Log("OUCH");
            }
            if (singleUse)
            {
                //anItem.destroySelf();
                Destroy(gameObject);
            }
            if (doSayDebug)
            {
                sayDebug();
            }
            if (doVibrate)
            {
                Vibrates();
            }
            if (doLevelComplete)
            {
                LevelComplete();
            }
            if (doSetGravity)
            {
                theGameObject.GetComponent<Rigidbody2D>().gravityScale = gravityNewValue;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (theGameObject.tag == "Player")
        {
            if (doLavaDamage)
            {
                damageMe(lavaDamageValue);
                Debug.Log("hot");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}