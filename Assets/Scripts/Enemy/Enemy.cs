using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public Animator anime = null;
    public int maxHealth = 100;
    public UnityEvent shock;

    public float moveSpeed;

    //コンバットAI
    public Transform rayCast;
    public Transform attackPoint;
    public LayerMask raycastMask;
    public LayerMask playerLayers;
    public float rayCastLength;
    public float attackDistance; //Minimum distance for attack
    public float timer; //Timer for cooldown between attacks
    public Transform leftLimit;
    public Transform rightLimit;
    public float attackRange = 0.5f;
    public bool attacking;
    public bool cantMove = false;
    public float stopTimeCounter;

    private SpriteRenderer sr = null;
    private Rigidbody2D rb2d = null;
    private bool isDead = false;

    private RaycastHit2D hit;
    private Transform target;
    private float distance; //Store the distance b/w enemy and player
    private bool attackMode;
    private bool inRange; //Check if Player is in range
    private bool cooling; //Check if Enemy is cooling after attack
    private float intTimer;

    void Awake()
    {
        SelectTarget();
        intTimer = timer; //Store the inital value of timer
    }


    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        anime = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (attacking)
        {
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
            foreach (Collider2D player in hitPlayer)
            {
                player.GetComponent<Player>().TakeDamage();
            }
        }
        if (!isDead)
        {
            if (sr.isVisible)
            {
                if (!attackMode)
                {
                    Move();
                }

                if (!InsideOfLimits() && !inRange && !anime.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack"))
                {
                    SelectTarget();
                }

                if (inRange)
                {
                    hit = Physics2D.Raycast(rayCast.position, transform.right, rayCastLength, raycastMask);
                }

                if (hit.collider != null)
                {
                    EnemyLogic();
                }
                else if (hit.collider == null)
                {
                    inRange = false;
                }

                if (inRange == false)
                {
                    StopAttack();
                }

            }
            else
            {
                rb2d.Sleep();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            target = trig.transform;
            inRange = true;
            Flip();
        }
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            anime.SetBool("enemy_attack", false);
        }
    }

    void Move()
    {
        if (!cantMove)
        {
            anime.SetBool("enemy_run", true);

            if (!anime.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack"))
            {
                Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
        }
    }

    void Attack()
    {
        timer = intTimer; //Reset Timer when Player enter Attack Range
        attackMode = true; //To check if Enemy can still attack or not
        anime.SetBool("enemy_run", false);
        anime.SetBool("enemy_attack", true);
        Flip();
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anime.SetBool("enemy_attack", false);
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideOfLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    private void SelectTarget()
    {
        float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        //Ternary Operator
        //target = distanceToLeft > distanceToRight ? leftLimit : rightLimit;

        Flip();
    }

    void Flip()
    {
        if (!isDead && !cantMove)
        {
            Vector3 rotation = transform.eulerAngles;
            if (transform.position.x > target.position.x)
            {
                rotation.y = 180;
            }
            else
            {
                rotation.y = 0;
            }

            //Ternary Operator
            //rotation.y = (currentTarget.position.x < transform.position.x) ? rotation.y = 180f : rotation.y = 0f;

            transform.eulerAngles = rotation;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {

            shock.Invoke();
            currentHealth -= damage;
            stopTimeCounter = 0.4f;
            anime.SetTrigger("enemy_damaged");
            if (currentHealth <= 0)
            {
                isDead = true;
                Die();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }

    void Die()
    {
        anime.SetBool("enemy_dead", true);
    }


    void RaycastDebugger()
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.red);
        }
        else if (attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.green);
        }
    }

}