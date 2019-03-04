using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushFire : MonoBehaviour
{

    private bool canFire = true;
    public float smokeTime;
    public GameObject bushSmoke;
    public GameObject fire;
    public GameObject cannon;
    private GameObject otherGlobal = null;


    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        cannon = GameObject.FindWithTag("cannonStem");
        otherGlobal = other.gameObject;
        if (canFire)
        {

            if (other.gameObject.tag == "cannonBush")
            {
                canFire = false;
                FindObjectOfType<AudioManager>().Play("discovery");
                FindObjectOfType<AudioManager>().Play("smoke");

                var smoke = Instantiate(bushSmoke, new Vector3(27, 14, 0), Quaternion.Euler(new Vector3(-90, 0, 0)));
                yield return new WaitForSeconds(2);
                FindObjectOfType<AudioManager>().Play("fireWhoosh");
                var fire1 = Instantiate(fire, new Vector3(24, 14, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                yield return new WaitForSeconds(1);

                var fire2 = Instantiate(fire, new Vector3(29, 13, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                yield return new WaitForSeconds(1);

                var fire3 = Instantiate(fire, new Vector3(26, 19, 0), Quaternion.Euler(new Vector3(0, 0, 0)));

                yield return new WaitForSeconds(1);

                //hide, dont destroy
                other.gameObject.SetActive(false);
                cannon = GameObject.FindWithTag("cannon");
                cannon.SendMessage("MakeVisible");


                Destroy(fire1);
                Destroy(fire2);
                Destroy(fire3);
                Destroy(smoke);


            }
        }
    }


}

