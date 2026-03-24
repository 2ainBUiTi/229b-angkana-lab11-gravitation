using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic; //for List

public class Gravity : MonoBehaviour
{
    Rigidbody rb;
    const float G = 0.006674f; //Gravitational Constant 6.674

    public static List<Gravity> otherObjectsList;

    [SerializeField] bool planet = false;
    [SerializeField] int orbitspeed = 1000;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //create a list for the first time
        if (otherObjectsList == null)
        {
            otherObjectsList = new List<Gravity>();
        }

        //add object (with gravity script) to attract to the list
        otherObjectsList.Add(this);

        //orbit
        if (!planet) {
            rb.AddForce(Vector3.left * orbitspeed);
        }
    }

    private void FixedUpdate()
    {
        foreach (Gravity obj in otherObjectsList)
        {
            //do not attract itself
            if (obj != this)
            {
                Attract(obj);
            }
        }
    }

    void Attract(Gravity other)
    { 
        Rigidbody otherRb = other.rb;

        //get direction between 2 objs (EARTH AND OTHERS)
        Vector3 direction = rb.position - otherRb.position;

        //get distance between 2 objs = r
        float distance = direction.magnitude;

        //if 2 objs are at the same position, just return = do nothing to avoid collision
        if(distance == 0f) { return; }

        //F = G*((m1 * m2)/r*r
        float forceMagnitude = G * (rb.mass * otherRb.mass) / Mathf.Pow(distance, 2);

        Vector3 gravityForce = forceMagnitude * direction.normalized;

        otherRb.AddForce(gravityForce);

       
    }
}
