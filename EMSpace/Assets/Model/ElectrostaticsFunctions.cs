using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElectrostaticsFunctions
{
    //if needed later on overload functions. one for dispatching and one for customization

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
        float coloumbConstant = 9 * Mathf.Pow(10,9);
        Vector3 R = fieldPointPos - pointSourcePos;
        Vector3 eField = (coloumbConstant * pointSourceCharge) / (Mathf.Pow(Vector3.Magnitude(R), 2)) * R.normalized;

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




    /// <summary>
    ///   <para>Returns the Magnetic Field vector at the specified point of measurement (rm).</para>
    /// </summary>
    /// <param name="L">the length of the current carrying wire</param>
    /// <param name="I">the current on the wire </param>
    /// <param name="N">the number of points in the numerical integral</param>
    /// <param name="rm"> the point where the magnetic field measurement is conducted</param>
    public static Vector3 LineOfCurrent(Vector3 rm, Vector3 rs, float I)
    {
        // M field from line of current

        // L   == length of the current carrying wire
        // I   == total current of the wire
        // N   == number of sample points for the integration
        // rm  == the point at which we measure the magnetic field
        int N = 100;
        float L = 0.9f;
        Vector3 dL = new Vector3(1, 0, 0) * (L / (float)N);     // Differential length of the wire
        Vector3 totalMfield = new Vector3(0, 0, 0); // Initialize the magnetic field to the contribution of the first wire segment

        // step through line points and treat each as a point of current
        for (int i = 0; i < N; i++)
        {

            // Move to the next point on the wire
            rs = rs + dL;

            // Calculate the contribution from the differential lenght of wire
            Vector3 mField = SinglePointOfCurrent1(rm, rs, I, dL);
            // Sum the contributions from each segment of wire
            totalMfield = totalMfield + mField;

        }
        return totalMfield;
    }

    public static Vector3 SinglePointOfCurrent(Vector3 fieldPointPos, Vector3 pointSourcePos, float I) // pass in current object later for know dl will be a const vector
    {
        // Define some useful constants
        float mu_0 = 1.2566370614359173f;
        mu_0 = mu_0 * Mathf.Pow(10, -6);
        float pi = 3.141592653589793f;
        Vector3 dL = new Vector3(0.1f, 0, 0);
        Vector3 R = fieldPointPos - pointSourcePos;
        Vector3 mField = (mu_0 * I / (4 * pi)) * (Vector3.Cross(dL, R.normalized) / (Mathf.Pow(Vector3.Magnitude(R), 2))); //bio savart law

        return mField;
    }

    public static Vector3 SinglePointOfCurrent1(Vector3 fieldPointPos, Vector3 pointSourcePos, float I, Vector3 dL) // pass in current object later for know dl will be a const vector
    {
        // Define some useful constants
        float mu_0 = 1.2566370614359173f;
        mu_0 = mu_0 * Mathf.Pow(10, -6);
        float pi = 3.141592653589793f;
        Vector3 R = fieldPointPos - pointSourcePos;
        Vector3 mField = (mu_0 * I / (4 * pi)) * (Vector3.Cross(dL, R.normalized) / (Mathf.Pow(Vector3.Magnitude(R), 2))); //bio savart law

        return mField;
    }

    public static Vector3 Solenoid(Vector3 rm, Vector3 pointSourcePos, float I)
    {
        // M field from a solenoid

        // l       ==  length of the solenoid in the axial direction
        // I       ==  total current of the wire (might be variable in the future) (Positive current means I points up at the origin end of the solenoid)
        // N_turns ==  number of turns in the solenoid
        // R       ==  radius of the solenoid turns
        // N       ==  Number of sample points for the integrals
        // rm      ==  the point at which we measure the magnetic field

        // Initialize the parameters of the solenoid

        float l = 0.9f;
        float N_turns = 100;
        float R = 0.1f;
        int N = 100;

        float pi = 3.141592653589793f;
        float pitch = l / (float)N_turns;
        float L = (float)N_turns * Mathf.Pow((Mathf.Pow((2 * pi * R), 2) + Mathf.Pow(pitch, 2)), 0.5f);
        float w = 2 * pi * (float)N_turns / L; // Angular frequency of the solenoid in the x-y plane

        // Determine the current's direction
        float dirI = 0;
        if (I < 0) { dirI = -1; }
        else { dirI = 1; }




        // Calculate the total length of the 

        float z = 0;                         // Axial parameter
        Vector3 totalMfield = new Vector3(0, 0, 0);
        for (int i = 0; i < N; i++)
        {
            Vector3 rs = new Vector3(-R * Mathf.Cos(w * z) + pointSourcePos.x, -R * Mathf.Sin(w * z) + pointSourcePos.y, z + pointSourcePos.z);
            Vector3 dL = new Vector3(-Mathf.Sin(w * z) + pointSourcePos.x, Mathf.Cos(w * z) + pointSourcePos.y, 0 + pointSourcePos.z) * (L / (float)N) * dirI;
            // Move to the next segment on the solenoid
            z = z + (l / (float)N);

            // Initialize the magnetic field to the contribution of the first wire segment
            totalMfield = totalMfield + SinglePointOfCurrent1(rm, rs, I, dL);

        }

        return totalMfield;

    }

}