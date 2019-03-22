using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{

    MovingButton movingButton;
    // Start is called before the first frame update
    void Start()
    {
        movingButton = transform.parent.gameObject.GetComponent<MovingButton>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        movingButton.press();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        movingButton.reset();
    }
}
