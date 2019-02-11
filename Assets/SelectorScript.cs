using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorScript : MonoBehaviour {

    public GameObject Bru;
    public GameObject Jotun;
    private Vector3 CharacterPosition;
    private Vector3 OffScreen;
    private int CharacterInt = 1;
    //gets the component, does it once upon start 
    private SpriteRenderer JotunRender, BruRender;

    private void Awake()
    {
        CharacterPosition = Bru.transform.position;
        OffScreen = Jotun.transform.position;
        JotunRender = Jotun.GetComponent<SpriteRenderer>();
        BruRender = Bru.GetComponent<SpriteRenderer>();
    }
    public void NextCharacter()
    {
        switch (CharacterInt)
        {
            case 1:
                BruRender.enabled = false;
                Bru.transform.position = OffScreen;
                Jotun.transform.position = CharacterPosition;
                JotunRender.enabled = true;
                CharacterInt++;
                break;
            case 2:
                JotunRender.enabled = false;
                Jotun.transform.position = OffScreen;
                Bru.transform.position = CharacterPosition;
                BruRender.enabled = true;
                CharacterInt++;
                ResetInteger();
                break;
            default:
                ResetInteger();
                break;
        }

    }


    public void PreviousCharacter()
    {
        switch (CharacterInt)
        {
            case 1:
                BruRender.enabled = false;
                Bru.transform.position = OffScreen;
                Jotun.transform.position = CharacterPosition;
                JotunRender.enabled = true;
                CharacterInt--;
                ResetInteger();
                break;
            case 2:
                JotunRender.enabled = false;
                Jotun.transform.position = OffScreen;
                Bru.transform.position = CharacterPosition;
                BruRender.enabled = true;
                CharacterInt--;
                
                break;
            default:
                ResetInteger();
                break;
        }
    }

    private void ResetInteger()
    {
        if (CharacterInt >= 2)
        {
            CharacterInt = 1;
        }
        else
        {
            CharacterInt = 2;
        }
    }

}
