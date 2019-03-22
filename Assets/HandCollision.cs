using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollision : MonoBehaviour
{
    PlayerManager PlayerManager;

    int grabObjLayer;
    int dragObjLayer;
    int handObjLayer;

    void Start() {
        PlayerManager =  transform.parent.parent.gameObject.GetComponent<PlayerManager>();
        grabObjLayer = 1 << LayerMask.NameToLayer("HandObjectGrab");
        dragObjLayer = 1 << LayerMask.NameToLayer("HandObjectDrag");
         handObjLayer = grabObjLayer | dragObjLayer;
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (handObjLayer == (handObjLayer | (1 << layer)) && other.gameObject.GetComponent<Rigidbody2D>() != null){
            print("OHYA");
            PlayerManager.HandTriggerEnter2D(other);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (handObjLayer == (handObjLayer | (1 << layer)) && other.gameObject.GetComponent<Rigidbody2D>() != null){
            PlayerManager.HandTriggerStay2D(other);
        }
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (handObjLayer == (handObjLayer | (1 << layer)) && other.gameObject.GetComponent<Rigidbody2D>() != null){
            PlayerManager.HandTriggerExit2D(other);
        }
        
    }
}
