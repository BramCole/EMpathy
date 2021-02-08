using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMake : MonoBehaviour
{
    [System.Serializable]
    public class someMaterial
    {
       public Material material;
    }
    public someMaterial arrowMat;
    static int length = 100;
    public GameObject newObject;
    Material[] myMaterial = new Material[length];

    Vector3 scaleSet = new Vector3(20, 30, 50);
  
    ArrayList arrowCollection = new ArrayList();
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < length; i++)
        {
            GameObject obj = Instantiate(newObject);
            obj.transform.position = new Vector3(0.0f, (i % 10) * 0.03f, 0.05f * i);
            obj.transform.localScale = scaleSet;
            arrowCollection.Add(obj);
            myMaterial[i] = arrowMat.material;
            myMaterial[i].color = Color.red;
        }

    }
    int count = 0;
    // Update is called once per frame
    void Update()
    {
        
        count++;

        foreach (GameObject i in arrowCollection)
        {
            i.transform.Rotate(0, 2f, 0);
        }
        foreach (Material i in myMaterial)
        {
     
            i.color = new Color( ((count+500)%1000f)*0.001f, 0, (count % 1000f) * 0.001f);
        }
    }




}