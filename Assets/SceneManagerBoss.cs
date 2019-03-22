using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerBoss : MonoBehaviour{
    public GameObject explosions;
    public GameObject rightBarrier;
    public GameObject bombSmoke;
    public GameObject bombFire;
        public GameObject bombExplode;

        public GameObject bomb; 



    void BombTorch(){
        bombSmoke.SetActive(true);
        bombFire.SetActive(true);
        Invoke("ExplodingRubble", 3);
    }

    void ExplodingRubble(){
        bombExplode.SetActive(true);
        bomb.SetActive(false);
        rightBarrier.SetActive(false);
    }

    

  




}
