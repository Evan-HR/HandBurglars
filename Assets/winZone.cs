using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class winZone : MonoBehaviour
{
public SceneManagerLevel1 sceneInfo;
public SceneManagerLevel2 sceneInfo2;

public Scene sceneCheck2;

    bool player1WinZone = false;
    bool player2WinZone = false;

    private void Awake()
    {
        
        sceneCheck2 = SceneManager.GetActiveScene();
    }


//sceneCheck.name=="Level1"
    void OnTriggerEnter2D(Collider2D col)
    {
        if(sceneCheck2.name=="Level1"){

            if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBody1")){

                player1WinZone = true;
            }
            if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBody2"))
            {

                player2WinZone = true;
            }

            if (player1WinZone && player2WinZone && sceneInfo.canWin==true) { 
                Initiate.Fade("Level2", Color.white, 0.8f);
            }
        }


        else if (sceneCheck2.name == "Level2")
        {

            if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBody1"))
            {

                player1WinZone = true;
            }
            if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBody2"))
            {

                player2WinZone = true;
            }

            if (player1WinZone && player2WinZone && sceneInfo2.canWin2 == true)
            {
                Initiate.Fade("Victory", Color.white, 0.8f);
            }
        }


    }

    void OnTriggerExit2D(Collider2D col)
    {
            if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBody"))
            {

                player1WinZone = false;
            }
            if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBody2"))
            {

                player2WinZone = false;
            }


    }
}
