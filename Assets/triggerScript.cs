using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class triggerScript : MonoBehaviour
{
    public int id; 
    public Scene sceneCheck;
    public GameObject triggerUp;
    public GameObject triggerDown;
    private GameObject SceneManagerScript;
    private bool firstExplosion=false;



    public SceneManagerLevel2 sceneInfo2;

    private SpriteRenderer triggerUpRender, triggerDownRender;
    private void Awake()
    {
        sceneCheck = SceneManager.GetActiveScene();
        SceneManagerScript = GameObject.FindGameObjectWithTag("LevelManager");
    }

    void OnTriggerEnter2D(Collider2D col1)
    {

if(sceneCheck.name=="Level1"){
            if (col1.gameObject.tag == "Player1" || col1.gameObject.tag=="Player2")
        {
            //run start boulder from SceneManagerLevel1 script
            SceneManagerScript.SendMessage("StartBoulder");
            triggerUp.SetActive(false);

        }


}
else if(sceneCheck.name=="Level2"){
            if ((col1.gameObject.layer == LayerMask.NameToLayer("PlayerBody") || col1.gameObject.layer == LayerMask.NameToLayer("PlayerBody2")) && id == 1)
        {
            triggerUp.SetActive(false);
            SceneManagerScript.SendMessage("FirstBarrierExplosion");
            firstExplosion=true;


        }

        if ((col1.gameObject.layer == LayerMask.NameToLayer("PlayerBody") || col1.gameObject.layer == LayerMask.NameToLayer("PlayerBody2")) && id==2)
        {

            triggerUp.SetActive(false);
            sceneInfo2.setWinLevel2();
            SceneManagerScript.SendMessage("SecondBarrierExplosion");


        }
}

    }
}
