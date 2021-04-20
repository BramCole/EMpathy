using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SettingsData
{
    private static string _viewType; //consider changing to int

    public static string viewType
    {
        get
        {
            return _viewType;   //we can add aditional logic to prevent improper values
        }
        set
        {
            Debug.Log(value);
            _viewType = value;
        }
    }


}