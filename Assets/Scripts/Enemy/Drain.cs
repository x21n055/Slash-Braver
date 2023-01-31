using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drain : MonoBehaviour
{
    public int speed;
    public float lifeTime;
    public bool attackRight = true;
    public bool castEnd = false;
    public bool drainAttack = false;
    public Vector3 offset;

    [SerializeField] GameObject target;

    private SpriteRenderer sr = null;
    private Animator anime = null;

    [SerializeField] Transform position;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!drainAttack)
        {
            transform.position = position.position;
        }
        if (this.transform.localScale.x == -1)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        else if (this.transform.localScale.x == 1)
        {
            transform.position += -transform.right * speed * Time.deltaTime;
        }
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            drainAttack = false ;
        }
        DrainActive();
        DrainAttack();
    }

    public void CastCall()
    {
        castEnd = true;
    }

    public void DrainAttack()
    {
        if (castEnd)
        {
            transform.position = target.transform.position + offset;
            if (offset.x <= 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (offset.x >= 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            lifeTime = 5;
            drainAttack = true;
            castEnd = false;
        }
        
    }

    void DrainActive()
    {
        if (drainAttack)
        {
            sr.color = new Color(255, 255, 255, 255);
        }
        else if (!drainAttack)
        {
            sr.color = new Color(255, 255, 255, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            drainAttack = false;
        }
    }
}
