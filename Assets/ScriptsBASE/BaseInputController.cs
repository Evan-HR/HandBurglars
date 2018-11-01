using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInputController : MonoBehaviour
{

    //directional booleans
    public bool Up;
    public bool Down;
    public bool Left;
    public bool Right;

    public bool Fire1;

    public bool inventory1;
    public bool inventory2;

    public float vert;
    public float horz;
    public bool shouldRespawn;

    public Vector3 TEMPVec3;
    private Vector3 zeroVector = new Vector3(0, 0, 0);

    public virtual void CheckInput()
    {

    }

    public virtual float GetHorizontal()
    {
        return horz;
    }

    public virtual float GetVertical()
    {
        return vert;
    }

    public virtual Vector3 GetMovementDirectionVector()
    {
        TEMPVec3 = zeroVector;

        if (Left || Right)
        {
            TEMPVec3.x = horz;
        }

        if (Up || Down)
        {
            TEMPVec3.y = vert;
        }

        return TEMPVec3;
    }



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }
}
