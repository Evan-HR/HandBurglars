using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderBreak : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.layer == LayerMask.NameToLayer("boulder")){
            GameObject.Find("SceneManager").GetComponent<SceneManagerLevel1>().DestroyRightDoor();
            GameObject.Find("SceneManager").GetComponent<SceneManagerLevel1>().setWinLevel1();
        }
    }
}
