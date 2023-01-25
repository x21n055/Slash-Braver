using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BringerOfDeath : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public float distance;              //プレイヤーとの座標的距離
    public float range;                 //プレイヤーとの距離
    private bool playerOnTheRight;      //プレイヤーは右にいるか

    private Rigidbody2D rb2d = null;
    private SpriteRenderer sr = null;
    private Animator anime = null;
    [SerializeField] GameObject target; //プレイヤーオブジェクトをアタッチ

    public bool attacking = false;
    public bool cantMove = false;  //硬直状態

    private bool isDead = false;    //死亡

    public UnityEvent shock;        //被ダメ衝撃
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
    }

    public void TakeDamage(int damage)  //被ダメ
    {
        if (!isDead && !attacking)
        {
            shock.Invoke();
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                isDead = true;
                Die();
            }
        }
    }

    void PlayerWhichSide() //プレイヤーが右にいるか左にいるか判定を行い、Flip()を呼び出す
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
        anime.SetBool("HeavyArmor_Dead", true);
    }
}
