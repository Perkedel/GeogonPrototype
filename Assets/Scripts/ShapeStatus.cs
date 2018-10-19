using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

//https://docs.unity3d.com/ScriptReference/UI.ScrollRect.OnDrag.html
//https://docs.unity3d.com/ScriptReference/UI.ScrollRect.OnBeginDrag.html

public class ShapeStatus : MonoBehaviour, IDragHandler, IDropHandler, IBeginDragHandler{
    public bool beingDragged = false;

    public void OnMouseDown()
    {
        beingDragged = true; //flue, false and true combined, hatchih!
        Debug.Log("MouseDowned");
    }

    public void OnMouseDrag()
    {
        beingDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        beingDragged = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        beingDragged = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        //Debug.Log("BeginDraged");
        beingDragged = true;
    }
}
