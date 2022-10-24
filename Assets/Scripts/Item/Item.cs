using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Animator anime = null;
    public LayerMask playerLayers;
    public Transform getPoint;
    public float getRadius = 0.5f;
    public bool get = false;
    private SpriteRenderer sr = null;


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (get)
        {
            Destroy(gameObject);
        }
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(getPoint.position, getRadius, playerLayers);
        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<Player>().GetFireItem();
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (getPoint == null)
            return;
        Gizmos.DrawWireSphere(getPoint.position, getRadius);

    }


}
