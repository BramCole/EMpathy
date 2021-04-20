using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Update is called once per frame
    public void ChangeToScene(string sceneToChangeTo)
    {
        SceneManager.LoadScene(sceneToChangeTo);
    }

    public void SetViewType(string type)
    {
        SceneManager.LoadScene(type);
        //SettingsData.viewType = type;
    }
}
