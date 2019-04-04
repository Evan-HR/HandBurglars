using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class winZone : MonoBehaviour
{
public SceneManagerLevel1 sceneInfo;
public SceneManagerLevel2 sceneInfo2;

public Scene sceneCheck2;

    int winners;

    private void Awake()
    {
        sceneCheck2 = SceneManager.GetActiveScene();
    }
    void Start() 
    {
        winners = 0;
    }

//sceneCheck.name=="Level1"
    void OnTriggerEnter2D(Collider2D col)
    {
        if(sceneCheck2.name=="Level1"){
        if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBody")){
            winners += 1;
        }
        if (winners == 2 && sceneInfo.canWin==true) { 
            Initiate.Fade("Level2", Color.white, 0.6f);
        }
        }
        else if(sceneCheck2.name=="Level2"){
        if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBody")){
            winners += 1;
        }
        if (winners == 2 && sceneInfo2.canWin2==true) { 
            Initiate.Fade("Victory", Color.white, 0.6f);
        }

        }
        

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBody")){
            winners -= 1;
        }
        if (winners < 2){  
            // no longer allowed to pass to next level
        }



    }
}
