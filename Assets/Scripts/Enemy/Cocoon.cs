using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cocoon : MonoBehaviour
{
    //データ
    public int maxHealth;
    public int currentHealth;
    public int speed;
    public float attack1Distance;
    public float stopTimeCounter;
    public float distance;              //プレイヤーとの座標的距離
    public float range;                 //プレイヤーとの距離
    private bool playerOnTheRight;
    public bool attacking = false;
    [SerializeField] GameObject target;

    //コンポーネント
    private Rigidbody2D rb2d = null;
    private SpriteRenderer sr = null;
    private Animator anime = null;
    //コンバットAI
    private bool inCombat = false;  //戦闘状態を判定
    private bool looming = false;   //プレイヤーに接近
    public bool cantMove = false;  //硬直状態
    public Transform setPosition;   //敵の初期配置場所
    public UnityEvent shock;        //被ダメ衝撃
    private bool isDead = false;    //死亡

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerWhichSide();
        Move();
        Combat();
    }

    void Move()
    {
        if (!cantMove && looming)
        {
            anime.SetBool("HeavyArmor_Walk", true);
            if (playerOnTheRight)
            {
                rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
            }
            else if(!playerOnTheRight)
            {
                rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
            }
        }
        else if(cantMove)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
    }

    void Combat()
    {
        range = Vector2.Distance(transform.position, target.transform.position);
        if (range <= attack1Distance && !attacking)
        {
            anime.SetTrigger("HeavyArmor_Attack1");
        }

    }

    //プレイヤー発見 戦闘Stateへ移行
    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            Debug.Log("戦闘状態");
            inCombat = true;
            looming = true;
        }
    }
    
    public void TakeDamage(int damage)  //被ダメ
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

    void PlayerWhichSide() //プレイヤーが右にいるか左にいるか判定を行い、Flipに引数を渡す
    {
        distance = this.gameObject.transform.position.x - target.transform.position.x;
        if (distance < 0)
        {
            playerOnTheRight = true;
        }
        else if (distance > 0)
        {
            playerOnTheRight = false;
        }
        Flip();
    }

    void Flip()    //PlayerWhichSide()から引数を受け取り、それに応じてフリップする
    {
        if (!isDead && !cantMove)
        {
            if (playerOnTheRight)
            {
                transform.localScale = new Vector3(1,1,1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    void Die()  //死亡
    {
        anime.SetBool("enemy_dead", true);
    }

    
}
