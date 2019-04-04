using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class winZone : MonoBehaviour
{
public SceneManagerLevel1 sceneInfo;

    int winners;
    void Start() 
    {
        winners = 0;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBody")){
            winners += 1;
        }
        if (winners == 2 && sceneInfo.canWin==true) { 
            Initiate.Fade("Level2", Color.white, 0.6f);
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
