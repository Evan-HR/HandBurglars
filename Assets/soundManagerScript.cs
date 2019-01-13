using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManagerScript : MonoBehaviour {
    public static AudioClip fireWhoosh, dash,smoke, jump, lostHealth, discovery, monsterSmash;
    static AudioSource audiosrc;
	// Use this for initialization
	void Start () {
        dash = Resources.Load<AudioClip>("dash");
        fireWhoosh = Resources.Load<AudioClip>("fireWhoosh");
        smoke = Resources.Load<AudioClip>("smoke");
        jump = Resources.Load<AudioClip>("jump");
        lostHealth = Resources.Load<AudioClip>("lostHealth");
        discovery = Resources.Load<AudioClip>("discovery");
        monsterSmash = Resources.Load<AudioClip>("smash");

        audiosrc = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "fireWhoosh":
                audiosrc.PlayOneShot(fireWhoosh);
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
            case "monsterSmash":
                audiosrc.PlayOneShot(monsterSmash);
                break;
            case "dash":
                audiosrc.PlayOneShot(dash);
                break;
        }
    }
}
