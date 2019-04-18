using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private GameObject SceneManager;

    private void Awake()
    {
        SceneManager = GameObject.FindGameObjectWithTag("LevelManager");
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "TorchFlame")
        {
            SceneManager.SendMessage("BombTorch");
        }
    }

    
}
