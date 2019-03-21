using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boulderPlank : MonoBehaviour
{
    public GameObject levelManager;

    private void Awake()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "ramp")
        {

            levelManager.SendMessage("StartRamp");
        }
    }


}
