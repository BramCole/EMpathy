using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElectrostaticsFunctions
{
    
    /// <summary>
	///   <para>Returns the Electric field vector at the specified fieldPointPosition.</para>
	/// </summary>
	/// <param name="fieldPointPos">fieldPoint position specified in world coordinates</param>
	/// <param name="pointSourcePos">the unit vector that points in the direction of the line </param>
    /// <param name="charge">the charge at each dl segment</param>
    /// <param name="length"> the length of the line specified in world coordinates</param>
    /// <param name="N">number of mesh segments</param>
    public static Vector3 LineCharge(Vector3 fieldPointPos, Vector3 pointSourcePos, float charge)   //we want to pass in some charge collection object instead of random stuff
    {
        // E field from line of charge

        // fieldPointpos == measurment point
        // pointSourcepos == start of line of charge
        // charge == total charge of line
        // length == length of line
        // direction == direction of line from pointsourcepos
        // N == Number of points on line

        float length = 0.9f;
        int N = 13;

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


    public static Vector3 SinglePointCharge(Vector3 fieldPointPos, Vector3 pointSourcePos, float pointSourceCharge)
    {
        float coloumbConstant = 9 * 10 ^ 9;
        Vector3 R = fieldPointPos - pointSourcePos;
        Vector3 eField = (coloumbConstant * pointSourceCharge) / (Mathf.Pow(Vector3.Magnitude(R), 2)) * Vector3.Normalize(R);

        return eField;
    }


    public static Vector3 plateCharge(Vector3 fieldPointPos, Vector3 plateSourcePos, float charge)
    {
        // E field from square plate of charge in xy plane
        // Probably wouldn't be too difficult to adjust for arbitrary orientation
        // Function models a plate as an NxN array of point charges. Calculates as N line charges of length N with equal spacing

        // fieldPointpos == measurment point
        // pointSourcepos == Centre of plate
        // charge == total charge of plate
        // length == length of one side of plate
        // N == Square root of number of points on plate
        float length = 0.6f;
          int N = 100;
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
            Vector3 eField = LineCharge(fieldPointPos, lineStart, dq_line);
            totalEfield = totalEfield + eField;


        }

        return totalEfield;

    }

    public static Vector3 plateCharge_analytical(Vector3 fieldPointPos, Vector3 plateSourcePos, float charge)
    {

        float length = 0.6f;
        int N = 100;
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


    public static Vector3 Capacitor(Vector3 fieldPointPos, Vector3 plateSourcePos, float charge)
    {
        float seperationZ = 0.30f;
        plateSourcePos = plateSourcePos + new Vector3(0, 0, 0);
        Vector3 secondPlate = plateSourcePos + new Vector3(0, 0, seperationZ);

        Vector3 totalEfield = plateCharge_analytical(fieldPointPos, plateSourcePos, -charge);
        totalEfield = totalEfield + plateCharge_analytical(fieldPointPos, secondPlate, -charge);

        return totalEfield;

    }

}