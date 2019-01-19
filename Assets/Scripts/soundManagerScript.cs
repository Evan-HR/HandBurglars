using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManagerScript : MonoBehaviour {
    public static AudioClip smoke, jump, lostHealth, smash, dash, fireWhoosh, discovery;
    static AudioSource fireSrc;
    static AudioSource discoverySrc;
    static AudioSource audiosrc;
    // Use this for initialization
    void Start () {
        dash = Resources.Load<AudioClip>("dash");
        fireWhoosh = Resources.Load<AudioClip>("fireWhoosh");
        smoke = Resources.Load<AudioClip>("smoke");
        jump = Resources.Load<AudioClip>("jump");
        lostHealth = Resources.Load<AudioClip>("lostHealth");
        discovery = Resources.Load<AudioClip>("discovery");
        smash = Resources.Load<AudioClip>("smash");
        audiosrc = GetComponent<AudioSource>();
        discoverySrc = GetComponent<AudioSource>();
        fireSrc = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "fireWhoosh":
                fireSrc.PlayOneShot(fireWhoosh);
                break;
            case "smoke":
                audiosrc.PlayOneShot(smoke);
                break;
            case "jump":
                audiosrc.PlayOneShot(jump);
                break;
            case "lostHealth":
                audiosrc.PlayOneShot(lostHealth);
                break;
            case "smash":
                audiosrc.PlayOneShot(smash);
                break;
            case "dash":
                audiosrc.PlayOneShot(dash);
                break;
            case "discovery":
                discoverySrc.volume = 0.1f;
                discoverySrc.PlayOneShot(discovery);
                break;
        }
    }
}
