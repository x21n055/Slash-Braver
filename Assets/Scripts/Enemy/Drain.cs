using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drain : MonoBehaviour
{
    public int speed;
    public float lifeTime = 1;
    public bool attackRight = true;
    public bool castCalled;
    public bool castEnd = false;
    public bool drainAttack = false;
    public bool thisIsFirst;
    public bool thisIsLast;
    public Vector3 offset;
    public Vector3 targetDirection;

    [SerializeField] GameObject target;
    [SerializeField] GameObject nextDrain;

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
            transform.position = target.transform.position + offset;
        }
        transform.position += targetDirection * speed * Time.deltaTime;
        if (drainAttack)
        {
            lifeTime -= Time.deltaTime;
        }
        if (lifeTime <= 0)
        {
            castCalled = false ;
            drainAttack = false;

        }
        DrainActive();
        DrainAttack();
    }

    public void CastCall()
    {
        lifeTime = 5;
        anime.SetTrigger("Drain_Attack");
        castCalled = true;
        if (thisIsFirst)
        {
            
            anime.SetTrigger("Drain_Release");
            StartCoroutine(CallNextDrain());
            IEnumerator CallNextDrain()
            {
                yield return new WaitForSeconds(0.8f);
                nextDrain.GetComponent<Drain>().ReleaseDrain();
            }
        }
    }

    public void ReleaseDrain()
    {
        anime.SetTrigger("Drain_Release");
        if (!thisIsLast)
        {
            StartCoroutine(CallNextDrain());
            IEnumerator CallNextDrain()
            {
                yield return new WaitForSeconds(0.8f);
                nextDrain.GetComponent<Drain>().ReleaseDrain();
            }
        }
        
    }

    public void DrainAttack()
    {
        if (castEnd)
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, 0)+ offset;
            drainAttack = true;
            castEnd = false;
        }
        
    }

    void DrainActive()
    {
        if (castCalled)
        {
            sr.color = new Color(255, 255, 255, 255);
        }
        else if (!castCalled)
        {
            sr.color = new Color(255, 255, 255, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            drainAttack = false;
            castCalled = false;
        }
    }
}
