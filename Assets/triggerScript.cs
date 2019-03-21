using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerScript : MonoBehaviour
{

    public GameObject triggerUp;
    public GameObject triggerDown;
    public GameObject levelManager;

    private SpriteRenderer triggerUpRender, triggerDownRender;

    private void Awake()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        triggerUpRender = triggerUp.GetComponent<SpriteRenderer>();
        triggerDownRender = triggerDown.GetComponent<SpriteRenderer>();
        triggerUpRender.enabled = true;
        triggerDownRender.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D col1)
    {

        if (col1.gameObject.tag == "Player")
        {
            //run start boulder from levelManager script
            levelManager.SendMessage("StartBoulder");
            triggerUpRender.enabled = false;
            triggerDownRender.enabled = true;


        }
    }
}
