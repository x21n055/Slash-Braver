using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cocoon : MonoBehaviour, IDamageable
{
    //データ
    public int maxHealth;
    public int currentHealth;
    public int speed;
    public float attack1Distance;       //通常近接攻撃
    public float attack2Distance;       //中距離突進攻撃
    public float attackAcceleration;    //突進時の加速度
    public float attack1Cool;           //近接攻撃クールタイム
    public float attack2Cool;           //中距離突進クールタイム
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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
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
            anime.SetBool("HeavyArmor_Walk", true);
            if (playerOnTheRight)
            {
                rb2d.velocity = new Vector2(0,0);
                rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
            }
            else if(!playerOnTheRight)
            {
                rb2d.velocity = new Vector2(0, 0);
                rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
            }
        }
        else if (!cantMove && approachingPlayer && !(range >= attack1Distance))
        {
            anime.SetBool("HeavyArmor_Walk", false);
            rb2d.velocity = new Vector2(0, 0);
        }
    }

    void Combat()
    {
        range = Vector2.Distance(transform.position, target.transform.position);
        if (inCombat)
        {
            if (range <= attack2Distance && !attacking && coolTime <= 0 && attack2Cool <= 0)
            {
                Flip();
                rb2d.velocity = new Vector2(0, 0);
                anime.SetBool("HeavyArmor_Walk", false);
                anime.SetTrigger("HeavyArmor_Attack3");
                coolTime = 3;
                attack2Cool = 7;
            }
            else if (range <= attack1Distance && !attacking && coolTime <= 0 && attack1Cool <= 0)
            {
                rb2d.velocity = new Vector2(0, 0);
                anime.SetBool("HeavyArmor_Walk", false);
                anime.SetTrigger("HeavyArmor_Attack1");
                coolTime = 3;
                attack1Cool = 3;
            }
        }
       

    }

    //プレイヤー発見 戦闘Stateへ移行
    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            inCombat = true;
            approachingPlayer = true;
        }
    }
    
    public void TakeDamage(int damage)  //被ダメ
    {
        if (!isDead && !attacking)
        {

            shock.Invoke();
            currentHealth -= damage;
            StartCoroutine(DamageEffect());
            IEnumerator DamageEffect()
            {

                for (int i = 0; i < 1; i++)
                {
                    sr.color = sr.color == defaultColor ? Color.white : defaultColor;
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
                transform.localScale = new Vector3(1,1,1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    public void Accelerate()
    {
        if (this.transform.localScale.x == -1)
        {
            rb2d.velocity = new Vector2(-attackAcceleration, rb2d.velocity.y);
        }
        else if (this.transform.localScale.x == 1)
        {
            rb2d.velocity = new Vector2(attackAcceleration, rb2d.velocity.y);
        }
    }
    void Die()  //死亡
    {
        anime.SetBool("HeavyArmor_Dead", true);
    }

    void TimeManage() //クールタイムなどを一元化
    {
        coolTime -= Time.deltaTime;
        attack1Cool -= Time.deltaTime;
        attack2Cool -= Time.deltaTime;
    }

    
}
