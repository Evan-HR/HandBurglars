using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boulderPlank : MonoBehaviour
{
    public GameObject SceneManagerLevel1;

    private void Awake()
    {
        SceneManagerLevel1 = GameObject.FindGameObjectWithTag("LevelManager");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "ramp")
        {

            SceneManagerLevel1.SendMessage("StartRamp");
        }

        if (col.gameObject.tag == "GroundSoundBoulder"){
            SceneManagerLevel1.SendMessage("BoulderFallGround");
        }

    }

        void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "RightDoor"){
            //print("got here collision");
            SceneManagerLevel1.SendMessage("DestroyRightDoor");
        }
        
    }

}
