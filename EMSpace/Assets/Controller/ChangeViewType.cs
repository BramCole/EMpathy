using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeViewType : MonoBehaviour
{

    public void setType(string type)
    {
        

        GameObject fieldToggle = GameObject.Find("Field");
        GameObject flowToggle = GameObject.Find("Flow");
        Toggle field = fieldToggle.GetComponent<Toggle>();
        Toggle flow = flowToggle.GetComponent<Toggle>();

        
        if (type == "field" && flow.isOn == true && field.isOn == true)
        {
            flow.isOn = false;
            SettingsData.viewType = type;
        }
        else if (type == "flow" && flow.isOn == true && field.isOn == true)
        {
            field.isOn = false;
            SettingsData.viewType = type;
        }
    }

}
