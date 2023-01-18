using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cocoon : MonoBehaviour
{
    //データ
    public int maxHealth;
    public int currentHealth;
    public float stopTimeCounter;

    //コンポーネント
    private Rigidbody2D rb2d = null;
    private SpriteRenderer sr = null;
    private Animator anime = null;
    //コンバットAI
    private bool inCombat = false;  //戦闘状態を判定
    private bool looming = false;   //プレイヤーに接近
    private bool cantMove = false;  //硬直状態
    public Transform setPosition;   //敵の初期配置場所
    private Transform target;       //プレイヤーの位置
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

    //プレイヤー発見 戦闘Stateへ移行
    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            target = trig.transform;
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

    }

    void Flip(bool playerOnTheRight)    //PlayerWhichSide()から引数を受け取り、それに応じてフリップする
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
