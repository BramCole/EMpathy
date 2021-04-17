using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct ElectrostaticsFunctions
{
    /// <summary>
	///   <para>Returns the Magnetic Field vector at the specified point of measurement (rm).</para>
	/// </summary>
	/// <param name="L">the length of the current carrying wire</param>
	/// <param name="I">the current on the wire </param>
    /// <param name="N">the number of points in the numerical integral</param>
    /// <param name="rm"> the point where the magnetic field measurement is conducted</param>
    public static Vector3 LineOfCurrent(float L, float I, int N, Vector3 rm)
    {
        // M field from line of current

        // L   == length of the current carrying wire
        // I   == total current of the wire
        // N   == number of sample points for the integration
        // rm  == the point at which we measure the magnetic field

        Vector3 dL = new Vector3(1, 0, 0) * (L/(float)N);     // Differential length of the wire
        Vector3 rs = new Vector3(0, 0, 0);                    // The wire starts at the origin
        Vector3 totalMfield = new Vector3(0, 0, 0); // Initialize the magnetic field to the contribution of the first wire segment

        // step through line points and treat each as a point of current
        for (int i = 0; i < N; i++)
        {
            // Define the displacement between the source point and the measurement point
            Vector3 r = rm - rs;

            // Move to the next point on the wire
            rs = rs + dL;

            // Calculate the contribution from the differential lenght of wire
            Vector3 mField = SinglePointOfCurrent(r, dL, I);
            // Sum the contributions from each segment of wire
            totalMfield = totalMfield + mField;

        }
        return totalMfield;
    }


    public static Vector3 SinglePointOfCurrent(Vector3 r, Vector3 dL, float I)
    {
        // Define some useful constants
        float mu_0 = 1.2566370614359173 * 10 ^ (-6);
        float pi = 3.141592653589793;

        Vector3 mField = mu_0 * I / (4 * pi) * Vector3.Cross(r.normalized,dL) / (Mathf.Pow(Vector3.Magnitude(r), 2));
        
        return mField;
    }


    public static Vector3 Solenoid(float l, float I, int N_turns, float R, int N, float rm)
    {
        // M field from a solenoid

        // l       ==  length of the solenoid in the axial direction
        // I       ==  total current of the wire (might be variable in the future) (Positive current means I points up at the origin end of the solenoid)
        // N_turns ==  number of turns in the solenoid
        // R       ==  radius of the solenoid turns
        // N       ==  Number of sample points for the integrals
        // rm      ==  the point at which we measure the magnetic field

        // Initialize the parameters of the solenoid
        float pi = 3.141592653589793;
        float z = 0;                         // Axial parameter
        float w = 2 * pi * (float)N_turns/L; // Angular frequency of the solenoid in the x-y plane

        // Determine the current's direction
        if (I < 0) { float dirI = -1; }
        else { float dirI = 1; }

        // Initialize the magnetic field at the measurement point
        Vector3 totalMfield = new Vector3(0, 0, 0);

        // Calculate the total length of the 
        float pitch = l / (float)N_turns;
        float L = (float)N_turns * ((2 * pi * r) ^ 2 + pitch ^ 2) ^ (0.5); 

        for (int i = 0; i < N; i++)
        {
            Vector3 rs = new Vector3(-R * Math.Cos(w * z), -R * Math.Sin(w * z), z);
            Vector3 r = rm - rs;
            Vector3 dL = new Vector3(-Math.Sin(w * z), Math.Cos(w * z), 0) * (L / (float)N) * dirI;
            // Move to the next segment on the solenoid
            z = z + (l / (float)N);

            // Initialize the magnetic field to the contribution of the first wire segment
            totalMfield = totalMfield + SinglePointOfCurrent(r, dL, I);

        }

        return totalMfield;

    }

    
}
