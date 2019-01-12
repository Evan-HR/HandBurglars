using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushFire : MonoBehaviour
{
    private bool canFire = false;
    public float smokeTime;
    public GameObject bushSmoke;
    public GameObject fire;
    private GameObject otherGlobal = null;


    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        otherGlobal = other.gameObject;
        if (other.gameObject.tag == "cannonBush")
        {

            var smoke = Instantiate(bushSmoke, new Vector3(27, 14, 0), Quaternion.Euler(new Vector3(-90, 0, 0)));
            yield return new WaitForSeconds(2);
            var fire1 = Instantiate(fire, new Vector3(24, 14, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
            yield return new WaitForSeconds(1);
            var fire2 = Instantiate(fire, new Vector3(29, 13, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
            yield return new WaitForSeconds(1);
            var fire3 = Instantiate(fire, new Vector3(26, 19, 0), Quaternion.Euler(new Vector3(0, 0, 0)));

            yield return new WaitForSeconds(1);

            //MUST destroy smoke before destroying the otherGlobal (bush)!
            Destroy(smoke);
            Destroy(otherGlobal);
            Destroy(fire1);
            Destroy(fire2);
            Destroy(fire3);
    
            
            

        }
    }


}

