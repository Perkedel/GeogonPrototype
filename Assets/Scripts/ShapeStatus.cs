using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//https://docs.unity3d.com/ScriptReference/UI.ScrollRect.OnDrag.html
//https://docs.unity3d.com/ScriptReference/UI.ScrollRect.OnBeginDrag.html

public class ShapeStatus : MonoBehaviour{
    public bool beingDragged = false;
    public bool beingBlocked = false;

    public void SetBeingDragged(bool value)
    {
        if (!beingBlocked)
        {
            beingDragged = value;
        } else
        {
            beingDragged = false;
        }
    }

    public void SetBeingBlocked(bool value)
    {
        beingBlocked = value;
    }

    //Use Event Trigger!!!

    public void Update()
    {
        if (beingBlocked)
        {
            gameObject.GetComponent<Scrollbar>().interactable = false;
        } else
        {
            gameObject.GetComponent<Scrollbar>().interactable = true;
        }
    }
}
