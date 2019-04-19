using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEye2D.Behaviour;

public class Damages : MonoBehaviour
{
    BoxCollider2D dmg_collider;
    public int dmg_val;
    public bool doesKnockBack;
    public float knockBackForce;
    int playerLayers;
     int playerLayer;

     int player2Layer;
     int playerGDLayer;
    Shake m_CameraShake;

    // Start is called before the first frame update
    void Start()
    {
        m_CameraShake = Camera.main.GetComponent<Shake>();
        dmg_collider = gameObject.GetComponent<BoxCollider2D>();
        playerLayer = 1 << LayerMask.NameToLayer("PlayerBody1");
        player2Layer = 1 << LayerMask.NameToLayer("PlayerBody2");
        playerGDLayer = 1 << LayerMask.NameToLayer("PlayerBodyGoingDown");
         playerLayers = playerLayer | playerGDLayer | player2Layer;
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other){
        int layer = other.gameObject.layer;
        if (playerLayers == (playerLayers | (1 << layer)) && other.gameObject.GetComponent<PlayerManager>() != null){
            if (other.gameObject.GetComponent<PlayerManager>().canBeHit){
                FindObjectOfType<AudioManager>().Play("monsterHandSquish");
                m_CameraShake.ShakeCamera(2f);
                other.gameObject.GetComponent<PlayerManager>().takeDamage(dmg_val);
                //inflict dmg to player
                // if knockback:
                // push player with force #TODO
            }
        }
    }
    void OnTriggerStay2D(Collider2D other){
        int layer = other.gameObject.layer;
        if (playerLayers == (playerLayers | (1 << layer)) && other.gameObject.GetComponent<PlayerManager>() != null){
            if (other.gameObject.GetComponent<PlayerManager>().canBeHit){
                other.gameObject.GetComponent<PlayerManager>().takeDamage(dmg_val);
                FindObjectOfType<AudioManager>().Play("monsterHandSquish");
                m_CameraShake.ShakeCamera(2f);
                //inflict dmg to player
                // if knockback:
                // push player with force #TODO
            }
        }
    }
}
