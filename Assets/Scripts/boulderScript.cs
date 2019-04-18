using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boulderScript : MonoBehaviour
{
    public GameObject SceneManagerLevel1;

    private void Awake()
    {
        SceneManagerLevel1 = GameObject.FindGameObjectWithTag("LevelManager");
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "boulder")
        {
            SceneManagerLevel1.SendMessage("MiddleCollide");
        }
    }
}
