using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonRotate : MonoBehaviour {

    float MaxRotUp = -35f;
    float MaxRotDown = 25f;
    //keeps track of rotation and speed 
    float Counter = 0f;

    bool up = false;
    bool down = false;

    public void Up()
    {
        up = true;
    }
    public void Down()
    {
        down = true;
    }

    public void StopRotations()
    {
        up = false;
        down = false;
    }

    //will keep rotating not just rotate once and stop
    //last val in vector is speed 
    public void FixedUpdate()
    {
        if (up == true && Counter > MaxRotUp)
        {
            transform.RotateAround(new Vector3(29, 16, 0), new Vector3(0, 0, -1), 1);
            Counter -= 1;
        }
        if (down == true && Counter < MaxRotDown)
        {
            transform.RotateAround(new Vector3(29, 16, 0), new Vector3(0, 0, 1), 1);
            Counter += 1;
           
        }
    }
}
