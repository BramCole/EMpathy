using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameObject chargeGenObject;
    public string functionChoice;

    

    void Start()
    { }

    

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
    }


    /// <summary>
    ///   <para>Destroys all arrows on the screen. Calls arrowGeneration function in generateArrows</para>
    /// </summary>
    /// <param name="eventData">an object that contains all relevant data corresponding to the event</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        //when it comes to using delegates we want a hashmap of delegates(some key which goes through the hash function and retrives the delegate
        //this solution scales well and wont bog up the code with if statments(use a dictonary as it is O(1)


        
        //this is where we call out function  use eventData.position to call position
        //Debug.Log("OnEndDrag");
        Vector3 dropPosition = Camera.main.ScreenToWorldPoint(eventData.position);  //we need the actual world position not local coords
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
        Debug.Log(SettingsData.viewType);
        if (SettingsData.viewType == "flow")
        {
            arrowScript.FlowGenFunc(functionChoice);
        }
        else if (SettingsData.viewType == "field")
        {
            arrowScript.arrowGenFunc(functionChoice);
        }
        else
        {
            arrowScript.arrowGenFunc(functionChoice);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }


}
