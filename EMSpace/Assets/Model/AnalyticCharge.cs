using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//evenly spaced square instead of sphere


public class AnalyticCharge : MonoBehaviour
{

    [System.Serializable]
    public class someMaterial
    {
        public Material material;  //uploading arrow materials
    }
    public someMaterial arrowMat;
    static int numArrows = 500;  //number of arrows to be made
    public GameObject newObject;
    Material[] myMaterial = new Material[numArrows];
    Vector3 scaleSet = new Vector3(20, 30, 50);  //scale of arrow
    ArrayList arrowCollection = new ArrayList();

   
    public Vector3 pointSourcePos;
    public float pointSourceCharge; //can change public things in inspector
    Vector3 fieldPointPos;
    Vector3 eField;
    
    float ySpacing = 0.15f;  //field block spacing
    float zSpacing = 0.15f;
    float xPlaneSpacing = 0.15f;

    int yNumRows = 8;
    int zNumArrows = 8;




    
    // Start is called before the first frame update
    void Start()
    {
        int xPlaneCount = 0;
        int arrowCount = 0;
        while (arrowCount < numArrows) //inside this while loop we generate a bunch of planes until we reach num arrows
        {
            if (xPlaneCount % 2 == 0)
            {
                fieldPointPos.x = pointSourcePos.x + (xPlaneCount / 2) * xPlaneSpacing;  //this mod thing makes it so arrows generate symmetrically around a point
            }                                                                             
            else 
            {
                fieldPointPos.x = pointSourcePos.x - (xPlaneCount / 2) * xPlaneSpacing;
            }



            for (int i =0; i<yNumRows  && arrowCount<numArrows; i++) {

                if(i%2 == 0)
                {
                    fieldPointPos.y = pointSourcePos.y + (i/2) * ySpacing;
                }
                else
                {
                    fieldPointPos.y = pointSourcePos.y - (i/2) * ySpacing;
                }


                for(int j = 0; j<zNumArrows && arrowCount<numArrows; j++)
                {

                    if (j % 2 == 0)
                    {
                        fieldPointPos.z = pointSourcePos.z + (j / 2) * zSpacing;
                    }
                    else
                    {
                        fieldPointPos.z = pointSourcePos.z - (j / 2) * zSpacing;
                    }

                    //------------------------------needs to be refactored so we can retain code cohesion--------  //this is where we add in the field calculation
                    float coloumbConstant = 9*10^9;
                    Vector3 R = fieldPointPos - pointSourcePos;
                    eField = (coloumbConstant * pointSourceCharge) / (Mathf.Pow(Vector3.Magnitude(R), 2)) * Vector3.Normalize(R);

                    Vector3 eHat = Vector3.Normalize(eField);
                    print(eHat.x);
                    float eMag = Vector3.Magnitude(eField);


                    //-------------------------------------------------------------------------------
                    Vector3 xHat = new Vector3(1, 0, 0); //points the arrow in the zHat direction
                    Vector3 yHat = new Vector3(0, 1, 0);
                    Vector3 zHat = new Vector3(0, 0, 1);

                    GameObject obj = Instantiate(newObject);
                    obj.transform.position = new Vector3(fieldPointPos.x, fieldPointPos.y, fieldPointPos.z);
                    obj.transform.localScale = scaleSet;
                    //obj.transform.Rotate((180 / Mathf.PI) * Mathf.Acos(Vector3.Dot(eHat,xHat)), (180 / Mathf.PI) * Mathf.Acos(Vector3.Dot(eHat, yHat)), (180 / Mathf.PI) * Mathf.Acos(Vector3.Dot(eHat, zHat)), Space.World);
          
                    obj.transform.Rotate(0, ((180/Mathf.PI) * Mathf.Atan2(-eHat.z,eHat.x))+180,(-180/Mathf.PI) * Mathf.Asin(eHat.y));
                    arrowCollection.Add(obj);
                    myMaterial[arrowCount] = arrowMat.material;
                    myMaterial[arrowCount].color = Color.red;

                    arrowCount++;


                }

            }

            xPlaneCount++;

        }

    }
    // Update is called once per frame
    void Update()
    {

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


    }
