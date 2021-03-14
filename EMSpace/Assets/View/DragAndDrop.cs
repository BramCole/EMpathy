using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameObject chargeGenObject;
    public string functionChoice;
                                                                                                                                                                                                
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //this is where we call out function  use eventData.position to call position
        Debug.Log("OnEndDrag");
        Vector3 dropPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        GenerateArrows arrowScript = chargeGenObject.GetComponent<GenerateArrows>();
        //Destroy All Arrows
        GameObject[] arrowsDestroy = GameObject.FindGameObjectsWithTag("clone");
        if (arrowsDestroy.Length > 0)
        {
            foreach (GameObject aClone in arrowsDestroy)
            {
                Destroy(aClone);
            }
        }
        arrowScript.arrowCollection.Clear();

        //Call Generate
        arrowScript.originPoint = dropPosition;
        arrowScript.arrowGenFunc(functionChoice);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }


}
