using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//evenly spaced square instead of sphere


public class AnalyticCharge : MonoBehaviour
{


    static int numArrows = 500;  //number of arrows to be made
    public GameObject newObject;
    Vector3 scaleSet = new Vector3(20, 30, 50);  //scale of arrow
    ArrayList arrowCollection = new ArrayList();
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
    void Start()
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

                    //------------------------------needs to be refactored so we can retain code cohesion--------  //ie this is where we call whatever field funtion we are using(eg get capacitor)
                    //currently just using point charge

                    eField = Capacitor(fieldPointPos, originPoint, pointSourceCharge);

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

    Vector3 LineCharge(Vector3 fieldPointPos, Vector3 pointSourcePos, float charge, float length = 0.9f, int N = 13)
    {
        // E field from line of charge

        // fieldPointpos == measurment point
        // pointSourcepos == start of line of charge
        // charge == total charge of line
        // length == length of line
        // direction == direction of line from pointsourcepos
        // N == Number of points on line
        
        Vector3 totalEfield = new Vector3(0, 0, 0);
        Vector3 direction = new Vector3(1, 0, 0);

        direction = direction.normalized;
        // divide charge and length by number of points
        float dq = charge / (float)N;
        float dL = length / (float)(N - 1);
        // Calculate vector to next point
        Vector3 L_step = dL * direction;

        // step through line points and treat each as a point charge
        for (int i = 0; i < N; i++)
        {
            Vector3 linePoint = pointSourcePos + (i * L_step);

            Vector3 eField = SinglePointCharge(fieldPointPos, linePoint, dq);
            // sum up efield from each point
            totalEfield = totalEfield + eField;

        }
        return totalEfield;
    }


    Vector3 SinglePointCharge(Vector3 fieldPointPos, Vector3 pointSourcePos, float pointSourceCharge)
    {
        float coloumbConstant = 9 * 10 ^ 9;
        Vector3 R = fieldPointPos - pointSourcePos;
        Vector3 eField = (coloumbConstant * pointSourceCharge) / (Mathf.Pow(Vector3.Magnitude(R), 2)) * Vector3.Normalize(R);

        return eField;
    }


    Vector3 plateCharge(Vector3 fieldPointPos, Vector3 plateSourcePos, float charge, float length = 0.6f, int N = 100)
    {
        // E field from square plate of charge in xy plane
        // Probably wouldn't be too difficult to adjust for arbitrary orientation
        // Function models a plate as an NxN array of point charges. Calculates as N line charges of length N with equal spacing

        // fieldPointpos == measurment point
        // pointSourcepos == Centre of plate
        // charge == total charge of plate
        // length == length of one side of plate
        // N == Square root of number of points on plate
        Vector3 totalEfield = new Vector3(0, 0, 0);

        // Calculate the total charge of each line
        float dq_line = charge / (float)N;
        // calculate the distance between points
        float dL = length / (float)(N - 1);
        // Lines are always pointing in positive X
        Vector3 direction = new Vector3(1, 0, 0);
        // find the coordinates of bottom left corner of plate
        Vector3 bottomLeftPlate = plateSourcePos - new Vector3(length / 2, length / 2, 0);
        // Step through each line charge
        for (int i = 0; i < N; i++)
        {
            // Start of first line is bottomLeftPlate. Subsequent lines start dL away in positive y direction
            Vector3 lineStart = bottomLeftPlate + i * dL * new Vector3(0, 1, 0);
            // Calculate E field from each line charge and sum
            Vector3 eField = LineCharge(fieldPointPos, lineStart, dq_line, length, N);
            totalEfield = totalEfield + eField;


        }

        return totalEfield;

    }

    Vector3 plateCharge_analytical(Vector3 fieldPointPos, Vector3 plateSourcePos, float charge, float length = 0.6f, int N = 100)
    {
        float top = length * length;
        Vector3 rad = fieldPointPos - plateSourcePos;
        float r = rad.magnitude;
        float a = length;
        float b = length;

        float sigma = charge / (length * length);

        float bottom = 4 * r * (float)Math.Sqrt((a / 2) * (a / 2) + (b / 2) * (b / 2) + r * r);

        float arc = sigma * (float)Math.Atan2(top, bottom);

        return sigma * rad.normalized;
    }


    Vector3 Capacitor(Vector3 fieldPointPos, Vector3 plateSourcePos, float charge, float seperationZ = 0.30f)
    {
        plateSourcePos = plateSourcePos + new Vector3(0, 0, 0);
        Vector3 secondPlate = plateSourcePos + new Vector3(0, 0, seperationZ);

        Vector3 totalEfield = plateCharge_analytical(fieldPointPos, plateSourcePos, -charge);
        totalEfield = totalEfield + plateCharge_analytical(fieldPointPos, secondPlate, -charge);

        return totalEfield;

    }
}
