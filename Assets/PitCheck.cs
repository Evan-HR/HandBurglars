using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitCheck : MonoBehaviour
{
        private GameManager gameManager;

        public GameObject torchPrefab;

        public Transform torchRespawn;
        LayerMask player_layer;
        LayerMask player2_layer;
        LayerMask grab_layer;
    // Start is called before the first frame update
    void Start()
    {
        player_layer = LayerMask.NameToLayer("PlayerBody1");
        player2_layer = LayerMask.NameToLayer("PlayerBody2");
        grab_layer = LayerMask.NameToLayer("HandObjectGrab");
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.gameObject.layer == player_layer) || (col.gameObject.layer == player2_layer)){
            if (col.gameObject.GetComponent<PlayerManager>() != null){
                
                col.gameObject.GetComponent<PlayerManager>().Respawn();
            }
        } else  if (col.gameObject.layer == grab_layer){
            print("here you go!");
            Object.Destroy(col.gameObject);
            Instantiate(torchPrefab, (Vector3) torchRespawn.position, Quaternion.identity);

        }
    }
}
