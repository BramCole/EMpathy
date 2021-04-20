using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

//evenly spaced square instead of sphere


public class GenerateArrows : MonoBehaviour
{

    
    static int numArrows = 1000;  //number of arrows to be made
    public GameObject newObject;
    Vector3 scaleSet = new Vector3(20, 30, 50);  //scale of arrow
    public ArrayList arrowCollection = new ArrayList();
    float[] eMagArray = new float[numArrows];

    public Vector3 originPoint;
    public float pointSourceCharge; //can change public things in inspector
    Vector3 fieldPointPos;
    Vector3 eField;

    float ySpacing = 0.15f;  //field block spacing
    float zSpacing = 0.15f;
    float xPlaneSpacing = 0.15f;

    int yNumRows = 7;
    int zNumArrows = 7;



    

    // Start is called before the first frame update

    Dictionary<string, MethodInfo> methodMap = new Dictionary<string, MethodInfo>();
    object eFuncObj;
    void Start() {
        //this code block uses dynamic method lookup (using reflection) to create a dictionary of functions to call
        //this way we can bind the function to the drag and drop images in the slider by using a string with the function name //reflection must occur at runtime

        Type myType = (typeof(ElectrostaticsFunctions));
        ConstructorInfo eFuncConstructor = myType.GetConstructor(Type.EmptyTypes);
        eFuncObj = eFuncConstructor.Invoke(new object[] { });

        MethodInfo[] arrayMethods = myType.GetMethods();   

        foreach (MethodInfo method in arrayMethods)
        {
            methodMap.Add(method.Name, method);   //we only need to run the reflection operation once when we start
        }
    }



    /// <summary>
    ///   <para>Generates arrows in a rectangular fashion around a center point. Arrow Generation is symmetric around the point. The colour range will be whatever
    ///   the max value of the point is to zero (the heatmap is RGB). The arrows are also rotated to represent proper direction. </para>
    /// </summary>
    /// <param name="funcChoice">a string representing which function out of the electrostatics functions is to be chosen. This will later be converted to using delegates.</param>
    public void arrowGenFunc(string funcChoice)
    {
        Vector3 xHat = new Vector3(1, 0, 0); //points the arrow in the zHat direction
        Vector3 yHat = new Vector3(0, 1, 0);
        Vector3 zHat = new Vector3(0, 0, 1);
        float coloumbConstant = 9*10^9;

        float maxField = 0;  //all mag no direction so zero is minimum

        int xPlaneCount = 0;
        int arrowCount = 0;



        while (arrowCount < numArrows) //inside this while loop we generate a bunch of planes until we reach num arrows
        {
            if (xPlaneCount % 2 == 0)
            {
                fieldPointPos.x = originPoint.x + (xPlaneCount / 2) * xPlaneSpacing;  //this mod thing makes it so arrows generate symmetrically around a point
            }
            else   //TODO consider turning into function to increase readability
            {
                fieldPointPos.x = originPoint.x - (xPlaneCount / 2) * xPlaneSpacing;
            }



            for (int i = 0; i < yNumRows && arrowCount < numArrows; i++)
            {

                if (i % 2 == 0)
                {
                    fieldPointPos.y = originPoint.y + (i / 2) * ySpacing;
                }
                else
                {
                    fieldPointPos.y = originPoint.y - (i / 2) * ySpacing - ySpacing;  //i starts at zero so we would apply an arrow to the same postion twice if we did not offset(could also alter i)
                }


                for (int j = 0; j < zNumArrows && arrowCount < numArrows; j++)
                {

                    if (j % 2 == 0)
                    {
                        fieldPointPos.z = originPoint.z + (j / 2) * zSpacing;
                    }
                    else
                    {
                        fieldPointPos.z = originPoint.z - (j / 2) * zSpacing - zSpacing;
                    }



                    Vector3 R = fieldPointPos - originPoint;

                    //-------------------------------------  //ie this is where we call whatever field funtion we are using(eg get capacitor)

                    object vectorInfo = methodMap[funcChoice].Invoke(eFuncObj, new object[] { fieldPointPos, originPoint, pointSourceCharge });
                    eField = (Vector3)vectorInfo;

                    //-------------------------------------------------------------------------------
                    Vector3 eHat = Vector3.Normalize(eField);
                    float eMag = Vector3.Magnitude(eField);
                    eMagArray[arrowCount] = eMag;


                    if (eMag > maxField)
                    {
                        maxField = eMag;
                    }


                    GameObject obj = Instantiate(newObject);
                    obj.transform.position = new Vector3(fieldPointPos.x, fieldPointPos.y, fieldPointPos.z);
                    obj.transform.localScale = scaleSet;
                    obj.transform.Rotate(0, ((180 / Mathf.PI) * Mathf.Atan2(-eHat.z, eHat.x)) + 180, (-180 / Mathf.PI) * Mathf.Asin(eHat.y));
                    obj.gameObject.tag = "clone";
                    arrowCollection.Add(obj);

                    arrowCount++;


                }

            }

            xPlaneCount++;

        }

        GameObject[] arrowArray = new GameObject[numArrows];
        arrowArray = (GameObject[])arrowCollection.ToArray(typeof(GameObject));  //get component does not work with arraylists

        for (int i = 0; i < arrowCollection.Count; i++)
        {
           MeshRenderer gameObjectRenderer = arrowArray[i].GetComponentInChildren<MeshRenderer>();   //get renderer and create a new material for each object
           Material newMaterial = new Material(Shader.Find("Standard"));
           newMaterial.color = heatMap(0.0f, maxField, eMagArray[i]);
           gameObjectRenderer.material = newMaterial;
        }

    }





    // Update is called once per frame
    void Update()
    {
        //an example of changing arrows

        //    count++;

        //    foreach (GameObject i in arrowCollection)
        //    {
        //        i.transform.Rotate(0, 2f, 0);
        //    }
        //    foreach (Material i in myMaterial)
        //    {
        //        //colour is rgb to 1 not 255
        //        i.color = new Color(((count + 500) % 1000f) * 0.001f, 0, (count % 1000f) * 0.001f);
        //    }
    }

    Color heatMap(float min,float max,float value)
    {
        float ratio = 2 * (value - min) / (max - min);
        float b = Mathf.Max(0, 1 - ratio);
        float r = Mathf.Max(0, ratio - 1);
        float g = 1 - b - r;
        return new Color(r, g, b);
        
    }



    public void FlowGenFunc(string funcChoice)
    {
        //to generate flow lines we need to choose starting points (we will choose radially from the charge collection origion(for now)
        //we gen arrow equivlent to vector field at point we then move a small amount in that vectors direction
        Vector3 xHat = new Vector3(1, 0, 0); //points the arrow in the zHat direction
        Vector3 yHat = new Vector3(0, 1, 0);
        Vector3 zHat = new Vector3(0, 0, 1);
        float coloumbConstant = 9 * 10 ^ 9;

        float maxField = 0;  //all mag no direction so zero is minimum
        int NumArrows = 0;
        float flowScale = 0.01f;

        Vector3 disp = new Vector3(0.01f, 0.01f, 0.01f);
        fieldPointPos = originPoint+ disp;

        int numFlows = 6;
       List<Vector3> positionArray = new List<Vector3>();

        positionArray.Add(new Vector3(originPoint.x + 0.01f, originPoint.y +0.01f, originPoint.z +0.01f));
        positionArray.Add(new Vector3(originPoint.x + 0.01f, originPoint.y + 0.01f, originPoint.z));
        positionArray.Add(new Vector3(originPoint.x + 0.01f, originPoint.y, originPoint.z + 0.01f));
        positionArray.Add(new Vector3(originPoint.x, originPoint.y + 0.01f, originPoint.z + 0.01f));

        positionArray.Add(new Vector3(originPoint.x - 0.01f, originPoint.y - 0.01f, originPoint.z - 0.01f));
        positionArray.Add(new Vector3(originPoint.x - 0.01f, originPoint.y - 0.01f, originPoint.z));
        positionArray.Add(new Vector3(originPoint.x - 0.01f, originPoint.y, originPoint.z - 0.01f));
        positionArray.Add(new Vector3(originPoint.x , originPoint.y - 0.01f, originPoint.z - 0.01f));
        /*
        for(int i=0; i<numFlows; i++)
        {



            for(int j=0; j<numFlows; j++)
            {
                positionArray.Add(new Vector3(originPoint.x + 0.01f*Mathf.Sin((2*3.1415f)/i) * Mathf.Cos((2 * 3.1415f) / j), originPoint.y + 0.01f * Mathf.Cos((2 * 3.1415f) / i) * Mathf.Sin((2 * 3.1415f) / j), originPoint.z + 0.1f * Mathf.Cos((2 * 3.1415f) / i)));
            }

        }*/

        for (int j = 0; j < positionArray.Count; j++)
        {
            fieldPointPos = positionArray[j];

            for (int i = 0; i < 60; i++)
            {
                object vectorInfo = methodMap[funcChoice].Invoke(eFuncObj, new object[] { fieldPointPos, originPoint, pointSourceCharge });
                eField = (Vector3)vectorInfo;

                Vector3 eHat = Vector3.Normalize(eField);
                float eMag = Vector3.Magnitude(eField);

                if (eMag > maxField)
                {
                    maxField = eMag;
                }


                GameObject obj = Instantiate(newObject);
                obj.transform.position = new Vector3(fieldPointPos.x, fieldPointPos.y, fieldPointPos.z);
                obj.transform.localScale = scaleSet;
                obj.transform.Rotate(0, ((180 / Mathf.PI) * Mathf.Atan2(-eHat.z, eHat.x)) + 180, (-180 / Mathf.PI) * Mathf.Asin(eHat.y));
                obj.gameObject.tag = "clone";
                arrowCollection.Add(obj);



                fieldPointPos = fieldPointPos + flowScale * eHat;

                NumArrows++;

            }
        }
    }



}



