using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldObject : MonoBehaviour
{
    public Dictionary<Vector3, Charge> chargeList; //the dictionary will enforce a uniquness of positions //operator overloading will be useful later on
    public Dictionary<Vector3, Current> currentList;


    public FieldObject() //default constructor
    {

    }

    public FieldObject(Dictionary<Vector3, Charge> chargeList, Dictionary<Vector3, Current> currentList)
    {
        this.chargeList = chargeList;
        this.currentList = currentList;
    }

    //TODO add constructor with an array of charges also

    public void addCharge(Charge newCharge) //could use generics for both but thats just confusing
    {
        try
        {
            chargeList.Add(newCharge.position, newCharge);
        }
        catch (Exception e)
        {
            Console.WriteLine("The new Charge failed to be added to the collection");
            throw (e);
        }
    }


    public Charge getCharge(Vector3 somePosition)
    {
        try
        {
            Charge value = chargeList[somePosition];
            return value;
        }
        catch (Exception e)
        {
            Console.WriteLine("The new Charge failed to be retrieved from the collection");
            throw (e);
        }
    }

    public void setCharge(Vector3 currentChargePosition, Vector3 newPosition)
    {
        //get charge and remove old position then sets new position
        try
        {
            Charge value = chargeList[currentChargePosition];
            chargeList.Remove(currentChargePosition);
            chargeList.Add(newPosition, value);

        }

        catch (Exception e)
        {
            Console.WriteLine("The new Charge failed to be modified from the collection");
            throw (e);
        }
    }

    public void setCharge(Vector3 currentChargePosition, Vector3 newPosition, float newMagnitude)
    {
        //get charge and remove old position then sets new position and magnitude
        try
        {
            Charge value = new Charge(newMagnitude, newPosition);
            chargeList.Remove(currentChargePosition);
            chargeList.Add(newPosition, value);

        }

        catch (Exception e)
        {
            Console.WriteLine("The new Charge failed to be modified from the collection");
            throw (e);
        }
    }



    public void removeCharge(Vector3 chargePosition)
    {
        try
        {
            chargeList.Remove(chargePosition);
        }
        catch (Exception e)
        {
            Console.WriteLine("The new Charge failed to be removed to the collection");
            throw (e);
        }
    }




    public void addCurrent(Current newCurrent) //could use generics for both but thats just confusing
    {
        try
        {
            currentList.Add(newCurrent.position, newCurrent);
        }
        catch (Exception e)
        {
            Console.WriteLine("The new Current failed to be added to the collection");
            throw (e);
        }
    }


    public Current getCurrent(Vector3 somePosition)
    {
        try
        {
            Current value = currentList[somePosition];
            return value;
        }
        catch (Exception e)
        {
            Console.WriteLine("The new Current failed to be retrieved from the collection");
            throw (e);
        }
    }


    public void setCurrent(Vector3 currentCurrentPosition, Vector3 newPosition)
    {
        //get Current and remove old position then sets new position
        try
        {
            Current value = currentList[currentCurrentPosition];
            currentList.Remove(currentCurrentPosition);
            currentList.Add(newPosition, value);

        }

        catch (Exception e)
        {
            Console.WriteLine("The new Current failed to be modified from the collection");
            throw (e);
        }
    }

    public void setCurrent(Vector3 currentCurrentPosition, Vector3 newPosition, float newMagnitude, Vector3 newDirection)
    {
        //get Current and remove old position then sets new position and magnitude
        try
        {
            Current value = new Current(newMagnitude, newPosition, newDirection);
            currentList.Remove(currentCurrentPosition);
            currentList.Add(newPosition, value);

        }

        catch (Exception e)
        {
            Console.WriteLine("The new Current failed to be modified from the collection");
            throw (e);
        }
    }


    public void removeCurrent(Vector3 CurrentPosition)
    {
        try
        {
            currentList.Remove(CurrentPosition);
        }
        catch (Exception e)
        {
            Console.WriteLine("The new Current failed to be removed to the collection");
            throw (e);
        }
    }

}


public class Charge
{
    public float magnitude { get; set; }
    public Vector3 position { get; set; }

   
    public Charge()
    {
        magnitude = 0.0f;
        position = new Vector3(0, 0, 0);
    }

    public Charge(float magnitude, Vector3 position)
    {
        this.magnitude = magnitude;
        this.position = position;
    }
}


public class Current
{
    public float magnitude { get; set; }
    public Vector3 position { get; set; }
    public Vector3 direction
    {
        get { return direction; }
        set
        {
            direction = value.normalized;  
        }
    }



    public Current()
    {
        magnitude = 0.0f;
        position = new Vector3(0, 0, 0);
        direction = new Vector3(1, 0, 0);
    }

    public Current(float magnitude, Vector3 position, Vector3 direction)
    {
        this.magnitude = magnitude;
        this.position = position;
        this.direction = direction;

    }
}