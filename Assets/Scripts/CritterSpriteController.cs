using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterSpriteController : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;
    public Sprite idle;
    public Sprite attack;
    private bool attacking;
    private Patrol patrol;

    // Start is called before the first frame update
    void Awake()
    {
       m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
       patrol = gameObject.GetComponent<Patrol>();
       attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (patrol.HasDetected() && !attacking){
            attacking = true;
            m_SpriteRenderer.sprite = attack;
        }
        if (!patrol.HasDetected() && attacking) {
            attacking = false;
            m_SpriteRenderer.sprite = idle;
        }
        print(patrol.HasDetected() + ", " + attacking);
    }
}
