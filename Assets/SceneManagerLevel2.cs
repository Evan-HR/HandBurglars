using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerLevel2 : MonoBehaviour{
    public GameObject dustCloud;

    public GameObject explosions;
    public GameObject firstBarrier;
    public GameObject secondBarrier;

    public GameObject explosions2;
    public GameObject dustCloud2;
    public GameObject bombSmoke;
    public GameObject bombFire;
        public GameObject bombExplode;

        public GameObject bomb; 

        public GameObject bottomBarrier;


    void FirstBarrierExplosion(){
        //print("got here");
        FindObjectOfType<AudioManager>().Play("Lvl1SecondCrack");
        FindObjectOfType<AudioManager>().Play("cannon");
        firstBarrier.SetActive(false);
        explosions.SetActive(true);
        dustCloud.SetActive(true);

    }
        void SecondBarrierExplosion(){
        //print("got here");
        FindObjectOfType<AudioManager>().Play("Lvl1SecondCrack");
        FindObjectOfType<AudioManager>().Play("cannon");
        secondBarrier.SetActive(false);
        explosions2.SetActive(true);
        dustCloud2.SetActive(true);

    }

    void BombTorch(){
        bombSmoke.SetActive(true);
        bombFire.SetActive(true);
        Invoke("ExplodingRubble", 3);
    }

    void ExplodingRubble(){
        bombExplode.SetActive(true);
        bomb.SetActive(false);
        bottomBarrier.SetActive(false);
    }

    

  




}
