using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BombScript : MonoBehaviour
{
    private GameObject sceneManager;
    public bool canTorch = true;
    public Scene scene;

    private void Awake()
    {
        scene = SceneManager.GetActiveScene();
        sceneManager = GameObject.FindGameObjectWithTag("LevelManager");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
if(scene.name=="POC_Boss"){
        if (col.gameObject.tag == "TorchFlame" && canTorch && !sceneManager.GetComponent<SceneManagerBoss>().bombLit)
        {
            canTorch = false;
            Invoke("canTorchTrue", 6);
            sceneManager.SendMessage("BombTorch");
        }
}else{
        if (col.gameObject.tag == "TorchFlame" && canTorch)
        {
            canTorch = false;
            Invoke("canTorchTrue", 6);
            sceneManager.SendMessage("BombTorch");
        }
}

    }

    void canTorchTrue()
    {
        canTorch = true;
    }

    
}
