using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int attackDamage;
    public int fireAttakDamage;

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
    private bool entryAttack = true;
    private bool cantMove = false;
    private bool fired;
    private float jumpTimeCounter;
    private float duration = 0.5f;
    private float durationCounter;
    private float attackRate = 0.2f;
    private float leftWaitTime;
    private float stopTimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene("Title1");
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
        {
            PlayerAttack();
        }
        Jump();
        Stop();
        TimeOrdhin();
        

        //ê›íuîªíË
        isGround = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);


    }

    private void FixedUpdate()
    {
        
        Move(); 
        anime.SetBool("Jump", isJumping);
        anime.SetBool("Ground", isGround);
    }

    //â°à⁄ìÆ
    void Move()
    {
        if (!cantMove)
        {
            float horizontalkey = Input.GetAxis("Horizontal");
            //â°à⁄ìÆ
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

    //ÉWÉÉÉìÉv
    void Jump()
    {

        Debug.Log(isGround);
        if (!cantMove)
        {
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
    }


    void PlayerAttack()
    {
            if (entryAttack && leftWaitTime <= 0)
            {
                Attack();
                durationCounter = duration;
                entryAttack = false;
                leftWaitTime = attackRate;
                stopTimeCounter = 0.5f;
                
            }
            else if (!entryAttack && durationCounter > 0f && leftWaitTime <=0)
            {
                Attack2();
                entryAttack = true;
                leftWaitTime = attackRate;
                stopTimeCounter = 0.5f;
            }
    }

    //çUåÇ
    void Attack()
    {
        rb2d.velocity = new Vector2(0, 0);
        if (!fired)
        {
            anime.SetTrigger("Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }
        else if (fired)
        {
            anime.SetTrigger("FireAttack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(fireAttakDamage);
            }
        }
        
        
    }
    //çUåÇÇQ
    void Attack2()
    {
        rb2d.velocity = new Vector2(0, 0);
        if (!fired)
        {
            anime.SetTrigger("Attack2");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }
        else if (fired)
        {
            anime.SetTrigger("FireAttack2");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(fireAttakDamage);
            }
        }
    }

    void Stop()
    {
        stopTimeCounter -= Time.deltaTime;
        if (stopTimeCounter > 0)
        {
            cantMove = true;
        }
        else
        {
            cantMove = false;
        }
    }
    
    void TimeOrdhin()
    {
            if (leftWaitTime > 0)
            {
                leftWaitTime -= Time.deltaTime;
            }

            if (durationCounter > 0f)
            {
                durationCounter -= Time.deltaTime;
            }
            else
            {
                durationCounter = 0;
                entryAttack = true;
            }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        
    }

    public void GetFireItem()
    {
        fired = true;
        
    }

    public void TakeDamage()
    {
        anime.SetTrigger("Damaged");
        stopTimeCounter = 0.7f;
        rb2d.velocity = new Vector2(0, 0);
    }

}
