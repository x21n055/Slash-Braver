using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    private Animator anime = null;
    private Rigidbody2D rb2d = null;

    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float horizontalkey = Input.GetAxis("Horizontal");
        //‰¡ˆÚ“®
        if (horizontalkey > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            anime.SetBool("Run", true);
        }
        else if (horizontalkey < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            anime.SetBool("Run", true);
        }
        else
        {
            anime.SetBool("Run", false);
        }

        rb2d.velocity = new Vector2(horizontalkey * speed, rb2d.velocity.y);
    }
}
