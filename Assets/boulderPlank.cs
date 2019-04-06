﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boulderPlank : MonoBehaviour
{
    public GameObject SceneManagerLevel1;
    public SceneManagerLevel1 SceneManagerVariables;

    private void Awake()
    {
        SceneManagerLevel1 = GameObject.FindGameObjectWithTag("LevelManager");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("level1BluePlank"))
        {
            print("GET HERE?");
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
            SceneManagerVariables.setWinLevel1();
            SceneManagerLevel1.SendMessage("DestroyRightDoor");
        
        }
        
    }

}
