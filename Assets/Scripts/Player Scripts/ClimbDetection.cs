using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbDetection : MonoBehaviour
{

    PlayerManager PlayerManager;
    // Start is called before the first frame update
    void Start()
    {
        PlayerManager =  transform.parent.gameObject.GetComponent<PlayerManager>();
    }

    void OnTriggerEnter2D(Collider2D collider){
        print("dfdfdfdfdfdfdfdfdfdfd");
        if (collider.gameObject.layer == LayerMask.NameToLayer("Ladder") ){
            PlayerManager.LadderEnter();
        } else if (collider.gameObject.layer == LayerMask.NameToLayer("Cover")){
            PlayerManager.CoverEnter();
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if (collider.gameObject.layer == LayerMask.NameToLayer("Ladder")){
            PlayerManager.LadderExit();

        } else if (collider.gameObject.layer == LayerMask.NameToLayer("Cover")){
            PlayerManager.CoverExit();
        }
    }
}
