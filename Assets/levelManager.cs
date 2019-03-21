using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour{
    public GameObject boulder;
    public GameObject plank;
    public GameObject plankInitial;
    public GameObject rampCollider;
    public GameObject ramp;
    public GameObject dustCloud;
    public SpriteRenderer rampColliderRender;
    public GameObject explosions;
    public GameObject middle;

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

  




}
