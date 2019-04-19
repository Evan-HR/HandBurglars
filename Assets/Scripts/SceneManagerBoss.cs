using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameEye2D.Behaviour;

public class SceneManagerBoss : MonoBehaviour{

    public GameObject bombSmoke;
    public GameObject bombFire;
    public GameObject bombRespawn;
    public bool bombLit = false;
        public GameObject bombExplode;
    //shake effect
    Shake m_CameraShake;
    public GameObject bomb;

    private void Awake()
    {
        m_CameraShake = Camera.main.GetComponent<Shake>();
    }
    // void BombBoss(){
    //     Vector2 tempPos = bomb.transform.position;
    //     bombExplode.transform.position = tempPos;
    //     bombExplode.SetActive(true);
    //     bomb.transform.position = bombRespawn.transform.position;
    // }

    void BombTorch(){
        bombLit = true;
        bombSmoke.SetActive(true);
        bombFire.SetActive(true);
        Invoke("explodeBomb", 7);
    }

    void explodeBomb(){
        bombLit = false;
        m_CameraShake.ShakeCamera(2f);
        //Vector2 tempPos = bomb.transform.position;
        //bombExplode.transform.position = tempPos;
        Object.Instantiate<GameObject>(bombExplode, bomb.transform.position, bomb.transform.rotation);
        bomb.transform.position = bombRespawn.transform.position;
        bombSmoke.SetActive(false);
        bombFire.SetActive(false);
    }

    

  




}
