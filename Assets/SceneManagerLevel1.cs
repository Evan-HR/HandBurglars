﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

public GameObject bombExplode;
public GameObject bomb;
public GameObject bottomHideout;

    private void Awake()
    {
        rampColliderRender = rampColliderRender.GetComponent<SpriteRenderer>();
    
    }

    void StartBoulder()
    {
        plank.SetActive(true);
        plankInitial.SetActive(false);
        FindObjectOfType<AudioManager>().Play("Lvl1FirstCrack");
    }

    void StartRamp()
    {
        FindObjectOfType<AudioManager>().Play("Lvl1SecondCrack");
        FindObjectOfType<AudioManager>().Play("Lvl1BallRoll");
        rampColliderRender.enabled = false;
        ramp.SetActive(true);
        dustCloud.SetActive(true);
    }

    void MiddleCollide()
    {
        FindObjectOfType<AudioManager>().Play("Lvl1DestroyMiddle");
        Invoke("Explosions", 0.5f);

    }
    void Explosions()
    {
        explosions.SetActive(true);
        middle.SetActive(false);
    }

    void BoulderFallGround(){
        dustCloud2.SetActive(true);
    }

    void DestroyRightDoor(){
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
        bombFire.SetActive(true);
        Invoke("ExplodingRubble", 3);
    }

    void ExplodingRubble(){
        bombExplode.SetActive(true);
        bomb.SetActive(false);
        bottomHideout.SetActive(false);
    }

  




}