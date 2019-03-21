using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollision : MonoBehaviour
{
    PlayerManager PlayerManager;

    void Start() {
        PlayerManager =  transform.parent.parent.gameObject.GetComponent<PlayerManager>();
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("HandObjects") && other.gameObject.GetComponent<Rigidbody2D>() != null){
            print("OHYA");
            PlayerManager.HandTriggerEnter2D(other);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("HandObjects") && other.gameObject.GetComponent<Rigidbody2D>() != null){
            PlayerManager.HandTriggerStay2D(other);
        }
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("HandObjects") && other.gameObject.GetComponent<Rigidbody2D>() != null){
            PlayerManager.HandTriggerExit2D(other);
        }
        
    }
}
