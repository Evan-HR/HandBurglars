using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pitCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other){
        print("hit");
        if (other.gameObject.layer == LayerMask.NameToLayer("HandObjectGrab")){
            Destroy(other.gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBody")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
