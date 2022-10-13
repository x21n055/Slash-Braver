using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int attackDamage = 40;

    public float speed;
    public float jumpForce;
    public float jumpTimer;
    public float checkRadius;
    public float attackRange = 0.5f;

    public Transform feetPos;
    public Transform attackPoint;
    public LayerMask whatIsGround;
    public LayerMask enemyLayers;

    private Animator anime = null;
    private Rigidbody2D rb2d = null;
    private bool isGround;
    private bool isJumping;
    private float jumpTimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //UŒ‚
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
        //ƒWƒƒƒ“ƒv
        isGround = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGround == true && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpTimeCounter = jumpTimer;
            rb2d.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rb2d.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
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

        anime.SetBool("Jump", isJumping);
        anime.SetBool("Ground", isGround);
    }

    //UŒ‚
    void Attack()
    {
        anime.SetTrigger("Attack");
        /*Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDmage(attackDamage);
        }*/
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == enemyTag)
        {
            anime.Play("Player_Damaged");
            isDown = true;
        }
    }*/

}
