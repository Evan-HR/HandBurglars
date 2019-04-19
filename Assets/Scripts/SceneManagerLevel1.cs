using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameEye2D.Behaviour;

public class SceneManagerLevel1 : MonoBehaviour{
    public GameObject boulder;
    public GameObject plank;
    public GameObject plankInitial;
    public GameObject rampCollider;
    public GameObject ramp;
    public GameObject dustCloud;
    public GameObject dustCloud2;
    public SpriteRenderer rampColliderRender;


    public GameObject rightDoor;

    public GameObject explosions;
    public GameObject middle;

    public GameObject explosions2;
    public GameObject dustCloud3;
    public GameObject bombSmoke;
    public GameObject bombFire;

    public bool canWin = false;

public GameObject bombExplode;
public GameObject bomb;
public GameObject bottomHideout;


    //shake effect
    Shake m_CameraShake;

    private void Awake()
    {
        m_CameraShake = Camera.main.GetComponent<Shake>();
        rampColliderRender = rampColliderRender.GetComponent<SpriteRenderer>();
    
    }

    void StartBoulder()
    {
        plank.SetActive(true);
        plankInitial.SetActive(false);
        FindObjectOfType<AudioManager>().Play("Lvl1FirstCrack");
    }
public void setWinLevel1(){
    canWin=true;
}

public bool getWinLevel1(){
    return canWin;
}
    void StartRamp()
    {
        print("got here");
        FindObjectOfType<AudioManager>().Play("Lvl1SecondCrack");
        FindObjectOfType<AudioManager>().Play("Lvl1BallRoll");
        rampColliderRender.enabled = false;
        ramp.SetActive(true);
        dustCloud.SetActive(true);
    }

    void MiddleCollide()
    {
        FindObjectOfType<AudioManager>().Play("Lvl1DestroyMiddle");
        Invoke("Explosions", 0.0f);

    }
    void Explosions()
    {
        explosions.SetActive(true);
        middle.SetActive(false);
    }

    void BoulderFallGround(){
        dustCloud2.SetActive(true);
    }

    public void DestroyRightDoor(){
        //print("got here");
        FindObjectOfType<AudioManager>().Play("Lvl1SecondCrack");
        FindObjectOfType<AudioManager>().Play("cannon");
        rightDoor.SetActive(false);
        explosions2.SetActive(true);
        boulder.SetActive(false);
        dustCloud3.SetActive(true);

    }

        void BombTorch(){
        bombSmoke.SetActive(true);
        FindObjectOfType<AudioManager>().Play("smoke");
        bombFire.SetActive(true);
        Invoke("ExplodingRubble", 3);
    }

    void ExplodingRubble(){
        FindObjectOfType<AudioManager>().Play("cannon");
        m_CameraShake.ShakeCamera(2f);
        bombExplode.SetActive(true);
        bomb.SetActive(false);
        bottomHideout.SetActive(false);
    }

  




}
