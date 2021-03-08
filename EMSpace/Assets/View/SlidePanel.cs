using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidePanel : MonoBehaviour
{
    //Process touch for panel display on if the touch is greater than this threshold.
    private float rightEdge = Screen.width * 0.75f;

    //Minimum swipe distance for showing/hiding the panel.
    float swipeDistance = 5f;


    float startXPos;
    bool processTouch = false;
    bool isExpanded = false;
    public Animator panelAnimation;



    void Update()
    {
        if (Input.touches.Length > 0)
            Panel(Input.GetTouch(0));
    }




    void Panel(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                //Get the start position of touch.

                startXPos = touch.position.x;
                Debug.Log(startXPos);
                //Check if we need to process this touch for showing panel.
                if (startXPos > rightEdge)
                {
                    processTouch = true;
                }
                break;
            case TouchPhase.Ended:
                if (processTouch)
                {
                    //Determine how far the finger was swiped.
                    float deltaX = touch.position.x - startXPos;


                    if (isExpanded && deltaX > (swipeDistance))
                    {

                        panelAnimation.SetTrigger("hideSide");
                        isExpanded = false;
                    }
                    else if (!isExpanded && deltaX < (-swipeDistance))
                    {
                        panelAnimation.SetTrigger("showSide");
                        isExpanded = true;
                    }

                    startXPos = 0f;
                    processTouch = false;
                }
                break;
            default:
                return;
        }
    }
}
