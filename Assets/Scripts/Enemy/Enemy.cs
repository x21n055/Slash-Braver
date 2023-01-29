using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamageable, Area
{
    public int maxHealth;
    public int currentHealth;
    public int speed;
    public int thisArea;
    public float attack1Distance;       //通常近接攻撃
    public float attack1Cool;           //近接攻撃クールタイム
    [SerializeField] private float coolTime;              //行動クールタイム
    public float distance;              //プレイヤーとの座標的距離
    public float range;                 //プレイヤーとの距離
    private bool playerOnTheRight;      //プレイヤーは右にいるか
    public bool attacking = false;
    [SerializeField] GameObject target;
    private Color defaultColor;

    //コンポーネント
    private Rigidbody2D rb2d = null;
    private SpriteRenderer sr = null;
    private Animator anime = null;

    //コンバットAI
    private bool inCombat = false;  //戦闘状態を判定
    private bool approachingPlayer = false;   //プレイヤーに接近
    public bool cantMove = false;  //硬直状態
    public Transform setPosition;   //敵の初期配置場所
    public UnityEvent shock;        //被ダメ衝撃
    private bool isDead = false;    //死亡
    void Start()
    {
        currentHealth = maxHealth;
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
        anime = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDead)
        {
            PlayerWhichSide();
            Move();
            Combat();
            TimeManage();
        }

    }
    void Move()
    {
        if (!cantMove && approachingPlayer && range >= attack1Distance)
        {
            anime.SetBool("enemy_run", true);
            if (playerOnTheRight)
            {
                rb2d.velocity = new Vector2(0, 0);
                rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
            }
            else if (!playerOnTheRight)
            {
                rb2d.velocity = new Vector2(0, 0);
                rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
            }
        }
        else if (!cantMove && approachingPlayer && !(range >= attack1Distance))
        {
            anime.SetBool("enemy_run", false);
            rb2d.velocity = new Vector2(0, 0);
        }
    }

    void Combat()
    {
        range = Vector2.Distance(transform.position, target.transform.position);
        if (inCombat)
        {
            if (range <= attack1Distance && !attacking && coolTime <= 0 && attack1Cool <= 0)
            {
                rb2d.velocity = new Vector2(0, 0);
                anime.SetBool("enemy_run", false);
                anime.SetTrigger("attack");
                coolTime = 3;
                attack1Cool = 3;
            }
        }


    }

    //プレイヤー発見 戦闘Stateへ移行
    public void IEngage(int area)
    {
        if (area == thisArea)
        {
            inCombat = true;
            approachingPlayer = true;
        }

    }

    public void TakeDamage(int damage)  //被ダメ
    {
        if (!isDead)
        {

            shock.Invoke();
            currentHealth -= damage;
            StartCoroutine(DamageEffect());
            IEnumerator DamageEffect()
            {
                anime.SetTrigger("enemy_damaged");
                for (int i = 0; i < 2; i++)
                {
                    sr.color = sr.color == defaultColor ? Color.red : defaultColor;
                    yield return new WaitForSeconds(0.1f);
                }
                if (currentHealth <= 0)
                {
                    isDead = true;
                    Die();
                }
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
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    void Die()  //死亡
    {
        anime.SetBool("enemy_dead", true);
    }

    void TimeManage() //クールタイムなどを一元化
    {
        coolTime -= Time.deltaTime;
        attack1Cool -= Time.deltaTime;
    }
}