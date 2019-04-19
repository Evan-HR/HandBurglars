using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private GameObject SceneManager;
    public bool canTorch = true;

    private void Awake()
    {
        SceneManager = GameObject.FindGameObjectWithTag("LevelManager");
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "TorchFlame" && canTorch)
        {
            canTorch = false;
            Invoke("canTorchTrue", 3);
            SceneManager.SendMessage("BombTorch");
        }
    }

    void canTorchTrue()
    {
        canTorch = true;
    }

    
}
